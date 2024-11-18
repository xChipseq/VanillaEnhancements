using AmongUs.Data;
using HarmonyLib;
using Reactor.Utilities;
using UnityEngine;

namespace VanillaEnhancements.Patches;

[HarmonyPatch()]
public static class SoundManagerPatches
{
    [HarmonyPatch(typeof(SoundManager), nameof(SoundManager.CrossFadeSound))]
    private static class SoundManager_CrossFadeSound
    {
        private static bool Prefix(SoundManager __instance, ref string name)
        {
            if ((name == "MapTheme" && ModConfig.DisableLobbyMusic.Value) || (name == "MainBG" && ModConfig.DisableMenuMusic.Value) || (name.StartsWith("HnS_Music") && ModConfig.DisableHNSMusic.Value))
            {
                return false;
            }
            return true;
        }
    }
}