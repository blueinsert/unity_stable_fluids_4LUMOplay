
using System;
using System.Collections.Generic;


public abstract partial class LocalAccountConfigDataBase
{

}

public abstract partial class LocalAccountConfig<T> where T: LocalAccountConfigDataBase
{
    public LocalAccountConfig()
    {
    }

    public void SetFileName(string name)
    {
        m_fileName = name;
    }

    public bool Save()
    {
        if (string.IsNullOrEmpty(m_fileName))
            return false;

        string saveText = UnityEngine.JsonUtility.ToJson(m_data, true);

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

        var data = UnityEngine.JsonUtility.FromJson<T>(saveText);
        if (data == null)
        {
            Debug.LogError(string.Format("LocalAccountConfig.Load {0} failed.", saveText));
            ResetLocalAccountConfigData();
            return false;
        }

        m_data = data;

        return true;
    }

    /// <summary>
    /// 重置数据(于Load失败时调用)
    /// </summary>
    protected abstract void ResetLocalAccountConfigData();

    public T Data { get { return m_data; } }
    public static LocalAccountConfig<T> Instance { set { s_instance = value; } get { return s_instance; } }

    static LocalAccountConfig<T> s_instance;
    static string m_fileName;
    protected T m_data;
}

