#region Using statements

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;

#endregion Using statements

#region Notes

// Registry
//Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
//Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run32
//Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce
//Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
//Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run32
//Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\RunOnce

// Folder
//%ProgramData%\Microsoft\Windows\Start Menu\Programs\Startup 
//%USERPROFILE%\AppData\Roaming\Microsoft\Windows\Start Menu

//Scheduled Tasks that is set to run at startup
//Services that are set to automatic

//users start menu startup folders

// Enabled|disabled info
//Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run
//Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run
//Computer\HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\StartupFolder
//Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\StartupFolder

//Depends on the first binary of the value: 0000 0000B, the least significant bit is turn on(0)/off(1). and the 5th is the switch enable(0)/disable(1) bit, for example: set xxxx 1xxxB for the first binary, the switch in windows setting will be:
//However, we usually set the first binary bit to 0x02 (enable) / 0x03 (disable).

// UWP apps 

//Computer\HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\
//Example "Your Phone" sub structure: Microsoft.YourPhone_8wekyb3d8bbwe\YourPhone\
//State=0 default disabled, 2 enabled, 1 disabled
//UserEnabledStartupOnce 0|1

//https://stackoverflow.com/questions/41160159/get-list-of-installed-windows-apps
//https://stackoverflow.com/questions/56265062/programmatically-get-list-of-installed-application-executables-windows10-c

//powershell
//get-StartApps
//parse all ending with !App

//navigate to 
//Computer\HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\
// <index of last space> . remove !App _add_ \ and first part until tab or multiple space, replace space with ''.
//set state 
//State 0 default disabled, 2 enabled 1 disabled
//and 
//UserEnabledStartupOnce 1

#endregion Notes

namespace StartupOrganizer
{
    internal class StartupHandler
    {
        #region Private variables

        private readonly List<StartupItem> m_StartupItems = new();

        #endregion Private variables

        #region Public properties

        public List<StartupItem> StartupItems
        {
            get
            {
                return m_StartupItems;
            }
            private set
            {
            }
        }

        public bool UnsavedChanges
        {
            get
            {
                return StartupItems.Exists(x => x.State == StartupItem.MODIFIED_STATE.MODIFIED) ||
                    StartupItems.Exists(x => x.State == StartupItem.MODIFIED_STATE.NEW);
            }
            private set
            {
            }
        }

        #endregion Public properties

        #region Public functions

        public void BrowseItem(System.Windows.Forms.ListViewItem listViewItem)
        {
            if (listViewItem is null) return;
            int id = Convert.ToInt32(listViewItem.Tag);
            if (!StartupItems.Exists(x => x.ID.Equals(id))) return;

            // get record via ID from StartupItems list
            StartupItem item = StartupItems.Find(x => x.ID.Equals(id));

            // check if type is folder or UWP
            if (item.Type == StartupItem.TYPE.FOLDER || item.Type == StartupItem.TYPE.UWP)
            {
                // run explorer.exe with folder as param
                ProcessStartInfo info = new() { UseShellExecute = true, WindowStyle = ProcessWindowStyle.Normal, FileName = item.Folder, WorkingDirectory = item.Folder };
                Process.Start(info);
                return;
            }
            else if (item.Type == StartupItem.TYPE.REGISTRY)
            {
                string key = $@"Computer\{item.RegistryKey}";
                string regScript = @$"REG ADD HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Applets\Regedit /v LastKey /d ""{key}"" /F
START regedit.exe";
                string scriptFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    $"{System.Windows.Forms.Application.ProductName}_REG.ps1");
                File.WriteAllText(scriptFile, regScript);
                FileHandler.RunPostScript(scriptFile);
                File.Delete(scriptFile);
                return;
            }
            throw new NotImplementedException();
        }

        public void RestoreBackedUpStartupItems(System.Windows.Forms.ListView list)
        {
            throw new NotImplementedException();
        }

        public void BackupStartupItems()
        {
            throw new NotImplementedException();
        }

        public void DeleteStartupItems()
        {
            throw new NotImplementedException();
        }

        public bool ItemAlreadyExist(StartupItem item)
        {
            return StartupItems.Exists(x => x.FullPath == item.FullPath) &&
                StartupItems.Exists(x => x.Parameters == item.Parameters);
        }

        public void AddStartupItem(StartupItem itemToAdd)
        {
            FillFileDetails(itemToAdd.FullPath, ref itemToAdd);
            StartupItems.Add(itemToAdd);
        }

        public void SaveStartupItems()
        {
            foreach (StartupItem item in StartupItems)
            {
                if (item.State != StartupItem.MODIFIED_STATE.MODIFIED &&
                    item.State != StartupItem.MODIFIED_STATE.NEW) //TODO: Add delete feature
                {
                    continue;
                }
                switch (item.Type)
                {
                    case StartupItem.TYPE.FOLDER:
                        SaveStartupItemFolder(item);
                        break;
                    case StartupItem.TYPE.REGISTRY:
                        SaveStartupItemRegistry(item);
                        break;
                    case StartupItem.TYPE.UWP:
                        SaveStartupItemUwp(item);
                        break;
                }
            }
        }

        public void PopulateListView(System.Windows.Forms.ListView listViewStartupItems)
        {
            listViewStartupItems.BeginUpdate();
            listViewStartupItems.Items.Clear();
            foreach (StartupItem startupItem in StartupItems)
            {
                System.Windows.Forms.ListViewItem listViewItem = new()
                {
                    Group = listViewStartupItems.Groups[startupItem.GroupIndex],
                    Tag = Convert.ToString(startupItem.ID),
                    Text = string.IsNullOrEmpty(startupItem.FileDescription) ? startupItem.ProductName : startupItem.FileDescription
                };
                string publisherAndCompany = string.IsNullOrWhiteSpace(startupItem.CompanyName) || startupItem.CompanyName == startupItem.Publisher ?
                    $"{startupItem.Publisher}" : $"{startupItem.CompanyName} ({startupItem.Publisher})";
                string[] row = { publisherAndCompany, startupItem.File, string.IsNullOrEmpty(startupItem.Parameters) ? "" : startupItem.Parameters,
                    startupItem.PartOfOS ? " YES " : " NO ", startupItem.Enabled ? "Enabled" : "Disabled",
                    startupItem.Type.ToString() };
                listViewItem.SubItems.AddRange(row);
                listViewStartupItems.Items.Add(listViewItem);
            }
            listViewStartupItems.EndUpdate();
        }

        public void LoadStartupItems()
        {
            CleanLoadedStartupItems();
            LoadStartupItemsFromRegistry(Constants.REG_ROOT.HKCU, false);
            LoadStartupItemsFromRegistry(Constants.REG_ROOT.HKCU, true);
            LoadStartupItemsFromRegistry(Constants.REG_ROOT.HKLM, false);
            LoadStartupItemsFromRegistry(Constants.REG_ROOT.HKLM, true);
            LoadStartupItemsFromStartupFolders();
            GetStartupItemsFromUWP();
        }

        public void EnableStartupItems()
        {
            throw new NotImplementedException();
        }

        public void DisableStartupItems()
        {
            throw new NotImplementedException();
        }

        public void CopyItemDetailsToClipboard()
        {
            throw new NotImplementedException();
        }

        #endregion Public methods

        #region Private methods

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
            GetUwpAppsFromRegistry(uwpApps, Constants.REG_ROOT.HKCU);
            GetUwpAppsFromRegistry(uwpApps, Constants.REG_ROOT.HKLM);
        }

        private void GetUwpAppsFromRegistry(List<UwpApp> uwpApps, Constants.REG_ROOT root)
        {
            foreach (UwpApp app in uwpApps)
            {
                string regKeyUwpApp = @$"{Constants.UWP_APP_SUBKEY_REG}\{app.ID}\{app.NoSpacesName}";
                RegistryKey k;
                if (root == Constants.REG_ROOT.HKCU)
                {
                    k = Registry.CurrentUser.OpenSubKey(regKeyUwpApp);
                    if (k is null) k = Registry.CurrentUser.OpenSubKey(regKeyUwpApp + "StartupId");
                }
                else
                {
                    // NOTE: Will not really get anything here...
                    k = Registry.LocalMachine.OpenSubKey(regKeyUwpApp);
                    if (k is null) k = Registry.LocalMachine.OpenSubKey(regKeyUwpApp + "StartupId");
                }
                if (k is null) continue;
                string state = k.GetValue("State").ToString();
                StartupItem startupItem = new()
                {
                    ID = StartupItems.Count + 1,
                    GroupIndex = 0,
                    ValueName = app.Name,
                    Folder = app.Folder,
                    Publisher = app.ID.Substring(0, app.ID.IndexOf('.') != -1 ? app.ID.IndexOf('.') : app.ID.Length),
                    File = app.Executable,
                    Enabled = !(state == "0" || state == "1"),
                    Parameters = string.Empty,
                    ProductName = app.Name,
                    Type = StartupItem.TYPE.UWP,
                    UwpAppDetails = app
                };
                StartupItems.Add(startupItem);
            }
        }

        private void GetStartupItemsFromFolder(string folder, int group)
        {
            foreach (string file in Directory.GetFiles(folder))
            {
                FileInfo fi = new(file);
                if (fi is null || fi.Extension.ToLower() == ".ini") continue;
                StartupItem startupItem = new();
                startupItem.ID = StartupItems.Count + 1;
                startupItem.GroupIndex = group;
                startupItem.ValueName = fi.Name;
                startupItem.FileType = FileHandler.GetFileType(file);
                if (startupItem.FileType == Constants.FILE_TYPE.SHORTCUT)
                {
                    startupItem.LinkFolder = fi.Directory.FullName;
                    startupItem.LinkName = fi.Name;
                    string targetFile = Shortcut.GetShortcutTarget(file);
                    FileInfo tfi = new(targetFile);
                    startupItem.Folder = tfi.Directory.FullName;
                    startupItem.File = tfi.Name;
                    startupItem.Parameters = string.Empty;
                    FillFileDetails(targetFile, ref startupItem);
                    if (string.IsNullOrEmpty(startupItem.ProductName))
                        startupItem.ProductName = startupItem.File;
                }
                else
                {
                    startupItem.Folder = fi.Directory.FullName;
                    startupItem.File = fi.Name;
                    FillFileDetails(file, ref startupItem);
                }
                // check if enabled or disabled
                startupItem.RegRoot = group == 0 ? Constants.REG_ROOT.HKCU : Constants.REG_ROOT.HKLM;
                using RegistryKey root = startupItem.RegRoot == Constants.REG_ROOT.HKCU ? Registry.CurrentUser : Registry.LocalMachine;
                using RegistryKey regKeyApprovedRun = root.OpenSubKey(Constants.APPROVED_STARTUP_FOLDER_SUBKEY_REG);
                if (regKeyApprovedRun != null && !string.IsNullOrEmpty(startupItem.ValueName))
                {
                    byte[] binaryData = (byte[])regKeyApprovedRun.GetValue(startupItem.ValueName);
                    if (binaryData != null && binaryData.Length > 0)
                    {
                        byte flag = binaryData[0];
                        startupItem.Enabled = flag != 3;
                        startupItem.BinaryValueData = binaryData;
                        startupItem.Kind = RegistryValueKind.Binary;
                        startupItem.RegistryKey = Constants.APPROVED_STARTUP_FOLDER_SUBKEY_REG;
                    }
                }
                startupItem.Type = StartupItem.TYPE.FOLDER;
                startupItem.State = StartupItem.MODIFIED_STATE.UNTOUCHED;
                StartupItems.Add(startupItem);
            }
        }

        private void LoadStartupItemsFromRegistry(Constants.REG_ROOT regRoot, bool reg32)
        {
            using RegistryKey root = regRoot == Constants.REG_ROOT.HKCU ? Registry.CurrentUser : Registry.LocalMachine;
            using RegistryKey regKeyRun = reg32 ?
                root.OpenSubKey(Constants.RUN_SUBKEY_REG32) : root.OpenSubKey(Constants.RUN_SUBKEY_REG);
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
                string parameters = string.Empty;
                string fileAndPath = string.Empty;
                bool isWindowsRunDll = false;
                if (runValueData.Contains("rundll32.exe", StringComparison.InvariantCultureIgnoreCase)) // can be multitude of files executed here, see https://www.computerhope.com/issues/ch000570.htm
                {
                    // it is a file executed via rundll32.exe
                    // example C:\Windows\system32\rundll32.exe C:\Windows\System32\LogiLDA.dll,LogiFetch
                    string runDllFile = runValueData.Split(' ', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries)[0];
                    string runDllParameters = runValueData[runDllFile.Length..].Trim();
                    runDllFile = runDllFile.Trim('"');
                    FileInfo fileInfo = new(runDllFile);
                    if (fileInfo != null && fileInfo.Directory.FullName.Equals(Environment.GetFolderPath(Environment.SpecialFolder.System), StringComparison.InvariantCultureIgnoreCase) ||
                        fileInfo.Directory.FullName.Equals(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), StringComparison.InvariantCultureIgnoreCase))
                    {
                        // the rundll32.exe file resides in Windows directory, most likely it is the real rundll32.exe file
                        FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(runDllFile);
                        if (fileVersionInfo != null && fileVersionInfo.ProductName == Constants.WINDOWS_OS_PRODUCT_NAME)
                        {
                            isWindowsRunDll = true;
                        }
                    }
                    if (runDllParameters.Contains(','))
                    {
                        // has parameters
                        parameters = runDllParameters.Length > (runDllParameters.IndexOf(',') + 1) ? runDllParameters[(runDllParameters.IndexOf(',') + 1)..] : string.Empty;
                        fileAndPath = runDllParameters[..runDllParameters.IndexOf(',')];
                    }
                    else
                    {
                        // no parameters
                        fileAndPath = runDllParameters.Trim('"');
                    }
                }
                else if (
                    (runValueData.Trim().Contains("rundll32", StringComparison.InvariantCultureIgnoreCase) && (
                            runValueData.Trim().Trim('"').StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.System)) ||
                            runValueData.Trim().Trim('"').StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86))
                        )) || runValueData.Trim().Trim('"').StartsWith("rundll32", StringComparison.InvariantCultureIgnoreCase)
                    ) // could be a file executed by rundll32.exe
                {
                    // example rundll32 C:\Windows\System32\LogiLDA.dll,LogiFetch
                    string runDllFile = runValueData.Split(' ', StringSplitOptions.RemoveEmptyEntries & StringSplitOptions.TrimEntries)[0];
                    string runDllParameters = runValueData[runDllFile.Length..].Trim();
                    runDllFile = runDllFile.Trim('"');
                    FileInfo fileInfo = new(runDllFile + ".exe"); // TODO: Check if rundll32 is accessable via PATH and add directory part from path here
                    if (fileInfo != null &&
                        fileInfo.Directory.FullName.Equals(Environment.GetFolderPath(Environment.SpecialFolder.System), StringComparison.InvariantCultureIgnoreCase) ||
                        fileInfo.Directory.FullName.Equals(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), StringComparison.InvariantCultureIgnoreCase))
                    {
                        // the rundll32.exe file resides in Windows directory, most likely it is the real rundll32.exe file
                        FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(runDllFile);
                        if (fileVersionInfo != null && fileVersionInfo.ProductName == Constants.WINDOWS_OS_PRODUCT_NAME)
                        {
                            isWindowsRunDll = true;
                        }
                    }
                    if (runDllParameters.Contains(','))
                    {
                        // has parameters
                        parameters = runDllParameters.Length > (runDllParameters.IndexOf(',') + 1) ? runDllParameters[(runDllParameters.IndexOf(',') + 1)..] : string.Empty;
                        fileAndPath = runDllParameters[..runDllParameters.IndexOf(',')];
                    }
                    else
                    {
                        // no parameters
                        fileAndPath = runDllParameters.Trim('"');
                    }
                }
                else
                {
                    if (runValueData.Contains('"'))
                        pathAndParams = runValueData.Split('"', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    else if (runValueData.Contains('/'))
                        pathAndParams = new string[] { runValueData.Substring(0, runValueData.IndexOf('/') - 1).Trim(),
                        runValueData[runValueData.IndexOf('/')..].Trim() };
                    else
                        pathAndParams = new string[] { runValueData.Trim() };
                    parameters = pathAndParams != null ? (pathAndParams.Length > 1 ? pathAndParams?[1] : string.Empty) : string.Empty;
                    fileAndPath = pathAndParams != null ? pathAndParams?[0] : string.Empty;
                }
                FileInfo fi = File.Exists(fileAndPath) ? new(fileAndPath) : null;
                StartupItem startupItem = new();
                startupItem.ID = StartupItems.Count + 1;
                startupItem.RegistryKey = regKeyRun.Name;
                startupItem.GroupIndex = regRoot == Constants.REG_ROOT.HKCU ? 0 : 1;
                if (fi is null)
                {
                    if (fileAndPath.Contains('\\'))
                    {
                        startupItem.Folder = fileAndPath[..fileAndPath.LastIndexOf('\\')];
                        startupItem.File = fileAndPath.Length > fileAndPath.LastIndexOf('\\') + 1 ? fileAndPath[(fileAndPath.LastIndexOf('\\') + 1)..] : fileAndPath;
                    }
                    else if (fileAndPath.Contains('/'))
                    {
                        startupItem.Folder = fileAndPath[..fileAndPath.LastIndexOf('/')];
                        startupItem.File = fileAndPath.Length > fileAndPath.LastIndexOf('/') + 1 ? fileAndPath[(fileAndPath.LastIndexOf('/') + 1)..] : fileAndPath;
                    }
                    else
                    {
                        startupItem.File = fileAndPath;
                        startupItem.Folder = string.Empty;
                    }
                }
                else
                {
                    startupItem.File = fi.Name;
                    startupItem.Folder = fi.Directory.FullName;
                }
                startupItem.StartedWithRunDLL = isWindowsRunDll;
                startupItem.Kind = kind;
                startupItem.ValueName = runValueName;
                startupItem.ValueData = runValueData;
                startupItem.Parameters = parameters;
                startupItem.State = StartupItem.MODIFIED_STATE.UNTOUCHED;
                // check if enabled or disabled
                using RegistryKey regKeyApprovedRun = root.OpenSubKey(Constants.APPROVED_RUN_SUBKEY_REG);
                if (regKeyApprovedRun != null && !string.IsNullOrEmpty(startupItem.ValueName))
                {
                    byte[] binaryData = (byte[])regKeyApprovedRun.GetValue(startupItem.ValueName);
                    if (binaryData != null && binaryData.Length > 0)
                    {
                        byte flag = binaryData[0];
                        startupItem.Enabled = flag != 3;
                    }
                }
                FillFileDetails(fileAndPath, ref startupItem);
                startupItem.Type = StartupItem.TYPE.REGISTRY;
                StartupItems.Add(startupItem);
            }
        }

        private void CleanLoadedStartupItems()
        {
            StartupItems.Clear();
        }

        #endregion Private methods

        #region Private static methods and functions

        private static void SaveStartupItemUwp(StartupItem item)
        {
            if (item.State == StartupItem.MODIFIED_STATE.DELETED) throw new NotImplementedException();
            //Example:
            //Computer\HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\
            //Microsoft.549981C3F5F10_8wekyb3d8bbwe
            //\CortanaStartupId

            // set ENABLED or DISABLED (only works for current user...)
            string regKeyUwpApp = @$"{Constants.UWP_APP_SUBKEY_REG}\{item.UwpAppDetails.ID}\{item.UwpAppDetails.NoSpacesName}";
            RegistryKey k = null;
            if (item.RegRoot == Constants.REG_ROOT.HKCU)
            {
                k = Registry.CurrentUser.OpenSubKey(regKeyUwpApp, RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.FullControl);
                if (k is null) k = Registry.CurrentUser.OpenSubKey(regKeyUwpApp + "StartupId", RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.FullControl);
            }
            if (k is null)
            {
                // NOTE: Will not really get anything here...
                k = Registry.LocalMachine.OpenSubKey(regKeyUwpApp, RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.FullControl);
                if (k is null) k = Registry.LocalMachine.OpenSubKey(regKeyUwpApp + "StartupId", RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.FullControl);
                if (k is null) k = Registry.CurrentUser.OpenSubKey(regKeyUwpApp, RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.FullControl);
                if (k is null) k = Registry.CurrentUser.OpenSubKey(regKeyUwpApp + "StartupId", RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.FullControl);
            }
            if (k is null) return;
            k.SetValue("State", new byte[] { (byte)(item.Enabled ? Constants.STATUS.ENABLED : Constants.STATUS.DISABLED), 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, RegistryValueKind.Binary);
        }

        private static void FillFileDetails(string fileName, ref StartupItem startupItem)
        {
            startupItem.Publisher = Constants.UNKNOWN;
            startupItem.ProductName = startupItem.ValueName;
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

            if (!string.IsNullOrEmpty(productName) && productName == Constants.WINDOWS_OS_PRODUCT_NAME)
            {
                startupItem.PartOfOS = true;
            }
            if (!string.IsNullOrEmpty(productName) && !startupItem.PartOfOS)
            {
                startupItem.ProductName = productName;
            }
            if (!string.IsNullOrEmpty(fileVersionInfo.FileDescription))
            {
                startupItem.FileDescription = fileVersionInfo.FileDescription;
            }
            startupItem.ProductVersion = fileVersionInfo.ProductVersion;
            startupItem.FileVersion = fileVersionInfo.FileVersion;
            if (startupItem.Publisher.Equals(Constants.UNKNOWN))
            {
                startupItem.Publisher = string.IsNullOrWhiteSpace(fileVersionInfo.CompanyName) ?
                    Constants.UNKNOWN : fileVersionInfo.CompanyName;
            }
            startupItem.CompanyName = string.IsNullOrWhiteSpace(fileVersionInfo.CompanyName) ?
                startupItem.Publisher : fileVersionInfo.CompanyName;
            startupItem.FileType = FileHandler.GetFileType(fileName);
            startupItem.X86 = FileHandler.PotentiallyA32bitApplication(fileName);
        }

        private static void SaveStartupItemFolder(StartupItem item)
        {
            // create a shortcut, place in startup folder
            string shortcutFileName = Path.Combine(item.LinkFolder, item.LinkName);
            Shortcut.Create(shortcutFileName, item.FullPath, item.Parameters, item.Folder, item.FileDescription, null);

            // add to registry if enabled or not
            using RegistryKey root = item.RegRoot == Constants.REG_ROOT.HKCU ? Registry.CurrentUser : Registry.LocalMachine;
            using RegistryKey allowedRun = root.OpenSubKey(item.RegistryKey, RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.FullControl);
            allowedRun.SetValue(item.LinkName,
                new byte[] { (byte)(item.Enabled ? Constants.STATUS.ENABLED : Constants.STATUS.DISABLED), 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RegistryValueKind.Binary);
        }

        private static void SaveStartupItemRegistry(StartupItem item)
        {
            using RegistryKey root = item.RegRoot == Constants.REG_ROOT.HKCU ? Registry.CurrentUser : Registry.LocalMachine;
            using RegistryKey run = root.OpenSubKey(Constants.RUN_SUBKEY_REG, true);
            run.SetValue(item.ProductName, item.ValueData);
            using RegistryKey allowedRun = root.OpenSubKey(item.Folder.Contains(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)) ?
                Constants.APPROVED_RUN_SUBKEY_REG32 : Constants.APPROVED_RUN_SUBKEY_REG, RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.FullControl);
            allowedRun.SetValue(item.ProductName,
                new byte[] { (byte)(item.Enabled ? Constants.STATUS.ENABLED : Constants.STATUS.DISABLED), 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                RegistryValueKind.Binary);
        }

        private static List<UwpApp> GetUwpApps()
        {
            string scriptFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                System.Windows.Forms.Application.ProductName + ".ps1");
            List<UwpApp> apps = new();
            try
            {
                File.WriteAllText(scriptFile, "get-StartApps");
                List<string> result = FileHandler.RunPostScript(scriptFile);
                foreach (string row in result)
                {
                    string trimmedRow = row.Trim();
                    if (trimmedRow.Length == 0 || trimmedRow.StartsWith("Name") || trimmedRow.StartsWith("-")) continue;
                    int doubleSpaceIndex = trimmedRow.IndexOf("  "); //NOTE: potentially miss app with longest name, but yeah
                    if (doubleSpaceIndex == -1) continue;
                    string name = trimmedRow[0..doubleSpaceIndex];
                    string noSpacesName = name.Replace(" ", string.Empty);
                    string id = trimmedRow[doubleSpaceIndex..].Trim();
                    if (!id.EndsWith("!App")) continue;
                    string packageName = id.IndexOf('_') > 0 ? id[..id.IndexOf('_')] : noSpacesName;
                    UwpApp uwpApp = new() { ID = id[0..^4], Name = name, NoSpacesName = noSpacesName, Executable = id };

                    // get some info needed for folder in which the uwp app is installed via powershell script
                    string scriptContents = @$"$v = ""_"" + (Get-AppxPackage -Name ""*{packageName}*""|" +
@$"Get-AppxPackageManifest).package.Identity.Version + ""_"" + (Get-AppxPackage -Name ""*{packageName}*""|" +
                        @$"Get-AppxPackageManifest).package.Identity.ProcessorArchitecture + ""__""
Write-Host $v";
                    File.WriteAllText(scriptFile, scriptContents);
                    result = FileHandler.RunPostScript(scriptFile);
                    if (result.Count == 1)
                    {
                        //Example: C:\Program Files\WindowsApps\Microsoft.YourPhone_1.21022.160.0_x64__8wekyb3d8bbwe
                        uwpApp.Folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                            "WindowsApps", uwpApp.ID.Replace("_", result[0]));
                    }
                    apps.Add(uwpApp);
                }
            }
            finally
            {
                if (File.Exists(scriptFile)) File.Delete(scriptFile);
            }
            return apps;
        }

        #endregion Private static methods and functions
    }
}
