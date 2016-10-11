#if DEBUG
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Acrux.Html
{
    public static class DebugHelper
    {
        public static TraceSwitch Output = new TraceSwitch("DebugOutput", "Controls the amount of debug.WriteLine() in DEBUG mode.", "1");
    }
}
#endif
