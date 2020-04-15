using ModLib.Attributes;
using System.Xml.Serialization;

namespace MyMod
{
    public class MyModSettings : SettingsBase
    {
        public const string InstanceID = "MyModSettings";
        public override string ModName => "MyMod";
        public override string ModuleFolderName => MyModFolder;
        [XmlElement]
        public override string ID { get; set; } = InstanceID;

        public static Settings Instance
        {
            get
            {
                return (MyModSettings)SettingsDatabase.GetSettings(InstanceID);
            }
        }

        [XmlElement]
        [SettingProperty("Some Multiplier", 1f, 3f, "This is a multiplier")]
        [SettingPropertyGroup("Group 1")]
        public float SomeMultiplier { get; set; } = 1.5f;
        [XmlElement]
        [SettingProperty("Some multiplier Enabled", "Enables SomeMultiplier")]
        [SettingPropertyGroup("Group 1")]
        public bool MultiplierEnabled { get; set; } = true;
        [XmlElement]
        [SettingProperty("Some Other Multiplier", 1f, 3f, "This is another multiplier")]
        [SettingPropertyGroup("Group 2")]
        public float SomeOtherMultiplier { get; set; } = 1.5f;
    }
}