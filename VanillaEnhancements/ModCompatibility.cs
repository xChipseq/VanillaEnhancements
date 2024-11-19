using BepInEx.Unity.IL2CPP;

namespace VanillaEnhancements;

internal static class ModCompatibility
{
    public static bool IsToUR { get; private set; }
    public static bool IsTOR { get; private set; }
    public static bool IsStellar { get; private set; }

    public static bool ShouldTurnOffTracking { get; private set; }

    public static void Load()
    {
        VELogger.Info($"Mod Compatibility Loading :: Checking for mods...");

        IsToUR = IL2CPPChainloader.Instance.Plugins.ContainsKey("com.slushiegoose.townofus");
        VELogger.Info($"Mod Compatibility Loading :: IsToUR: {IsToUR}");

        IsTOR = IL2CPPChainloader.Instance.Plugins.ContainsKey("me.eisbison.theotherroles");
        VELogger.Info($"Mod Compatibility Loading :: IsTOR: {IsTOR}");

        IsStellar = IL2CPPChainloader.Instance.Plugins.ContainsKey("me.fluff.stellarroles");
        VELogger.Info($"Mod Compatibility Loading :: IsStellar: {IsStellar}");

        ShouldTurnOffTracking = IsToUR || IsTOR || IsStellar;
        VELogger.Info($"Mod Compatibility Loading :: ShouldTurnOffTracking: {ShouldTurnOffTracking}");
        VELogger.Info($"Mod Compatibility Loading :: All checks completed");
    }
}