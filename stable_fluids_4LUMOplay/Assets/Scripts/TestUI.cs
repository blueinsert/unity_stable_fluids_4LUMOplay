using System.Collections;
using System.Collections.Generic;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class TestUI : MonoBehaviour
{

    static void AppendLine(StringBuilder sb, bool textColor, string name, string value)
    {
        if (sb.Length > 0)
            sb.Append("\n");
        if (textColor)
            sb.Append(name);
        else
            sb.Append(name);
        sb.Append(": ");
        sb.Append(value);
    }

    public static string BuildSystemInfoText(bool textColor)
    {
        var sb = new StringBuilder();
        AppendLine(sb, textColor, "SystemInfo", string.Empty);
        AppendLine(sb, textColor, "Quality Setting", QualitySettings.names[QualitySettings.GetQualityLevel()]);
        AppendLine(sb, textColor, "Resolution", string.Format("{0} x {1}", Screen.width, Screen.height));
        AppendLine(sb, textColor, "Refresh Rate", Screen.currentResolution.refreshRateRatio.ToString());
        AppendLine(sb, textColor, "Safe Area", Screen.safeArea.ToString());
        AppendLine(sb, textColor, "BatteryLevel", SystemInfo.batteryLevel.ToString());
        AppendLine(sb, textColor, "BatteryStatus", SystemInfo.batteryStatus.ToString());
        AppendLine(sb, textColor, "CopyTextureSupport", SystemInfo.copyTextureSupport.ToString());
        AppendLine(sb, textColor, "DeviceModel", SystemInfo.deviceModel);
        AppendLine(sb, textColor, "DeviceName", SystemInfo.deviceName);
        AppendLine(sb, textColor, "DeviceType", SystemInfo.deviceType.ToString());
        AppendLine(sb, textColor, "DeviceUniqueIdentifier", SystemInfo.deviceUniqueIdentifier);
        AppendLine(sb, textColor, "GraphicsDeviceID", SystemInfo.graphicsDeviceID.ToString());
        AppendLine(sb, textColor, "GraphicsDeviceName", SystemInfo.graphicsDeviceName);
        AppendLine(sb, textColor, "GraphicsDeviceType", SystemInfo.graphicsDeviceType.ToString());
        AppendLine(sb, textColor, "GraphicsDeviceVendor", SystemInfo.graphicsDeviceVendor);
        AppendLine(sb, textColor, "GraphicsDeviceVendorID", SystemInfo.graphicsDeviceVendorID.ToString());
        AppendLine(sb, textColor, "GraphicsDeviceVersion", SystemInfo.graphicsDeviceVersion.ToString());
        AppendLine(sb, textColor, "GraphicsMemorySize", SystemInfo.graphicsMemorySize.ToString());
        AppendLine(sb, textColor, "GraphicsMultiThreaded", SystemInfo.graphicsMultiThreaded.ToString());
        AppendLine(sb, textColor, "GraphicsShaderLevel", SystemInfo.graphicsShaderLevel.ToString());
        AppendLine(sb, textColor, "GraphicsUVStartsAtTop", SystemInfo.graphicsUVStartsAtTop.ToString());
        AppendLine(sb, textColor, "MaxCubemapSize", SystemInfo.maxCubemapSize.ToString());
        AppendLine(sb, textColor, "MaxTextureSize", SystemInfo.maxTextureSize.ToString());
        AppendLine(sb, textColor, "NpotSupport", SystemInfo.npotSupport.ToString());
        AppendLine(sb, textColor, "OperatingSystem", SystemInfo.operatingSystem);
        AppendLine(sb, textColor, "OperatingSystemFamily", SystemInfo.operatingSystemFamily.ToString());
        AppendLine(sb, textColor, "ProcessorCount", SystemInfo.processorCount.ToString());
        AppendLine(sb, textColor, "ProcessorFrequency", SystemInfo.processorFrequency.ToString());
        AppendLine(sb, textColor, "ProcessorType", SystemInfo.processorType.ToString());
        AppendLine(sb, textColor, "SupportedRenderTargetCount", SystemInfo.supportedRenderTargetCount.ToString());
        AppendLine(sb, textColor, "Supports2DArrayTextures", SystemInfo.supports2DArrayTextures.ToString());
        AppendLine(sb, textColor, "Supports3DTextures", SystemInfo.supports3DTextures.ToString());
        AppendLine(sb, textColor, "Supports3DRenderTextures", SystemInfo.supports3DRenderTextures.ToString());
        AppendLine(sb, textColor, "SupportsAcceleromete", SystemInfo.supportsAccelerometer.ToString());
        AppendLine(sb, textColor, "SupportsAsyncCompute", SystemInfo.supportsAsyncCompute.ToString());
        AppendLine(sb, textColor, "SupportsAudio", SystemInfo.supportsAudio.ToString());
        AppendLine(sb, textColor, "SupportsComputeShaders", SystemInfo.supportsComputeShaders.ToString());
        AppendLine(sb, textColor, "SupportsCubemapArrayTextures", SystemInfo.supportsCubemapArrayTextures.ToString());
        AppendLine(sb, textColor, "SupportsGPUFence", SystemInfo.supportsGraphicsFence.ToString());
        AppendLine(sb, textColor, "SupportsGyroscope", SystemInfo.supportsGyroscope.ToString());
        AppendLine(sb, textColor, "SupportsImageEffects", SystemInfo.supportsImageEffects.ToString());
        AppendLine(sb, textColor, "SupportsInstancing", SystemInfo.supportsInstancing.ToString());
        AppendLine(sb, textColor, "SupportsLocationService", SystemInfo.supportsLocationService.ToString());
        AppendLine(sb, textColor, "SupportsMotionVectors", SystemInfo.supportsMotionVectors.ToString());
        AppendLine(sb, textColor, "SupportsMultisampledTextures", SystemInfo.supportsMultisampledTextures.ToString());
        AppendLine(sb, textColor, "SupportsRawShadowDepthSampling", SystemInfo.supportsRawShadowDepthSampling.ToString());
        AppendLine(sb, textColor, "SupportsRenderToCubemap", SystemInfo.supportsRenderToCubemap.ToString());
        AppendLine(sb, textColor, "Supports HDR RenderTexturesFormat", SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.DefaultHDR).ToString());
        AppendLine(sb, textColor, "Supports Texture Format ETC2_RGBA8", SystemInfo.SupportsTextureFormat(TextureFormat.ETC2_RGBA8).ToString());
        AppendLine(sb, textColor, "Supports Texture Format ASTC_RGBA_4x4", SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_4x4).ToString());
        AppendLine(sb, textColor, "SupportsShadows", SystemInfo.supportsShadows.ToString());
        AppendLine(sb, textColor, "SupportsSparseTextures", SystemInfo.supportsSparseTextures.ToString());
        AppendLine(sb, textColor, "SupportsTextureWrapMirrorOnce", SystemInfo.supportsTextureWrapMirrorOnce.ToString());
        AppendLine(sb, textColor, "SupportsVibration", SystemInfo.supportsVibration.ToString());
        AppendLine(sb, textColor, "SystemMemorySize", SystemInfo.systemMemorySize.ToString());
        AppendLine(sb, textColor, "UnsupportedIdentifie", SystemInfo.unsupportedIdentifier);
        AppendLine(sb, textColor, "UsesReversedZBuffe", SystemInfo.usesReversedZBuffer.ToString());
        AppendLine(sb, textColor, "DataPath", Application.dataPath);
        AppendLine(sb, textColor, "PersistentDataPath", Application.persistentDataPath);
        AppendLine(sb, textColor, "StreamingAssetsPath", Application.streamingAssetsPath);
        AppendLine(sb, textColor, "TemporaryCachePath", Application.temporaryCachePath);
        return sb.ToString();
    }

    public void Awake()
    {
        
    }

    void ComputeFps()
    {
        ++m_updateCount;
        float now = Time.realtimeSinceStartup;
        float dt = now - m_lastFpsResetTime;
        if (dt > 0.25f)
        {
            m_fps = m_updateCount / dt;
            m_updateCount = 0;
            m_lastFpsResetTime = now;
        }
    }

    void Update()
    {
        ComputeFps();

        if (Input.GetKeyUp(KeyCode.Escape))
        {
#if UNITY_EDITOR
            EditorApplication.isPaused = !EditorApplication.isPaused;
#endif
        }
    }

    void InitStyle(int unit)
    {
        m_unit = unit;
        m_buttonStyle = new GUIStyle(GUI.skin.button);
        m_buttonStyle.fontSize = unit * 7 / 8;
        m_buttonStyle.padding = new RectOffset();

        m_textFieldStyle = new GUIStyle(GUI.skin.textField);
        m_textFieldStyle.fontSize = unit * 7 / 8;
        m_textFieldStyle.padding = new RectOffset();

        m_textStyle = new GUIStyle(GUI.skin.label);
        m_textStyle.normal.textColor = Color.white;
        m_textStyle.fontSize = unit * 7 / 8;
        m_textStyle.padding = new RectOffset();

        m_textStyleGray = new GUIStyle(GUI.skin.label);
        m_textStyleGray.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
        m_textStyleGray.fontSize = unit * 7 / 8;
        m_textStyleGray.padding = new RectOffset();

        m_textStyleSmall = new GUIStyle(GUI.skin.label);
        m_textStyleSmall.normal.textColor = Color.white;
        m_textStyleSmall.fontSize = unit * 7 / 10;
        m_textStyleSmall.padding = new RectOffset();

        m_textStyleSmallGray = new GUIStyle(GUI.skin.label);
        m_textStyleSmallGray.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
        m_textStyleSmallGray.fontSize = unit * 7 / 10;
        m_textStyleSmallGray.padding = new RectOffset();

        m_textStyleSmallCenter = new GUIStyle(GUI.skin.label);
        m_textStyleSmallCenter.normal.textColor = Color.white;
        m_textStyleSmallCenter.fontSize = unit * 7 / 10;
        m_textStyleSmallCenter.padding = new RectOffset();
        m_textStyleSmallCenter.alignment = TextAnchor.UpperCenter;

        m_textStyleSmall2 = new GUIStyle(GUI.skin.label);
        m_textStyleSmall2.normal.textColor = Color.white;
        m_textStyleSmall2.fontSize = unit * 5 / 10;
        m_textStyleSmall2.padding = new RectOffset();
    }

    void OnGUI()
    {
        if (FluidSimulationLocalConfig.Instance != null && !FluidSimulationLocalConfig.Instance.Data.isHideDebugInfo)
        {
            int unit = Screen.width / 80;
            if (unit != m_unit)
            {
                InitStyle(unit);
            }

            int x = m_unit * 2;
            int y = Screen.height - m_unit * 2;
            GUI.Label(new Rect(x, y, m_unit * 10, m_unit * 2), $"FPS  {m_fps:f1} ({1000.0f / m_fps:f1})", m_textStyle);

            y -= m_unit;
            GUI.Label(new Rect(x, y, m_unit * 10, m_unit * 2), string.Format("{0}", FluidSimulation.Instance.CurrentOutputTextureInfo.m_desc), m_textStyle);
        }
    }


    int m_updateCount;
    float m_fps;
    float m_lastFpsResetTime;

    int m_unit = 0;
    GUIStyle m_buttonStyle = null;
    GUIStyle m_textFieldStyle = null;
    GUIStyle m_textStyle = null;
    GUIStyle m_textStyleGray = null;
    GUIStyle m_textStyleSmall = null;
    GUIStyle m_textStyleSmallGray = null;
    GUIStyle m_textStyleSmallCenter = null;
    GUIStyle m_textStyleSmall2 = null;
}
