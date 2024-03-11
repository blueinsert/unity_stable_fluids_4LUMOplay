using UnityEngine;
using System.Text;
using System;
using bluebean.UGFramework.Log;

/// <summary>
/// It will override System.Debug or UnityEngine.Debug even if you are using System or UnityEngine namespace.
/// Don't put Debug class in BlackJack namespace. 
/// </summary>
public static partial class Debug
{
    static public void LogException(Exception e)
    {
        Log(e.ToString());
    }

    static public void Log(string str)
    {
        if (m_mainThread == null || LogManager.Instance == null)
        {
            UnityEngine.Debug.Log(str);
            return;
        }

        if (LogManager.Instance.NeedFileLog)
        {
            LogManager.Instance.FileLogger.WriteLog(str, "D");
        }

        // 不在主线程，直接返回
        if (System.Threading.Thread.CurrentThread != m_mainThread) return;
        
        if (LogManager.Instance.NeedEngineLog)
        {
            LogManager.Instance.IsCallingEngineLog = true;
            UnityEngine.Debug.Log(str);
            LogManager.Instance.IsCallingEngineLog = false;
        }
    }

    static public void Log(params object[] paramList)
    {
        string str = ParamListToString(paramList);
        Log(str);
    }

    static public void LogWarning(string str)
    {
        if (m_mainThread == null || LogManager.Instance == null)
        {
            UnityEngine.Debug.LogWarning(str);
            return;
        }


        if (LogManager.Instance.NeedFileLog)
        {
            LogManager.Instance.FileLogger.WriteLog(str, "W");
        }

        // 不在主线程，直接返回
        if (System.Threading.Thread.CurrentThread != m_mainThread) return;

        if (LogManager.Instance.NeedEngineLog)
        {
            LogManager.Instance.IsCallingEngineLog = true;
            UnityEngine.Debug.LogWarning(str);
            LogManager.Instance.IsCallingEngineLog = false;
        }
    }
	static public void LogWarning(params object[] paramList) 
	{
		string str = ParamListToString(paramList);
        LogWarning(str);
	}

    static public void LogError(string str)
    {
        if (m_mainThread == null || LogManager.Instance == null)
        {
            UnityEngine.Debug.LogError(str);
            return;
        }

        if (LogManager.Instance.NeedFileLog)
        {
            LogManager.Instance.FileLogger.WriteLog(str, "E");
        }

        // 不在主线程，直接返回
        if (System.Threading.Thread.CurrentThread != m_mainThread) return;

        if (LogManager.Instance.NeedEngineLog)
        {
            LogManager.Instance.IsCallingEngineLog = true;
            UnityEngine.Debug.LogError(str);
            LogManager.Instance.IsCallingEngineLog = false;
        }
    }
	static public void LogError(params object[] paramList) 
	{
		string str = ParamListToString(paramList);
        LogError(str);
	}

    static public void Assert(bool value, string str)
    {
        if (!value)
        {
            if (m_mainThread != null && System.Threading.Thread.CurrentThread != m_mainThread) return;
            LogError(str);
        }
    }
    static public void Assert(bool value, params object[] paramList)
    {
        if (!value)
        {
            if (m_mainThread != null && System.Threading.Thread.CurrentThread != m_mainThread) return;
            LogError(paramList);
        }
    }

    static public void WriteLine(string str)
    {
        Log(str);
    }
	static public void WriteLine(params object[] paramList) 
	{
        Log(paramList);
	}

	static public void SystemLogException(params object[] paramList) 
	{
        LogError(paramList);
	}

	static public void Break()
	{
        if (m_mainThread != null && System.Threading.Thread.CurrentThread != m_mainThread) return;
		UnityEngine.Debug.Break();
	}

	static string ParamListToString(object[] paramList)
	{
		if (paramList == null || paramList.Length == 0)
			return "";

        if (paramList.Length == 1)
        {
            return paramList[0].ToString();
        }

		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < paramList.Length; ++i)
		{
			if (i != 0)
				sb.Append(";\t");

			object param = paramList[i];
			if (param != null)
				sb.Append(paramList[i].ToString());
			else
				sb.Append("NULL_PARAM");
		}
		return sb.ToString();
	}

    public static System.Threading.Thread m_mainThread = null;
}


///Draw part 
public static partial class Debug
{
    public static void DrawRect(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, Color c)
    {
        if (m_mainThread != null && System.Threading.Thread.CurrentThread != m_mainThread) return;
        UnityEngine.Debug.DrawLine(p1, p2, c);
        UnityEngine.Debug.DrawLine(p2, p3, c);
        UnityEngine.Debug.DrawLine(p3, p4, c);
        UnityEngine.Debug.DrawLine(p4, p1, c);
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
	{
        if (m_mainThread != null && System.Threading.Thread.CurrentThread != m_mainThread) return;
		UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
	}
	
	public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
	{
        if (m_mainThread != null && System.Threading.Thread.CurrentThread != m_mainThread) return;
		UnityEngine.Debug.DrawLine(start, end, color, duration);
	}
	
	public static void DrawLine(Vector3 start, Vector3 end, Color color)
	{
        if (m_mainThread != null && System.Threading.Thread.CurrentThread != m_mainThread) return;
		UnityEngine.Debug.DrawLine(start, end, color);
	}
	
	public static void DrawLine(Vector3 start, Vector3 end)
	{
        if (m_mainThread != null && System.Threading.Thread.CurrentThread != m_mainThread) return;
		UnityEngine.Debug.DrawLine(start, end);
	}
	
	public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest)
	{
        if (m_mainThread != null && System.Threading.Thread.CurrentThread != m_mainThread) return;
		UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
	}
	
	public static void DrawRay(Vector3 start, Vector3 dir, Color color)
	{
        if (m_mainThread != null && System.Threading.Thread.CurrentThread != m_mainThread) return;
		UnityEngine.Debug.DrawRay(start, dir, color);
	}
	
	public static void DrawRay(Vector3 start, Vector3 dir)
	{
        if (m_mainThread != null && System.Threading.Thread.CurrentThread != m_mainThread) return;
		UnityEngine.Debug.DrawRay(start, dir);
	}
	
	public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
	{
        if (m_mainThread != null && System.Threading.Thread.CurrentThread != m_mainThread) return;
		UnityEngine.Debug.DrawRay(start, dir, color);
	}
}

