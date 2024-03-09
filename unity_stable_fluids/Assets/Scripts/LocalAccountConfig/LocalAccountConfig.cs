
using System;
using System.Collections.Generic;


// 只支援数组，不支援List
public partial class LocalAccountConfigData
{
    public int SimResolution = 256;
    public int DyeResolution = 1024;
    public float DyeDisspiate = 0.1f;
    public float VelocityDisspiate = 0.1f;
    public float DyeDiffuse = 0f;
    public float VelocityDiffuse = 0f;
    public int PressureIterNum = 66;
    public float AddForce = 12000f;
    public float AddRadius = 0.3f;
    public bool IsShowSettingButton = false;
    public bool IsHideDebugInfo = true;
}

public partial class LocalAccountConfig
{
    public LocalAccountConfig()
    {
        m_data = new LocalAccountConfigData();
    }

    public void SetFileName(string name)
    {
        m_fileName = name;
    }

    public bool Save()
    {
        if (string.IsNullOrEmpty(m_fileName))
            return false;

        string saveText = JsonUtility.Serialize(m_data);
        return FileUtility.WriteText(m_fileName, saveText);
    }

    public bool Load()
    {
        if (string.IsNullOrEmpty(m_fileName))
        {
            ResetLocalAccountConfigData();
            return false;
        }

        if (!FileUtility.IsFileExist(m_fileName))
        {
            ResetLocalAccountConfigData();
            return false;
        }

        string saveText = FileUtility.ReadText(m_fileName);
        if (string.IsNullOrEmpty(saveText))
        {
            ResetLocalAccountConfigData();
            return false;
        }

        var data = JsonUtility.Deserialize<LocalAccountConfigData>(saveText);

        if (data == null)
        {
            Debug.LogError(string.Format("LocalAccountConfig.Load {0} failed.", saveText));
            ResetLocalAccountConfigData();
            return false;
        }

        var jsonData = JsonMapper.ToObject(saveText);
      
        m_data = data;

        return true;
    }

    /// <summary>
    /// 重置数据(于Load失败时调用)
    /// </summary>
    private void ResetLocalAccountConfigData()
    {
       
    }

    public LocalAccountConfigData Data { get { return m_data; } }
    public static LocalAccountConfig Instance { set { s_instance = value; } get { return s_instance; } }

    static LocalAccountConfig s_instance;
    static string m_fileName;
    LocalAccountConfigData m_data;
}

