using BepInEx.Configuration;

namespace VanillaEnhancements;

internal static class ModConfig
{
    public const string GeneralSection = "General";
    public const string MeetingSection = "Meeting";
    public const string ChatSection = "Chat";
    public const string MusicSection = "Music";
    public const string TaskTrackingSection = "Task Tracking";
    public const string RoleTrackingSection = "Role Tracking";

    // General
    public static ConfigEntry<bool> DarkMode { get; private set; }

    // Meeting
    public static ConfigEntry<bool> DisableNameplates { get; private set; }
    public static ConfigEntry<bool> DisableLevelIndicators { get; private set; }

    // Chat
    public static ConfigEntry<bool> EnablePlayerMuting { get; private set; }
    public static ConfigEntry<bool> DisableChatCooldown { get; private set; }

    // Music
    public static ConfigEntry<bool> DisableMenuMusic { get; private set; }
    public static ConfigEntry<bool> DisableHNSMusic { get; private set; }
    public static ConfigEntry<bool> DisableLobbyMusic { get; private set; }

    // Task tracking
    public static ConfigEntry<bool> TaskTracking { get; private set; }
    public static ConfigEntry<TaskTrackingOptionsEnum> TaskTrackingOptions { get; private set; }

    // Role tracking
    public static ConfigEntry<bool> ShowTeam { get; private set; }
    public static ConfigEntry<bool> ShowRole { get; private set; }


    public static void Bind(ConfigFile config)
    {
        DarkMode = config.Bind(GeneralSection, "DarkMode", true, "Give your eyes some rest");

        DisableNameplates = config.Bind(MeetingSection, "DisableNameplates", false, "Makes everyone's nameplate the default one\nAlso they are all black when dark mode is enabled");
        DisableLevelIndicators = config.Bind(MeetingSection, "DisableLevelIndicators", false, "Removes the level indicators");

        EnablePlayerMuting = config.Bind(ChatSection, "PlayerMuting", true, "Enables player muting\nYou can mute any player to make their messages not be shown for you");
        DisableChatCooldown = config.Bind(ChatSection, "DisableChatCooldown", false, "Disables the chat cooldown\nPlease note that you can get banned for this so do not spam");

        DisableMenuMusic = config.Bind(MusicSection, "DisableMenuMusic", false);
        DisableHNSMusic = config.Bind(MusicSection, "DisableHNSMusic", false);
        DisableLobbyMusic = config.Bind(MusicSection, "DisableLobbyMusic", false);

        TaskTracking = config.Bind(TaskTrackingSection, "TaskTracking", true, "Enables task tracking\nYou will see how many tasks are done next to your name");
        TaskTrackingOptions = config.Bind(TaskTrackingSection, "TaskTrackingOptions", TaskTrackingOptionsEnum.Everyone, "Local: Tracks only your tasks\nEveryoneButLocal: Tracks tasks of every player besides you\nEveryone: Tracks tasks of everyone\nRemember that you can see other's tasks only when dead");

        ShowTeam = config.Bind(RoleTrackingSection, "ShowTeam", true, "Shows you the team of every player when you die");
        ShowRole = config.Bind(RoleTrackingSection, "ShowRole", true, "Shows you the role of every player when you die\nShowTeam needs to be turned on");
    }

    public enum TaskTrackingOptionsEnum
    {
        Local,
        EveryoneButLocal,
        Everyone
    }
}