using System;
using System.Reflection;

namespace ModLib
{
    public class Ref
    {
        private readonly Func<object> getter;
        private readonly Action<object> setter;
        private readonly PropertyInfo propInfo = null;
        private readonly object instance = null;

        public object Value
        {
            get
            {
                if (propInfo != null)
                    return propInfo.GetValue(instance);
                else
                    return getter();
            }
            set
            {
                if (propInfo != null)
                    propInfo.SetValue(instance, value);
                else
                    setter(value);
            }
        }

        public Ref(Func<object> getter, Action<object> setter)
        {
            this.getter = getter ?? throw new ArgumentNullException(nameof(getter));
            this.setter = setter;
        }

        public Ref(Func<object> getter) : this(getter, null)
        {

        }

        public Ref(PropertyInfo propInfo, object instance)
        {
            this.propInfo = propInfo;
            this.instance = instance;
        }
    }
}
