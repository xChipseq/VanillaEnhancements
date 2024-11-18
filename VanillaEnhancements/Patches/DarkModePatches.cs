using System.Linq;
using HarmonyLib;
using Reactor.Utilities;
using TMPro;
using UnityEngine;

namespace VanillaEnhancements.Patches;

[HarmonyPatch()]
public static class DarkModePatches
{
    [HarmonyPatch(typeof(ChatBubble), nameof(ChatBubble.SetCosmetics))]
    private static class ChatBubblePatch
    {
        private static void Postfix(ChatBubble __instance)
        {
            if (!PluginSingleton<VanillaEnhancementsPlugin>.Instance.DarkMode.Value) return;

            __instance.Background.color = new UnityEngine.Color(0.2f, 0.2f, 0.2f);
            __instance.MaskArea.color = new UnityEngine.Color(0.1f, 0.1f, 0.1f);
            __instance.TextArea.color = new UnityEngine.Color(1, 1, 1);
            __instance.TextArea.outlineWidth = __instance.NameText.outlineWidth * 0.75f;
        }
    }

    [HarmonyPatch(typeof(ChatController), nameof(ChatController.Update))]
    private static class ChatControllerPatch
    {
        private static void Postfix(ChatController __instance)
        {
            if (!PluginSingleton<VanillaEnhancementsPlugin>.Instance.DarkMode.Value) return;

            __instance.backgroundImage.color = new UnityEngine.Color(0.2f, 0.2f, 0.2f);

            __instance.chatButton.inactiveSprites.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            __instance.chatButton.activeSprites.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            __instance.chatButton.selectedSprites.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            __instance.chatButton.transform.FindChild("Background").GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    [HarmonyPatch(typeof(FreeChatInputField), nameof(FreeChatInputField.Awake))]
    private static class FreeChatInputField_Awake
    {
        private static void Postfix(FreeChatInputField __instance)
        {
            if (!PluginSingleton<VanillaEnhancementsPlugin>.Instance.DarkMode.Value) return;

            __instance.background.color = new UnityEngine.Color(0.2f, 0.2f, 0.2f);

            var comp = __instance.background.GetComponent<ButtonRolloverHandler>();
            comp.OutColor = new UnityEngine.Color(0.15f, 0.15f, 0.15f);
            comp.OverColor = new UnityEngine.Color(0.25f, 0.25f, 0.25f);
            comp.UnselectedColor = new UnityEngine.Color(0.15f, 0.15f, 0.15f);
            __instance.textArea.gameObject.GetComponent<TextMeshPro>().color = UnityEngine.Color.white;
        }
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    private static class MeetingHud_Start
    {
        private static void Postfix(MeetingHud __instance)
        {
            if (!PluginSingleton<VanillaEnhancementsPlugin>.Instance.DarkMode.Value) return;
            __instance.meetingContents.transform.FindChild("PhoneUI").FindChild("baseColor").GetComponent<SpriteRenderer>().color = new Color(0.01f, 0.01f, 0.01f);
            __instance.Glass.color = new Color(0.7f, 0.7f, 0.7f, 0.3f);
            __instance.SkipVoteButton.GetComponent<SpriteRenderer>().color = new Color(0.4f, 0.4f, 0.4f);

            foreach (SpriteRenderer playerMaterialColors in __instance.PlayerColoredParts)
            {
                playerMaterialColors.color = new Color(0.25f, 0.25f, 0.25f);
                PlayerMaterial.SetColors(7, playerMaterialColors);
            }
        }
    }

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    private static class HudManager_Update
    {
        private static void Postfix(HudManager __instance)
        {
            if (!PluginSingleton<VanillaEnhancementsPlugin>.Instance.DarkMode.Value) return;
            __instance.MapButton.inactiveSprites.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            __instance.MapButton.activeSprites.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            __instance.MapButton.selectedSprites.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            __instance.MapButton.transform.FindChild("Background").GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);

            __instance.SettingsButton.GetComponent<PassiveButton>().inactiveSprites.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            __instance.SettingsButton.GetComponent<PassiveButton>().activeSprites.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            __instance.SettingsButton.GetComponent<PassiveButton>().selectedSprites.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            __instance.SettingsButton.transform.FindChild("Background").GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    [HarmonyPatch(typeof(ProgressTracker), nameof(ProgressTracker.FixedUpdate))]
    private static class ProgressTracker_FixedUpdate
    {
        private static void Postfix(ProgressTracker __instance)
        {
            if (!PluginSingleton<VanillaEnhancementsPlugin>.Instance.DarkMode.Value) return;
            
            __instance.transform.FindChild("Background").GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f);
        }
    }

    [HarmonyPatch(typeof(FriendsListButton), nameof(FriendsListButton.Awake))]
    private static class FriendsListButton_Awake
    {
        private static void Postfix(FriendsListButton __instance)
        {
            if (!PluginSingleton<VanillaEnhancementsPlugin>.Instance.DarkMode.Value) return;
            __instance.Button.transform.FindChild("Inactive").GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            __instance.Button.transform.FindChild("Active").GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            __instance.Button.transform.FindChild("Selected").GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            __instance.Button.transform.FindChild("background").GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
            __instance.Button.transform.FindChild("NotifCount").GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f);
        }
    }

    [HarmonyPatch(typeof(LobbyInfoPane), nameof(LobbyInfoPane.Awake))]
    private static class LobbyInfoPane_Awake
    {
        private static void Postfix(LobbyInfoPane __instance)
        {
            if (!PluginSingleton<VanillaEnhancementsPlugin>.Instance.DarkMode.Value) return;
            __instance.InfoPaneBackground.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f);
        }
    }
}