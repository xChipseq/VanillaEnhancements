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
            if (!PluginSingleton<VanillaEnhancementsPlugin>.Instance.TaskTracking.Value || !AmongUsClient.Instance.IsGameStarted) return;

            if (__instance.AmOwner || PlayerControl.LocalPlayer.Data.IsDead)
            {
                if (!__instance.AmOwner && PluginSingleton<VanillaEnhancementsPlugin>.Instance.TaskTrackingOptions.Value == TaskTrackingOptionsEnum.Local) return;
                if (__instance.AmOwner && PluginSingleton<VanillaEnhancementsPlugin>.Instance.TaskTrackingOptions.Value == TaskTrackingOptionsEnum.EveryoneButLocal) return;

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
            if (!PluginSingleton<VanillaEnhancementsPlugin>.Instance.TaskTracking.Value || !AmongUsClient.Instance.IsGameStarted) return;

            var player = playerInfo.Object;

            if (player.AmOwner || PlayerControl.LocalPlayer.Data.IsDead)
            {
                var totalTasks = player.Data.Tasks.Count;
                var tasksDone = player.Data.Tasks.ToArray().Where(x => x.Complete).Count();

                __instance.NameText.text = $"{player.Data.PlayerName} ({tasksDone}/{totalTasks})";
            }
        }
    }
}