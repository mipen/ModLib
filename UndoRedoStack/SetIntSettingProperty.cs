using ModLib.GUI.ViewModels;
using ModLib.Interfaces;
using System;

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
            if (settingProperty == null) throw new ArgumentNullException(nameof(settingProperty));
            Value = value;
            SettingProperty = settingProperty;
            originalValue = SettingProperty.IntValue;
        }

        public void DoAction()
        {
            SettingProperty.IntValue = (int)Value;
        }

        public void UndoAction()
        {
            SettingProperty.IntValue = originalValue;
        }
    }
}
