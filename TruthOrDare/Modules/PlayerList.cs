using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Interface;
using Dalamud.IoC;
using Dalamud.Utility;
using TruthOrDare.Models;
using TruthOrDare.Windows;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Utility;

namespace TruthOrDare.Modules
{
    public class PlayerList
    {
        private readonly MainWindow MainWindow;
        public bool Enabled = false;
        public List<Player> Players;
        public Player newPlayer;
        public PlayerManager PlayerManager;
        public Player Dealer;

        public PlayerList(MainWindow mainWindow)
        {
            newPlayer = new Player();
            Players = new List<Player>()
            {
            };
            MainWindow = mainWindow;
            Initialize();
        }

        public void Initialize()
        {
            Dealer = new Player(0);
            Players = new List<Player>();
        }

        private void AddParty()
        {
            if (MainWindow.Config.ChatChannel == "/p" && MainWindow.Config.AutoParty)
            {
                if (MainWindow.Game != null && Players != null)
                {
                    if (PlayerManager == null)
                    {
                        PlayerManager = new PlayerManager();
                    }
                    if (TruthOrDare.ClientState.LocalPlayer != null)
                    {
                        PlayerManager.UpdateParty(ref Players, TruthOrDare.ClientState.LocalPlayer.Name.TextValue, MainWindow.Config.AutoNameMode);
                    }
                }
            }
        }

        private void AddPlayer()
        {
            if (string.IsNullOrEmpty(newPlayer.Name))
                return;

            if (Players.FirstOrDefault(p => p.Name.ToLower().Equals(newPlayer.Name.ToLower())) == null)
            {
                Player p = new Player()
                {
                    Name = newPlayer.Name,
                };
                p.Alias = p.GetAlias(NameMode.Both);
                Players.Add(p);
            }
        }

        private void AddTarget()
        {
            var target = TruthOrDare.ClientState.LocalPlayer.TargetObject;
            if (target.ObjectKind == Dalamud.Game.ClientState.Objects.Enums.ObjectKind.Player)
            {
                newPlayer.Name = target.Name.TextValue;
                AddPlayer();
            }
        }

        private void Clear()
        {
            newPlayer = new Player();
            Players.Clear();
        }

        public void DrawPlayerList()
        {
            if (string.IsNullOrEmpty(Dealer.Name) && TruthOrDare.ClientState != null)
            {
                Dealer.Name = TruthOrDare.ClientState.LocalPlayer.Name.TextValue;
                Dealer.Alias = Dealer.GetAlias(NameMode.Both);
            }

            if (ImGui.Button("Add Party"))
            {
                AddParty();
            }
            ImGui.SameLine();
            if (ImGui.Button("Add Target"))
            {
                AddTarget();
            }
            ImGui.SameLine();
            if (ImGui.Button("Clear Players"))
            {
                Clear();
            }
            DrawAddPlayer();

            ImGui.Text("Player List:");
            ImGui.SameLine();
            ImGui.Text(Players.Count.ToString());
            DrawPlayers();
        }

        private void DrawAddPlayer()
        {
            ImGui.Columns(2);
            ImGui.SetColumnWidth(0, 160 + 2 * ImGuiHelpers.GlobalScale); //Name
            ImGui.SetColumnWidth(1, 160 + 2 * ImGuiHelpers.GlobalScale); //Add

            ImGui.Separator();

            ImGui.Text("Name");
            ImGui.NextColumn();
            ImGui.Text("Add");
            ImGui.NextColumn();

            ImGui.Separator();

            ImGui.InputText($"###name", ref newPlayer.Name, 255);
            ImGui.NextColumn();

            if (ImGui.Button("Add"))
            {
                AddPlayer();
                MainWindow.Game.Initialize();
            }
            ImGui.NextColumn();

            ImGui.Separator();
            ImGui.Columns(1);
        }

        private void DrawPlayers()
        {
            ImGui.Columns(3);
            ImGui.SetColumnWidth(0, 160 + 3 * ImGuiHelpers.GlobalScale); //First name
            ImGui.SetColumnWidth(1, 160 + 3 * ImGuiHelpers.GlobalScale); //Remove
            ImGui.SetColumnWidth(2, 160 + 3 * ImGuiHelpers.GlobalScale); //Remove
            ImGui.Separator();

            ImGui.Text("Name");
            ImGui.NextColumn();
            ImGui.Text("Enable");
            ImGui.NextColumn();
            ImGui.Text("Remove");
            ImGui.NextColumn();

            ImGui.Separator();

            foreach (var player in Players)
            {
                ImGui.Text(player.Name);
                ImGui.NextColumn();
                ImGui.Checkbox($"Enable {player.Name}", ref player.enabled);
                ImGui.NextColumn();
                if (ImGui.Button($"Delete {player.Name}", new Vector2(160, 25)))
                {
                    RemovePlayer(player.Name);
                    MainWindow.Game.Initialize();
                };
                ImGui.NextColumn();
                ImGui.Separator();
            }

            ImGui.Columns(1);
        }

        private void RemovePlayer(string name)
        {
            var p = Players.Find(p => p.Name.ToLower().Equals(name.ToLower()));
            if (p != null)
            {
                Players.Remove(p);
            }
        }
    }
}
