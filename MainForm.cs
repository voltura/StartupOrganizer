#region Using statements

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;

#endregion Using statements

#region Notes

//Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
//Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce
//Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
//Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce
//Scheduled Tasks that is set to run at startup
//Services that are set to automatic

#endregion Notes

namespace StartupOrganizer
{
    public partial class MainForm : Form
    {
        #region Private variables

        private readonly List<StartupItem> m_StartupItems;
        const string UNKNOWN = "Unknown";
        const string CURRENT_USER_RUN_REG = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        const string LOCAL_MACHINE_RUN_REG = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        const string CURRENT_USER_RUN_32_REG = @"HKEY_CURRENT_USER\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run";
        const string LOCAL_MACHINE_RUN_32_REG = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run";

        #endregion Private variables

        #region Constructor

        public MainForm()
        {
            m_StartupItems = new List<StartupItem>();
            InitializeComponent();
        }

        #endregion Constructor

        #region Events

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadStartupItems();
        }

        private void Load_Click(object sender, EventArgs e)
        {
            LoadStartupItems();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Details_Click(object sender, EventArgs e)
        {
            ShowDetails();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            SaveStartupItems();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            AddStartupItem();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            DeleteStartupItems();
        }

        private void Backup_Click(object sender, EventArgs e)
        {
            BackupStartupItems();
        }

        private void Restore_Click(object sender, EventArgs e)
        {
            RestoreBackedUpStartupItems();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            BrowseBackupItems();
        }

        #endregion Events

        #region Private methods

        private void BrowseBackupItems()
        {
            throw new NotImplementedException();
        }

        private void RestoreBackedUpStartupItems()
        {
            throw new NotImplementedException();
        }
        private void BackupStartupItems()
        {
            throw new NotImplementedException();
        }
        private void DeleteStartupItems()
        {
            throw new NotImplementedException();
        }
        private void AddStartupItem()
        {
            throw new NotImplementedException();
        }
        private void SaveStartupItems()
        {
            throw new NotImplementedException();
        }

        private void LoadStartupItemsFromRegistry(string registrySubKey)
        {
            RegistryKey regKeyRun = registrySubKey.Equals(CURRENT_USER_RUN_REG) ? 
                Registry.CurrentUser.OpenSubKey(registrySubKey.Replace(@"HKEY_CURRENT_USER\","")) : 
                Registry.LocalMachine.OpenSubKey(registrySubKey.Replace(@"HKEY_LOCAL_MACHINE\", ""));
            if (regKeyRun is null) return;
            string[] runValueNames = regKeyRun.GetValueNames();

            for (int i = 0; i < runValueNames.Length; i++)
            {
                string runValueName = runValueNames[i];
                if (string.IsNullOrEmpty(runValueName)) continue;
                RegistryValueKind kind = regKeyRun.GetValueKind(runValueName);
                object valueDataObject = regKeyRun.GetValue(runValueName, string.Empty);
                string runValueData = Convert.ToString(valueDataObject ?? string.Empty);
                string[] pathAndParams;
                if (runValueData.Contains('"'))
                    pathAndParams = runValueData.Split('"', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                else if (runValueData.Contains('/'))
                    pathAndParams = new string[] { runValueData.Substring(0, runValueData.IndexOf('/') - 1).Trim(), runValueData[runValueData.IndexOf('/')..].Trim() };
                else
                    pathAndParams = new string[] { runValueData.Trim() };
                string parameters = pathAndParams != null ? (pathAndParams.Length > 1 ? pathAndParams?[1] : string.Empty) : string.Empty;
                string exeAndPath = pathAndParams != null ? pathAndParams?[0] : string.Empty;
                FileInfo fi = File.Exists(exeAndPath) ? new(exeAndPath) : null;                
                StartupItem startupItem = new();
                startupItem.ID = m_StartupItems.Count + 1;
                startupItem.RegistryKey = registrySubKey;
                startupItem.GroupIndex = registrySubKey.Equals(CURRENT_USER_RUN_REG) || registrySubKey.Equals(CURRENT_USER_RUN_32_REG) ? 0 : 1;
                startupItem.Folder = fi == null ? string.Empty : fi.Directory.FullName;
                startupItem.Executable = fi == null ? string.Empty : fi.Name;
                startupItem.Kind = kind;
                startupItem.ValueName = runValueName;
                startupItem.ValueData = runValueData;
                startupItem.Parameters = parameters;
                FillFileDetails(exeAndPath, ref startupItem);
                m_StartupItems.Add(startupItem);
            }
        }

        private void PopulateListView()
        {
            listViewStartupItems.BeginUpdate();
            listViewStartupItems.Items.Clear();
            foreach (StartupItem startupItem in m_StartupItems)
            {
                ListViewItem listViewItem = new()
                {
                    Group = listViewStartupItems.Groups[startupItem.GroupIndex],
                    Tag = Convert.ToString(startupItem.ID),
                    Text = startupItem.Name
                };
                string[] row = { startupItem.Publisher, startupItem.Executable, startupItem.Parameters, startupItem.PartOfOS ? " YES " : " NO " };
                listViewItem.SubItems.AddRange(row);
                listViewStartupItems.Items.Add(listViewItem);
            }
            listViewStartupItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listViewStartupItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewStartupItems.EndUpdate();
            int formWidth = 100;
            foreach(ColumnHeader columnHeader in listViewStartupItems.Columns)
            {
                formWidth += columnHeader.Width;
            }
            Width = formWidth > this.Width ? formWidth : Width;
        }

        private static void FillFileDetails(string fileName, ref StartupItem startupItem)
        {
            startupItem.Publisher = UNKNOWN;
            startupItem.Name = startupItem.ValueName;
            if (!File.Exists(fileName)) 
            {
                return;
            }
            try
            {
                X509Certificate cert = X509Certificate.CreateFromSignedFile(fileName);
                startupItem.Publisher = cert.Subject[3..cert.Subject.IndexOf(',')];
            }
            catch
            {
            }
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(fileName);
            var productName = fileVersionInfo.ProductName;
            
            if (!string.IsNullOrEmpty(productName) && productName == @"Microsoft® Windows® Operating System") 
            {
                startupItem.PartOfOS = true;
            }
            if (!string.IsNullOrEmpty(productName) && !startupItem.PartOfOS)
            {
                startupItem.Name = productName;
            } 
            else if (!string.IsNullOrEmpty(fileVersionInfo.FileDescription))
            {
                startupItem.Name = fileVersionInfo.FileDescription;
            }
            startupItem.ProductVersion = fileVersionInfo.ProductVersion;
            startupItem.FileVersion = fileVersionInfo.FileVersion;
            if (startupItem.Publisher.Equals(UNKNOWN))
            {
                startupItem.Publisher = string.IsNullOrEmpty(fileVersionInfo.CompanyName) ? UNKNOWN : fileVersionInfo.CompanyName;
            }
        }

        private void ShowDetails()
        {
            throw new NotImplementedException();
        }

        private void CleanLoadedStartupItems()
        {
            m_StartupItems.Clear();
        }

        private void LoadStartupItems()
        {
            CleanLoadedStartupItems();
            LoadStartupItemsFromRegistry(CURRENT_USER_RUN_REG);
            LoadStartupItemsFromRegistry(CURRENT_USER_RUN_32_REG);
            LoadStartupItemsFromRegistry(LOCAL_MACHINE_RUN_REG);
            LoadStartupItemsFromRegistry(LOCAL_MACHINE_RUN_32_REG);
            PopulateListView();
        }

        #endregion Private methods
    }
}
