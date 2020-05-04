using System;
using System.Linq;
using System.Reflection;

namespace ModLib.Definitions
{
    public static class AssemblyChecker
    {
        private const string AssemblyName = "ModLib";
        private static bool _assemblyLoaded = false;
        private static Assembly _assembly = null;

        public static bool AssemblyLoaded
        {
            get
            {
                if (!_assemblyLoaded)
                {
                    if (Assembly != null)
                        _assemblyLoaded = true;
                }
                return _assemblyLoaded;
            }
        }

        public static Assembly Assembly
        {
            get
            {
                if (_assembly == null)
                {
                    _assembly = (from t in AppDomain.CurrentDomain.GetAssemblies()
                                 where t.GetName().Name == AssemblyName
                                 select t).FirstOrDefault();
                }
                return _assembly;
            }
        }
    }
}
