using ModLib.GUI.ViewModels;
using ModLib.Interfaces;

namespace ModLib
{
    public class SetFloatSettingProperty : IAction
    {
        public Ref Context { get; } = null;

        public object Value { get; }

        private SettingProperty SettingProperty { get; }
        private float originalValue;

        public SetFloatSettingProperty(SettingProperty settingProperty, float value)
        {
            Value = value;
            SettingProperty = settingProperty;
            originalValue = SettingProperty.IntValue;
        }

        public void Do()
        {
            SettingProperty.FloatValue = (float)Value;
        }

        public void Undo()
        {
            SettingProperty.FloatValue = originalValue;
        }
    }
}
