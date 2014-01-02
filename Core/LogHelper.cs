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
                reporter.WriteLog(ReportType.Abort, -1, new StringBuilder().AppendFormat(fmt, objs).ToString());
        }

        public void Abort(int code, string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Abort, code, new StringBuilder().AppendFormat(fmt, objs).ToString());
        }

        public void Assert(string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Assert, -1, new StringBuilder().AppendFormat(fmt, objs).ToString());
        }

        public void Assert(int code, string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Assert, code, new StringBuilder().AppendFormat(fmt, objs).ToString());
        }

        public void Error(string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Error, -1, new StringBuilder().AppendFormat(fmt, objs).ToString());
        }

        public void Error(int code, string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Error, code, new StringBuilder().AppendFormat(fmt, objs).ToString());
        }

        public void Log(string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Log, 0, new StringBuilder().AppendFormat(fmt, objs).ToString());
        }

        public void Log(int code, string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Log, code, new StringBuilder().AppendFormat(fmt, objs).ToString());
        }

        public void Print(string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.StdOut, 0, new StringBuilder().AppendFormat(fmt, objs).ToString());
        }

        public void Print(int code, string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.StdOut, code, new StringBuilder().AppendFormat(fmt, objs).ToString());
        }

        public void Success(string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Success, 0, new StringBuilder().AppendFormat(fmt, objs).ToString());
        }

        public void Success(int code, string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Success, code, new StringBuilder().AppendFormat(fmt, objs).ToString());
        }

        public void Warning(string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Warning, 0, new StringBuilder().AppendFormat(fmt, objs).ToString());
        }

        public void Warning(int code, string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(ReportType.Warning, code, new StringBuilder().AppendFormat(fmt, objs).ToString());
        }

        public void WriteIndirect(ReportType t, int code, string fmt, params object[] objs)
        {
            if(reporter != null)
                reporter.WriteLog(t, code, new StringBuilder().AppendFormat(fmt, objs).ToString());
        }

        public void SetReporter(IReporter r)
        {
            reporter = r;
        }
    }
}
