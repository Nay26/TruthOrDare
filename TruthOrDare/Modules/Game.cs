using Dalamud.Interface;
using Dalamud.Utility;
using TruthOrDare.Extensions;
using TruthOrDare.Models;
using TruthOrDare.Windows;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using static Lumina.Data.Parsing.Layer.LayerCommon;

namespace TruthOrDare.Modules
{
    public class Game
    {
        private readonly MainWindow MainWindow;
        public bool Enabled = false;
        private KeyValuePair<Player,Player> pair;
        private List<Player> players; //list of all players
        private Configuration Config;
        private static Random rng = new Random();
        private bool printMatches;
        //private XivCommon xivCommon;

        public Game(MainWindow mainWindow)
        {          
            MainWindow = mainWindow;
            Config = MainWindow.Config;
            Initialize();
        }

        public void Dispose()
        { 
        }

        public void Initialize()
        {
            pair = new KeyValuePair<Player, Player>();
        }

        public void DrawMatch()
        {      
            ImGui.Checkbox("Chat Results", ref printMatches);
            if (ImGui.Button("New Round"))
            {
                players = new List<Player>(MainWindow.PlayerList.players);
                players.Shuffle();               
                MatchPlayers();
                if (printMatches)
                {
                    PrintMatches();
                }
            }
            ImGui.SameLine();
            DrawMatches();
        }

        private void PrintMatches()
        {
            //XIVCommon.Functions.Chat.SendMessage("hollli");
        }

        private void MatchPlayers()
        {
            var highestPlayer = new Player();
            var lowestPlayer = new Player();
            lowestPlayer.Roll = 1000;
            foreach (var player in players)
            {
                player.Roll = rng.Next(1000);
            }
            foreach (var player in players)
            {
                if (player.Roll >= highestPlayer.Roll)
                {
                    highestPlayer = player;
                }
                if (player.Roll <= lowestPlayer.Roll)
                {
                    lowestPlayer = player;
                }
            }
            pair = new KeyValuePair<Player,Player>(highestPlayer, lowestPlayer);

        }
     

        private void DrawMatches()
        {
            ImGui.Columns(2);
            ImGui.SetColumnWidth(0, 350 + 5 * ImGuiHelpers.GlobalScale); //First name
            ImGui.SetColumnWidth(1, 500 + 5 * ImGuiHelpers.GlobalScale); //Second Name

            ImGui.Separator();

            ImGui.Text("Winner");
            ImGui.NextColumn();
            ImGui.Text("Loser");
            ImGui.NextColumn();   

            ImGui.Separator();

            ImGui.Text($"{pair.Key.FirstName} {pair.Key.SecondName}");
            ImGui.NextColumn();
            ImGui.Text($"{pair.Value.FirstName} {pair.Value.SecondName}");
            ImGui.NextColumn();
            ImGui.Separator();
            ImGui.Separator(); 
            ImGui.Separator();
            players = players.OrderByDescending(p => p.Roll).ToList();

            foreach (var player in players)
            {
                ImGui.Text($"{player.FirstName} {player.SecondName}");
                ImGui.NextColumn();
                ImGui.Text($"{player.Roll}");
                ImGui.NextColumn();
                ImGui.Separator();
            }
        }
    }
    
}
