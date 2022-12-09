using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Interface;
using Dalamud.IoC;
using Dalamud.Utility;
using FFSpeedDate.Models;
using FFSpeedDate.Windows;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;

namespace FFSpeedDate.Modules
{
    public class PlayerList
    {
        private readonly MainWindow MainWindow;
        public bool Enabled = false;
        public List<Player> players;
        public Player newPlayer;
        private int gender;

        private Configuration Config;

        public PlayerList(MainWindow mainWindow)
        {
            newPlayer = new Player();
            players = new List<Player>()
            {
                new Player()
                {
                    FirstName = "name",
                    SecondName = "snarrme",
                    Gender = Gender.Male,
                    LikesMale = true,
                    LikesFemale= true,
                },
                new Player()
                {
                    FirstName = "ndame",
                    SecondName = "srrrrrrrrrname",
                    Gender = Gender.Male,
                    LikesMale = true,
                    LikesFemale= true,
                },
                new Player()
                {
                    FirstName = "ndame",
                    SecondName = "snamthxykrrrrre",
                    Gender = Gender.Female,
                    LikesMale = true,
                    LikesFemale= false,
                },
                new Player()
                {
                    FirstName = "ndame",
                    SecondName = "snadfthjxdftme",
                    Gender = Gender.Male,
                    LikesMale = true,
                    LikesFemale= true,
                },
                new Player()
                {
                    FirstName = "ndame",
                    SecondName = "snagjjfyjfye",
                    Gender = Gender.Female,
                    LikesMale = true,
                    LikesFemale= false,
                },
                new Player()
                {
                    FirstName = "ndamergdgdr",
                    SecondName = "sname",
                    Gender = Gender.Male,
                    LikesMale = true,
                    LikesFemale= true,
                },
                new Player()
                {
                    FirstName = "ndame",
                    SecondName = "snamfghfghfe",
                    Gender = Gender.Male,
                    LikesMale = false,
                    LikesFemale= true,
                },
                new Player()
                {
                    FirstName = "ndame",
                    SecondName = "snamhgfhfghfe",
                    Gender = Gender.Male,
                    LikesMale = true,
                    LikesFemale= true,
                },
                new Player()
                {
                    FirstName = "ndame",
                    SecondName = "snamwetwetsggeggwe",
                    Gender = Gender.Female,
                    LikesMale = false,
                    LikesFemale= true,
                },
                new Player()
                {
                    FirstName = "ndamewetwtetwet",
                    SecondName = "sname",
                    Gender = Gender.Male,
                    LikesMale = true,
                    LikesFemale= true,
                },
                new Player()
                {
                    FirstName = "ndamssssse",
                    SecondName = "sname",
                    Gender = Gender.Female,
                    LikesMale = true,
                    LikesFemale= true,
                },
                new Player()
                {
                    FirstName = "ndawetwetwetwme",
                    SecondName = "snassssme",
                    Gender = Gender.Female,
                    LikesMale = false,
                    LikesFemale= true,
                },
                new Player()
                {
                    FirstName = "ndamwetwete",
                    SecondName = "snssame",
                    Gender = Gender.Male,
                    LikesMale = true,
                    LikesFemale= false,
                },
                new Player()
                {
                    FirstName = "ndassssqasdame",
                    SecondName = "sname",
                    Gender = Gender.Male,
                    LikesMale = false,
                    LikesFemale= true,
                },
                new Player()
                {
                    FirstName = "ndaewefwfme",
                    SecondName = "sname",
                    Gender = Gender.Male,
                    LikesMale = true,
                    LikesFemale= true,
                },
                new Player()
                {
                    FirstName = "ndamtwetwetwe",
                    SecondName = "sname",
                    Gender = Gender.Female,
                    LikesMale = false,
                    LikesFemale= true,
                },
                new Player()
                {
                    FirstName = "ndwtwetwetame",
                    SecondName = "sname",
                    Gender = Gender.Male,
                    LikesMale = true,
                    LikesFemale= false,
                },
                new Player()
                {
                    FirstName = "ndwetwetwetame",
                    SecondName = "sname",
                    Gender = Gender.Female,
                    LikesMale = true,
                    LikesFemale= false,
                },
                new Player()
                {
                    FirstName = "ndwetwetwsssetame",
                    SecondName = "sname",
                    Gender = Gender.Female,
                    LikesMale = true,
                    LikesFemale= false,
                }
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
            ImGui.Columns(6);
            ImGui.SetColumnWidth(0, 90 + 5 * ImGuiHelpers.GlobalScale); //First name
            ImGui.SetColumnWidth(1, 90 + 5 * ImGuiHelpers.GlobalScale); //Second Name
            ImGui.SetColumnWidth(2, 140 + 5 * ImGuiHelpers.GlobalScale); //Gender
            ImGui.SetColumnWidth(3, 90 + 5 * ImGuiHelpers.GlobalScale); //Likes M?
            ImGui.SetColumnWidth(4, 90 + 5 * ImGuiHelpers.GlobalScale); //Likes F?
            ImGui.SetColumnWidth(5, 50 + 5 * ImGuiHelpers.GlobalScale); //Add

            ImGui.Separator();

            ImGui.Text("First name");
            ImGui.NextColumn();
            ImGui.Text("Second name");
            ImGui.NextColumn();
            ImGui.Text("Gender");
            ImGui.NextColumn();
            ImGui.Text("Likes M?");
            ImGui.NextColumn();
            ImGui.Text("Likes F?");
            ImGui.NextColumn();
            ImGui.Text("Add");
            ImGui.NextColumn();

            ImGui.Separator();

            ImGui.InputText($"###firstName", ref newPlayer.FirstName, 255);
            ImGui.NextColumn();
            ImGui.InputText($"###secondName", ref newPlayer.SecondName, 255);
            ImGui.NextColumn();
            ImGui.Combo($"###gender", ref gender, new string[] {Gender.Male.ToString(), Gender.Female.ToString()},2);
            ImGui.NextColumn();
            ImGui.Checkbox($"###likesMale", ref newPlayer.LikesMale);
            ImGui.NextColumn();
            ImGui.Checkbox($"###likesFemale", ref newPlayer.LikesFemale);
            ImGui.NextColumn();
            if (ImGui.Button("Add"))
            {
                AddPlayer();
                MainWindow.Match.Initialize();
            }
            ImGui.NextColumn();

            ImGui.Separator();
            ImGui.Columns(1);
        }

        private void DrawPlayers()
        {
            ImGui.Columns(6);
            ImGui.SetColumnWidth(0, 90 + 5 * ImGuiHelpers.GlobalScale); //First name
            ImGui.SetColumnWidth(1, 90 + 5 * ImGuiHelpers.GlobalScale); //Second Name
            ImGui.SetColumnWidth(2, 140 + 5 * ImGuiHelpers.GlobalScale); //Gender
            ImGui.SetColumnWidth(3, 90 + 5 * ImGuiHelpers.GlobalScale); //Likes M?
            ImGui.SetColumnWidth(4, 90 + 5 * ImGuiHelpers.GlobalScale); //Likes F?
            ImGui.SetColumnWidth(5, 50 + 5 * ImGuiHelpers.GlobalScale); //Remove

            ImGui.Separator();

            ImGui.Text("First name");
            ImGui.NextColumn();
            ImGui.Text("Second name");
            ImGui.NextColumn();
            ImGui.Text("Gender");
            ImGui.NextColumn();
            ImGui.Text("Likes M?");
            ImGui.NextColumn();
            ImGui.Text("Likes F?");
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
                ImGui.Text(player.Gender.ToString());
                ImGui.NextColumn();
                ImGui.Text(player.LikesMale ? "Yes" : "No");
                ImGui.NextColumn();
                ImGui.Text(player.LikesFemale ? "Yes" : "No");
                ImGui.NextColumn();
                if (ImGui.Button("Delete", new Vector2(40, 25)))
                {                  
                    players.Remove(player);
                    MainWindow.Match.Initialize();
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

            if (newPlayer.LikesMale == false && newPlayer.LikesFemale == false)
                return;

            if (players.FirstOrDefault(p => p.FirstName.ToLower().Equals(newPlayer.FirstName.ToLower()) && p.SecondName.ToLower().Equals(newPlayer.SecondName.ToLower())) == null)
            {
                newPlayer.Gender = (Gender)gender;
                Player p = new Player()
                {
                    FirstName = newPlayer.FirstName,
                    SecondName = newPlayer.SecondName,
                    Gender = newPlayer.Gender,
                    LikesFemale = newPlayer.LikesFemale,
                    LikesMale = newPlayer.LikesMale,
                };
                players.Add(p);
            }
        }

        private void AddTarget()
        {
            var target = FFSpeedDate.ClientState.LocalPlayer.TargetObject;
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
