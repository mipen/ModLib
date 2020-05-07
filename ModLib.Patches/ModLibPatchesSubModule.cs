using HarmonyLib;
using ModLib.Debugging;
using System;
using TaleWorlds.MountAndBlade;

namespace ModLib.Patches
{
    public class ModLibPatchesSubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {
                //Settings settings = FileDatabase.Get<Settings>(Settings.SettingsInstanceID);
                //if (settings == null) settings = new Settings();
                //SettingsDatabase.RegisterSettings(settings);

                var harmony = new Harmony("mod.modlib.patches.mipen");
                harmony.PatchAll();

            }
            catch (Exception ex)
            {
                ModDebug.ShowError($"An error occurred whilst initialising ModLib", "Error during initialisation", ex);
            }
        }
    }
}
