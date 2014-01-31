using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class LogHelper
    {
        private IReporter reporter = null;

        public LogHelper()
        {
        }

        public LogHelper(IReporter r)
        {
            this.SetReporter(r);
        }

        public void Abort(string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Abort, -1, String.Format(fmt, objs));
        }

        public void Abort(int code, string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Abort, code, String.Format(fmt, objs));
        }

        public void Assert(string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Assert, -1, String.Format(fmt, objs));
        }

        public void Assert(int code, string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Assert, code, String.Format(fmt, objs));
        }

        public void Error(string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Error, -1, String.Format(fmt, objs));
        }

        public void Error(int code, string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Error, code, String.Format(fmt, objs));
        }

        public void Log(string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Log, 0, String.Format(fmt, objs));
        }

        public void Log(int code, string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Log, code, String.Format(fmt, objs));
        }

        public void Print(string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.StdOut, 0, String.Format(fmt, objs));
        }

        public void Print(int code, string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.StdOut, code, String.Format(fmt, objs));
        }

        public void Success(string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Success, 0, String.Format(fmt, objs));
        }

        public void Success(int code, string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Success, code, String.Format(fmt, objs));
        }

        public void Warning(string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Warning, 0, String.Format(fmt, objs));
        }

        public void Warning(int code, string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Warning, code, String.Format(fmt, objs));
        }

        public void WriteIndirect(ReportType t, int code, string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(t, code, String.Format(fmt, objs));
        }

        public void AITrace(string fmt, params object[] objs)
        {
            if (reporter != null)
                reporter.WriteLog(ReportType.AITrace, 0, String.Format(fmt, objs));
        }

        public void SetReporter(IReporter r)
        {
            reporter = r;
        }
    }
}
