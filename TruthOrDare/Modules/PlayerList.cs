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

namespace TruthOrDare.Modules
{
    public class PlayerList
    {
        private readonly MainWindow MainWindow;
        public bool Enabled = false;
        public List<Player> players;
        public Player newPlayer;

        private Configuration Config;

        public PlayerList(MainWindow mainWindow)
        {
            newPlayer = new Player();
            players = new List<Player>()
            {
            };
            MainWindow = mainWindow;
            Config = MainWindow.Config;
            Initialize();
        }

        public void Dispose()
        { 
        }

        public void Initialize()
        {
          
        }

        public void DrawPlayerList()
        {
            ImGui.Text("Add Player:");
            ImGui.SameLine();
            if (ImGui.Button("Add Target")) {
                AddTarget();
            }
            ImGui.SameLine();
            if (ImGui.Button("Clear Input"))
            {
                Clear();
            }
            DrawAddPlayer();

            ImGui.Text("Player List:");
            ImGui.SameLine();
            ImGui.Text(players.Count.ToString());
            DrawPlayers();
        }

        private void DrawAddPlayer()
        {
            ImGui.Columns(3);
            ImGui.SetColumnWidth(0, 90 + 5 * ImGuiHelpers.GlobalScale); //First name
            ImGui.SetColumnWidth(1, 90 + 5 * ImGuiHelpers.GlobalScale); //Second Name
            ImGui.SetColumnWidth(2, 140 + 5 * ImGuiHelpers.GlobalScale); //Gender
            ImGui.SetColumnWidth(5, 50 + 5 * ImGuiHelpers.GlobalScale); //Add

            ImGui.Separator();

            ImGui.Text("First name");
            ImGui.NextColumn();
            ImGui.Text("Second name");
            ImGui.NextColumn();
            ImGui.Text("Add");
            ImGui.NextColumn();

            ImGui.Separator();

            ImGui.InputText($"###firstName", ref newPlayer.FirstName, 255);
            ImGui.NextColumn();
            ImGui.InputText($"###secondName", ref newPlayer.SecondName, 255);
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
            ImGui.SetColumnWidth(0, 90 + 5 * ImGuiHelpers.GlobalScale); //First name
            ImGui.SetColumnWidth(1, 90 + 5 * ImGuiHelpers.GlobalScale); //Second Name
            ImGui.SetColumnWidth(5, 50 + 5 * ImGuiHelpers.GlobalScale); //Remove

            ImGui.Separator();

            ImGui.Text("First name");
            ImGui.NextColumn();
            ImGui.Text("Second name");
            ImGui.NextColumn();
            ImGui.Text("Remove");
            ImGui.NextColumn();

            ImGui.Separator();

            foreach (var player in players)
            {
                ImGui.Text(player.FirstName);
                ImGui.NextColumn();
                ImGui.Text(player.SecondName);
                ImGui.NextColumn();
                if (ImGui.Button("Delete", new Vector2(40, 25)))
                {                  
                    players.Remove(player);
                    MainWindow.Game.Initialize();
                };
                ImGui.NextColumn();
                ImGui.Separator();
            }

           
            ImGui.Columns(1);
        }
        private void AddPlayer()
        {
            if (string.IsNullOrEmpty(newPlayer.FirstName) && string.IsNullOrEmpty(newPlayer.FirstName))
                return;

            if (players.FirstOrDefault(p => p.FirstName.ToLower().Equals(newPlayer.FirstName.ToLower()) && p.SecondName.ToLower().Equals(newPlayer.SecondName.ToLower())) == null)
            {
                Player p = new Player()
                {
                    FirstName = newPlayer.FirstName,
                    SecondName = newPlayer.SecondName,
                };
                players.Add(p);
            }
        }

        private void AddTarget()
        {
            var target = TruthOrDare.ClientState.LocalPlayer.TargetObject;
            if (target.ObjectKind == Dalamud.Game.ClientState.Objects.Enums.ObjectKind.Player)
            {          
                newPlayer.FirstName = target.Name.TextValue.Split(" ").First();
                newPlayer.SecondName = target.Name.TextValue.Split(" ").Last();
            }                  
        }

        private void Clear()
        {
            newPlayer = new Player();
        }
    }
}
