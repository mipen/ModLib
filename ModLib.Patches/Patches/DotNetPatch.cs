using HarmonyLib;
using ModLib.Debugging;
using System;
using TaleWorlds.DotNet;

namespace ModLib.Patches.Patches
{
    [HarmonyPatch(typeof(Managed), "ApplicationTick")]
    public class DotNetPatch
    {
        static void Finalizer(Exception __exception)
        {
            if (__exception != null)
            {
                ModDebug.ShowError($"Mount and Blade Bannerlord has encountered an error and needs to close. See the error information below.",
                      "Mount and Blade Bannerlord has crashed", __exception);
            }
        }

        static bool Prepare()
        {
            return Settings.Instance.DebugMode;
        }
    }
}
