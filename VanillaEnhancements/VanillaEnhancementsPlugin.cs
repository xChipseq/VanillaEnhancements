﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Newtonsoft.Json.Linq;
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
    public const string VersionString = "1.1.1";
    public static bool IsDevRelease = false;

    public static System.Version Version = System.Version.Parse(VersionString);
    public Harmony Harmony { get; } = new(Id);

    public override void Load()
    {
        VELogger.Info("Vanilla Enhancements is loading...");

        bool dev = CheckDevRelease().Result;
        IsDevRelease = dev;
        ReactorCredits.Register("VanillaEnhancements", VersionString + (dev ? "-indev" : ""), false, ReactorCredits.AlwaysShow);
        ModConfig.Bind(Config);
        PlayerMuting.Setup();
        IL2CPPChainloader.Instance.Finished += ModCompatibility.Load;
        try
        {
            Harmony.PatchAll();
        }
        catch (Exception exception)
        {
            VELogger.Error($"An error occured while loading Vanilla Enhancements-{VersionString + (dev ? "-indev" : "")} ({Id})");
            VELogger.Error(exception.Message);
        }
        VELogger.Info("Vanilla Enhancements finished loading");
    }

    public static async Task<bool> CheckDevRelease()
    {
        string url = $"https://api.github.com/repos/xChipseq/VanillaEnhancements/tags";
        bool[] exists = await HttpVersionExists(url);
        VELogger.Info($"IsDevRelease: {!exists[0]} ({(exists[1] ? "success" : "fail")})");
        return !exists[0];
    }

    static async Task<bool[]> HttpVersionExists(string url)
    {
        bool found = false;
        using HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "C# App");
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            using JsonDocument jsonDoc = JsonDocument.Parse(content);
            var root = jsonDoc.RootElement;
            foreach (var tag in root.EnumerateArray())
            {
                string name = tag.GetProperty("name").GetString();
                if (name == VersionString)
                {
                    found = true;
                    break;
                }
            }

            return [found, true];
        }
        catch (HttpRequestException ex)
        {
            VELogger.Error($"Failed to check for dev release: {ex.Message}");
            return [false, false];
        }
    }
}
