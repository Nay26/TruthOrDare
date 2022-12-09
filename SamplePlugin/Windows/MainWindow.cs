using Dalamud.Interface.Colors;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Numerics;
using FFSpeedDate.Modules;

namespace FFSpeedDate.Windows;

public class MainWindow : Window, IDisposable
{
    private readonly FFSpeedDate plugin;
    public static Configuration Config { get; set; }
    public Match Match;
    public PlayerList PlayerList;

    private MainTab currentMainTab = MainTab.PlayerList;

    public MainWindow(FFSpeedDate plugin) : base(
        "FF Speed Date", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 630),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;

        Match = new Match(this);
        PlayerList = new PlayerList(this);
    }

    public void Initialize()
    {
        Match = new Match(this);
        PlayerList = new PlayerList(this);
    }

    public void Dispose()
    {
    }

    public override void Draw()
    {
        DrawMainTabs();

        switch (currentMainTab)
        {
            case MainTab.PlayerList:
                {
                    PlayerList.DrawPlayerList();
                    break;
                }
            case MainTab.Match:
                {
                    Match.DrawMatch();
                    break;
                }
            case MainTab.About:
                {
                    DrawAbout();
                    break;
                }
            default:
                PlayerList.DrawPlayerList();
                break;
        }

    }

    private void DrawMainTabs()
    {
        if (ImGui.BeginTabBar("FFSpeedDateMainTabBar", ImGuiTabBarFlags.NoTooltip))
        {
            if (ImGui.BeginTabItem("Player List###FFSpeedDate_PlayerList_MainTab"))
            {
                currentMainTab = MainTab.PlayerList;
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Match###FFSpeedDate_Match_MainTab"))
            {
                currentMainTab = MainTab.Match;
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("About###FFSpeedDate_About_MainTab"))
            {
                currentMainTab = MainTab.About;
                ImGui.EndTabItem();
            }

            ImGui.EndTabBar();
            ImGui.Spacing();
        }
    }
    private void DrawAbout()
    {
        ImGui.TextColored(ImGuiColors.DalamudGrey, "About");
        ImGui.TextWrapped("Made by Adiana Umbra@Cerberus");
        ImGui.TextWrapped("Thanks to Primu Pyon@Omega who's code I have shamelessly modified");
        ImGui.Separator();

        ImGui.Columns(1);
        ImGui.Separator();
        ImGuiHelpers.ScaledDummy(5);
        if (ImGui.Button("Close"))
        {
            IsOpen = false;
        }
    }
}
