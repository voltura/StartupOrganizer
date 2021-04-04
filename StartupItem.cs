﻿using Microsoft.Win32;

namespace StartupOrganizer
{
    internal struct StartupItem
    {
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
        public string Type { get; set; }
    }
}
