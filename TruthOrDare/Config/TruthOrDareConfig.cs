using System.Collections.Generic;

namespace TruthOrDare
{
    public class TruthOrDareConfig
    {
        public int MaxRoll { get; set; } = 999;

        public Dictionary<string, string> Messages = new Dictionary<string, string>() {
            { "RoundStart", " Starting a New Round!  <se.12>" },
            { "PlayerRolled", "\" #player#  Rolls a #roll#\" " },
            { "PlayerRolled69", "\" #player#  Rolls a #roll#. Nice! Now take some clothes off!\"" },
            { "PlayerRolled1", "\" #player#  Rolls a #roll#. What a total bottom! Now take some clothes off!\"" },
            { "PlayerRolled999", "\" #player#  Rolls a #roll#. Bow down to their magnificence! Now take some clothes off!\"" },
            { "PlayerRolled666", "\" #player#  Rolls a #roll#. The number of the beast! Now take some clothes off!\"" },
            { "PlayerRolled420", "\" #player#  Rolls a #roll#. Blaze it! Now take some clothes off!\"" },
            { "WinMessage", "\" #winner#  Asks  #loser#  Truth or Dare!\"" },
            { "StatMostWins", "\" #player#  has been working overtime with #mostwins# wins!\"" },
            { "StatMostLosses", "\" #player#  is the biggest bottom with #mostlosses# losses!\"" },
            { "StatBiggestRivals", "\" #rival1#  and  #rival2#  have been paired the most times! Current score #rival1Wins# : #rival2Wins#!\""},
        };
    }
}
