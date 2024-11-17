using System.Linq;
using HarmonyLib;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using TMPro;
using UnityEngine;

namespace VanillaEnhancements.Patches;

[HarmonyPatch()]
public static class RoleTrackingPatches
{
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    private static class HudManager_Update
    {
        private static void Postfix(HudManager __instance)
        {
            if (LobbyBehaviour.Instance) return;
            if (GameManager.Instance.IsHideAndSeek()) return;
            if (!PluginSingleton<VanillaEnhancementsPlugin>.Instance.ShowTeam.Value) return;

            if (PlayerControl.LocalPlayer.Data.IsDead)
            {
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player.AmOwner) continue;

                    player.cosmetics.nameText.color = Color.white;
                    if (player.cosmetics.showColorBlindText == true)
                    {
                        player.cosmetics.colorBlindText.gameObject.transform.localPosition = new Vector3(0, -1.1f, 0);
                        player.cosmetics.colorBlindText.text = $"<size=70%>{player.cosmetics.GetColorBlindText()}</size>";
                    }

                    TextMeshPro roleText = null;
                    if (!player.gameObject.transform.FindChild("Names").FindChild("RoleText_TMP"))
                    {
                        var text = GameObject.Instantiate(player.gameObject.transform.FindChild("Names").FindChild("ColorblindName_TMP"));
                        var tmp = text.GetComponent<TextMeshPro>();
                        text.gameObject.transform.SetParent(player.gameObject.transform.FindChild("Names").transform);
                        text.gameObject.name = "RoleText_TMP";
                        text.gameObject.transform.localPosition = Vector3.zero;
                        text.transform.localPosition = new Vector3(0f, -0.25f, 0);
                        tmp.text = "Role";

                        roleText = tmp;
                    }

                    if (roleText != null)
                    {
                        if (PluginSingleton<VanillaEnhancementsPlugin>.Instance.ShowRole.Value)
                        {
                            player.cosmetics.nameText.gameObject.transform.localPosition = Vector3.zero;
                            roleText.gameObject.active = !player.inVent;
                            roleText.text = $"<size=75%><color=#{player.Data.Role.TeamColor.ToHtmlStringRGBA()}>{player.Data.Role.NiceName}</color></size>";
                        }
                        else
                        {
                            player.cosmetics.nameText.gameObject.transform.localPosition = Vector3.zero;
                            roleText.gameObject.active = !player.inVent;
                            roleText.text = $"<size=75%><color=#{player.Data.Role.TeamColor.ToHtmlStringRGBA()}>{player.Data.Role.TeamType}</color></size>";
                        }
                    }
                }
            }
            else
            {
                foreach (var player in PlayerControl.AllPlayerControls)
                {
                    if (player.AmOwner) continue;

                    try
                    {
                        player.gameObject.transform.FindChild("Names").FindChild("RoleText_TMP").gameObject.active = false;
                        player.cosmetics.nameText.gameObject.transform.localPosition = new Vector3(0, -0.15f, 0);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(PlayerVoteArea), nameof(PlayerVoteArea.SetCosmetics))]
    private static class PlayerVoteArea_SetCosmetics
    {
        private static void Postfix(PlayerVoteArea __instance, ref NetworkedPlayerInfo playerInfo)
        {
            if (!PluginSingleton<VanillaEnhancementsPlugin>.Instance.ShowTeam.Value) return;
            if (!PlayerControl.LocalPlayer.Data.IsDead) return;

            var player = playerInfo.Object;
            var role = player.Data.Role;

            if (player)
            {
                var roleText = GameObject.Instantiate(__instance.ColorBlindName, __instance.ColorBlindName.transform.parent);
                roleText.name = "RoleText";

                if (PluginSingleton<VanillaEnhancementsPlugin>.Instance.ShowRole.Value)
                {
                    roleText.text = $"<size=75%><color=#{role.TeamColor.ToHtmlStringRGBA()}>{role.NiceName}</color></size>";
                }
                else
                {
                    roleText.text = $"<size=75%><color=#{role.TeamColor.ToHtmlStringRGBA()}>{role.TeamType}</color></size>";
                }

                __instance.ColorBlindName.transform.localPosition = __instance.PlayerIcon.transform.localPosition;
                __instance.ColorBlindName.transform.SetWorldZ(-12.9f);
            }
        }
    }
}