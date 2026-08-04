using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;

namespace RemoveServerTimeOffset;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInProcess("BookOfTravels.exe")]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log = null!;

    private void Awake()
    {
        Log = Logger;

        new Harmony(PluginInfo.PLUGIN_GUID).PatchAll();

        Log.LogInfo($"{PluginInfo.PLUGIN_NAME} loaded.");
    }
}

[HarmonyPatch(typeof(NetworkSyncManager), "InitTimeNow")]
internal static class PatchInitTimeNow
{
    static readonly FieldInfo OverallTimeOffset = typeof(NetworkSyncManager)
        .GetField("overallTimeOffset", BindingFlags.Instance | BindingFlags.NonPublic);

    static void Prefix(NetworkSyncManager __instance) {
        try
        {
            OverallTimeOffset.SetValue(__instance, 0);
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"{PluginInfo.PLUGIN_NAME} threw: {ex}");
        }
    }
}
