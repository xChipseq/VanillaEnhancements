using BepInEx.Unity.IL2CPP;

namespace VanillaEnhancements;

public static class ModCompatibility
{
    public static bool IsToUR;
    public static bool IsTOR;
    public static bool IsStellar;

    public static bool ShouldTurnOffTracking;

    public static void Load()
    {
        IsToUR = IL2CPPChainloader.Instance.Plugins.ContainsKey("com.slushiegoose.townofus");
        IsTOR = IL2CPPChainloader.Instance.Plugins.ContainsKey("me.eisbison.theotherroles");
        IsStellar = IL2CPPChainloader.Instance.Plugins.ContainsKey("me.fluff.stellarroles");

        ShouldTurnOffTracking = IsToUR || IsTOR || IsStellar;
    }
}