using System.Reflection;
using static ModLib.Definitions.AssemblyChecker;

namespace ModLib.Definitions
{
    public static class SettingsDatabase
    {
        private const string SettingsDatabaseTypeName = "SettingsDatabase";
        private const string GetSettingsMethodName = "GetSettings";
        private static MethodInfo _getSettingsMethodInfo = null;

        private static MethodInfo GetSettingsMethod
        {
            get
            {
                if (AssemblyLoaded)
                {
                    if (_getSettingsMethodInfo == null)
                        _getSettingsMethodInfo = AssemblyChecker.Assembly.GetType(SettingsDatabaseTypeName).GetMethod(GetSettingsMethodName);
                    return _getSettingsMethodInfo;
                }
                return null;
            }
        }

        public static SettingsBase GetSettings(string uniqueID)
        {
            if (AssemblyLoaded)
            {
                return (SettingsBase)GetSettingsMethod.Invoke(null, new object[] { uniqueID });
            }
            else
                return null;
        }

    }
}
