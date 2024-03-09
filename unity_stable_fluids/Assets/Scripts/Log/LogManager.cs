using UnityEngine;
using System;

namespace bluebean.UGFramework.Log
{
    /// <summary>
    /// 日志管理器
    /// </summary>
    public class LogManager
    {
        private LogManager()
        {
            Application.logMessageReceived += OnReceiveUnityEngineLog;
        }

        void OnReceiveUnityEngineLog(string log, string stackTrace, UnityEngine.LogType type)
        {
            if (FileLogger == null)
                return;
            if (IsCallingEngineLog)
                return;

            if (NeedFileLog)
            {
                if (type == LogType.Log)
                    FileLogger.WriteLog(log, "D");
                else if (type == LogType.Warning)
                    FileLogger.WriteLog(log, "W");
                else
                    FileLogger.WriteLog(log, "E");
            }
        }

        /// <summary>
        /// 构造日志管理器
        /// </summary>
        /// <returns></returns>
        public static LogManager CreateLogManager()
        { 
            if (m_instance == null)
	        {
		        m_instance = new LogManager();
	        }
            return m_instance;
        }

        public bool Initlize(bool needEngineLog, bool needFileLog, string logFileRoot, string logName)
        {
            NeedEngineLog = needEngineLog;
            NeedFileLog = needFileLog;

            if (needFileLog)
            {
                if(FileLogger==null)
                    FileLogger = new FileLogger(logFileRoot, logName);
            }
            return true;
        }

        public void Uninitlize()
        {
            if (FileLogger != null)
            {
                FileLogger.Close();
                FileLogger = null;
            }
            m_instance = null;
        }

        /// <summary>
        /// 单例访问器
        /// </summary>
        public static LogManager Instance { get {
            return m_instance; 
        } }
        private static LogManager m_instance;

        public FileLogger FileLogger { get; private set; }
        public bool NeedFileLog = false;
        public bool NeedEngineLog = true;
        public bool IsCallingEngineLog = false;
    }
}
