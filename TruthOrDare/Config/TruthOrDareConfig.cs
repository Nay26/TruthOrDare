using System.Collections.Generic;

namespace TruthOrDare
{
    public class TruthOrDareConfig
    {
        public int MaxRoll { get; set; } = 999;

        public Dictionary<string, string> Messages_Rules = new Dictionary<string, string>() {
            { "Title", " Blackjack Rules " },
        };

        public Dictionary<string, string> Messages = new Dictionary<string, string>() {
            { "RoundStart", "Round Starting! Good Luck! <se.12>" },
            { "PlayerRolled", "\" #player#  Rolls a #roll#\" }," },
            { "Winner", "\" #player#  Will ask them Truth or Dare!\" }," },
            { "Loser", "\" #player#  Has the lowest roll!\" }," },
        };
    }
}
