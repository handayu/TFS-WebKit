using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xilium.CefGlue;

namespace UseOnlineTradingSystem
{
    class BsCefApp : CefApp
    {
        private CefRenderProcessHandler _renderProcessHandler = new BsRenderProcessHandler();

        protected override void OnBeforeCommandLineProcessing(string processType, CefCommandLine commandLine)
        {
        }
     
        protected override CefRenderProcessHandler GetRenderProcessHandler()
        {
            return _renderProcessHandler;
        }
    }
}
