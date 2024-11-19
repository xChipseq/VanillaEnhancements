using System.Linq;
using HarmonyLib;
using Reactor.Utilities;

namespace VanillaEnhancements.Patches;

[HarmonyPatch()]
public static class TaskTrackingPatches
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    private static class PlayerControlPatch
    {
        private static void Postfix(PlayerControl __instance)
        {
            if (ModCompatibility.ShouldTurnOffTracking)
            {
                __instance.cosmetics.nameText.text = __instance.Data.PlayerName;
                VELogger.Info($"Mod Compatibility :: Task tracking is disabled, not adding the task text");
                return;
            } 
            if (!ModConfig.TaskTracking.Value || !AmongUsClient.Instance.IsGameStarted) return;

            if (__instance.AmOwner || PlayerControl.LocalPlayer.Data.IsDead)
            {
                if (!__instance.AmOwner && ModConfig.TaskTrackingOptions.Value == ModConfig.TaskTrackingOptionsEnum.Local) return;
                if (__instance.AmOwner && ModConfig.TaskTrackingOptions.Value == ModConfig.TaskTrackingOptionsEnum.EveryoneButLocal) return;
                if (__instance.Data.Role.TeamType == RoleTeamTypes.Impostor) return;
                if (PlayerControl.LocalPlayer.Data.Role.TeamType == RoleTeamTypes.Impostor && !PlayerControl.LocalPlayer.Data.IsDead) return;

                var totalTasks = __instance.Data.Tasks.Count;
                var tasksDone = __instance.Data.Tasks.ToArray().Where(x => x.Complete).Count();
                __instance.cosmetics.nameText.text = $"{__instance.Data.PlayerName} ({tasksDone}/{totalTasks})";
            }
        }
    }

    [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.SetCosmetics))]
    private static class PlayerVoteAreaPatch
    {
        private static void Postfix(PlayerVoteArea __instance, ref NetworkedPlayerInfo playerInfo)
        {
            if (ModCompatibility.ShouldTurnOffTracking)
            {
                __instance.NameText.text = playerInfo.PlayerName;
                VELogger.Info($"Mod Compatibility :: Task tracking is disabled, not adding the meeting task text");
                return;
            } 
            if (!ModConfig.TaskTracking.Value || !AmongUsClient.Instance.IsGameStarted) return;

            var player = playerInfo.Object;
            if (player.Data.Role.TeamType == RoleTeamTypes.Impostor) return;

            if (player.AmOwner || PlayerControl.LocalPlayer.Data.IsDead)
            {
                var totalTasks = player.Data.Tasks.Count;
                var tasksDone = player.Data.Tasks.ToArray().Where(x => x.Complete).Count();

                __instance.NameText.text = $"{player.Data.PlayerName} ({tasksDone}/{totalTasks})";
            }
        }
    }
}