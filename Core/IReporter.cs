using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public enum ReportType
    {
        Abort,
        Assert,
        Error,
        Log,
        StdOut,
        Success,
        Warning,

        AITrace,
    }

    public interface IReporter
    {
        void WriteLog(ReportType t, int code, string log);
    }
}
