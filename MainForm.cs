#region Using statements

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

#endregion Using statements

#region Notes

//Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
//Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce
//Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
//Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce
//Scheduled Tasks that is set to run at startup
//Services that are set to automatic
//C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp
//%ProgramData%\Microsoft\Windows\Start Menu\Programs\Startup 
//%USERPROFILE%\AppData\Roaming\Microsoft\Windows\Start Menu
//users start menu startup folders

// Enabled|disabled info - how to read?
//Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run
//Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\StartupFolder
//Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\StartupFolder
/*HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run
HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run
Programs unique to the user are in the HKEY_LOCAL_CURRENT_USER whereas programs installed for all users are under HKEY_LOCAL_MACHINE.

which binary value would we use for each option?

Depends on the first binary of the value: 0000 0000B, the least significant bit is turn on(0)/off(1). and the 5th is the switch enable(0)/disable(1) bit, for example: set xxxx 1xxxB for the first binary, the switch in windows setting will be:


However, we usually set the first binary bit to 0x02 (enable) / 0x03 (disable).

 UWP apps 

 Computer\HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\Microsoft.YourPhone_8wekyb3d8bbwe\YourPhone\State 0 default disabled, 2 enabled 1 disabled
UserEnabledStartupOnce 1


https://stackoverflow.com/questions/41160159/get-list-of-installed-windows-apps

https://stackoverflow.com/questions/56265062/programmatically-get-list-of-installed-application-executables-windows10-c


powershell
get-StartApps

parse all ending with !App

navigate to 
Computer\HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\
 <index of last space> . remove !App _add_ \ and first part until tab or multiple space, replace space with ''.

set state 
State 0 default disabled, 2 enabled 1 disabled
and 
UserEnabledStartupOnce 1

 */

#endregion Notes

namespace StartupOrganizer
{
    public partial class MainForm : Form
    {
        #region Private variables

        private readonly List<StartupItem> m_StartupItems;
        const string UNKNOWN = "Unknown";
        const string CURRENT_USER_RUN_REG = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        const string CURRENT_USER_APPROVED_RUN_REG = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";
        const string CURRENT_USER_APPROVED_STARTUP_FOLDER_REG = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\StartupFolder";
        const string LOCAL_MACHINE_RUN_REG = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        const string LOCAL_MACHINE_APPROVED_RUN_REG = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";
        const string LOCAL_MACHINE_APPROVED_STARTUP_FOLDER_REG = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\StartupFolder";
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
            BrowseItem();
        }
        private void Enable_Click(object sender, EventArgs e)
        {
            EnableStartupItems();
        }

        private void Disable_Click(object sender, EventArgs e)
        {
            DisableStartupItems();
        }

        #endregion Events

        #region Private methods

        private void BrowseItem()
        {
            if (listViewStartupItems.SelectedItems is null) return;

            // get first selected ID of list via tag
            ListViewItem item = listViewStartupItems.SelectedItems[0];
            if (item is null) return;
            int id = Convert.ToInt32(item.Tag);

            // get record via ID from m_StartupItems list
            StartupItem match = m_StartupItems.Find(x => x.ID.Equals(id));

            // check if type is folder
            if (match.Type == "Folder" || match.Type == "UWP")
            {
                // get folder + executable
                string fullPath = Path.Combine(match.Folder, match.Executable);

                // run explorer.exe with folder + exe as param
                ProcessStartInfo info = new() { UseShellExecute = true, WindowStyle = ProcessWindowStyle.Normal, FileName = match.Folder, WorkingDirectory = match.Folder };
                Process.Start(info);
                return;
            }
            else if (match.Type == "Registry")
            {
                string key = $@"Computer\{match.RegistryKey}";
                string regScript = @$"REG ADD HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Applets\Regedit /v LastKey /d ""{key}"" /F
START regedit.exe";
                string scriptFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    $"{Application.ProductName}_REG.ps1");
                File.WriteAllText(scriptFile, regScript);
                RunPostScript(scriptFile);
                File.Delete(scriptFile);
                return;
            }
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

        private void LoadStartupItemsFromStartupFolders()
        {
            string userStartupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string computerStartupFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup);
            GetStartupItemsFromFolder(userStartupFolder, 0);
            GetStartupItemsFromFolder(computerStartupFolder, 1);
        }

        private void GetStartupItemsFromUWP()
        {
            List<UwpApp> uwpApps = GetUwpApps();
            for (int i = 0; i < 2; i++)
            {
                foreach (UwpApp app in uwpApps)
                {
                    string regKeyUwpApp = @$"SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\{app.ID}\{app.NoSpacesName}";
                    RegistryKey k;
                    if (i == 0)
                    {
                        k = Registry.CurrentUser.OpenSubKey(regKeyUwpApp);
                        if (k == null) k = Registry.CurrentUser.OpenSubKey(regKeyUwpApp + "StartupId");
                    }
                    else
                    {
                        // NOTE: Will not really get anything here...
                        k = Registry.LocalMachine.OpenSubKey(regKeyUwpApp);
                        if (k == null) k = Registry.LocalMachine.OpenSubKey(regKeyUwpApp + "StartupId");
                    }
                    if (k != null)
                    {
                        string state = k.GetValue("State").ToString();
                        Debug.WriteLine(app.Name + $" {((state == "0" || state == "1") ? "Disabled" : "Enabled")}");
                        StartupItem startupItem = new();
                        startupItem.ID = m_StartupItems.Count + 1;
                        startupItem.GroupIndex = 0;
                        startupItem.ValueName = app.Name;
                        startupItem.Folder = app.Folder;
                        startupItem.Publisher = app.ID.Substring(0, app.ID.IndexOf('.') != -1 ? app.ID.IndexOf('.') : app.ID.Length);
                        startupItem.Executable = app.Executable;
                        startupItem.Enabled = !(state == "0" || state == "1");
                        startupItem.Parameters = string.Empty;
                        startupItem.Name = app.Name;
                        startupItem.Type = "UWP";
                        m_StartupItems.Add(startupItem);
                    }
                }
            }
        }

        private static List<UwpApp> GetUwpApps()
        {
            string scriptFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.ProductName + ".ps1");
            File.WriteAllText(scriptFile, "get-StartApps");
            List<string> result = RunPostScript(scriptFile);
            File.Delete(scriptFile);
            List<UwpApp> apps = new();
            foreach (string row in result)
            {
                string trimmedRow = row.Trim();
                if (trimmedRow.StartsWith("Name") || trimmedRow.StartsWith("-")) continue;
                if (trimmedRow.Length != 0)
                {
                    int doubleSpaceIndex = trimmedRow.IndexOf("  "); //NOTE: potentially miss app with longest name, but yeah
                    if (doubleSpaceIndex != -1)
                    {
                        string name = trimmedRow[0..doubleSpaceIndex];
                        string noSpacesName = name.Replace(" ", string.Empty);
                        string id = trimmedRow[doubleSpaceIndex..].Trim();
                        if (id.EndsWith("!App"))
                        {
                            string packageName = id.IndexOf('_') > 0 ? id[..id.IndexOf('_')] : noSpacesName;
                            UwpApp uwpApp = new() { ID = id[0..^4], Name = name, NoSpacesName = noSpacesName, Executable = id };

                            // get some info needed for folder in which the uwp app is installed via powershell script
                            scriptFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.ProductName + ".ps1");
                            string scriptContents = @$"$myvar = ""_"" + (Get-AppxPackage -Name ""*{packageName}*""|" +
@$"Get-AppxPackageManifest).package.Identity.Version + ""_"" + (Get-AppxPackage -Name ""*{packageName}*""|" +
                                @$"Get-AppxPackageManifest).package.Identity.ProcessorArchitecture + ""__""
Write-Host $myvar";
                            File.WriteAllText(scriptFile, scriptContents);
                            result = RunPostScript(scriptFile);
                            if (result.Count == 1)
                            {
                                //Example: C:\Program Files\WindowsApps\Microsoft.YourPhone_1.21022.160.0_x64__8wekyb3d8bbwe
                                uwpApp.Folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "WindowsApps", uwpApp.ID.Replace("_", result[0]));
                            }
                            File.Delete(scriptFile);
                            apps.Add(uwpApp);
                        }
                    }
                }
            }
            return apps;
        }

        internal static List<string> RunPostScript(string scriptFile)
        {
            List<string> output = new();
            ProcessStartInfo startInfo = new("powershell", $"-ExecutionPolicy Bypass -File {scriptFile}")
            {
                CreateNoWindow = true,
                Verb = "runas",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            using (Process process = new()
            {
                EnableRaisingEvents = true,
                StartInfo = startInfo
            })
            {
                process.OutputDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    if (e.Data != null)
                    {
                        output.Add(e.Data);
                    }
                };
                process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) =>
                {
                    if (e.Data != null)
                    {
                        output.Add(e.Data);
                    }
                };
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit(2000);
            }
            return output;
        }

        private void GetStartupItemsFromFolder(string folder, int group)
        {
            foreach (string file in Directory.GetFiles(folder))
            {
                FileInfo fi = new(file);
                if (fi is null || fi.Extension.ToLower() == ".ini") continue;
                StartupItem startupItem = new();
                startupItem.ID = m_StartupItems.Count + 1;
                startupItem.GroupIndex = group;
                startupItem.ValueName = fi.Name;
                if (fi.Extension.ToLower() == ".lnk")
                {
                    startupItem.LinkFolder = fi.Directory.FullName;
                    startupItem.LinkName = fi.Name;

                    string targetFile = GetShortcutTarget(file);
                    FileInfo tfi = new(targetFile);
                    startupItem.Folder = tfi.Directory.FullName;
                    startupItem.Executable = tfi.Name;
                    FillFileDetails(targetFile, ref startupItem);
                    if (string.IsNullOrEmpty(startupItem.Name))
                        startupItem.Name = startupItem.Executable;
                    if (string.IsNullOrEmpty(startupItem.Parameters))
                        startupItem.Parameters = "(shortcut)";
                }
                else
                {
                    startupItem.Folder = fi.Directory.FullName;
                    startupItem.Executable = fi.Name;
                    FillFileDetails(file, ref startupItem);
                }
                // check if enabled or disabled
                using RegistryKey regKeyApprovedRun = group == 0 ?
                    Registry.CurrentUser.OpenSubKey(CURRENT_USER_APPROVED_STARTUP_FOLDER_REG.Replace(@"HKEY_CURRENT_USER\", string.Empty)) :
                    Registry.LocalMachine.OpenSubKey(LOCAL_MACHINE_APPROVED_STARTUP_FOLDER_REG.Replace(@"HKEY_LOCAL_MACHINE\", string.Empty));
                if (regKeyApprovedRun != null && !string.IsNullOrEmpty(startupItem.ValueName))
                {
                    byte[] binaryData = (byte[])regKeyApprovedRun.GetValue(startupItem.ValueName);
                    if (binaryData != null && binaryData.Length > 0)
                    {
                        byte flag = binaryData[0];
                        startupItem.Enabled = flag != 3;
                    }
                }
                startupItem.Type = "Folder";
                m_StartupItems.Add(startupItem);
            }
        }

        private static string GetShortcutTarget(string file)
        {
            try
            {
                if (Path.GetExtension(file).ToLower() != ".lnk")
                {
                    throw new Exception("Supplied file must be a .LNK file");
                }

                FileStream fileStream = File.Open(file, FileMode.Open, FileAccess.Read);
                using BinaryReader fileReader = new(fileStream);
                fileStream.Seek(0x14, SeekOrigin.Begin);     // Seek to flags
                uint flags = fileReader.ReadUInt32();        // Read flags
                if ((flags & 1) == 1)
                {                      // Bit 1 set means we have to
                                       // skip the shell item ID list
                    fileStream.Seek(0x4c, SeekOrigin.Begin); // Seek to the end of the header
                    uint offset = fileReader.ReadUInt16();   // Read the length of the Shell item ID list
                    fileStream.Seek(offset, SeekOrigin.Current); // Seek past it (to the file locator info)
                }

                long fileInfoStartsAt = fileStream.Position; // Store the offset where the file info
                                                             // structure begins
                uint totalStructLength = fileReader.ReadUInt32(); // read the length of the whole struct
                fileStream.Seek(0xc, SeekOrigin.Current); // seek to offset to base pathname
                uint fileOffset = fileReader.ReadUInt32(); // read offset to base pathname
                                                           // the offset is from the beginning of the file info struct (fileInfoStartsAt)
                fileStream.Seek(fileInfoStartsAt + fileOffset, SeekOrigin.Begin); // Seek to beginning of
                                                                                  // base pathname (target)
                long pathLength = totalStructLength + fileInfoStartsAt - fileStream.Position - 2; // read
                                                                                                  // the base pathname. I don't need the 2 terminating nulls.
                char[] linkTarget = fileReader.ReadChars((int)pathLength); // should be unicode safe
                var link = new string(linkTarget);

                int begin = link.IndexOf("\0\0");
                if (begin > -1)
                {
                    int end = link.IndexOf(@"\\", begin + 2) + 2;
                    end = link.IndexOf('\0', end) + 1;

                    string firstPart = link.Substring(0, begin);
                    string secondPart = link[end..];

                    return firstPart + secondPart;
                }
                else
                {
                    return link;
                }
            }
            catch
            {
                return string.Empty;
            }
        }
        private void LoadStartupItemsFromRegistry(string registrySubKey)
        {
            bool userReg = registrySubKey.Equals(CURRENT_USER_RUN_REG) || registrySubKey.Equals(CURRENT_USER_RUN_32_REG);
            using RegistryKey regKeyRun = userReg ?
                Registry.CurrentUser.OpenSubKey(registrySubKey.Replace(@"HKEY_CURRENT_USER\", string.Empty)) :
                Registry.LocalMachine.OpenSubKey(registrySubKey.Replace(@"HKEY_LOCAL_MACHINE\", string.Empty));
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
                // check if enabled or disabled
                using RegistryKey regKeyApprovedRun = userReg ?
                    Registry.CurrentUser.OpenSubKey(CURRENT_USER_APPROVED_RUN_REG.Replace(@"HKEY_CURRENT_USER\", string.Empty)) :
                    Registry.LocalMachine.OpenSubKey(LOCAL_MACHINE_APPROVED_RUN_REG.Replace(@"HKEY_LOCAL_MACHINE\", string.Empty));
                if (regKeyApprovedRun != null && !string.IsNullOrEmpty(startupItem.ValueName))
                {
                    byte[] binaryData = (byte[])regKeyApprovedRun.GetValue(startupItem.ValueName);
                    if (binaryData != null && binaryData.Length > 0)
                    {
                        byte flag = binaryData[0];
                        startupItem.Enabled = flag != 3;
                    }
                }
                FillFileDetails(exeAndPath, ref startupItem);
                startupItem.Type = "Registry";
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
                string[] row = { startupItem.Publisher, startupItem.Executable, startupItem.Parameters,
                    startupItem.PartOfOS ? " YES " : " NO ", startupItem.Enabled ? "Enabled" : "Disabled",
                    startupItem.Type };
                listViewItem.SubItems.AddRange(row);
                listViewStartupItems.Items.Add(listViewItem);
            }
            listViewStartupItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listViewStartupItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewStartupItems.EndUpdate();
            int formWidth = 100;
            foreach (ColumnHeader columnHeader in listViewStartupItems.Columns)
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
            LoadStartupItemsFromStartupFolders();
            GetStartupItemsFromUWP();
            PopulateListView();
        }

        private void EnableStartupItems()
        {
            throw new NotImplementedException();
        }

        private void DisableStartupItems()
        {
            throw new NotImplementedException();
        }

        #endregion Private methods
    }
}
