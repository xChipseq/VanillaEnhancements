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
        VELogger.Info($"PlayerMuting.MutePlayer() :: Muting {player.Data.PlayerName} ({player.Data.PlayerId})");
        if (!MutedPlayers.Contains(player.Data.PlayerName))
            MutedPlayers.Add(player.Data.PlayerName);
    }
    public static void MutePlayer(string name)
    {
        VELogger.Info($"PlayerMuting.MutePlayer() :: Unmuting player with name {name}");
        if (!MutedPlayers.Contains(name))
            MutedPlayers.Add(name);
    }

    public static void UnmutePlayer(this PlayerControl player)
    {
        VELogger.Info($"PlayerMuting.UnmutePlayer() :: Unmuting {player.Data.PlayerName} ({player.Data.PlayerId})");
        if (MutedPlayers.Contains(player.Data.PlayerName))
            MutedPlayers.Remove(player.Data.PlayerName);
    }
    public static void UnmutePlayer(string name)
    {
        VELogger.Info($"PlayerMuting.UnmutePlayer() :: Unmuting player with name {name}");
        if (MutedPlayers.Contains(name))
            MutedPlayers.Remove(name);
    }

    public static bool IsMuted(this PlayerControl player)
    {
        bool isMuted = MutedPlayers.Contains(player.Data.PlayerName);
        VELogger.Info($"PlayerMuting.IsMuted() :: Player {player.Data.PlayerName} is {(isMuted ? "muted" : "not muted")} ({isMuted})");
        return isMuted;
    }
    public static bool IsMuted(string name)
    {
        bool isMuted = MutedPlayers.Contains(name);
        VELogger.Info($"PlayerMuting.IsMuted() :: Player with name {name} is {(isMuted ? "muted" : "not muted")} ({isMuted})");
        return isMuted;
    }
}