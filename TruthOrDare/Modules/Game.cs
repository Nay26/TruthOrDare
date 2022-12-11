using Dalamud.Game.Text;
using Dalamud.Interface;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using TruthOrDare.Extensions;
using TruthOrDare.Models;
using TruthOrDare.Windows;

namespace TruthOrDare.Modules
{
    public class Game
    {
        private readonly MainWindow MainWindow;
        private KeyValuePair<Player, Player> pair;
        private List<Player> players; //list of all players
        private static Random rng = new Random();
        private bool printMatches;
        string debug = "not recieved";
        int LastRoll = 0;
        private Player CurrentPlayer;
        private int currentPlayerNumber;
        private List<string> QueuedMessages = new List<string>();

        private Event CurrentEvent = Event.RoundStarted;
        private enum Event { RoundStarted, PlayersRolled, WinnerDeclared }

        public Game(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            Initialize();
        }

        public void Initialize()
        {
            pair = new KeyValuePair<Player, Player>();
        }

        private void RollPlayers()
        {
            if (currentPlayerNumber < players.Count())
            {
                CurrentPlayer = players[currentPlayerNumber];
                SendRoll();
            }
        }

        private void FindWinner()
        {
            var highestPlayer = new Player();
            var lowestPlayer = new Player();
            lowestPlayer.Roll = 1000;
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
            pair = new KeyValuePair<Player, Player>(highestPlayer, lowestPlayer);
            SendMessage($"{FormatMessage(MainWindow.Config.TruthOrDareConfig.Messages["Loser"], lowestPlayer)}");
            SendMessage($"{FormatMessage(MainWindow.Config.TruthOrDareConfig.Messages["Winner"], highestPlayer)}");
        }

        public void CheckPlayerRoll(string sender, string message)
        {
            try
            {
                LastRoll = int.Parse(MainWindow.Config.RollCommand == "/dice" ? message.Replace("Random! (1-999) ", "") : Regex.Replace(message, ".*You roll a ([^\\(]+)\\(.*", "$1", RegexOptions.Singleline).Trim());
                CurrentPlayer.Roll = LastRoll;
                SendMessage($"{FormatMessage(MainWindow.Config.TruthOrDareConfig.Messages["PlayerRolled"], CurrentPlayer)}");
                currentPlayerNumber++;
                if (currentPlayerNumber == players.Count && CurrentEvent == Event.RoundStarted)
                {
                    CurrentEvent = Event.PlayersRolled;
                }
            }
            catch { }
        }

        public void DrawMatch()
        {
            ImGui.Text(CurrentEvent.ToString());
            if (ImGui.Button("New Round"))
            {
                CurrentEvent = Event.RoundStarted;
                players = new List<Player>(MainWindow.PlayerList.Players);
                currentPlayerNumber = 0;
                foreach (var player in players)
                {
                    player.Roll = -1;
                }
                players.Shuffle();

                SendMessage($"{FormatMessage(MainWindow.Config.TruthOrDareConfig.Messages["RoundStart"], MainWindow.PlayerList.Dealer)}");
            }
            ImGui.SameLine();
            if (ImGui.Button("Roll Next Player"))
            {
                if (CurrentEvent == Event.RoundStarted)
                {
                    RollPlayers();
                }

            }
            ImGui.SameLine();
            if (ImGui.Button("Declare Results"))
            {
                if (CurrentEvent == Event.PlayersRolled)
                {
                    FindWinner();
                    players = players.OrderByDescending(p => p.Roll).ToList();
                }
            }
            DrawResults();
            if (QueuedMessages.Count > 0)
            {
                TruthOrDare.XivCommon.Functions.Chat.SendMessage(QueuedMessages[0]);
                QueuedMessages.RemoveAt(0);
            }
            DrawPlayers();
        }    

        private void DrawResults()
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
            if (pair.Key != null)
            {

                ImGui.Text($"{pair.Key.Name}");
                ImGui.NextColumn();
                ImGui.Text($"{pair.Value.Name}");
                ImGui.NextColumn();
            }
            ImGui.Separator();
            ImGui.Spacing();
            ImGui.Spacing();
        }

        private void DrawPlayers()
        {
            ImGui.Separator();
            ImGui.Text($"Name");
            ImGui.NextColumn();
            ImGui.Text($"Roll");
            ImGui.NextColumn();
            ImGui.Separator();
            foreach (var player in players)
            {
                ImGui.Text($"{player.Name}");
                ImGui.NextColumn();
                ImGui.Text($"{player.Roll}");
                ImGui.NextColumn();
                ImGui.Separator();
            }
        }

        private void SendMessageToQueue(string message, MessageType messageType)
        {

            if (!string.IsNullOrWhiteSpace(message))
            {
                if (messageType == MessageType.Normal)
                {
                    QueuedMessages.Add($"{(MainWindow.Config.Debug ? "/echo" : "/p")} {message}");
                }
                else if (messageType == MessageType.TruthOrDareRoll)
                {
                    int.TryParse(message, out int num);
                    QueuedMessages.Add($"{MainWindow.Config.RollCommand} {(num == 0 ? MainWindow.Config.TruthOrDareConfig.MaxRoll : num)}");
                }
            }
        }

        public void OnChatMessage(XivChatType type, uint senderId, ref Dalamud.Game.Text.SeStringHandling.SeString sender, ref Dalamud.Game.Text.SeStringHandling.SeString message, ref bool isHandled)
        {

            if (isHandled) { return; }

            ReceivedMessage(sender.TextValue, message.TextValue);
        }

        private void ReceivedMessage(string sender, string message)
        {
            CheckPlayerRoll(sender, message);
        }


        private string FormatMessage(string message, Player player)
        {
            debug = "Here" + message + player;
            if (string.IsNullOrWhiteSpace(message)) { return ""; }

            return message.Replace("#dealer#", player.Alias)
                .Replace("#player#", player.Alias)
                .Replace("#roll#", player.Roll.ToString());
        }

        private void SendMessage(string message)
        {
            debug = "here" + message;
            SendMessageToQueue(message, MessageType.Normal);

        }

        private void SendRoll()
        {
            SendMessageToQueue(MainWindow.Config.TruthOrDareConfig.MaxRoll.ToString(), MessageType.TruthOrDareRoll);
        }
    }
}
