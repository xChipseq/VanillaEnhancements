using System;
using AmongUs.Data;
using HarmonyLib;
using Reactor.Utilities;
using UnityEngine;

namespace VanillaEnhancements.Patches;

[HarmonyPatch()]
public static class MeetingPatches
{
    [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.SetCosmetics))]
    [HarmonyPriority(Priority.Last)]
    private static class PlayerVoteArea_SetCosmetics
    {
        private static void Postfix(PlayerVoteArea __instance, ref NetworkedPlayerInfo playerInfo)
        {
            if (!ModConfig.DisableNameplates.Value) __instance.Background.sprite = ShipStatus.Instance.CosmeticsCache.GetNameplate(playerInfo.DefaultOutfit.NamePlateId).Image;
            if (ModConfig.DarkMode.Value && ModConfig.DisableNameplates.Value) __instance.Background.color = new Color(0.1f, 0.1f, 0.1f);
            __instance.NameText.text = playerInfo.PlayerName;
            __instance.LevelNumberText.transform.GetParent().gameObject.SetActive(!ModConfig.DisableLevelIndicators.Value);
        }
    }
}