using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace EnableItemBasedAppearanceColors;

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

[HarmonyPatch(typeof(UtilityManager), nameof(UtilityManager.FeatureAvailable))]
static class Patch_UtilityManager_FeatureAvailable
{
    static bool Prefix(UpdateFeature feature, ref bool __result)
    {
        try
        {
            if (feature == UpdateFeature.ClothingColor)
            {
                __result = true;
                return false; // skip original function call
            }
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"Patch_UtilityManager_FeatureAvailable threw: {ex}");
        }

        return true; // run original function otherwise
    }
};
