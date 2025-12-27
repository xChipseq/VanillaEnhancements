using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Reactor.Utilities;
using UnityEngine;

namespace VanillaEnhancements.Patches;

[HarmonyPatch()]
public static class ChatCommands
{
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
    [HarmonyPriority(Priority.First)]
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
                        VELogger.Info($"Chat Commands :: Muting {foundPlayer.Data.PlayerName}");
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
                        VELogger.Info($"Chat Commands :: Unuting {foundPlayer.Data.PlayerName}");
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
            if (text.ToLower().StartsWith("/muted"))
            {
                if (PlayerMuting.MutedPlayers.Count == 0)
                {
                    __instance.AddChat(PlayerControl.LocalPlayer, "There are no muted players", false);
                    return false;
                }
                
                string fullString = "%^command Muted players:\n";
                Dictionary<PlayerControl, byte> all = new();
                foreach (var code in PlayerMuting.MutedPlayers)
                {
                    var foundPlayerByCode = AmongUsClient.Instance.allClients.ToArray().FirstOrDefault(c => c.FriendCode == code);
                    all.Add(foundPlayerByCode.Character, foundPlayerByCode.Character.PlayerId);
                }

                all = all.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
                foreach (var player in all)
                {
                    fullString += $"{player.Key.Data.PlayerName} - {player.Value}\n";
                }

                __instance.AddChat(PlayerControl.LocalPlayer, fullString, false);
                VELogger.Info($"Chat Commands :: Showing a list of all muted players");

                return false;
            }

            // Host commands
            if (text.ToLower().StartsWith("/kick "))
            {
                if (GameManager.Instance.GameHasStarted)
                {
                    return false;
                }
                if (!AmongUsClient.Instance.CanKick())
                {
                    __instance.AddChat(PlayerControl.LocalPlayer, "%^error Only the host can kick players", false);
                    return false;
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
                        InnerNet.ClientData client = AmongUsClient.Instance.GetClient(foundPlayer.PlayerId);
                        AmongUsClient.Instance.KickPlayer(client.Id, false);
                        __instance.AddChat(PlayerControl.LocalPlayer, $"%^command {foundPlayer.Data.PlayerName} has been kicked from the lobby", false);
                        VELogger.Info($"Chat Commands :: {foundPlayer.Data.PlayerName} has been kicked");
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
                if (GameManager.Instance.GameHasStarted)
                {
                    return false;
                }
                if (!AmongUsClient.Instance.CanBan())
                {
                    __instance.AddChat(PlayerControl.LocalPlayer, "%^error Only the host can ban players", false);
                    return false;
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
                        InnerNet.ClientData client = AmongUsClient.Instance.GetClient(foundPlayer.PlayerId);
                        AmongUsClient.Instance.KickPlayer(client.Id, true);
                        __instance.AddChat(PlayerControl.LocalPlayer, $"%^command {foundPlayer.Data.PlayerName} has been banned from the lobby", false);
                        VELogger.Info($"Chat Commands :: {foundPlayer.Data.PlayerName} has been banned");
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
                Dictionary<PlayerControl, byte> all = new();
                foreach (var player in AmongUsClient.Instance.allClients)
                {
                    all.Add(player.Character, player.Character.PlayerId);
                }

                all = all.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
                foreach (var player in all)
                {
                    fullString += $"{player.Key.Data.PlayerName} {(player.Key.AmOwner ? "(You) " : "")}- {player.Value}\n";
                }

                __instance.AddChat(PlayerControl.LocalPlayer, fullString, false);
                VELogger.Info($"Chat Commands :: Showing a list of all player ids");

                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(ChatBubble), nameof(ChatBubble.SetText))]
    [HarmonyPriority(Priority.First)]
    private static class ChatBubble_SetText
    {
        private static bool Prefix(ChatBubble __instance, ref string chatText)
        {
            string modifiedText = chatText;
            if (chatText.StartsWith("%^error "))
            {
                modifiedText = chatText[8..];
                if (!ModConfig.DarkMode.Value)
                    __instance.Background.color = new UnityEngine.Color(1f, 0.3f, 0.3f);
                else
                    __instance.Background.color = new UnityEngine.Color(0.6f, 0.2f, 0.2f);
            }
            else if (chatText.StartsWith("%^command "))
            {
                modifiedText = chatText[10..];
                if (!ModConfig.DarkMode.Value)
                    __instance.Background.color = new UnityEngine.Color(0.2f, 0.2f, 1f);
                else
                    __instance.Background.color = new UnityEngine.Color(0.2f, 0.2f, 0.5f);
            }
            else if (chatText.StartsWith("%^muting "))
            {
                modifiedText = chatText[9..];
                if (!ModConfig.DarkMode.Value)
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