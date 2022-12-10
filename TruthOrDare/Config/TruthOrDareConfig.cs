using System.Collections.Generic;

namespace TruthOrDare
{
    public enum DealerStandMode { None, _16, _17 }

    public class TruthOrDareConfig
    {
        public int MaxRoll { get; set; } = 1000;

        public Dictionary<string, string> Messages_Rules = new Dictionary<string, string>() {
            { "Title", " Blackjack Rules " },
            { "BetLimit", "》Bet Limits: #minbet# ~ #maxbet#" },
            { "NormWin", "》Normal Win: #nmulti#x Payout" },
            { "BJWin_True", "》Blackjack Win: #bjmulti#x Payout (Only Natural Blackjack)" },
            { "BJWin_False", "》Blackjack Win: #bjmulti#x Payout (Any hand valued 21)" },
            { "DealerStand_True", "》Dealer stands on #stand#" },
            { "DealerStand_False", "》Dealer plays without stand limitation" },
            { "DoubleMustHit_True", "》Double must hit, limited to 1 hit" },
            { "DoubleMustHit_False", "》Double can stand/hit, limited to 1 hit" },
            { "AllowSplit_True", "》Split available on first pair." },
            { "AllowSplit_False", "》Split unavailable." },
            { "PushAllowBet_True", "》Pushed bet can add to bet" },
            { "PushAllowBet_False", "》Pushed bet can't add to bet" },
            { "PushAllowDouble_True", "》Pushed bet can double down" },
            { "PushAllowDouble_False", "》Pushed bet can't double down" }
        };

        public Dictionary<string, string> Messages = new Dictionary<string, string>() {
            { "PlaceBets", " Round Starting  Place your bets!  #minbet# ~ #maxbet#  <se.12>" },
            { "BetsPlaced", "Bets have been placed. Good luck!! ^^ <se.12>" },

            { "PlayerBet", " #player#  bets #bet#!" },
            { "PlayerBetPushed", " #player#  bets with push of #bet#!" },
            { "PlayerBetDouble", " #player#  doubles their bet to #bet#!! ＜Stand/Hit＞" },
            { "PlayerBetSplitDouble", " #player#  doubles their bet to #betperhanddouble# for this hand!! ＜Stand/Hit＞" },
            { "PlayerBetSplit", " #player#  splits their hand, #betperhand# bet per hand!" },
            { "PlayerBetDoubleMustHit", " #player#  doubles their bet to #bet#!! ＜Hit!＞" },
            { "PlayerBetSplitDoubleMustHit", " #player#  doubles their bet to #betperhanddouble# for this hand!! ＜Hit!＞" },
            { "PlayerDraw2", " #player#  draws 2 cards: #cards# (#value#)" },
            { "PlayerDraw2Blackjack", " #player#  draws 2 cards: #cards# (#value#) ★BLACKJACK★ <se.7>" },
            { "PlayerDraw2Split", " #player#  draws 2nd card for hand #handindex#: #cards# (#value#)" },
            { "PlayerDraw2SplitBlackjack", " #player#  draws 2nd card for hand #handindex#: #cards# (#value#) ★BLACKJACK★ <se.7>" },
            { "PlayerStandHit", " #player#  Hand: #cards# (#value#) ＜Stand/Hit＞" },
            { "PlayerStandHitDouble", " #player#  Hand: #cards# (#value#) ＜Stand/Hit/Double＞" },
            { "PlayerStandHitSplit", " #player#  Hand: #cards# (#value#) ＜Stand/Hit/Split＞" },
            { "PlayerStandHitDoubleSplit", " #player#  Hand: #cards# (#value#) ＜Stand/Hit/Double/Split＞" },
            { "PlayerStand", " #player#  decides to stand!" },
            { "PlayerHitUnder21", " #player#  Hand: #cards# (#value#) ＜Stand/Hit＞" },
            { "PlayerHitUnder21Doubled", " #player#  Hand: #cards# (#value#) ＜Stand!＞" },
            { "PlayerHit21", " #player#  Hand: #cards# (#value#) ★BLACKJACK★ <se.7>" },
            { "PlayerHitOver21", " #player#  Hand: #cards# (#value#) BUST ;w; <se.11>" },

            { "DealerDraw1", " #dealer#  reveals 1st card: #cards#  (#value#)" },
            { "DealerDraw2UnderStand", " #dealer#  reveals 2nd card: #cards# (#value#) 《#stand# ＜Hit!＞" },
            { "DealerDraw2Stand", " #dealer#  reveals 2nd card: #cards# (#value#) 》#stand# ＜Stand!＞" },
            { "DealerDraw2NoStandReq", " #dealer#  reveals 2nd card: #cards# (#value#) ＜Stand/Hit＞" },
            { "DealerDraw2Blackjack", " #dealer#  reveals 2nd card: #cards# (#value#) ★BLACKJACK★ <se.4>" },

            { "DealerHitUnderStand", " #dealer#  Hand: #cards# (#value#) 《#stand# ＜Hit!＞" },
            { "DealerHitStand", " #dealer#  Hand: #cards# (#value#) 》#stand# ＜Stand!＞" },
            { "DealerHitNoStandReq", " #dealer#  Hand: #cards# (#value#) ＜Stand/Hit＞" },
            { "DealerHit21", " #dealer#  Hand: #cards# (#value#) ★BLACKJACK★ <se.4>" },
            { "DealerOver21", " #dealer#  Hand: #cards# (#value#) BUST ;w; <se.11>" },

            { "Win", " #player#  wins with Hand: #cards# (#value#) #winnings#!! <se.15>" },
            { "WinSplit1", " #player#  wins with Hand: #cards# (#value#) #winnings#!! <se.15>" },
            { "WinSplit2", " #player#  wins with Hands: #cards# (#value#) #winnings#!! <se.15>" },
            { "Loss", "" },
            { "Draw", " #player#  matched dealer's Hand: #cards# (#value#) #bet# ＜Push/Refund＞" },
            { "DrawSplit1", " #player#  matched dealer's Hand: #cards# (#value#) #betdrawsplit# ＜Refund＞" },
            { "DrawSplit2", " #player#  matched dealer's Hand x2: #cards# (#value#) #bet# ＜Refund＞" },
            { "NoWinners", " #dealer#  wins this round, sorry! ^^ <se.11>" }
        };
    }
}
