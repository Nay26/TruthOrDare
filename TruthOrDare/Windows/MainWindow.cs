using Dalamud.Interface.Colors;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Numerics;
using TruthOrDare.Modules;
using System.Collections.Generic;
using Dalamud.Game.Text;
using XivCommon;

namespace TruthOrDare.Windows;

public class MainWindow : Window, IDisposable
{
    private readonly TruthOrDare plugin;
    public static Configuration Config { get; set; }
    public Game Game;
    public PlayerList PlayerList;
    private MainTab currentMainTab = MainTab.PlayerList;

    public MainWindow(TruthOrDare plugin) : base(
        "Truth Or Dare", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 630),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;
        Game = new Game(this);
        PlayerList = new PlayerList(this);
    }

    public void Initialize()
    {
        Game = new Game(this);
        PlayerList = new PlayerList(this);
    }

    public void Dispose()
    {
    }
    public void Close_Window(object? sender, System.EventArgs e) => IsOpen = false;
    
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
            case MainTab.Game:
                {
                    Game.DrawMatch();
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
        if (ImGui.BeginTabBar("TruthOrDareMainTabBar", ImGuiTabBarFlags.NoTooltip))
        {
            if (ImGui.BeginTabItem("Player List###TruthOrDare_PlayerList_MainTab"))
            {
                currentMainTab = MainTab.PlayerList;
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("Game###TruthOrDare_Match_MainTab"))
            {
                currentMainTab = MainTab.Game;
                ImGui.EndTabItem();
            }

            if (ImGui.BeginTabItem("About###TruthOrDare_About_MainTab"))
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
        ImGui.TextWrapped("Thanks to Amazing Primu Pyon@Omega who's code I have shamelessly modified");
        ImGui.Separator();
        ImGui.Spacing();
        ImGui.TextWrapped("+ Use Add Party to add all players in your current party to the player list(Have to be nearby).");
        ImGui.TextWrapped("+ Use Add Target to add current target to the player list");
        ImGui.TextWrapped("+ Or type a name and add it manually");
        ImGui.Separator();
        ImGui.Spacing();
        ImGui.TextWrapped("1) Press New Round to begin a new round");
        ImGui.TextWrapped("2) Press Roll Next Player to roll the dice for the next player. (Has to be one at a time due to chat spam limits)");
        ImGui.TextWrapped("3) Once all players have been rolled (no longer have a roll of -1) click declare results");
        ImGui.Columns(1);
        ImGui.Separator();
        ImGuiHelpers.ScaledDummy(5);
        if (ImGui.Button("Close"))
        {
            IsOpen = false;
        }
    }
}
