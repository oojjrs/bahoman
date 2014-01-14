using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Core;

namespace bball
{
    class Log : IReporter
    {
        private static readonly Log instance = new Log();
        private string path = "";
        private List<string> logs = new List<string>();

        #region From IReporter
        public void WriteLog(ReportType t, int code, string log)
        {
            var now = DateTime.Now.ToString("YYYY-MM-DD HH:mm:ss");
            switch (t)
            {
                case ReportType.Abort:
                    logs.Add(String.Format("[{0}][CRITICAL] {1}", now, log));
                    this.SaveLogs();
                    throw new ApplicationException("더 이상 정상적인 진행을 할 수 없습니다. 로그 파일을 확인해주세요.");
                case ReportType.Assert:
                    logs.Add(String.Format("[{0}][ASSERT] {1}", now, log));
                    Debug.Assert(false, log);
                    break;
                case ReportType.Error:
                    logs.Add(String.Format("[{0}][ERROR] {1}", now, log));
                    break;
                case ReportType.Log:
                    logs.Add(String.Format("[{0}][LOG] {1}", now, log));
                    break;
                case ReportType.StdOut:
                    logs.Add(String.Format("[{0}] {1}", now, log));
                    break;
                case ReportType.Success:
                    logs.Add(String.Format("[{0}][SUCCESS] {1}", now, log));
                    break;
                case ReportType.Warning:
                    logs.Add(String.Format("[{0}][WARNING] {1}", now, log));
                    break;
            }
        }
        #endregion

        ~Log()
        {
            this.SaveLogs();
        }

        private void SaveLogs()
        {
            if (logs.Count > 0 && path != "")
                File.WriteAllLines(path, logs.ToArray());
        }

        public static Log Instance
        {
            get { return instance; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }
    }
}
