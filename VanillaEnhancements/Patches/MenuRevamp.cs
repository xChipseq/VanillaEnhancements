using System;
using HarmonyLib;
using Reactor.Patches;
using TMPro;
using UnityEngine;

namespace VanillaEnhancements;

public static class MenuRevamp
{
    [HarmonyPatch(typeof(AccountManager), nameof(AccountManager.Awake))]
    public static class AccountManager_Awake
    {
        public static void Postfix(AccountManager __instance)
        {
            // hides the account manager thing at the top of the screen, nobody uses that
            __instance.gameObject.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.LateUpdate))]
    public static class MainMenuManager_Awake
    {
        public static void Postfix(MainMenuManager __instance)
        {
            var aspectScaler = __instance.mainMenuUI.transform.FindChild("AspectScaler");
            var leftPanel = aspectScaler.Find("LeftPanel").gameObject;
            var leftPanelButtons = __instance.mainButtons[0].transform.parent.gameObject;
            var rightPanel = aspectScaler.Find("RightPanel").gameObject;

            // Removes the background
            aspectScaler.FindChild("BackgroundTexture").gameObject.SetActive(false);
            // Centers the left panel
            leftPanel.GetComponent<AspectScaledAsset>().originalDistanceFromEdge = 2.5f;
            // Removes the left panel background
            leftPanel.GetComponent<SpriteRenderer>().color = Color.clear;
            // Removes the divider
            leftPanelButtons.transform.FindChild("Divider")?.gameObject.SetActive(false);

            // Removes the right panel's frame
            rightPanel.GetComponent<SpriteRenderer>().color = Color.clear;
            // Removes shine from right panel
            rightPanel.transform.FindChild("WindowShine").gameObject.SetActive(false);
            // Masked black screen changes
            var maskBlack = rightPanel.transform.FindChild("MaskedBlackScreen").gameObject;
            maskBlack.GetComponent<SpriteRenderer>().color = Color.clear;
            maskBlack.transform.SetWorldZ(-20);
            // Some tint changes
            var tint = __instance.screenTint;
            tint.transform.SetWorldZ(-19);
            tint.transform.localScale = new Vector2(15, 15);
            // Gamemode buttons
            var gmButtons = __instance.gameModeButtons;
            gmButtons.GetComponent<AspectPosition>().anchorPoint = new Vector2(0.4759f, 0.487f);
            gmButtons.transform.FindChild("Divider").gameObject.SetActive(false);
            gmButtons.transform.FindChild("Header").gameObject.SetActive(false);

            // Changes the reactor version shower position
            var version = GameObject.Find("ReactorVersion");
            var aspectPosition = version.AddComponent<AspectPosition>();
            var distanceFromEdge = new Vector3(10.13f, 2.75f, -1);
            aspectPosition.Alignment = AspectPosition.EdgeAlignments.LeftTop;
            aspectPosition.DistanceFromEdge = distanceFromEdge;
            aspectPosition.AdjustPosition();
        }
    }
}