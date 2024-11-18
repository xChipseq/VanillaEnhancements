using System.Collections.Generic;
using Reactor.Utilities;

namespace VanillaEnhancements;

public static class PlayerMuting
{
    public static List<string> MutedPlayers;

    public static void Setup()
    {
        MutedPlayers = new();
    }

    public static void MutePlayer(this PlayerControl player)
    {
        Logger<VanillaEnhancementsPlugin>.Info($"muting {player.Data.PlayerName} ({player.Data.PlayerId}) - {player.Data.FriendCode}");
        if (!MutedPlayers.Contains(player.Data.FriendCode)) MutedPlayers.Add(player.Data.FriendCode);
    }
    public static void MutePlayer(string code)
    {
        if (!MutedPlayers.Contains(code)) MutedPlayers.Add(code);
    }

    public static void UnmutePlayer(this PlayerControl player)
    {
        Logger<VanillaEnhancementsPlugin>.Info($"unmuting {player.Data.PlayerName} ({player.Data.PlayerId}) - {player.FriendCode}");
        if (MutedPlayers.Contains(player.Data.FriendCode)) MutedPlayers.Remove(player.Data.FriendCode);
    }
    public static void UnmutePlayer(string code)
    {
        if (MutedPlayers.Contains(code)) MutedPlayers.Remove(code);
    }

    public static bool IsMuted(this PlayerControl player)
    {
        return MutedPlayers.Contains(player.Data.FriendCode);
    }
    public static bool IsMuted(string code)
    {
        return MutedPlayers.Contains(code);
    }
}