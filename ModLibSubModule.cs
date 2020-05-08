using ModLib.Debugging;
using ModLib.GUI.GauntletUI;
using System;
using System.Windows.Forms;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace ModLib
{
    public class ModLibSubModule : MBSubModuleBase
    {
        public static string ModuleFolderName { get; } = "ModLib";

        protected override void OnSubModuleLoad()
        {
            try
            {
                FileDatabase.Initialise(ModuleFolderName);

                Module.CurrentModule.AddInitialStateOption(new InitialStateOption("ModOptionsMenu", new TextObject("Mod Options"), 9990, () =>
                {
                    ScreenManager.PushScreen(new ModOptionsGauntletScreen());
                }, false));
            }
            catch (Exception ex)
            {
                ModDebug.ShowError($"An error occurred whilst initialising ModLib", "Error during initialisation", ex);
            }
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            SettingsDatabase.LoadAllSettings();
            SettingsDatabase.BuildModSettingsVMs();
        }
    }
}
