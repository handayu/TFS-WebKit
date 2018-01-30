using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xilium.CefGlue;

namespace UseOnlineTradingSystem
{
    class BsLifeSpanHandler: CefLifeSpanHandler
    {
        private BsClient bClient;
        public BsLifeSpanHandler(BsClient bc)
        {
            bClient = bc;
        }
        protected override void OnAfterCreated(Xilium.CefGlue.CefBrowser browser)
        {
            base.OnAfterCreated(browser);
            bClient.Created(browser);
        }
    }
}
