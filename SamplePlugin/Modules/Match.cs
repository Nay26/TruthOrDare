using Dalamud.Interface;
using Dalamud.Utility;
using FFSpeedDate.Extensions;
using FFSpeedDate.Models;
using FFSpeedDate.Windows;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using static Lumina.Data.Parsing.Layer.LayerCommon;

namespace FFSpeedDate.Modules
{
    public class Match
    {
        private readonly MainWindow MainWindow;
        public bool Enabled = false;
        private List<KeyValuePair<Player,Player>> playerPairs;
        private List<Player> players; //list of all players
        private List<KeyValuePair<Player, List<Player>>> potentialMatchesForPlayer; //list of all players with potential matches
        private List<Player> noMatches;
        private Configuration Config;
        private static Random rng = new Random();
        private bool usePreferences;
        private bool printMatches;
        //private XivCommon xivCommon;

        public Match(MainWindow mainWindow)
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
            playerPairs = new List<KeyValuePair<Player, Player>>();
            noMatches = new List<Player>();
            potentialMatchesForPlayer = new List<KeyValuePair<Player, List<Player>>>();
        }

        public void DrawMatch()
        {
            ImGui.Text("Use Preferences?");
            ImGui.SameLine();
            ImGui.Checkbox("UsePreferences", ref usePreferences);
            ImGui.SameLine();
            ImGui.Checkbox("Chat Results", ref printMatches);
            if (ImGui.Button("New Matches"))
            {
                players = new List<Player>(MainWindow.PlayerList.players);
                players.Shuffle();
                CalculatePotentialMatches();
                if (usePreferences)
                {
                    MatchPlayers();
                } else
                {
                    RotateMatchPlayers();
                }
                if (printMatches)
                {
                    PrintMatches();
                }
            }
            ImGui.SameLine();
            if (ImGui.Button("Next Match Round"))
            {
                RemoveMadeMatches();
                if (usePreferences)
                {
                    MatchPlayers();
                } else
                {
                    RotateMatchPlayers();
                }
                if (printMatches)
                {
                    PrintMatches();
                }
                
            }
            DrawMatches();
        }

        private void PrintMatches()
        {
            //XIVCommon.Functions.Chat.SendMessage("hollli");
        }

        private void RemoveMadeMatches()
        {
            //remove from potentialMatchesForPlayer for each player
            foreach (var player in players)
            {
                if (!noMatches.Contains(player) && !string.IsNullOrEmpty(player.FirstName))
                {
                    var lastMatchPair = playerPairs.First(p => p.Value == player || p.Key == player);
                    if (lastMatchPair.Value == player)
                    {
                        potentialMatchesForPlayer.Find(p => p.Key == player).Value.Remove(lastMatchPair.Key);
                    }
                    else
                    {
                        potentialMatchesForPlayer.Find(p => p.Key == player).Value.Remove(lastMatchPair.Value);
                    }
                }
            }
        }

        private void CalculatePotentialMatches()
        {
            potentialMatchesForPlayer.Clear();
            if (usePreferences)
            {
                foreach (var player in players)
                {
                    var tempList = new List<Player>();
                    foreach (var potentialMatch in players)
                    {
                        if (player == potentialMatch)
                            continue;

                        switch (potentialMatch.Gender)
                        {
                            case Gender.Male:
                                if (player.LikesMale)
                                {
                                    switch (player.Gender)
                                    {
                                        case Gender.Male:
                                            if (potentialMatch.LikesMale)
                                            {
                                                tempList.Add(potentialMatch);
                                            }
                                            break;
                                        case Gender.Female:
                                            if (potentialMatch.LikesFemale)
                                            {
                                                tempList.Add(potentialMatch);
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case Gender.Female:
                                if (player.LikesFemale)
                                {
                                    switch (player.Gender)
                                    {
                                        case Gender.Male:
                                            if (potentialMatch.LikesMale)
                                            {
                                                tempList.Add(potentialMatch);
                                            }
                                            break;
                                        case Gender.Female:
                                            if (potentialMatch.LikesFemale)
                                            {
                                                tempList.Add(potentialMatch);
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    
                    }
                    potentialMatchesForPlayer.Add(new KeyValuePair<Player, List<Player>>(player, new List<Player>(tempList)));
                }
            }
            else
            {
                foreach (var player in players)
                {
                    var tempList = new List<Player>(players);
                    tempList.Remove(player);
                    potentialMatchesForPlayer.Add(new KeyValuePair<Player,List<Player>>(player,new List<Player>(tempList)));
                }
            }
            potentialMatchesForPlayer.Sort((x, y) => x.Value.Count.CompareTo(y.Value.Count));
        }

        private void RotateMatchPlayers()
        {
            playerPairs.Clear();
            noMatches.Clear();
            var temp = players[1];
            for (int i = 1; i < players.Count; i++)
            {
                if (i+1 < players.Count)
                {
                    players[i] = players[i + 1];
                }
                else
                {
                    players[i] = temp;
                }
            }

            var tempPlayer = new Player();

            if (!(players.Count % 2 == 0))
            {
                players.Add(tempPlayer);
            }

            for (int i = 0; i < (players.Count / 2); i++)
            {
                playerPairs.Add(new KeyValuePair<Player, Player>(players[i], players[players.Count - 1 - i]));
            }


            foreach (var pair in playerPairs)
            {
                if (string.IsNullOrEmpty(pair.Key.FirstName))
                {
                    noMatches.Add(pair.Value);
                } else if (string.IsNullOrEmpty(pair.Value.FirstName))
                {
                    noMatches.Add(pair.Key);
                }
            }
        }

        private void MatchPlayers()
        {
            var minNoMatches = players.Count;
            var bestPairs = new List<KeyValuePair<Player, Player>>();

            for (int i = 0; i < 1000; i++)
            {
                //order by potential matches
                playerPairs.Clear();
                noMatches.Clear();
                foreach (var playerMatchListPair in potentialMatchesForPlayer)
                {
                    var player = playerMatchListPair.Key;
                    var playerMatches = playerMatchListPair.Value;

                    bool matched = false;
                    if (playerMatches.Count > 0)
                    {
                        //if player already matched skip
                        if (playerPairs.Exists(p => (p.Value == player) || (p.Key == player)))
                        {
                            continue;
                        }

                        foreach (var potentialMatch in playerMatches)
                        {
                            //if potential match not matched
                            if (!(playerPairs.Exists(p => (p.Value == potentialMatch) || (p.Key == potentialMatch))))
                            {
                                playerPairs.Add(new KeyValuePair<Player, Player>(player, potentialMatch));
                                matched = true;
                                break;
                            }
                        }
                    }
                    if (!matched)
                    {
                        noMatches.Add(player);
                    }
                }

                if (noMatches.Count < minNoMatches)
                {
                    minNoMatches = noMatches.Count;
                    bestPairs = playerPairs;
                }
                if (minNoMatches == 0 )
                {
                    break;
                }
            }
            playerPairs = bestPairs;

        }

        private void SortPlayersByMatches()
        {
            potentialMatchesForPlayer.OrderBy(x => x.Value.Count).ToList();
        }

        private void DrawMatches()
        {
            ImGui.Columns(2);
            ImGui.SetColumnWidth(0, 350 + 5 * ImGuiHelpers.GlobalScale); //First name
            ImGui.SetColumnWidth(1, 500 + 5 * ImGuiHelpers.GlobalScale); //Second Name

            ImGui.Separator();

            ImGui.Text("Player 1");
            ImGui.NextColumn();
            ImGui.Text("Player 2");
            ImGui.NextColumn();   

            ImGui.Separator();

            foreach (var pair in playerPairs)
            {
                ImGui.Text($"{pair.Key.FirstName} {pair.Key.SecondName}");
                ImGui.NextColumn();
                ImGui.Text($"{pair.Value.FirstName} {pair.Value.SecondName}");
                ImGui.NextColumn();
                ImGui.Separator();
            }
            
            ImGui.Columns(1);
            ImGui.Separator();
            ImGui.Text("Couldn't find matches this round for:");
            foreach (var player in noMatches)
            {
                ImGui.Text($"{player.FirstName} {player.SecondName}");        
            }

            ImGui.Separator();
            ImGui.Text("Potential Matches:");
            ImGui.Columns(2);     
            foreach (var p in potentialMatchesForPlayer)
            {
                var player = p.Key;
                ImGui.Text($"{player.FirstName} {player.SecondName}");
                ImGui.NextColumn();
                foreach (var match in p.Value)
                {
                    ImGui.Text($"{match.FirstName} {match.SecondName}");
                }
                ImGui.NextColumn();
                ImGui.Separator();
            }
        }
    }
    
}
