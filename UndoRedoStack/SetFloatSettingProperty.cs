using ModLib.GUI.ViewModels;
using ModLib.Interfaces;
using System;

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
            if (settingProperty == null) throw new ArgumentNullException(nameof(settingProperty));

            Value = value;
            SettingProperty = settingProperty;
            originalValue = SettingProperty.IntValue;
        }

        public void DoAction()
        {
            SettingProperty.FloatValue = (float)Value;
        }

        public void UndoAction()
        {
            SettingProperty.FloatValue = originalValue;
        }
    }
}
