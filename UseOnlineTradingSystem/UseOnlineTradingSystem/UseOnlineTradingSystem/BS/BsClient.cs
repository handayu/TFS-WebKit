using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xilium.CefGlue;

namespace UseOnlineTradingSystem
{
    public class BsClient : CefClient
    {
        public event EventHandler OnCreated;
        private readonly CefLifeSpanHandler lifeSpanHandler;
        private readonly CefContextMenuHandler contextMenuHandler;
        private readonly CefLoadHandler loadHandler;

        public BsClient(string type, Control control)
        {
            lifeSpanHandler = new BsLifeSpanHandler(this);
            contextMenuHandler = new BsContextMenuHandler();
            loadHandler = new BsLoadHandler(type, control);
        }
        protected override CefLifeSpanHandler GetLifeSpanHandler()
        {
            return lifeSpanHandler;
        }
        protected override CefContextMenuHandler GetContextMenuHandler()
        {
            return contextMenuHandler;
        }
        protected override CefLoadHandler GetLoadHandler()
        {
            return loadHandler;
        }
        public void Created(CefBrowser bs)
        {
            if (OnCreated != null)
            {
                OnCreated(bs, EventArgs.Empty);
            }
        }
    }
}
