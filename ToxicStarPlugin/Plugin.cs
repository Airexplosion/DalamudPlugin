using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Interface.Windowing;
using System.Net.NetworkInformation;
using Dalamud.Game.Network;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using System.Reflection.Emit;
using System;
using Dalamud.Logging;
using Dalamud.Hooking;
using Dalamud.Game;
using Lumina.Excel.GeneratedSheets;
using ToxicStarPlugin.Windows;

namespace ToxicStarPlugin
{
    /// <summary>
    /// 插件入口
    /// </summary>
    public sealed partial class Plugin : IDalamudPlugin
    {
        //基础
        public string Name => "ToxicStar Plugin";

        //指令
        private const string _settingName = "/tssetting";
        private const string _openName = "/tsopen";
        private const string _closeName = "/tsclose";

        //插件接口
        public DalamudPluginInterface PluginInterface { get; init; }
        //通用管理器
        private CommandManager _commandManager { get; init; }
        //地址解析
        private SigScanner _sigScanner { get; init; }
        //窗口
        private WindowSystem _windowSystem = new("ToxicStarPlugin");
        private MainWindow _mainWindow { get; init; }

        //构造函数
        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager,
            [RequiredVersion("1.0")] SigScanner sigScanner)
        {
            PluginInterface = pluginInterface;
            _commandManager = commandManager;
            _sigScanner = sigScanner;

            //读取并加载图片 然后传入窗口类中
            _mainWindow = new MainWindow();
            _mainWindow.OnInit(this);
            _windowSystem.AddWindow(_mainWindow);
            
            //绑定指令监听
            this._commandManager.AddHandler(_settingName, new CommandInfo(OnSetting)
            {
                HelpMessage = "打开设置窗口"
            });
            this._commandManager.AddHandler(_openName, new CommandInfo(OnOpen)
            {
                HelpMessage = "开启，字如其意"
            });
            this._commandManager.AddHandler(_closeName, new CommandInfo(OnClose)
            {
                HelpMessage = "关闭，字如其意"
            });

            //绘制UI
            this.PluginInterface.UiBuilder.Draw += DrawUI;
        }

        //绘制UI方法
        private void DrawUI()
        {
            this._windowSystem.Draw();
        }

        //退出方法
        public void Dispose()
        {
            //移除窗口监听
            this._windowSystem.RemoveAllWindows();
            //关闭窗口
            _mainWindow.Dispose();
            //移除指令监听
            this._commandManager.RemoveHandler(_settingName);
            this._commandManager.RemoveHandler(_openName);
            this._commandManager.RemoveHandler(_closeName);
        }
    }
}
