using HarmonyLib;

namespace UnlockAccountModes;

internal static class AccountUnlocks
{
    // account unlock flag bits based on the AccountUnlocks enum
    private const int FlagAdmin      = 2;
    private const int FlagDev        = 4;
    private const int FlagBacker     = 8;
    private const int FlagEarlyBird  = 16;
    private const int FlagSecretCult = 32;

    // read the config file and bitwise OR the combined flag values
    internal static int ResolveFlags()
    {
        int flags = 0;
        if (PluginConfig.EnableDebug.Value)         flags |= FlagAdmin | FlagDev;
        if (PluginConfig.EnableBacker.Value)        flags |= FlagBacker;
        if (PluginConfig.EnableEarlyBird.Value)     flags |= FlagEarlyBird;
        if (PluginConfig.EnableSecretCultist.Value) flags |= FlagSecretCult;
        return flags;
    }

    // set the account unlock flags on PlayerBase by writing to the accountUnlocks
    // SyncVar field directly, AND setting it through NetworkaccountUnlocks so that
    // Mirror's dirty/sync propagation is triggered (just in case that matters)
    internal static void ApplyUnlocks(object playerBaseInstance)
    {
        if (playerBaseInstance == null)
        {
            Plugin.Log.LogWarning("playerBaseInstance is null, skipping");
            return;
        }

        int flags = ResolveFlags();
        var enumType = AccessTools.TypeByName("AccountUnlocks");
        var flagsEnum = Enum.ToObject(enumType, flags);

        var playerBaseType = AccessTools.TypeByName("PlayerBase");
        var accountUnlocks = AccessTools.Field(playerBaseType, "accountUnlocks");
        accountUnlocks.SetValue(playerBaseInstance, flagsEnum);

        var networkUnlocks = AccessTools.Property(playerBaseType, "NetworkaccountUnlocks");

        try
        {
            networkUnlocks.SetValue(playerBaseInstance, flagsEnum);
        }
        catch (Exception e)
        {
            Plugin.Log.LogWarning($"NetworkaccountUnlocks setter threw, continuing: {e.Message}");
        }
    }
}