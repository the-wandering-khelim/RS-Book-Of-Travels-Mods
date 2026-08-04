using BepInEx.Configuration;

namespace UnlockAccountModes;

internal static class PluginConfig
{
    // config bools
    internal static ConfigEntry<bool> EnableDebug = null!;
    internal static ConfigEntry<bool> EnableBacker = null!;
    internal static ConfigEntry<bool> EnableEarlyBird = null!;
    internal static ConfigEntry<bool> EnableSecretCultist = null!;

    internal static void Init(ConfigFile config)
    {
        EnableDebug = config.Bind(
            "General",
            "EnableDebugMode",
            true,
            "If true, enables the Debug account mode, which allows the debug menu to be opened in-game. If false, mode is not turned on.");

        EnableBacker = config.Bind(
            "General",
            "EnableBackerMode",
            false,
            "If true, enables the Backer account mode, which allows getting the Traveller's Knapsack from any Trainmasters Stash. If false, mode is not turned on.");

        EnableEarlyBird = config.Bind(
            "General",
            "EnableEarlyBirdMode",
            false,
            "If true, enables the Early Bird account mode, which allows getting the early bird Lantern of Early Light and Ring of Cycles items from any Trainmasters Stash. If false, mode is not turned on.");

        EnableSecretCultist = config.Bind(
            "General",
            "EnableSecretCultistMode",
            false,
            "If true, enables the Secret Cultist account mode, which allows getting the Fan of the Hidden from any Trainmasters Stash. If false, mode is not turned on.");
    }
}