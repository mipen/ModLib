using ModLib.Definitions;
using ModLib.Definitions.Attributes;
using ModLib.GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ModLib
{
    public static class SettingsBaseExtensionMethods
    {
        private const char SubGroupDelimiter = '/';

        internal static List<SettingPropertyGroup> GetSettingPropertyGroups(this SettingsBase sb)
        {
            var groups = new List<SettingPropertyGroup>();
            // Find all the properties in the settings instance which have the SettingProperty attribute.
            var propList = (from p in sb.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                            let propAttr = p.GetCustomAttribute<SettingPropertyAttribute>(true)
                            let groupAttr = p.GetCustomAttribute<SettingPropertyGroupAttribute>(true)
                            where propAttr != null
                            let groupAttrToAdd = groupAttr ?? SettingPropertyGroupAttribute.Default
                            select new SettingProperty(propAttr, groupAttrToAdd, p, sb)).ToList();

            //Loop through each property
            foreach (var settingProp in propList)
            {
                //First check that the setting property is set up properly.
                CheckIsValid(settingProp);
                //Find the group that the setting property should belong to. This is the default group if no group is specifically set with the SettingPropertyGroup attribute.
                SettingPropertyGroup group = GetGroupFor(settingProp, groups);
                group.Add(settingProp);
            }

            //If there is more than one group in the list, remove the misc group so that it can be added to the bottom of the list after sorting.
            SettingPropertyGroup miscGroup = GetGroupFor(SettingPropertyGroupAttribute.DefaultGroupName, groups);
            if (miscGroup != null && groups.Count > 1)
                groups.Remove(miscGroup);
            else
                miscGroup = null;

            //Sort the list of groups alphabetically.
            groups.Sort((x, y) => x.GroupName.CompareTo(y.GroupName));
            if (miscGroup != null)
                groups.Add(miscGroup);

            foreach (var group in groups)
                group.SetParentGroup(null);

            return groups;
        }

        private static SettingPropertyGroup GetGroupFor(SettingProperty sp, ICollection<SettingPropertyGroup> groupsList)
        {
            //If the setting somehow doesn't have a group attribute, throw an error.
            if (sp.GroupAttribute == null)
                throw new Exception($"SettingProperty {sp.Name} has null GroupAttribute");

            SettingPropertyGroup group;
            //Check if the intended group is a sub group
            if (sp.GroupAttribute.GroupName.Contains(SubGroupDelimiter))
            {
                //Intended group is a sub group. Must find it. First get the top group.
                string topGroupName = GetTopGroupName(sp.GroupAttribute.GroupName, out string truncatedGroupName);
                SettingPropertyGroup topGroup = groupsList.GetGroup(topGroupName);
                if (topGroup == null)
                {
                    topGroup = new SettingPropertyGroup(sp.GroupAttribute, topGroupName);
                    groupsList.Add(topGroup);
                }
                //Find the sub group
                group = GetGroupForRecursive(truncatedGroupName, topGroup.SettingPropertyGroups, sp);
            }
            else
            {
                //Group is not a subgroup, can find it in the main list of groups.
                group = groupsList.GetGroup(sp.GroupAttribute.GroupName);
                if (group == null)
                {
                    group = new SettingPropertyGroup(sp.GroupAttribute);
                    groupsList.Add(group);
                }
            }
            return group;
        }

        private static SettingPropertyGroup GetGroupFor(string groupName, ICollection<SettingPropertyGroup> groupsList)
        {
            return groupsList.GetGroup(groupName);
        }

        private static SettingPropertyGroup GetGroupForRecursive(string groupName, ICollection<SettingPropertyGroup> groupsList, SettingProperty sp)
        {
            if (groupName.Contains(SubGroupDelimiter))
            {
                //Need to go deeper
                string topGroupName = GetTopGroupName(groupName, out string truncatedGroupName);
                SettingPropertyGroup topGroup = GetGroupFor(topGroupName, groupsList);
                if (topGroup == null)
                {
                    topGroup = new SettingPropertyGroup(sp.GroupAttribute, topGroupName);
                    groupsList.Add(topGroup);
                }
                return GetGroupForRecursive(truncatedGroupName, topGroup.SettingPropertyGroups, sp);
            }
            else
            {
                //Reached the bottom level, can return the final group.
                SettingPropertyGroup group = groupsList.GetGroup(groupName);
                if (group == null)
                {
                    group = new SettingPropertyGroup(sp.GroupAttribute, groupName);
                    groupsList.Add(group);
                }
                return group;
            }
        }

        private static string GetTopGroupName(string groupName, out string truncatedGroupName)
        {
            int index = groupName.IndexOf(SubGroupDelimiter);
            string topGroupName = groupName.Substring(0, index);

            truncatedGroupName = groupName.Remove(0, index + 1);
            return topGroupName;
        }

        private static void CheckIsValid(SettingProperty prop)
        {
            if (!prop.Property.CanRead)
                throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} must have a getter.");
            if (!prop.Property.CanWrite)
                throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} must have a setter.");

            if (prop.SettingType == SettingType.Int || prop.SettingType == SettingType.Float)
            {
                if (prop.MinValue == prop.MaxValue)
                    throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} is a numeric type but the MinValue and MaxValue are the same.");
                if (prop.GroupAttribute != null && prop.GroupAttribute.IsMainToggle)
                    throw new Exception($"Property {prop.Property.Name} in {prop.SettingsInstance.GetType().FullName} is marked as the main toggle for the group but is a numeric type. The main toggle must be a boolean type.");
            }
        }

    }
}
