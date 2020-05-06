using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using static ModLib.Definitions.AssemblyChecker;

namespace ModLib.Definitions
{
    public static class SettingsDatabase
    {
        private const string SettingsDatabaseTypeName = "ModLib.SettingsDatabase";
        private const string GetSettingsMethodName = "GetSettings";
        private const string LoadSettingsFromTypeMethodName = "LoadSettingsFromType";
        private static MethodInfo _getSettingsMethodInfo = null;
        private static MethodInfo _loadSettingsFromTypeMethodInfo = null;
        private static Dictionary<Type, SettingsBase> defaults = new Dictionary<Type, SettingsBase>();


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
                            _getSettingsMethodInfo = type.GetMethod(GetSettingsMethodName, BindingFlags.Static | BindingFlags.NonPublic);
                    }
                    return _getSettingsMethodInfo;
                }
                return null;
            }
        }
        private static MethodInfo LoadSettingsFromTypeMethod
        {
            get
            {
                if (AssemblyLoaded)
                {
                    if (_loadSettingsFromTypeMethodInfo == null)
                    {
                        var type = AssemblyChecker.Assembly.GetType(SettingsDatabaseTypeName);
                        if (type == null)
                            MessageBox.Show("Cannot find type for SettingsDatabase");
                        else
                            _loadSettingsFromTypeMethodInfo = type.GetMethod(LoadSettingsFromTypeMethodName, BindingFlags.Static | BindingFlags.NonPublic);
                    }
                    return _loadSettingsFromTypeMethodInfo;
                }
                return null;
            }
        }

        public static SettingsBase GetSettings<T>() where T : SettingsBase
        {
            SettingsBase defaultSB = (SettingsBase)Activator.CreateInstance(typeof(T));
            if (!defaults.ContainsKey(typeof(T)))
                defaults.Add(typeof(T), defaultSB);

            if (AssemblyLoaded)
            {
                SettingsBase sb = (SettingsBase)GetSettingsMethod?.Invoke(null, new object[] { defaultSB.ID });
                if (sb == null)
                {
                    LoadSettingsFromTypeMethod?.Invoke(null, new object[] { typeof(T) });
                    sb = (SettingsBase)GetSettingsMethod?.Invoke(null, new object[] { defaultSB.ID });
                    if (sb == null)
                        return defaults[typeof(T)];
                }
                return sb;
            }
            else
                return defaults[typeof(T)];
        }

    }
}
