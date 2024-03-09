using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIIntent
{
    public string ID { get { return m_id; } }

    private string m_id;
    private readonly Dictionary<string, object> m_customParamDic = new Dictionary<string, object>();

    public UIIntent(string id)
    {
        m_id = id;
    }

    public void SetCustomParam(string key, object value)
    {
        if (m_customParamDic.ContainsKey(key))
        {
            m_customParamDic[key] = value;
        }
        else
        {
            m_customParamDic.Add(key, value);
        }
    }

    public T GetCustomClassParam<T>(string key) where T : class
    {
        if (m_customParamDic.ContainsKey(key))
        {
            return m_customParamDic[key] as T;
        }
        return null;
    }

    public T GetCustomStructParam<T>(string key) where T : struct
    {
        if (m_customParamDic.ContainsKey(key))
        {
            return (T)m_customParamDic[key];
        }
        return default(T);
    }

    public void ClearCustomParam(string key)
    {
        if (m_customParamDic.ContainsKey(key))
        {
            m_customParamDic.Remove(key);
        }
    }

    public void ClearAllCustomParam()
    {
        m_customParamDic.Clear();
    }
}

