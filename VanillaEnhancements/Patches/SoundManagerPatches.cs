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
            if ((name == "MapTheme" && PluginSingleton<VanillaEnhancementsPlugin>.Instance.DisableLobbyMusic.Value) || (name == "MainBG" && PluginSingleton<VanillaEnhancementsPlugin>.Instance.DisableMenuMusic.Value) || (name.StartsWith("HnS_Music") && PluginSingleton<VanillaEnhancementsPlugin>.Instance.DisableHNSMusic.Value))
            {
                return false;
            }
            return true;
        }
    }
}