using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace UnlockAccountModes;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInProcess("BookOfTravels.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log = null!;

    private void Awake()
    {
        Log = Logger;

        PluginConfig.Init(Config);

        new Harmony(PluginInfo.PLUGIN_GUID).PatchAll();

        Log.LogInfo($"{PluginInfo.PLUGIN_NAME} loaded.");
    }
}

[HarmonyPatch]
internal static class Patch_PlayerBase_OnStartLocalPlayer
{
    static MethodBase? TargetMethod()
    {
        var type = AccessTools.TypeByName("PlayerBase");
        return type == null ? null : AccessTools.Method(type, "OnStartLocalPlayer");
    }

    static void Postfix(object __instance)
    {
        try
        {
            AccountUnlocks.ApplyUnlocks(__instance);
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"Patch_PlayerBase_OnStartLocalPlayer threw: {ex}");
        }
    }
}