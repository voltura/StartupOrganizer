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

        #endregion Private variables

        #region Constructor

        public MainForm()
        {
            m_StartupItems = new List<StartupItem>();
            InitializeComponent();
        }

        #endregion Constructor

        #region Events

        private void Load_Click(object sender, EventArgs e)
        {
            LoadUserStartupItemsFromRegistry();
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

        private void LoadUserStartupItemsFromRegistry()
        {
            using RegistryKey hkcuRun = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            if (hkcuRun is null) return;
            m_StartupItems.Clear();
            string[] runValueNames = hkcuRun.GetValueNames();

            for (int i = 0; i < runValueNames.Length; i++)
            {
                string runValueName = runValueNames[i];
                RegistryValueKind kind = hkcuRun.GetValueKind(runValueName);
                object valueDataObject = hkcuRun.GetValue(runValueName, string.Empty);
                string runValueData = Convert.ToString(valueDataObject ?? string.Empty);
                string[] pathAndParams = runValueData.Split('"', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                string parameters = pathAndParams != null ? (pathAndParams.Length > 1 ? pathAndParams?[1] : string.Empty) : string.Empty;
                string exeAndPath = pathAndParams != null ? pathAndParams?[0] : string.Empty;
                FileInfo fi = File.Exists(exeAndPath) ? new(exeAndPath) : null;                
                StartupItem startupItem = new();
                startupItem.ID = i;
                startupItem.Folder = fi == null ? string.Empty : fi.Directory.FullName;
                startupItem.Executable = fi == null ? string.Empty : fi.Name;
                startupItem.Kind = kind;
                startupItem.ValueName = runValueName;
                startupItem.ValueData = runValueData;
                startupItem.Parameters = parameters;
                FillFileDetails(exeAndPath, ref startupItem);
                m_StartupItems.Add(startupItem);
            }
            PopulateListView();
        }

        private void PopulateListView()
        {
            listViewStartupItems.BeginUpdate();
            listViewStartupItems.Items.Clear();
            foreach (StartupItem startupItem in m_StartupItems)
            {
                string[] row = { Convert.ToString(startupItem.ID), startupItem.Name, startupItem.Publisher, startupItem.Executable, startupItem.Parameters };
                ListViewItem listViewItem = new(row);
                listViewStartupItems.Items.Add(listViewItem);
            }
            listViewStartupItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listViewStartupItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewStartupItems.EndUpdate();
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
            if (!string.IsNullOrEmpty(productName))
            {
                startupItem.Name = productName;
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

        #endregion Private methods
    }
}
