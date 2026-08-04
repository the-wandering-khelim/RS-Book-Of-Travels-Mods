using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace EnableUnreleasedForms;

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

[HarmonyPatch(typeof(UtilityManager), nameof(UtilityManager.PlayerFormAvailable))]
static class Patch_UtilityManager_PlayerFormAvailable
{
    // these are the seven unfinished forms (the typo on fifteenth haha)
    private static readonly HashSet<PlayerForms> _hiddenForms = new HashSet<PlayerForms>
    {
        PlayerForms.Second,
        PlayerForms.Fifth,
        PlayerForms.Tenth,
        PlayerForms.Eleventh,
        PlayerForms.Fithteenth,
        PlayerForms.Eightteenth,
        PlayerForms.Nineteenth
    };

    static bool Prefix(PlayerForms form, ref bool __result)
    {
        try
        {
            if (_hiddenForms.Contains(form))
            {
                __result = true;
                return false; // skip original function call
            }
        }
        catch (Exception ex)
        {
            Plugin.Log.LogError($"Patch_UtilityManager_PlayerFormAvailable threw: {ex}");
        }

        return true; // run original function otherwise
    }
};
