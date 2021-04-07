using Microsoft.Win32;
using System;

namespace StartupOrganizer
{
    internal struct StartupItem : IComparable, IEquatable<StartupItem>
    {
        #region Public properties

        public int ID { get; set; }
        public string RegistryKey { get; set; }
        public int GroupIndex { get; set; }
        public string ValueName { get; set; }
        public RegistryValueKind Kind { get; set; }
        public string ValueData { get; set; }
        public string Executable { get; set; }
        public string Folder { get; set; }
        public string Publisher { get; set; }
        public string Name { get; set; }
        public string Parameters { get; set; }
        public string ProductVersion { get; set; }
        public string FileVersion { get; set; }
        public bool PartOfOS { get; set; }
        public bool Enabled { get; set; }
        public string LinkName { get; set; }
        public string LinkFolder { get; set; }
        public TYPE Type { get; set; }
        public MODIFIED_STATE State { get; set; }

        #endregion Public properties

        #region Public enums

        public enum MODIFIED_STATE
        {
            UNTOUCHED,
            MODIFIED,
            NEW
        }
        public enum TYPE
        {
            FOLDER,
            REGISTRY,
            UWP
        }

        #endregion Public enums

        public int CompareTo(object obj)
        {
            return ID.CompareTo(obj);
        }

        public bool Equals(StartupItem other)
        {
            return ID == other.ID;
        }

        public static bool operator ==(StartupItem si1, StartupItem si2)
        {
            return si1.Equals(si2);
        }

        public static bool operator !=(StartupItem si1, StartupItem si2)
        {
            return !(si1 == si2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var si2 = (StartupItem)obj;
            return (ID == si2.ID);
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode() ^
                RegistryKey.GetHashCode() ^
                GroupIndex.GetHashCode() ^
                ValueName.GetHashCode() ^
                Kind.GetHashCode() ^
                ValueData.GetHashCode() ^
                Executable.GetHashCode() ^
                Folder.GetHashCode() ^
                Publisher.GetHashCode() ^
                Name.GetHashCode() ^
                Parameters.GetHashCode() ^
                ProductVersion.GetHashCode() ^
                FileVersion.GetHashCode() ^
                PartOfOS.GetHashCode() ^
                Enabled.GetHashCode() ^
                LinkName.GetHashCode() ^
                LinkFolder.GetHashCode() ^
                Type.GetHashCode() ^
                State.GetHashCode();
        }
    }
}
