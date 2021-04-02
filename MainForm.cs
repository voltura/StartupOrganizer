using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace StartupOrganizer
{
    public partial class MainForm : Form
    {
        private readonly List<StartupItem> m_StartupItems;
        public MainForm()
        {
            m_StartupItems = new List<StartupItem>();
            InitializeComponent();
        }

        private void Load_Click(object sender, EventArgs e)
        {
            //Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
            //Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce
            //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
            //Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce
            //Scheduled Tasks that is set to run at startup
            //Services that are set to automatic
            LoadUserStartupItemsFromRegistry();
        }

        private class StartupItem
        {
            public string RegistryKey { get; set; }
            public string ValueName { get; set; }
            public RegistryValueKind Kind { get; set; }
            public string ValueData { get; set; }
            public string Executable { get; set; }
            public string Folder { get; set; }
            public string Publisher { get; set; }
            public string Name { get; set; }
            public string Parameters { get; set; }
        }

        private void LoadUserStartupItemsFromRegistry()
        {
            using RegistryKey hkcuRun = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
            if (hkcuRun is null) return;
            m_StartupItems.Clear();
            listViewStartupItems.Items.Clear();
            string[] runValueNames = hkcuRun.GetValueNames();
            foreach (string runValueName in runValueNames)
            {
                StartupItem startupItem = new();
                string runValueData = Convert.ToString(hkcuRun.GetValue(runValueName, string.Empty));
                RegistryValueKind kind = hkcuRun.GetValueKind(runValueName);
                startupItem.Kind = kind;
                startupItem.ValueName = runValueName;
                startupItem.ValueData = runValueData;
                string[] pathAndParams = runValueData.Split('"', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                string exeAndPath = pathAndParams?[0];
                FileInfo fi = new(exeAndPath);
                startupItem.Folder = fi.Directory.FullName;
                startupItem.Executable = fi.Name;
                string parameters = pathAndParams.Length > 1 ? pathAndParams?[1] : string.Empty;
                startupItem.Parameters = parameters;
                m_StartupItems.Add(startupItem);
            }
            listViewStartupItems.BeginUpdate();
            foreach (StartupItem startupItem in m_StartupItems)
            {
                string[] row = { startupItem.ValueName, startupItem.Publisher, startupItem.Executable, startupItem.Parameters };
                ListViewItem listViewItem = new(row);
                listViewStartupItems.Items.Add(listViewItem);
            }
            listViewStartupItems.EndUpdate();
        }
    }
}
