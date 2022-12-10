using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Hooking;
using Dalamud.Logging;
using System;
using System.Collections.Generic;
using TruthOrDare.Models;
using TruthOrDare.Util;

namespace TruthOrDare
{
    public unsafe class PlayerManager : IDisposable
    {
        private const int GroupMemberOffset = 0x0CC8;
        private const int AllianceMemberOffset = 0x0E14;
        private const int AllianceSizeOffset = 0x0EB4;
        private const int GroupMemberSize = 0x20;
        private const int GroupMemberIdOffset = 0x18;

        private readonly Hook<UpdatePartyDelegate> _updatePartyHook;
        private IntPtr _hudPtr = IntPtr.Zero;

        public PlayerManager()
        {
            UpdateParty updateParty = new(TruthOrDare.SigScanner);
            _updatePartyHook = updateParty.CreateHook(UpdatePartyHook)!;
        }

        private readonly int[,] _idOffsets = {
            {
                GroupMemberOffset + 0 * GroupMemberSize + GroupMemberIdOffset,
                GroupMemberOffset + 1 * GroupMemberSize + GroupMemberIdOffset,
                GroupMemberOffset + 2 * GroupMemberSize + GroupMemberIdOffset,
                GroupMemberOffset + 3 * GroupMemberSize + GroupMemberIdOffset,
                GroupMemberOffset + 4 * GroupMemberSize + GroupMemberIdOffset,
                GroupMemberOffset + 5 * GroupMemberSize + GroupMemberIdOffset,
                GroupMemberOffset + 6 * GroupMemberSize + GroupMemberIdOffset,
                GroupMemberOffset + 7 * GroupMemberSize + GroupMemberIdOffset,

                AllianceMemberOffset + 0 * 4,
                AllianceMemberOffset + 1 * 4,
                AllianceMemberOffset + 2 * 4,
                AllianceMemberOffset + 3 * 4,
                AllianceMemberOffset + 4 * 4,
                AllianceMemberOffset + 5 * 4,
                AllianceMemberOffset + 6 * 4,
                AllianceMemberOffset + 7 * 4,
                AllianceMemberOffset + 8 * 4,
                AllianceMemberOffset + 9 * 4,
                AllianceMemberOffset + 10 * 4,
                AllianceMemberOffset + 11 * 4,
                AllianceMemberOffset + 12 * 4,
                AllianceMemberOffset + 13 * 4,
                AllianceMemberOffset + 14 * 4,
                AllianceMemberOffset + 15 * 4,
            },
            {
                GroupMemberOffset + 0 * GroupMemberSize + GroupMemberIdOffset,
                GroupMemberOffset + 1 * GroupMemberSize + GroupMemberIdOffset,
                GroupMemberOffset + 2 * GroupMemberSize + GroupMemberIdOffset,
                GroupMemberOffset + 3 * GroupMemberSize + GroupMemberIdOffset,

                AllianceMemberOffset + 0 * 4,
                AllianceMemberOffset + 1 * 4,
                AllianceMemberOffset + 2 * 4,
                AllianceMemberOffset + 3 * 4,
                AllianceMemberOffset + 8 * 4,
                AllianceMemberOffset + 9 * 4,
                AllianceMemberOffset + 10 * 4,
                AllianceMemberOffset + 11 * 4,
                AllianceMemberOffset + 16 * 4,
                AllianceMemberOffset + 17 * 4,
                AllianceMemberOffset + 18 * 4,
                AllianceMemberOffset + 19 * 4,
                AllianceMemberOffset + 24 * 4,
                AllianceMemberOffset + 25 * 4,
                AllianceMemberOffset + 26 * 4,
                AllianceMemberOffset + 27 * 4,
                AllianceMemberOffset + 32 * 4,
                AllianceMemberOffset + 33 * 4,
                AllianceMemberOffset + 34 * 4,
                AllianceMemberOffset + 35 * 4,
            },
        };

        public int GroupSize => *(int*)(_hudPtr + AllianceSizeOffset);
        public bool IsAlliance => GroupSize == 8;
        public bool IsPvP => GroupSize == 4;
        public bool IsGroup => GroupSize == 0;

        public void UpdateParty(ref List<Player> players, string dealerName, NameMode nameMode)
        {
            List<Player> partyMembers = new List<Player>();

            for (int i = 0; i < 200; i += 2)
            {
                var obj = TruthOrDare.Objects[i];
                if (obj is not PlayerCharacter player)
                {
                    continue;
                }

                uint playerId = player.ObjectId;
                if (FindGroupMemberById(playerId) != null && player.Name.TextValue != dealerName)
                {
                    Player newPlayer = new Player((int)playerId, player.Name.TextValue);
                    newPlayer.Alias = newPlayer.GetAlias(nameMode);
                    partyMembers.Add(newPlayer);
                }
            }

            foreach (Player player in players)
            {
                if (partyMembers.Find(x => x.ID == player.ID) == null)
                {
                    player.Name = "";
                }
            }
            players.RemoveAll(player => player.Name == "");

            foreach (Player partyMember in partyMembers)
            {
                if (players.Find(x => x.ID == partyMember.ID) == null)
                {
                    players.Add(partyMember);
                }
            }
        }

        public (int groupId, int indexId)? FindGroupMemberById(uint playerId)
        {
            if (_hudPtr == IntPtr.Zero) { return null; }

            int groupSize = GroupSize;
            int numGroups;
            if (groupSize == 0)
            {
                numGroups = 1;
                groupSize = 8;
            }
            else
            {
                numGroups = groupSize == 4 ? 6 : 3;
            }

            int count = numGroups * groupSize;
            int pvp = groupSize == 4 ? 1 : 0;
            for (int i = 0; i < count; i++)
            {
                uint id = *(uint*)(_hudPtr + _idOffsets[pvp, i]);
                if (id == playerId)
                {
                    return (i / groupSize, i % groupSize);
                }
            }

            return null;
        }

        private void UpdatePartyHook(IntPtr hudPtr)
        {
            _hudPtr = hudPtr;
            PluginLog.LogVerbose($"HUD Address 0x{_hudPtr.ToInt64():X16}");
            _updatePartyHook.Original(hudPtr);
            _updatePartyHook.Disable();
            _updatePartyHook.Dispose();
        }

        public void Dispose() => _updatePartyHook.Dispose();
    }
}
