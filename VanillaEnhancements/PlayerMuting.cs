using System.Collections.Generic;

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
        if (!MutedPlayers.Contains(player.FriendCode)) MutedPlayers.Add(player.FriendCode);
    }
    public static void MutePlayer(string code)
    {
        if (!MutedPlayers.Contains(code)) MutedPlayers.Add(code);
    }

    public static void UnmutePlayer(this PlayerControl player)
    {
        if (MutedPlayers.Contains(player.FriendCode)) MutedPlayers.Remove(player.FriendCode);
    }
    public static void UnmutePlayer(string code)
    {
        if (MutedPlayers.Contains(code)) MutedPlayers.Remove(code);
    }

    public static bool IsMuted(this PlayerControl player)
    {
        return MutedPlayers.Contains(player.FriendCode);
    }
    public static bool IsMuted(string code)
    {
        return MutedPlayers.Contains(code);
    }
}