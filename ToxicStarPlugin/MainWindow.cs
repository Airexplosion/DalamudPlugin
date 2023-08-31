using System;
using System.IO;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using ImGuiScene;

namespace ToxicStarPlugin.Windows;

public class MainWindow : Window, IDisposable
{
    private TextureWrap logoImg;
    private Plugin plugin;
    public int Speed;

    public MainWindow() : base(
        "菜单", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {

    }

    public void OnInit(Plugin plugin)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(600, 600),
            MaximumSize = new Vector2(600, 600)
        };

        var imagePath = Path.Combine(plugin.PluginInterface.AssemblyLocation.Directory?.FullName!, "ToxicStar Logo 512x.png");
        var logoImg = plugin.PluginInterface.UiBuilder.LoadImage(imagePath);

        this.logoImg = logoImg;
        this.plugin = plugin;
        this.Speed = 0;
    }

    public override void Draw()
    {
        ImGui.InputInt("速度", ref Speed);

        ImGui.Spacing();

        ImGui.Text("Have a star:");
        ImGui.Indent(55);
        ImGui.Image(this.logoImg.ImGuiHandle, new Vector2(this.logoImg.Width, this.logoImg.Height));
        ImGui.Unindent(55);
    }

    public void Dispose()
    {
        this.logoImg.Dispose();
    }
}
