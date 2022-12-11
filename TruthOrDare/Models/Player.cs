using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TruthOrDare.Models
{
        public class Player
        {
            public int ID;
            public string Name = "";
            public string Alias = "";
            public int TotalBet = 0;
            public int BetPerHand = 0;
            public int Winnings = 0;
            public int TotalWinnings = 0;
            public int Roll;


            private enum ChatNameDisplayTypes { FullName, SurnameAbbrv, ForenameAbbrv, Initials }
            private unsafe ChatNameDisplayTypes ChatNameDisplayType { get { return (ChatNameDisplayTypes)ConfigModule.Instance()->GetIntValue(ConfigOption.LogNameType); } }

            public Player()
            {

            }
            public Player(int id, string name = "", string alias = "")
            {
                ID = id;
                Name = name;
                Alias = alias;
            }

            public string GetAlias(NameMode nameMode)
            {
                return GetAlias(Name, nameMode);
            }

            public string GetAlias(string name, NameMode nameMode)
            {
                switch (nameMode)
                {
                    case NameMode.First:
                        return !name.Contains(' ') ? name : name.Substring(0, name.IndexOf(" ")).Trim();
                    case NameMode.Last:
                        return !name.Contains(' ') ? name : name.Substring(name.IndexOf(" ")).Trim();
                }

                return name;
            }

            public unsafe string GetNameFromDisplayType(string name)
            {
                if (name.Contains(' '))
                {
                    var displayType = ChatNameDisplayType;

                    if (displayType != ChatNameDisplayTypes.FullName)
                    {
                        string[] n = name.Split(' ');
                        switch (displayType)
                        {
                            case ChatNameDisplayTypes.ForenameAbbrv:
                                return $"{n[0].Substring(0, 1)}. {n[1]}";
                            case ChatNameDisplayTypes.SurnameAbbrv:
                                return $"{n[0]} {n[1].Substring(0, 1)}.";
                            case ChatNameDisplayTypes.Initials:
                                return $"{n[0].Substring(0, 1)}. {n[1].Substring(0, 1)}.";
                        }
                    }
                }

                return name;
            }
        }
    
}