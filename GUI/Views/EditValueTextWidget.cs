using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace ModLib.GUI.Views
{
    public class EditValueTextWidget : EditableTextWidget
    {
        private EditableText editableText = null;

        [DataSourceProperty]
        public SettingType SettingType { get; set; } = SettingType.Float;
        [DataSourceProperty]
        public float MaxValue { get; set; } = 0f;
        [DataSourceProperty]
        public float MinValue { get; set; } = 0f;

        public EditValueTextWidget(UIContext context) : base(context)
        {
            editableText = (EditableText)typeof(EditableTextWidget).GetField("_editableText", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(this);
        }

        public override void HandleInput(IReadOnlyList<int> lastKeysPressed)
        {
            if (Input.IsKeyDown(InputKey.LeftControl) && Input.IsKeyPressed(InputKey.V))
                return;
            if (lastKeysPressed == null) return;

            if (lastKeysPressed.Count > 0)
            {
                for (int i = 0; i < lastKeysPressed.Count; i++)
                {
                    int key = lastKeysPressed[i];
                    if (Enum.IsDefined(typeof(KeyCodes), key))
                    {
                        if (key == (int)KeyCodes.Minus)
                        {
                            if (editableText.SelectedTextBegin != 0)
                                continue;
                        }
                        else if (SettingType == SettingType.Float)
                        {
                            //Handle input for float types
                            if (key == (int)KeyCodes.Decimal)
                            {
                                if (RealText.Count('.') >= 1)
                                    continue;
                            }
                        }
                        else if (SettingType == SettingType.Int)
                        {
                            //Handle input for int types.
                            if (key == (int)KeyCodes.Decimal)
                                continue;
                        }
                        base.HandleInput(lastKeysPressed);
                        float value;
                        if (float.TryParse(RealText, out value))
                        {
                            float newVal = value;
                            if (value > MaxValue)
                                newVal = MaxValue;
                            else if (value < MinValue)
                                newVal = MinValue;
                            if (newVal != value)
                            {
                                string format = SettingType == SettingType.Int ? "0" : "0.00";
                                RealText = newVal.ToString(format);
                                editableText.SetCursorPosition(0, true);
                            }
                        }
                    }
                }
            }
            else
                base.HandleInput(lastKeysPressed);
        }


        private enum KeyCodes
        {
            Zero = 48,
            One = 49,
            Two = 50,
            Three = 51,
            Four = 52,
            Five = 53,
            Six = 54,
            Seven = 55,
            Eight = 56,
            Nine = 57,
            Decimal = 46,
            Minus = 45,
            Backspace = 8
        }
    }
}
