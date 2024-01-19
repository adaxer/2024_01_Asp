using Serilog;
using System.Diagnostics;

namespace HelloRazor.Lib;

public class AppTraceListener : TraceListener
{
    public override void Write(string? message)
    {
        if (message != null)
        {
            Log.Logger.Debug(message ?? "");
        }
    }

    public override void WriteLine(string? message)
    {
        Write(message + "\r\n");
    }
}
