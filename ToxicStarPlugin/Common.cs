using Dalamud.Hooking;
using Dalamud.Logging;
using System;

namespace ToxicStarPlugin
{
    public partial class Plugin
    {
        public delegate IntPtr RenderDelegate(IntPtr renderManager);
        private Hook<RenderDelegate> renderDelegateHook;
        private string signature = "E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? 48 8D 0D ?? ?? ?? ?? E8 ?? ?? ?? ?? 48 89 7C 24 38";
        /// <summary>
        /// 打开设置界面
        /// </summary>
        /// <param name="command"></param>
        /// <param name="args"></param>
        private void OnSetting(string command, string args)
        {
            _mainWindow.IsOpen = true;
        }

        private void OnOpen(string command, string args)
        {
            PluginLog.Information("OnOpen");
            //useItem
            var renderAddress = _sigScanner.ScanText(signature);
            PluginLog.Information("renderAddress=" + renderAddress);

            this.renderDelegateHook = Hook<RenderDelegate>.FromAddress(renderAddress, this.RenderDetour);
            this.renderDelegateHook.Enable();
        }

        private unsafe IntPtr RenderDetour(IntPtr renderManager)
        {
            var res = this.renderDelegateHook.Original(renderManager);
            PluginLog.Information("renderManager=" + renderManager);
            PluginLog.Information("res=" + res);
            PluginLog.Information("使用了道具");
            return res;
        }

        private void OnClose(string command, string args)
        {
            PluginLog.Information("OnClose");
            this.renderDelegateHook.Dispose();
        }
    }
}
