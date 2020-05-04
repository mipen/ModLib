using System.Reflection;
using System.Windows.Forms;
using static ModLib.Definitions.AssemblyChecker;

namespace ModLib.Definitions
{
    public static class SettingsDatabase
    {
        private const string SettingsDatabaseTypeName = "ModLib.SettingsDatabase";
        private const string GetSettingsMethodName = "GetSettings";
        private static MethodInfo _getSettingsMethodInfo = null;

        private static MethodInfo GetSettingsMethod
        {
            get
            {
                if (AssemblyLoaded)
                {
                    if (_getSettingsMethodInfo == null)
                    {
                        var type = AssemblyChecker.Assembly.GetType(SettingsDatabaseTypeName);
                        if (type == null)
                            MessageBox.Show("Cannot find type for SettingsDatabase");
                        else
                            _getSettingsMethodInfo = type.GetMethod(GetSettingsMethodName);
                    }
                    return _getSettingsMethodInfo;
                }
                return null;
            }
        }

        public static SettingsBase GetSettings(string uniqueID)
        {
            if (AssemblyLoaded)
            {
                return (SettingsBase)GetSettingsMethod?.Invoke(null, new object[] { uniqueID });
            }
            else
                return null;
        }

    }
}
