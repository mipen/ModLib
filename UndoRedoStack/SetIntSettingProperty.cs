using ModLib.GUI.ViewModels;
using ModLib.Interfaces;

namespace ModLib
{
    public class SetIntSettingProperty : IAction
    {
        public Ref Context { get; } = null;

        public object Value { get; }

        private SettingProperty SettingProperty { get; }
        private int originalValue;

        public SetIntSettingProperty(SettingProperty settingProperty, int value)
        {
            Value = value;
            SettingProperty = settingProperty;
            originalValue = SettingProperty.IntValue;
        }

        public void Do()
        {
            SettingProperty.IntValue = (int)Value;
        }

        public void Undo()
        {
            SettingProperty.IntValue = originalValue;
        }
    }
}
