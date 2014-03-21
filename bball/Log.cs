using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

using Core;

namespace bball
{
    class Log : IReporter
    {
        private static readonly Log instance = new Log();
        private string path = "";
        private List<string> logs = new List<string>();
        private Player currentPlayer = null;
        private Player targetPlayer = null;
        private Dictionary<Player, List<string>> aitraces = new Dictionary<Player, List<string>>();

        public void WriteLog(ReportType t, int code, string log)
        {
            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string formatted = "";
            switch (t)
            {
                case ReportType.Abort:
                    formatted = String.Format("[{0}][CRITICAL] {1}", now, log);
                    break;
                case ReportType.Assert:
                    formatted = String.Format("[{0}][ASSERT] {1}", now, log);
                    break;
                case ReportType.Error:
                    formatted = String.Format("[{0}][ERROR] {1}", now, log);
                    break;
                case ReportType.Log:
                    formatted = String.Format("[{0}][LOG] {1}", now, log);
                    break;
                case ReportType.StdOut:
                    formatted = String.Format("[{0}] {1}", now, log);
                    break;
                case ReportType.Success:
                    formatted = String.Format("[{0}][SUCCESS] {1}", now, log);
                    break;
                case ReportType.Warning:
                    formatted = String.Format("[{0}][WARNING] {1}", now, log);
                    break;
                case ReportType.AITrace:
                    if (currentPlayer != null)
                    {
                        List<string> traces;
                        if (aitraces.TryGetValue(currentPlayer, out traces) == false)
                        {
                            traces = new List<string>();
                            aitraces[currentPlayer] = traces;
                        }

                        var str = String.Format("[{0}] {1}", now, log);
                        traces.Add(str);
                    }
                    return;
                default:
                    throw new Exception("새로운 에러 타입에 대한 동작을 정의해주세요");
            }

            logs.Add(formatted);
            switch (t)
            {
                case ReportType.Abort:
                    this.SaveLogs();
                    throw new ApplicationException("더 이상 정상적인 진행을 할 수 없습니다. 로그 파일을 확인해주세요.");
                case ReportType.Assert:
                    Debug.Assert(false, log);
                    break;
            }
        }

        ~Log()
        {
            this.SaveLogs();
        }

        public void ClearAITracing()
        {
            aitraces.Clear();
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

        public Player CurrentPlayer
        {
            set { currentPlayer = value; }
        }

        public Player TargetPlayer
        {
            get { return targetPlayer; }
            set { targetPlayer = value; }
        }

        public List<string> TargetLog
        {
            get
            {
                if (targetPlayer != null)
                    return aitraces[targetPlayer];
                return new List<string>();
            }
        }
    }
}
