using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Reactor;
using Reactor.Networking.Attributes;
using Reactor.Utilities;

namespace VanillaEnhancements;

[BepInPlugin(Id, "Vanilla Enhancements", VersionString)]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[ReactorModFlags(Reactor.Networking.ModFlags.None)]
public partial class VanillaEnhancementsPlugin : BasePlugin
{
    public const string Id = "com.chipseq.vanillaenhancements";
    public const string VersionString = "1.1.0";
    public static System.Version Version = System.Version.Parse(VersionString);

    public Harmony Harmony { get; } = new(Id);

    public override void Load()
    {
        ReactorCredits.Register("VanillaEnhancements", VersionString, false, ReactorCredits.AlwaysShow);

        ModConfig.Bind(Config);
        ModCompatibility.Load();
        PlayerMuting.Setup();
        Harmony.PatchAll();
    }
}
