using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;
using bluebean.UGFramework.Log;
using System.IO;

[System.Serializable]
public class Ejecter
{
    [SerializeField]
    public float x;
    [SerializeField]
    public float y;
    [SerializeField]
    public float dx;
    [SerializeField]
    public float dy;
    [SerializeField]
    public Color color;
    [SerializeField]
    public float peroid = 0.2f;
    [System.NonSerialized]
    public float m_timer;
    [SerializeField]
    public float radius = 0.01f;
}

[System.Serializable]
public class SunrayConfig
{
    [SerializeField]
    public bool enabled = false;
    [SerializeField]
    public float weight = 0.5f;
}

public class FluidConfigConst
{
    public static readonly float DiffuseRange = 0.245f;
    public static readonly float DisspiateRange = 2.0f;
    public static readonly float PressureIterRange = 400;
    public static readonly List<int> VelocityResolutionOptions = new () { 64, 128, 256, 512, 1024, 2048 };
    public static readonly List<int> DyeResolutionOptions = new () { 64, 128, 256, 512, 1024, 2048 };
    public static readonly Vector2Int SplatForceRange = new Vector2Int(5000, 15000);
}

[System.Serializable]
public class FluidConfig
{
    [SerializeField]
    public int simResolution = 256;
    [SerializeField]
    public int dyeResolution = 1024;
    [SerializeField]
    public float dyeDisspiate = 0.1f;
    [SerializeField]
    public float velocityDisspiate = 0.1f;
    [SerializeField]
    public float dyeDiffuse = 0f;
    [SerializeField]
    public float velocityDiffuse = 0f;
    [SerializeField]
    public int pressureIterNum = 66;
    [SerializeField]
    public float addForce = 12000f;
    [SerializeField]
    public float addRadius = 0.3f;
    [SerializeField]
    public bool colorful = true;
    [SerializeField]
    public float colorUpdatePeriod = 0.1f;

    [SerializeField]
    public SunrayConfig sunrayConfig = new SunrayConfig();
    [SerializeField]
    public List<Ejecter> ejecters = new List<Ejecter>();

    public FluidConfig()
    {
        //m_ejecters.Add(new Ejecter()
        //{
        //    x = 0.01f,
        //    y = 0.72f,
        //    dx = 1000,
        //    dy = 0,
        //    color = FluidSimulation.GenerateColor(),
        //    radius = 0.001f,
        //    Peroid = 0.05f,
        //});
        //m_ejecters.Add(new Ejecter()
        //{
        //    x = 0.99f,
        //    y = 0.38f,
        //    dx = -1000,
        //    dy = 0,
        //    color = FluidSimulation.GenerateColor(),
        //    radius = 0.001f,
        //    Peroid = 0.05f,
        //});
        //m_ejecters.Add(new Ejecter()
        //{
        //    x = 0.38f,
        //    y = 0.01f,
        //    dx = 0,
        //    dy = 10,
        //    color = FluidSimulation.GenerateColor(),
        //});
        //m_ejecters.Add(new Ejecter()
        //{
        //    x = 0.72f,
        //    y = 0.99f,
        //    dx = 0,
        //    dy = -10,
        //    color = FluidSimulation.GenerateColor(),
        //});
    }
}

public class PointerData
{
    public int ID = 0;
    public bool IsDown = false;
    public bool IsMove = false;
    public float x;
    public float y;
    public float lastX;
    public float lastY;
    public float deltaX;
    public float deltaY;
    public Color color;
}

public class OutputTextureInfo
{
    public FBO m_fbo;
    public DoubleFBO m_doubleFBO;
    public string m_desc;
}

public class FBO
{
    public static RenderTexture CreateRenderTexture(int w, int h, RenderTextureFormat format, FilterMode filterMode)
    {
        var renderTexture = new RenderTexture(w, h, 0);
        //renderTexture.graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.A10R10G10B10_XRSRGBPack32;
        renderTexture.format = format;
        renderTexture.enableRandomWrite = true; // 允许随机写入  
        renderTexture.filterMode = filterMode; // 过滤模式为点采样  
        renderTexture.antiAliasing = 1; // 抗锯齿级别  
        renderTexture.wrapMode = TextureWrapMode.Clamp;
        renderTexture.Create(); // 创建 RenderTexture  
        return renderTexture;
    }

    public static FBO Create(int w, int h, RenderTextureFormat format, FilterMode filterMode)
    {
        FBO fbo = new FBO();
        fbo.target = CreateRenderTexture(w, h, format, filterMode);
        return fbo;
    }

    public RenderTexture target = null;

    public void Release()
    {
        target.Release();
        target = null;
    }
}

public class DoubleFBO
{
    public static DoubleFBO Create(int w, int h, RenderTextureFormat format, FilterMode filterMode)
    {
        DoubleFBO fbo = new DoubleFBO();
        fbo.m_read = FBO.Create(w, h, format, filterMode);
        fbo.m_write = FBO.Create(w, h, format, filterMode);
        return fbo;
    }

    public FBO m_read = null;
    public FBO m_write = null;

    public FBO Read()
    {
        return m_read;
    }

    public FBO Write()
    {
        return m_write;
    }

    public void Swap()
    {
        var temp = m_read;
        m_read = m_write;
        m_write = temp;
    }

    public void Release()
    {
        m_write.Release();
        m_read.Release();
    }

}

public class FluidSimulation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler,IPointerClickHandler
{
    public static FluidSimulation Instance;

    public FluidConfig FluidConfig
    {
        get
        {
            return m_config;
        }
    }

    private FluidConfig m_config;

    private Vector2Int m_simResolution;
    private Vector2Int m_dyeResolution;
    private Vector2 m_simTexelSize;
    private Vector2 m_dyeTexelSize;

    private DoubleFBO m_velocity = null;
    private DoubleFBO m_dye = null;
    private DoubleFBO m_divergence = null;
    private DoubleFBO m_press = null;
    private FBO m_sunrayMask = null;
    private FBO m_sunray = null;
    private FBO m_final = null;

    //public RenderTexture m_temp = null;
    //public RenderTexture m_temp2 = null;

    private List<OutputTextureInfo> m_outputTextures = new List<OutputTextureInfo>();
    private int m_outputIndex = 0;

    public Material m_addSourceMaterial = null;
    public Material m_advectMaterial = null;
    public Material m_diffuseMaterial = null;
    public Material m_divergenceMaterial = null;
    public Material m_pressMaterial = null;
    public Material m_subtractPressureGradientMaterial = null;
    public Material m_boundMaterial = null;
    public Material m_linSolverMaterial = null;
    public Material m_dissipateMaterial = null;
    public Material m_displayMaterial = null;
    public Material m_copyMaterial = null;
    public Material m_clearMaterial = null;
    public Material m_sunrayMaskMaterial = null;
    public Material m_sunrayMaterial = null;
    public Material m_postEffectMaterial = null;
    public RawImage m_output = null;

    private Queue<int> m_randomSourcePerFrame = new Queue<int>();
    private List<PointerData> m_pointerDatas = new List<PointerData>();
    private float m_colorUpdateTimer = 0;
    private bool m_clearFlag = false;
    public bool m_pausing = false;
    private float m_lastClickTime = -999;
    private Vector2 m_lastClickPos = Vector2.zero;

    private const int FrameRate = 60;
    private float DeltaTime
    {
        get
        {
            return 1.0f / FrameRate;
        }
    }

    public OutputTextureInfo CurrentOutputTextureInfo
    {
        get
        {
            var texture = m_outputTextures[m_outputIndex];
            return texture;
        }
    }

    private void Awake()
    {
        Instance = this;

        var logMgr = LogManager.CreateLogManager();
        logMgr.Initlize(true, true, Path.Combine(UnityEngine.Application.persistentDataPath, "Log/"), "Log_");
        Debug.Log(TestUI.BuildSystemInfoText(false));

        var localConfigPath = Application.persistentDataPath + "/Config/";
        try
        {
            Directory.CreateDirectory(localConfigPath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("InitLocalConfig CreateDirectory Error: " + e);
        }
        FluidSimulationLocalConfig.Instance = new FluidSimulationLocalConfig();
        var configFileName = localConfigPath + "FluidSimulationConfig.txt";
        FluidSimulationLocalConfig.Instance.SetFileName(configFileName);
        if (FluidSimulationLocalConfig.Instance.Load())
        {
            Debug.Log(string.Format("load local config from {0}", configFileName));
            Debug.Log(string.Format("config:\n", FluidSimulationLocalConfig.Instance.ToString()));
        }
        else
        {
            if (!FileUtility.IsFileExist(configFileName))
            {
                Debug.Log(string.Format("init local config file at {0}", configFileName));
                FluidSimulationLocalConfig.Instance.Save();
                Debug.Log(string.Format("config:\n", FluidSimulationLocalConfig.Instance.ToString()));
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        m_config = FluidSimulationLocalConfig.Instance.Data.fluidConfig;

        m_output.material = m_displayMaterial;
        m_outputIndex = 0;

        InitFrameBuffers();

    }

    private void AddOutputTexture(DoubleFBO output, string desc)
    {
        m_outputTextures.Add(new OutputTextureInfo() { m_doubleFBO = output, m_desc = desc });
    }

    private void AddOutputTexture(FBO output, string desc)
    {
        m_outputTextures.Add(new OutputTextureInfo() { m_fbo = output, m_desc = desc });
    }

    private void SwitchOutput()
    {
        var texture = m_outputTextures[m_outputIndex];
        if (texture.m_doubleFBO != null)
            m_output.texture = texture.m_doubleFBO.Read().target;
        else if (texture.m_fbo != null)
            m_output.texture = texture.m_fbo.target;
        //m_displayMaterial.SetTexture("_MainTex", texture);
        //m_output.material = m_displayMaterial;
    }


    Vector2Int GetResolution(int resolution)
    {
        float radio = Screen.width / (float)Screen.height;
        int h = Mathf.Min(resolution, Screen.height);
        int w = Mathf.FloorToInt(radio * h);
        return new Vector2Int(w, h);
    }

    void InitFrameBuffers()
    {
        m_simResolution = GetResolution(m_config.simResolution);
        m_dyeResolution = GetResolution(m_config.dyeResolution);
        m_simTexelSize = new Vector2(1.0f / m_simResolution.x, 1.0f / m_simResolution.y);
        m_dyeTexelSize = new Vector2(1.0f / m_dyeResolution.x, 1.0f / m_dyeResolution.y);
        //ClearFrameBuffers();
        if (m_velocity == null)
            m_velocity = DoubleFBO.Create(m_simResolution.x, m_simResolution.y, RenderTextureFormat.RGFloat, FilterMode.Bilinear);
        else
        {
            var velocity = DoubleFBO.Create(m_simResolution.x, m_simResolution.y, RenderTextureFormat.RGFloat, FilterMode.Bilinear);
            Copy(m_velocity, velocity);
            m_velocity.Release();
            m_velocity = velocity;
        }
        if (m_dye == null)
            m_dye = DoubleFBO.Create(m_dyeResolution.x, m_dyeResolution.y, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear);
        else
        {
            var dye = DoubleFBO.Create(m_dyeResolution.x, m_dyeResolution.y, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear);
            Copy(m_dye, dye);
            m_dye.Release();
            m_dye = dye;
        }
        if (m_divergence == null)
            m_divergence = DoubleFBO.Create(m_simResolution.x, m_simResolution.y, RenderTextureFormat.RFloat, FilterMode.Point);
        else
        {
            var divergence = DoubleFBO.Create(m_simResolution.x, m_simResolution.y, RenderTextureFormat.RFloat, FilterMode.Point);
            Copy(m_divergence, divergence);
            m_divergence.Release();
            m_divergence = divergence;
        }
        if (m_press == null)
            m_press = DoubleFBO.Create(m_simResolution.x, m_simResolution.y, RenderTextureFormat.RFloat, FilterMode.Point);
        else
        {
            var press = DoubleFBO.Create(m_simResolution.x, m_simResolution.y, RenderTextureFormat.RFloat, FilterMode.Point);
            Copy(m_press, press);
            m_press.Release();
            m_press = press;
        }
        if (m_sunrayMask == null)
            m_sunrayMask = FBO.Create(m_dyeResolution.x, m_dyeResolution.y, RenderTextureFormat.RFloat, FilterMode.Bilinear);
        else
        {
            var sunrayMask = FBO.Create(m_dyeResolution.x, m_dyeResolution.y, RenderTextureFormat.RFloat, FilterMode.Bilinear);
            m_sunrayMask.Release();
            m_sunrayMask = sunrayMask;
        }
        if (m_sunray == null)
            m_sunray = FBO.Create(m_dyeResolution.x, m_dyeResolution.y, RenderTextureFormat.RFloat, FilterMode.Bilinear);
        else
        {
            var sunray = FBO.Create(m_dyeResolution.x, m_dyeResolution.y, RenderTextureFormat.RFloat, FilterMode.Bilinear);
            m_sunray.Release();
            m_sunray = sunray;
        }
        if (m_final == null)
            m_final = FBO.Create(m_dyeResolution.x, m_dyeResolution.y, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear);
        else
        {
            var final = FBO.Create(m_dyeResolution.x, m_dyeResolution.y, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear);
            m_final.Release();
            m_final = final;
        }

        m_outputTextures.Clear();
        AddOutputTexture(m_final, "染料");
        //AddOutputTexture(m_dye, "染料");
        AddOutputTexture(m_velocity, "速度");
        AddOutputTexture(m_divergence, "散度");
        AddOutputTexture(m_press, "压力");
        //AddOutputTexture(m_sunrayMask, "sunrayMask");
        //AddOutputTexture(m_sunray, "sunray");
        SwitchOutput();
    }

    public void ChangeSimFrameBufferSizeByIndex(int index)
    {
        var newResolution = FluidConfigConst.VelocityResolutionOptions[index];
        if (m_config.simResolution == newResolution)
            return;
        m_config.simResolution = newResolution;
        InitFrameBuffers();
    }


    public void ChangeDyeFrameBufferSizeByIndex(int index)
    {
        var newResolution = FluidConfigConst.DyeResolutionOptions[index];
        if (m_config.dyeResolution == newResolution)
            return;
        m_config.dyeResolution = newResolution;
        InitFrameBuffers();
    }

    void Blit(FBO fbo, Material m)
    {
        Graphics.Blit(null, fbo.target, m);
    }

    void Copy(FBO source, FBO target)
    {
        m_copyMaterial.SetTexture("_Source", source.target);
        Blit(target, m_copyMaterial);
    }

    void Copy(DoubleFBO source, DoubleFBO target)
    {
        Copy(source.Read(), target.Read());
        Copy(source.Write(), target.Write());
    }

    void Advect(FBO velocity, DoubleFBO source, float bound, Vector2 texelSize)
    {
        m_advectMaterial.SetTexture("_velocity", velocity.target);
        m_advectMaterial.SetTexture("_source", source.Read().target);
        m_advectMaterial.SetFloat("_dt", DeltaTime);
        m_advectMaterial.SetVector("_texelSize", new Vector4(texelSize.x, texelSize.y, 1.0f, 1.0f));
        Blit(source.Write(), m_advectMaterial);
        source.Swap();
        //SetBound(bound, source);
    }

    void Diffuse(DoubleFBO source, float a, float bound, Vector2 texelSize)
    {
        if (a <= 0)
            return;
        //Debug.Log(string.Format("diffuse a:{0}", a));
        bool useImplicit = false;
        if (useImplicit)
        {
            //m_linSolverMaterial.SetFloat("_a", a);
            //m_linSolverMaterial.SetFloat("_c", 1 + 4 * a);
            //m_linSolverMaterial.SetTexture("_Right", source);
            //m_linSolverMaterial.SetTexture("_Left", m_temp);
            //m_linSolverMaterial.SetVector("_texelSize", new Vector4(texelSize.x, texelSize.y, 1.0f, 1.0f));
            //for (int i = 0; i < 20; i++)
            //{
            //    m_linSolverMaterial.SetTexture("_Left", m_temp);
            //    Graphics.Blit(m_temp, m_temp2, m_linSolverMaterial);
            //    Graphics.Blit(m_temp2, m_temp, m_copyMaterial);
            //}
            //Graphics.Blit(m_temp, source, m_copyMaterial);
        }
        else
        {
            m_diffuseMaterial.SetVector("_texelSize", new Vector4(texelSize.x, texelSize.y, 1.0f, 1.0f));

            m_diffuseMaterial.SetFloat("_a", a);//0.23
            for (int i = 0; i < 7; i++)
            {
                m_diffuseMaterial.SetTexture("_Source", source.Read().target);
                Blit(source.Write(), m_diffuseMaterial);
                source.Swap();
                //SetBound(bound, source);
            }

        }
    }

    void CalcDivergence()
    {
        m_divergenceMaterial.SetTexture("_Source", m_velocity.Read().target);
        m_divergenceMaterial.SetVector("_texelSize", new Vector4(m_simTexelSize.x, m_simTexelSize.y, 1.0f, 1.0f));
        Blit(m_divergence.Write(), m_divergenceMaterial);
        m_divergence.Swap();
        //SetBound(0, m_divergence);
    }

    void ResolvePress()
    {
        m_pressMaterial.SetTexture("_Divergence", m_divergence.Read().target);
        m_pressMaterial.SetVector("_texelSize", new Vector4(m_simTexelSize.x, m_simTexelSize.y, 1.0f, 1.0f));
        for (int i = 0; i < m_config.pressureIterNum; i++)
        {
            m_pressMaterial.SetTexture("_Pressure", m_press.Read().target);
            Blit(m_press.Write(), m_pressMaterial);
            m_press.Swap();
            //SetBound(0, m_press);
        }
    }

    void Project()
    {
        CalcDivergence();
        //resove press
        Clear(m_press, 0.89f);
        ResolvePress();

        m_subtractPressureGradientMaterial.SetTexture("_Velocity", m_velocity.Read().target);
        m_subtractPressureGradientMaterial.SetTexture("_Pressure", m_press.Read().target);
        m_subtractPressureGradientMaterial.SetVector("_texelSize", new Vector4(m_simTexelSize.x, m_simTexelSize.y, 1.0f, 1.0f));
        Blit(m_velocity.Write(), m_subtractPressureGradientMaterial);
        m_velocity.Swap();
        //SetBound(1, m_velocity);
    }

    void Dissipate(DoubleFBO source, float speed)
    {
        if (speed <= 0)
            return;
        m_dissipateMaterial.SetTexture("_Source", source.Read().target);
        m_dissipateMaterial.SetFloat("_dissipation", speed);
        m_dissipateMaterial.SetFloat("_dt", DeltaTime);
        //Debug.Log(string.Format("Dissipate strength:{0} dt:{1}", speed, DeltaTime));
        Blit(source.Write(), m_dissipateMaterial);
        source.Swap();
    }

    void SetBound(int bound, int ndim, Vector2 texelSize, RenderTexture source)
    {
        m_boundMaterial.SetVector("_texelSize", new Vector4(texelSize.x, texelSize.y, 1.0f, 1.0f));
        m_boundMaterial.SetInt("_b", bound);
        m_boundMaterial.SetTexture("_Source", source);
        m_boundMaterial.SetInt("_ndim", ndim);
        //Blit(source, source, m_boundMaterial);
    }

    void Step()
    {
        Advect(m_velocity.Read(), m_velocity, 1, m_simTexelSize);
        Diffuse(m_velocity, m_config.velocityDiffuse, 1.0f, m_simTexelSize);
        Project();
        Dissipate(m_velocity, m_config.velocityDisspiate);

        //wrong used m_dyeTexelSize,cause speed very small
        //Advect(m_velocity.Read(), m_dye, 0, m_dyeTexelSize);
        Advect(m_velocity.Read(), m_dye, 0, m_simTexelSize);
        Diffuse(m_dye, m_config.dyeDiffuse, 0, m_dyeTexelSize);
        Dissipate(m_dye, m_config.dyeDisspiate);
    }

    public void RandomAdd()
    {
        m_randomSourcePerFrame.Enqueue((int)(Random.Range(0, 100) / 100f * 25) + 5);
    }

    void RandomAddSource(int num)
    {
        for (int i = 0; i < num; i++)
        {
            var color = GenerateColor();
            color *= 10f;
            var x = Random.Range(0, 100) / 100f;
            var y = Random.Range(0, 100) / 100f;
            var dx = 1500f * (Random.Range(0, 100) / 100f - 0.5f);
            var dy = 1500f * (Random.Range(0, 100) / 100f - 0.5f);
            float radio = Screen.width / (float)Screen.height;
            float radius = m_config.addRadius / 100f * radio;
            AddSource(x, y, dx, dy, color, radius);
        }
    }

    void AddSource(float x, float y, float deltaX, float deltaY, Color color, float radius)
    {
        float radio = Screen.width / (float)Screen.height;
        m_addSourceMaterial.SetTexture("_Source", m_dye.Read().target);
        m_addSourceMaterial.SetFloat("_aspectRadio", radio);
        m_addSourceMaterial.SetVector("_color", new Vector4(color.r, color.g, color.b, color.a));
        m_addSourceMaterial.SetVector("_point", new Vector4(x, y));
        m_addSourceMaterial.SetFloat("_radius", radius);
        Blit(m_dye.Write(), m_addSourceMaterial);
        m_dye.Swap();

        m_addSourceMaterial.SetVector("_color", new Vector4(deltaX, deltaY));
        m_addSourceMaterial.SetTexture("_Source", m_velocity.Read().target);
        Blit(m_velocity.Write(), m_addSourceMaterial);
        m_velocity.Swap();
    }

    void ApplyEjecters()
    {
        foreach (var ejecter in m_config.ejecters)
        {
            ejecter.m_timer += DeltaTime;
            if (ejecter.m_timer > ejecter.peroid)
            {
                ejecter.m_timer -= ejecter.peroid;
                AddSource(ejecter.x, ejecter.y, ejecter.dx, ejecter.dy, ejecter.color, ejecter.radius);
            }

        }
    }

    void ApplyInput()
    {
        foreach (var pointerData in m_pointerDatas)
        {
            if (pointerData.IsMove)
            {
                pointerData.IsMove = false;
                float radio = Screen.width / (float)Screen.height;
                float radius = m_config.addRadius / 100f * radio;
                AddSource(pointerData.x, pointerData.y, pointerData.deltaX * m_config.addForce, pointerData.deltaY * m_config.addForce, pointerData.color, radius);
            }
        }
        if (m_randomSourcePerFrame.Count != 0)
        {
            RandomAddSource(m_randomSourcePerFrame.Dequeue());
        }
        if (m_config.colorful)
        {
            m_colorUpdateTimer += Time.deltaTime;
            if (m_colorUpdateTimer > m_config.colorUpdatePeriod)
            {
                m_colorUpdateTimer -= m_config.colorUpdatePeriod;
                foreach (var pointerData in m_pointerDatas)
                {
                    pointerData.color = GenerateColor();
                }
            }
        }
        ApplyEjecters();
    }

    void Clear(DoubleFBO target, float value)
    {
        m_clearMaterial.SetFloat("_value", value);
        m_clearMaterial.SetTexture("_Source", target.Read().target);
        Blit(target.Write(), m_clearMaterial);
        target.Swap();
    }

    public void Clear()
    {
        if (!m_clearFlag)
            m_clearFlag = true;
        Debug.Log("FluidSimulation:Clear");
    }

    void ClearImpl()
    {
        Clear(m_dye, 0f);
        Clear(m_velocity, 0f);
        foreach (var ejecter in m_config.ejecters)
        {
            ejecter.color = GenerateColor();
        }
    }

    void SunrayEffects()
    {
        m_sunrayMaskMaterial.SetTexture("_Source", m_dye.Read().target);
        Blit(m_sunrayMask, m_sunrayMaskMaterial);

        m_sunrayMaterial.SetTexture("_Source", m_sunrayMask.target);
        m_sunrayMaterial.SetFloat("_weight", 1.0f);
        Blit(m_sunray, m_sunrayMaterial);
    }

    void PostEffects()
    {
        if (m_config.sunrayConfig.enabled)
            SunrayEffects();

        m_postEffectMaterial.SetTexture("_Source", m_dye.Read().target);
        m_postEffectMaterial.SetTexture("_sunray", m_sunray.target);
        if (m_config.sunrayConfig.enabled)
            m_postEffectMaterial.EnableKeyword("Sunray");
        else
            m_postEffectMaterial.DisableKeyword("Sunray");
        Blit(m_final, m_postEffectMaterial);
    }

    // Update is called once per frame
    void Update()
    {
        ApplyInput();
        if (!m_pausing)
        {
            Step();
        }
        PostEffects();
        if (m_clearFlag)
        {
            m_clearFlag = false;
            ClearImpl();
        }
        EventListerner();
    }

    public static Color HSV2RGB(float h, float s, float v)
    {
        float r = 0, g = 0, b = 0, i, f, p, q, t;
        i = Mathf.Floor(h * 6);
        f = h * 6 - i;
        p = v * (1 - s);
        q = v * (1 - f * s);
        t = v * (1 - (1 - f) * s);

        switch (i % 6)
        {
            case 0: r = v; g = t; b = p; break;
            case 1: r = q; g = v; b = p; break;
            case 2: r = p; g = v; b = t; break;
            case 3: r = p; g = q; b = v; break;
            case 4: r = t; g = p; b = v; break;
            case 5: r = v; g = p; b = q; break;
        }

        return new Color(r, g, b);
    }

    public static Color GenerateColor()
    {
        var c = HSV2RGB(Random.Range(0, 100) / 100.0f, 1.0f, 1.0f);
        c.r *= 0.15f;
        c.g *= 0.15f;
        c.b *= 0.15f;
        return c;
    }

    void UpdatePointerDownData(PointerData pointerData, int id, float x, float y)
    {
        //Debug.Log(string.Format("UpdatePointerDownData id:{0} x:{1} y{2}", id, x, y));
        pointerData.ID = id;
        pointerData.x = x;
        pointerData.y = y;
        pointerData.IsDown = true;
        pointerData.IsMove = false;
        pointerData.lastX = x;
        pointerData.lastY = y;
        pointerData.deltaX = 0;
        pointerData.deltaY = 0;
        pointerData.color = GenerateColor();
    }

    void UpdatePointerMoveData(PointerData pointerData, float x, float y)
    {
        //Debug.Log(string.Format("UpdatePointerMoveData id:{0} x:{1} y{2}", pointerData.ID, x, y));
        pointerData.lastX = pointerData.x;
        pointerData.lastY = pointerData.y;
        float radio = Screen.width / (float)Screen.height;
        pointerData.deltaX = (x - pointerData.lastX) * radio;
        pointerData.deltaY = y - pointerData.lastY;
        pointerData.x = x;
        pointerData.y = y;
        pointerData.IsMove = pointerData.deltaX != 0 || pointerData.deltaY != 0;
    }

    void UpdatePointerUpData(PointerData pointerData)
    {
        //Debug.Log(string.Format("UpdatePointerUpData id:{0}", pointerData.ID));
        pointerData.ID = 999;
        pointerData.IsMove = false;
        pointerData.IsDown = false;
        //DebugPointerDatas();
    }

    void EventListerner()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    var mousePos = Input.mousePosition;
        //    float u = mousePos.x / (float)Screen.width;
        //    float v = mousePos.y / (float)Screen.height;
        //    var pointer = m_pointerDatas[0];

        //    UpdatePointerDownData(pointer, -1, u, v);
        //}
        //if (Input.GetMouseButton(0))
        //{
        //    var mousePos = Input.mousePosition;
        //    float u = mousePos.x / (float)Screen.width;
        //    float v = mousePos.y / (float)Screen.height;
        //    var pointer = m_pointerDatas[0];

        //    UpdatePointerMoveData(pointer, -1, u, v);
        //}
        //if (Input.GetMouseButtonUp(0))
        //{
        //    var mousePos = Input.mousePosition;
        //    float u = mousePos.x / (float)Screen.width;
        //    float v = mousePos.y / (float)Screen.height;
        //    var pointer = m_pointerDatas[0];

        //    UpdatePointerUpData(pointer, -1, u, v);
        //}
        if (Input.GetKeyUp(KeyCode.C))
        {
            Clear();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            LeftSwitchOutput();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            RightSwitchOutput();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            m_randomSourcePerFrame.Enqueue((int)(Random.Range(0, 100) / 100f * 25) + 5);
        }
    }

    public void LeftSwitchOutput()
    {
        m_outputIndex--;
        if (m_outputIndex < 0)
        {
            m_outputIndex = m_outputTextures.Count - 1;
        }
        SwitchOutput();
    }

    public void RightSwitchOutput()
    {
        m_outputIndex++;
        if (m_outputIndex >= m_outputTextures.Count)
        {
            m_outputIndex = 0;
        }
        SwitchOutput();
    }

    private void OnRenderObject()
    {

    }

    private void ClearFrameBuffers()
    {
        if (m_velocity != null)
        {
            m_velocity.Release();
            m_velocity = null;
        }
        if (m_dye != null)
        {
            m_dye.Release();
            m_dye = null;
        }
        if (m_divergence != null)
        {
            m_divergence.Release();
            m_divergence = null;
        }
        if (m_press != null)
        {
            m_press.Release();
            m_press = null;
        }
        if (m_sunrayMask != null)
        {
            m_sunrayMask.Release();
            m_sunrayMask = null;
        }
        if (m_sunray != null)
        {
            m_sunray.Release();
            m_sunray = null;
        }
        if (m_final != null)
        {
            m_final.Release();
            m_final = null;
        }
    }

    private void OnDestroy()
    {
        ClearFrameBuffers();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log(string.Format("OnPointerDown:{0} id:{1}", eventData.position, eventData.pointerId));
        var id = eventData.pointerId;
        var pointer = m_pointerDatas.Find((e) => { return e.ID == id; });
        if(pointer != null)
        {
            return;//has processed
        }
        pointer = m_pointerDatas.Find((e) => { return e.ID == 999; });
        if (pointer == null)
        {
            pointer = new PointerData();
            m_pointerDatas.Add(pointer);
        }

        var pos = eventData.position;
        var mousePos = pos;
        float u = mousePos.x / (float)Screen.width;
        float v = mousePos.y / (float)Screen.height;
        UpdatePointerDownData(pointer, id, u, v);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //Debug.Log(string.Format("OnPointerMove:{0} id:{1}", eventData.position, eventData.pointerId, eventData.button));

        var id = eventData.pointerId;
        //DebugPointerDatas();
        var pointer = m_pointerDatas.Find((e) => { return e.ID == id; });
#if UNITY_ANDROID && !UNITY_EDITOR
        if (pointer == null)
        {
            // OnPointerMove may occurs eariler than OnPointerDown
            Debug.Log(string.Format("OnPointerMove:Call OnPointerDown:{0} id:{1}", eventData.position, eventData.pointerId));
            OnPointerDown(eventData);
        }
#endif
        pointer = m_pointerDatas.Find((e) => { return e.ID == id; });
        if (pointer != null)
        {
            if (pointer.IsDown)
            {
                var pos = eventData.position;
                //Debug.Log(string.Format("OnPointerMove:{0} id:{1}", pos, eventData.pointerId));
                var mousePos = pos;
                float u = mousePos.x / (float)Screen.width;
                float v = mousePos.y / (float)Screen.height;
                UpdatePointerMoveData(pointer, u, v);
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log(string.Format("OnPointerUp:{0} id:{1}", eventData.position, eventData.pointerId));
        var id = eventData.pointerId;
        var pointer = m_pointerDatas.Find((e) => { return e.ID == id; });
        if (pointer != null)
        {
            UpdatePointerUpData(pointer);
        }
        else
        {
            Debug.LogError("OnPointerUp target pointer == null");
        }
            
    }

    private void DebugPointerDatas()
    {
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < m_pointerDatas.Count; i++)
        {
            var data = m_pointerDatas[i];
            sb.Append("index:").Append(i).Append(" id:").Append(data.ID).Append("\n");
        }
        Debug.Log("m_pointerDatas:" + sb.ToString());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("FluidSimulation:OnPointerClick");
        var clickTime = Time.time;
        var clickPos = eventData.position;
        if(clickTime - m_lastClickTime < Time.deltaTime * 20 && (m_lastClickPos-clickPos).magnitude < 10)
        {
            RandomAdd();
        }
        m_lastClickTime = clickTime;
        m_lastClickPos = clickPos;
    }
}
