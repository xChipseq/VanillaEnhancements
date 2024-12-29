using AmongUs.Data;
using HarmonyLib;
using Reactor.Utilities;
using UnityEngine;

namespace VanillaEnhancements.Patches;

[HarmonyPatch()]
public static class ChatPatches
{
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.Awake))]
    private static class ChatController_Awake
    {
        private static void Postfix(ChatController __instance)
        {
            // Removes the quick chat button
            __instance.quickChatButton.gameObject.SetActive(!ModConfig.NoQuickChat.Value);
        }
    }

    [HarmonyPatch(typeof(ChatController), nameof(ChatController.AddChat))]
    private static class ChatController_AddChat
    {
        private static bool Prefix(ChatController __instance, ref PlayerControl sourcePlayer, ref string chatText)
        {
            if (chatText.StartsWith("%^muting") || chatText.StartsWith("%^command") || chatText.StartsWith("%^error")) return true;
            if (sourcePlayer.IsMuted())
            {
                VELogger.Info($"ChatPatches :: Player {sourcePlayer.Data.PlayerName} is muted, not showing the message");
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
    private static class ChatController_SendChat
    {
        private static bool Prefix(ChatController __instance)
        {
            float num = ModConfig.DisableChatCooldown.Value ? 0f : 3f - __instance.timeSinceLastMessage;
            string freeChatText = __instance.freeChatField.Text;

            if (num > 0f && !freeChatText.StartsWith("/"))
            {
                __instance.sendRateMessageText.gameObject.SetActive(true);
                __instance.sendRateMessageText.text = $"Too fast. Wait {Mathf.CeilToInt(num)} second(s)";
                return false;
            }
            if (__instance.quickChatMenu.CanSend)
            {
                __instance.SendQuickChat();
            }
            else
            {
                if (__instance.quickChatMenu.IsOpen || string.IsNullOrWhiteSpace(__instance.freeChatField.Text) || DataManager.Settings.Multiplayer.ChatMode != InnerNet.QuickChatModes.FreeChatOrQuickChat)
                {
                    return false;
                }
                __instance.SendFreeChat();
            }
            __instance.timeSinceLastMessage = 0f;
            __instance.freeChatField.Clear();
            __instance.quickChatMenu.Clear();
            __instance.quickChatField.Clear();
            __instance.UpdateChatMode();

            return false;
        }
    }
}