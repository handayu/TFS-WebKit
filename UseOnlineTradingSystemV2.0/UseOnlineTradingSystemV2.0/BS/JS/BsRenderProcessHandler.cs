using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Xilium.CefGlue;

namespace UseOnlineTradingSystem
{
    public class BsRenderProcessHandler: CefRenderProcessHandler
    {
        private BsAv8Handler exampleA;
        protected override void OnWebKitInitialized()
        {
            #region 原生方式注册 ExampleA

            exampleA = new BsAv8Handler();

            const string exampleAJavascriptCode = @"function exampleA() {}
            if (!exampleA) exampleA = {};
            (function() {
                exampleA.__defineGetter__('myParam',
                function() {
                    native function GetMyParam();
                    return GetMyParam();
                });
                exampleA.__defineSetter__('myParam',
                function(arg0) {
                    native function SetMyParam(arg0);
                    SetMyParam(arg0);
                });

                exampleA.myFunction = function() {
                    native function MyFunction();
                    return MyFunction();
                };
                exampleA.getMyParam = function() {
                    native function GetMyParam();
                    return GetMyParam();
                };
                exampleA.setMyParam = function(arg0) {
                    native function SetMyParam(arg0);
                    SetMyParam(arg0);
                };
                exampleA.setAccount = function(arg0,arg1) {
                    native function SetAccount(arg0,arg1);
                    SetAccount(arg0,arg1);
                };
                exampleA.setCookies = function(arg0) {
                    native function SetCookies(arg0);
                    SetCookies(arg0);
                };
            })();";

            CefRuntime.RegisterExtension("exampleAExtensionName", exampleAJavascriptCode, exampleA);

            #endregion 原生方式注册 ExampleA
            base.OnWebKitInitialized();
        }
    }
}
