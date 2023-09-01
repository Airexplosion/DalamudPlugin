using Dalamud.Game.ClientState.Keys;
using Dalamud.Hooking;
using Dalamud.Logging;
using System;

namespace ToxicStarPlugin
{
    public partial class Plugin
    {
        public delegate IntPtr MoveDelegate(IntPtr renderManager);
        private Hook<MoveDelegate> _moveDelegate;
        //Move Main
        private string _signature = "E8 ?? ?? ?? ?? 44 0F 28 D8 E9 ?? ?? ?? ??";
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
            this._moveDelegate?.Dispose();
            var renderAddress = _sigScanner.ScanText(_signature);
            this._moveDelegate = Hook<MoveDelegate>.FromAddress(renderAddress, MoveExec);
            this._moveDelegate.Enable();
        }

        /// <summary>
        /// 移动Hook执行函数
        /// </summary>
        /// <param name="renderManager"></param>
        /// <returns></returns>
        private IntPtr MoveExec(IntPtr renderManager)
        {
            var  res = _moveDelegate.Original(renderManager);
            res *= _mainWindow.Speed;
            return res;
        }

        private void OnClose(string command, string args)
        {
            PluginLog.Information("OnClose");
            this._moveDelegate?.Dispose();
        }
    }
}
