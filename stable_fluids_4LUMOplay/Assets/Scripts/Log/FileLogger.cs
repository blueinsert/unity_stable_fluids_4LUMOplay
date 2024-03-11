using System;
using System.IO;

namespace bluebean.UGFramework.Log
{
    public class FileLogger
    {
        public FileLogger(string logFileRoot, string logName)
        {
            if (!Directory.Exists(logFileRoot))
            {
                try
                {
                    Directory.CreateDirectory(logFileRoot);
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.Log("Create log directory fail " + e.ToString());
                    return;
                }
            }

            _logFileRoot = logFileRoot;
            _logName = logName;

            _logFileFullPath = GetNewFileFullPath();

            _logStreamWriter = new StreamWriter(_logFileFullPath);

            UnityEngine.Debug.Log("Create Log File " + _logFileFullPath);
        }

        private string GetNewFileFullPath()
        {
            string fullPath = _logFileRoot + _logName + DateTime.Now.ToString("yyyy_MMdd_HHmm_ss") + ".txt";
            return fullPath;
        }

        public void WriteLog(string msg, string level)
        {
            if (_logStreamWriter == null)
                return;

            lock (_logStreamWriter)
            {
                try
                {
                    var now = DateTime.Now;
                    string log = string.Format("[{0}][{1}] {2}", level, now.ToString("yyyy-MM-dd HH:mm:ss:fff"), msg);
                    _logStreamWriter.WriteLine(log);
                    _logStreamWriter.Flush();
                    if (EventOnLog != null)
                        EventOnLog(log);
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        public void Close()
        {
            if (_logStreamWriter == null)
                return;

            lock (_logStreamWriter)
            {
                try
                {
                    _logStreamWriter.Close();
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        private string _logFileRoot;
        private string _logName;
        private string _logFileFullPath;
        private StreamWriter _logStreamWriter;
        public event Action<string> EventOnLog = null;
    }
}
