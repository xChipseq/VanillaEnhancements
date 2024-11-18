using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Reactor;
using Reactor.Networking.Attributes;
using Reactor.Utilities;

namespace VanillaEnhancements;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[ReactorModFlags(Reactor.Networking.ModFlags.None)]
public partial class VanillaEnhancementsPlugin : BasePlugin
{
    public Harmony Harmony { get; } = new(Id);

    // Config
    // General
    public ConfigEntry<bool> DarkMode { get; private set; }

    // Meeting
    public ConfigEntry<bool> DisableNameplates { get; private set; }
    public ConfigEntry<bool> DisableLevelIndicators { get; private set; }

    // Chat
    public ConfigEntry<bool> EnablePlayerMuting { get; private set; }
    public ConfigEntry<bool> DisableChatCooldown { get; private set; }

    // Music
    public ConfigEntry<bool> DisableMenuMusic { get; private set; }
    public ConfigEntry<bool> DisableHNSMusic { get; private set; }
    public ConfigEntry<bool> DisableLobbyMusic { get; private set; }

    // Task tracking
    public ConfigEntry<bool> TaskTracking { get; private set; }
    public ConfigEntry<TaskTrackingOptionsEnum> TaskTrackingOptions { get; private set; }

    // Role tracking
    public ConfigEntry<bool> ShowTeam { get; private set; }
    public ConfigEntry<bool> ShowRole { get; private set; }

    public override void Load()
    {
        // config binding
        DarkMode = Config.Bind("General", "DarkMode", true, "Give your eyes some rest");

        DisableNameplates = Config.Bind("Meeting", "DisableNameplates", false, "Makes everyone's nameplate the default one\nAlso they are all black when dark mode is enabled");
        DisableLevelIndicators = Config.Bind("Meeting", "DisableLevelIndicators", false, "Removes the level indicators");

        EnablePlayerMuting = Config.Bind("Chat", "PlayerMuting", true, "Enables player muting\nYou can mute any player to make their messages not be shown for you");
        DisableChatCooldown = Config.Bind("Chat", "DisableChatCooldown", false, "Disables the chat cooldown\nPlease note that you can get banned for this so do not spam");

        DisableMenuMusic = Config.Bind("Music", "DisableMenuMusic", false);
        DisableHNSMusic = Config.Bind("Music", "DisableHNSMusic", false);
        DisableLobbyMusic = Config.Bind("Music", "DisableLobbyMusic", false);

        TaskTracking = Config.Bind("Task Tracking", "TaskTracking", true, "Enables task tracking\nYou will see how many tasks are done next to your name");
        TaskTrackingOptions = Config.Bind("Task Tracking", "TaskTrackingOptions", TaskTrackingOptionsEnum.Everyone, "Local: Tracks only your tasks\nEveryoneButLocal: Tracks tasks of every player besides you\nEveryone: Tracks tasks of everyone\nRemember that you can see other's tasks only when dead");

        ShowTeam = Config.Bind("Role Tracking", "ShowTeam", true, "Shows you the team of every player when you die");
        ShowRole = Config.Bind("Role Tracking", "ShowRole", true, "Shows you the role of every player when you die\nShowTeam needs to be turned on");
        
        PlayerMuting.Setup();
        ReactorCredits.Register("VanillaEnhancements", "1.0.2", false, ReactorCredits.AlwaysShow);
        Harmony.PatchAll();
    }
}

public enum TaskTrackingOptionsEnum
{
    Local,
    EveryoneButLocal,
    Everyone
}
