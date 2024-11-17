using System;
using System.Linq;
using HarmonyLib;
using Reactor.Utilities;
using UnityEngine;

namespace VanillaEnhancements.Patches;

[HarmonyPatch()]
public static class ChatCommands
{
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendFreeChat))]
    private static class ChatController_SendFreeChat
    {
        private static bool Prefix(ChatController __instance)
        {
            string text = __instance.freeChatField.Text;

            // Player muting
            if (text.ToLower().StartsWith("/mute "))
            {
                string argument = text[6..].ToLower();
                PlayerControl foundPlayer = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(p => p.Data.PlayerName.ToLower() == argument);
                if (!foundPlayer)
                {
                    try
                    {
                        foundPlayer = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(p => p.Data.PlayerId == Convert.ToByte(argument));
                    }
                    catch
                    {
                    }
                }

                if (foundPlayer)
                {
                    if (!foundPlayer.AmOwner)
                    {
                        foundPlayer.MutePlayer();
                        __instance.AddChat(PlayerControl.LocalPlayer, $"%^muting {foundPlayer.Data.PlayerName} has been muted");
                    }
                    else
                    {
                        __instance.AddChat(PlayerControl.LocalPlayer, $"%^error You can't mute yourself");
                    }
                }
                else
                {
                    __instance.AddChat(PlayerControl.LocalPlayer, $"%^error Could not find player \"{argument}\"");
                }

                return false;
            }
            if (text.ToLower().StartsWith("/unmute "))
            {
                string argument = text[8..].ToLower();
                PlayerControl foundPlayer = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(p => p.Data.PlayerName.ToLower() == argument);
                if (!foundPlayer)
                {
                    try
                    {
                        foundPlayer = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(p => p.Data.PlayerId == Convert.ToByte(argument));
                    }
                    catch
                    {
                    }
                }

                if (foundPlayer)
                {
                    if (foundPlayer.IsMuted())
                    {
                        foundPlayer.UnmutePlayer();
                        __instance.AddChat(PlayerControl.LocalPlayer, $"%^muting {foundPlayer.Data.PlayerName} has been unmuted");
                    }
                    else
                    {
                        __instance.AddChat(PlayerControl.LocalPlayer, $"%^error This player is not muted");
                    }
                }
                else
                {
                    __instance.AddChat(PlayerControl.LocalPlayer, $"%^error Could not find player \"{argument}\"");
                }

                return false;
            }

            // Host commands
            if (text.ToLower().StartsWith("/kick "))
            {
                if (!AmongUsClient.Instance.CanKick())
                {
                    __instance.AddChat(PlayerControl.LocalPlayer, "%^error Only the host can kick players", false);
                }

                string argument = text[6..].ToLower();
                PlayerControl foundPlayer = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(p => p.Data.PlayerName.ToLower() == argument);
                if (!foundPlayer)
                {
                    try
                    {
                        foundPlayer = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(p => p.Data.PlayerId == Convert.ToByte(argument));
                    }
                    catch
                    {
                    }
                }

                if (foundPlayer)
                {
                    if (foundPlayer != PlayerControl.LocalPlayer)
                    {
                        AmongUsClient.Instance.KickPlayer(foundPlayer.Data.PlayerId, false);
                        __instance.AddChat(PlayerControl.LocalPlayer, $"%^command {foundPlayer.Data.PlayerName} has been kicked from the lobby", false);
                    }
                    else
                    {
                        __instance.AddChat(PlayerControl.LocalPlayer, $"%^error You can't kick yourself", false);
                    }
                }
                else
                {
                    __instance.AddChat(PlayerControl.LocalPlayer, $"%^error Could not find player \"{argument}\"", false);
                }

                return false;
            }
            if (text.ToLower().StartsWith("/ban "))
            {
                if (!AmongUsClient.Instance.CanBan())
                {
                    __instance.AddChat(PlayerControl.LocalPlayer, "%^error Only the host can ban players", false);
                }

                string argument = text[5..].ToLower();
                PlayerControl foundPlayer = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(p => p.Data.PlayerName.ToLower() == argument);
                if (!foundPlayer)
                {
                    try
                    {
                        foundPlayer = PlayerControl.AllPlayerControls.ToArray().FirstOrDefault(p => p.Data.PlayerId == Convert.ToByte(argument));
                    }
                    catch
                    {
                    }
                }

                if (foundPlayer)
                {
                    if (foundPlayer != PlayerControl.LocalPlayer)
                    {
                        AmongUsClient.Instance.KickPlayer(foundPlayer.Data.PlayerId, true);
                        __instance.AddChat(PlayerControl.LocalPlayer, $"%^command {foundPlayer.Data.PlayerName} has been banned from the lobby", false);
                    }
                    else
                    {
                        __instance.AddChat(PlayerControl.LocalPlayer, $"%^error You can't ban yourself", false);
                    }
                }
                else
                {
                    __instance.AddChat(PlayerControl.LocalPlayer, $"%^error Could not find player \"{argument}\"", false);
                }

                return false;
            }

            if (text.ToLower().StartsWith("/ids"))
            {
                string fullString = "%^command All player ids:\n";
                foreach (var player in AmongUsClient.Instance.allClients)
                {
                    fullString += $"{player.PlayerName} {(player.Character.AmOwner ? "(You) " : "")}- {player.Character.PlayerId}\n";
                }
                __instance.AddChat(PlayerControl.LocalPlayer, fullString, false);

                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(ChatBubble), nameof(ChatBubble.SetText))]
    private static class ChatBubble_SetText
    {
        private static bool Prefix(ChatBubble __instance, ref string chatText)
        {
            string modifiedText = chatText;
            if (chatText.StartsWith("%^error "))
            {
                modifiedText = chatText[8..];
                if (!PluginSingleton<VanillaEnhancementsPlugin>.Instance.DarkMode.Value)
                    __instance.Background.color = new UnityEngine.Color(1f, 0.3f, 0.3f);
                else
                    __instance.Background.color = new UnityEngine.Color(0.6f, 0.2f, 0.2f);
            }
            else if (chatText.StartsWith("%^command "))
            {
                modifiedText = chatText[10..];
                if (!PluginSingleton<VanillaEnhancementsPlugin>.Instance.DarkMode.Value)
                    __instance.Background.color = new UnityEngine.Color(0.2f, 0.2f, 1f);
                else
                    __instance.Background.color = new UnityEngine.Color(0.2f, 0.2f, 0.5f);
            }
            else if (chatText.StartsWith("%^muting "))
            {
                modifiedText = chatText[9..];
                if (!PluginSingleton<VanillaEnhancementsPlugin>.Instance.DarkMode.Value)
                    __instance.Background.color = new UnityEngine.Color(0.8f, 0.8f, 0.8f);
                else
                    __instance.Background.color = new UnityEngine.Color(0.4f, 0.4f, 0.4f);
            }

            __instance.TextArea.text = modifiedText;
            __instance.TextArea.ForceMeshUpdate(true, true);
            __instance.Background.size = new Vector2(5.52f, 0.2f + __instance.NameText.GetNotDumbRenderedHeight() + __instance.TextArea.GetNotDumbRenderedHeight());
            __instance.MaskArea.size = __instance.Background.size - new Vector2(0f, 0.03f);
            return false;
        }
    }
}