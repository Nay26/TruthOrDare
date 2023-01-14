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
        private Player highestPlayer = new Player();
        private Player lowestPlayer = new Player();
        private Player rival1 = new Player();
        private Player rival2 = new Player();
        private int rival1Wins = 0;
        private int rival2Wins = 0;

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

            var lowroll = 1000;
            foreach (var player in players)
            {
                if (player.Roll >= highestPlayer.Roll)
                {
                    highestPlayer = player;
                }
                if (player.Roll <= lowroll)
                {
                    lowestPlayer = player;
                    lowroll = lowestPlayer.Roll;
                }
            }
            DoMatchCalculate();
            CalculateRivals();
            pair = new KeyValuePair<Player, Player>(highestPlayer, lowestPlayer);
            SendMessage($"{FormatMessage(MainWindow.Config.TruthOrDareConfig.Messages["WinMessage"], lowestPlayer)}");
            lowestPlayer.losses += 1;
            highestPlayer.wins += 1;
            
        }

        private void DoMatchCalculate()
        {
            if (!lowestPlayer.lossesToPlayer.ContainsKey(highestPlayer))
            {
                lowestPlayer.lossesToPlayer.Add(highestPlayer, 1);
            } else
            {
                lowestPlayer.lossesToPlayer[highestPlayer] += 1;
            }

            if (!highestPlayer.winsToPlayer.ContainsKey(lowestPlayer))
            {
                highestPlayer.winsToPlayer.Add(lowestPlayer, 1);
            }
            else
            {
                highestPlayer.winsToPlayer[lowestPlayer] += 1;
            }


            if (!lowestPlayer.winsToPlayer.ContainsKey(highestPlayer))
            {
                lowestPlayer.winsToPlayer.Add(highestPlayer, 0);
            }
            if (!highestPlayer.winsToPlayer.ContainsKey(lowestPlayer))
            {
                highestPlayer.winsToPlayer.Add(lowestPlayer, 0);
            }
        }

        private void CalculateRivals()
        {
            int highestWins = 0;
            foreach (var player in players)
            {
                foreach (var matchup in player.winsToPlayer)
                {
                    if (matchup.Value > highestWins)
                    {
                        highestWins = matchup.Value;
                        rival2 = matchup.Key;
                        rival1 = player;
                        rival1Wins = matchup.Value;
                        rival2Wins = rival2.winsToPlayer[rival1];
                    }             
                }
            }
        }

        public void CheckPlayerRoll(string sender, string message)
        {
            try
            {
                LastRoll = int.Parse(MainWindow.Config.RollCommand == "/dice" ? message.Replace("Random! (1-999) ", "") : Regex.Replace(message, ".*You roll a ([^\\(]+)\\(.*", "$1", RegexOptions.Singleline).Trim());
                if (CurrentPlayer.Roll == -1)
                {
                    CurrentPlayer.Roll = LastRoll;
                    if (CurrentPlayer.Roll == 69)
                    {
                        SendMessage($"{FormatMessage(MainWindow.Config.TruthOrDareConfig.Messages["PlayerRolled69"], CurrentPlayer)}");
                    }
                    else
                    {
                        SendMessage($"{FormatMessage(MainWindow.Config.TruthOrDareConfig.Messages["PlayerRolled"], CurrentPlayer)}");
                    }
                    currentPlayerNumber++;
                    if (currentPlayerNumber == players.Count && CurrentEvent == Event.RoundStarted)
                    {
                        CurrentEvent = Event.PlayersRolled;
                    }
                }             
            }
            catch { }
        }

        public void DrawMatch()
        {
            if (ImGui.Button("New Round"))
            {
                CurrentEvent = Event.RoundStarted;
                players = new List<Player>(MainWindow.PlayerList.Players.Where(p => p.enabled).ToList());
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
                    CurrentEvent= Event.WinnerDeclared;
                }
            }
            ImGui.SameLine();
            if (ImGui.Button("Declare Stats"))
            {
                SendMessage($"{FormatMessage(MainWindow.Config.TruthOrDareConfig.Messages["StatMostWins"], highestPlayer)}");
                SendMessage($"{FormatMessage(MainWindow.Config.TruthOrDareConfig.Messages["StatMostLosses"], lowestPlayer)}");
                SendMessage($"{FormatMessage(MainWindow.Config.TruthOrDareConfig.Messages["StatBiggestRivals"], lowestPlayer)}");
            }
            DrawResults();
            if (QueuedMessages.Count > 0)
            {
                TruthOrDare.Chat.SendMessage(QueuedMessages[0]);
                QueuedMessages.RemoveAt(0);
            }
            DrawPlayers();
        }    

        private void DrawResults()
        {
            ImGui.Columns(3);
            ImGui.SetColumnWidth(0, 150 + 3 * ImGuiHelpers.GlobalScale); //First name
            ImGui.SetColumnWidth(1, 150 + 3 * ImGuiHelpers.GlobalScale); //Second Name
            ImGui.SetColumnWidth(2, 150 + 3 * ImGuiHelpers.GlobalScale); //Second Name

            ImGui.Separator();

            ImGui.Text("Winner");
            ImGui.NextColumn();
            ImGui.Text("Loser");
            ImGui.NextColumn();
            ImGui.Text("Matchup");
            ImGui.NextColumn();
            ImGui.Separator();
            if (pair.Key != null)
            {

                ImGui.Text($"{pair.Key.Name}");
                ImGui.NextColumn();
                ImGui.Text($"{pair.Value.Name}");
                ImGui.NextColumn();
                ImGui.Text($"{pair.Key.winsToPlayer[pair.Value]} vs {pair.Value.winsToPlayer[pair.Key]}");
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
            ImGui.Text($"Win/Loss");
            ImGui.NextColumn();
            ImGui.Separator();
            foreach (var player in players)
            {
                ImGui.Text($"{player.Name}");
                ImGui.NextColumn();
                ImGui.Text($"{player.Roll}");
                ImGui.NextColumn();
                ImGui.Text($"{player.wins} / {player.losses}");
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
                .Replace("#roll#", player.Roll.ToString())
                .Replace("#winner#", highestPlayer.Name)
                .Replace("#loser#", lowestPlayer.Name)
                .Replace("#mostwins#", highestPlayer.wins.ToString())
                .Replace("#mostlosses#", lowestPlayer.losses.ToString())
                .Replace("#rival1#", rival1.Alias)
                .Replace("#rival2#", rival2.Alias)
                .Replace("#rival1Wins#", rival1Wins.ToString())
                .Replace("#rival2Wins#", rival2Wins.ToString()); 
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
