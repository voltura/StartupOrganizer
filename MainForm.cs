#region Using statements

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;

#endregion Using statements

namespace StartupOrganizer
{
    public partial class MainForm : Form
    {
        #region Private variables

        private readonly List<StartupItem> m_StartupItems;
        private readonly AddForm m_AddForm = new();
        private readonly LoadingForm m_LoadingForm = new();

        internal bool UnsavedChanges
        {
            get
            {
                return m_UnsavedChanges;
            }
            set
            {
                m_UnsavedChanges = value;
                btnSave.Enabled = m_UnsavedChanges;
            }
        }

        private bool m_UnsavedChanges;

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
            LoadStartupItems(false, @"Loading Startup Organizer
Please stand by...");
            Tag = "Loaded";
        }

        private void Load_Click(object sender, EventArgs e)
        {
            if (UserConfirmedPerformOperation()) 
                LoadStartupItems(false, @"Loading Startup Items
Please stand by...");
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!UserConfirmedPerformOperation())
            {
                e.Cancel = true;
            }
        }

        private void ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Tag)
            {
                case "Details":
                    ShowDetails();
                    break;
                case "Delete":
                    DeleteStartupItems();
                    break;
                case "Enable":
                    EnableStartupItems();
                    break;
                case "Disable":
                    DisableStartupItems();
                    break;
                case "Copy":
                    CopyItemDetailsToClipboard();
                    break;
            }
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

        private void Copy_Click(object sender, EventArgs e)
        {
            CopyItemDetailsToClipboard();
        }

        private void Setup_Click(object sender, EventArgs e)
        {
            ShowSetup();
        }

        #endregion Events

        #region Private methods

        private bool UserConfirmedPerformOperation()
        {
            bool unsavedChanges = m_StartupItems.Contains(new StartupItem { State = StartupItem.MODIFIED_STATE.MODIFIED | StartupItem.MODIFIED_STATE.NEW });
            if (unsavedChanges && DialogResult.No == MessageBox.Show(this, "Unsaved changes will be lost", "Continue?", MessageBoxButtons.YesNo)) return false;
            return true;
        }

        private void BrowseItem()
        {
            if (listViewStartupItems.SelectedItems is null) return;

            // get first selected ID of list via tag
            ListViewItem item = listViewStartupItems.SelectedItems[0];
            if (item is null) return;
            int id = Convert.ToInt32(item.Tag);

            // get record via ID from m_StartupItems list
            StartupItem match = m_StartupItems.Find(x => x.ID.Equals(id));

            // check if type is folder or UWP
            if (match.Type == StartupItem.TYPE.FOLDER || match.Type == StartupItem.TYPE.UWP)
            {
                // run explorer.exe with folder as param
                ProcessStartInfo info = new() { UseShellExecute = true, WindowStyle = ProcessWindowStyle.Normal, FileName = match.Folder, WorkingDirectory = match.Folder };
                Process.Start(info);
                return;
            }
            else if (match.Type == StartupItem.TYPE.REGISTRY)
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
            if (DialogResult.OK == m_AddForm.ShowDialog(this))
            {
                StartupItem itemToAdd = m_AddForm.StartupItemToAdd;
                FillFileDetails(itemToAdd.FullPath, ref itemToAdd);
                m_StartupItems.Add(itemToAdd);
                UnsavedChanges = true;
                LoadStartupItems(true, @"Updating Startup Item list
Please stand by...");
            }
        }
        private void SaveStartupItems()
        {
            foreach (StartupItem item in m_StartupItems)
            {
                if (item.State == StartupItem.MODIFIED_STATE.MODIFIED || item.State == StartupItem.MODIFIED_STATE.NEW)
                {
                    switch (item.Type)
                    {
                        case StartupItem.TYPE.FOLDER:
                            SaveStartupItemFolder(item);
                            break;
                        case StartupItem.TYPE.REGISTRY:
                            SaveStartupItemRegistry(item);
                            break;
                        case StartupItem.TYPE.UWP:
                            SaveStartupItemUwp();
                            break;
                    }
                }
            }
            UnsavedChanges = false;
        }

        private void SaveStartupItemUwp()
        {
            throw new NotImplementedException();
        }

        private void SaveStartupItemFolder(StartupItem item)
        {
            throw new NotImplementedException();
        }

        private static void SaveStartupItemRegistry(StartupItem item)
        {
            using RegistryKey root = item.RegRoot == Constants.REG_ROOT.HKCU ? Registry.CurrentUser : Registry.LocalMachine;
            using RegistryKey hkcuRun = root.OpenSubKey(Constants.RUN_SUBKEY_REG, true);
            hkcuRun.SetValue(item.ProductName, item.ValueData);
            if (!item.Enabled)
            {
                using RegistryKey allowedRun = root.OpenSubKey(item.Folder.Contains(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)) ? Constants.APPROVED_RUN_SUBKEY_REG32 : Constants.APPROVED_RUN_SUBKEY_REG, true);
                allowedRun.SetValue(item.ProductName,
                    new byte[] { 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 },
                    RegistryValueKind.Binary);
            }
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
                        StartupItem startupItem = new();
                        startupItem.ID = m_StartupItems.Count + 1;
                        startupItem.GroupIndex = 0;
                        startupItem.ValueName = app.Name;
                        startupItem.Folder = app.Folder;
                        startupItem.Publisher = app.ID.Substring(0, app.ID.IndexOf('.') != -1 ? app.ID.IndexOf('.') : app.ID.Length);
                        startupItem.File = app.Executable;
                        startupItem.Enabled = !(state == "0" || state == "1");
                        startupItem.Parameters = string.Empty;
                        startupItem.ProductName = app.Name;
                        startupItem.Type = StartupItem.TYPE.UWP;
                        m_StartupItems.Add(startupItem);
                    }
                }
            }
        }

        private static List<UwpApp> GetUwpApps()
        {
            string scriptFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Application.ProductName + ".ps1");
            List<UwpApp> apps = new();
            try
            {
                File.WriteAllText(scriptFile, "get-StartApps");
                List<string> result = RunPostScript(scriptFile);
                foreach (string row in result)
                {
                    string trimmedRow = row.Trim();
                    if (trimmedRow.Length == 0 || trimmedRow.StartsWith("Name") || trimmedRow.StartsWith("-")) continue;
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
                            string scriptContents = @$"$v = ""_"" + (Get-AppxPackage -Name ""*{packageName}*""|" +
@$"Get-AppxPackageManifest).package.Identity.Version + ""_"" + (Get-AppxPackage -Name ""*{packageName}*""|" +
                                @$"Get-AppxPackageManifest).package.Identity.ProcessorArchitecture + ""__""
Write-Host $v";
                            File.WriteAllText(scriptFile, scriptContents);
                            result = RunPostScript(scriptFile);
                            if (result.Count == 1)
                            {
                                //Example: C:\Program Files\WindowsApps\Microsoft.YourPhone_1.21022.160.0_x64__8wekyb3d8bbwe
                                uwpApp.Folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "WindowsApps", uwpApp.ID.Replace("_", result[0]));
                            }
                            apps.Add(uwpApp);
                        }
                    }
                }
            }
            finally
            {
                if (File.Exists(scriptFile)) File.Delete(scriptFile);
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

        private static void SetFileType(FileInfo fi, ref StartupItem startupItem)
        {
            if (fi == null) return;
            startupItem.FileType = fi.Extension.ToLower() switch
            {
                ".exe" => Constants.FILE_TYPE.EXECUTABLE,
                ".lnk" => Constants.FILE_TYPE.SHORTCUT,
                ".dll" => Constants.FILE_TYPE.DLL,
                _ => Constants.FILE_TYPE.OTHER,
            };
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
                SetFileType(fi, ref startupItem);
                if (startupItem.FileType == Constants.FILE_TYPE.SHORTCUT)
                {
                    startupItem.LinkFolder = fi.Directory.FullName;
                    startupItem.LinkName = fi.Name;
                    string targetFile = GetShortcutTarget(file);
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
                using RegistryKey regKeyApprovedRun = group == 0 ?
                    Registry.CurrentUser.OpenSubKey(Constants.CURRENT_USER_APPROVED_STARTUP_FOLDER_REG.Replace(@"HKEY_CURRENT_USER\", string.Empty)) :
                    Registry.LocalMachine.OpenSubKey(Constants.LOCAL_MACHINE_APPROVED_STARTUP_FOLDER_REG.Replace(@"HKEY_LOCAL_MACHINE\", string.Empty));
                if (regKeyApprovedRun != null && !string.IsNullOrEmpty(startupItem.ValueName))
                {
                    byte[] binaryData = (byte[])regKeyApprovedRun.GetValue(startupItem.ValueName);
                    if (binaryData != null && binaryData.Length > 0)
                    {
                        byte flag = binaryData[0];
                        startupItem.Enabled = flag != 3;
                    }
                }
                startupItem.Type = StartupItem.TYPE.FOLDER;
                startupItem.State = StartupItem.MODIFIED_STATE.UNTOUCHED;
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
                        fileInfo.Directory.FullName.Equals(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), StringComparison.InvariantCultureIgnoreCase)) {
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
                startupItem.ID = m_StartupItems.Count + 1;
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
            listViewStartupItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listViewStartupItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewStartupItems.EndUpdate();
            int formWidth = 100;
            foreach (ColumnHeader columnHeader in listViewStartupItems.Columns)
            {
                formWidth += columnHeader.Width;
            }
            Width = formWidth > Width ? formWidth : Width;
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
                startupItem.Publisher = string.IsNullOrWhiteSpace(fileVersionInfo.CompanyName) ? Constants.UNKNOWN : fileVersionInfo.CompanyName;
            }
            startupItem.CompanyName = string.IsNullOrWhiteSpace(fileVersionInfo.CompanyName) ? startupItem.Publisher : fileVersionInfo.CompanyName;
            FileInfo fi = new(fileName);
            SetFileType(fi, ref startupItem);
            if (fi != null)
            {
                CompilationMode mode = GetCompilationMode(fi);
                bool potential32 = mode.HasFlag(CompilationMode.Bit32) || fi.DirectoryName.Contains(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
                startupItem.X86 = potential32;
            }
        }

        public static CompilationMode GetCompilationMode(FileInfo info)
        {
            if (!info.Exists) throw new ArgumentException($"{info.FullName} does not exist");

            var intPtr = IntPtr.Zero;
            try
            {
                uint unmanagedBufferSize = 4096;
                intPtr = Marshal.AllocHGlobal((int)unmanagedBufferSize);

                using (var stream = File.Open(info.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var bytes = new byte[unmanagedBufferSize];
                    stream.Read(bytes, 0, bytes.Length);
                    Marshal.Copy(bytes, 0, intPtr, bytes.Length);
                }

                //Check DOS header magic number
                if (Marshal.ReadInt16(intPtr) != 0x5a4d) return CompilationMode.Invalid;

                // This will get the address for the WinNT header  
                var ntHeaderAddressOffset = Marshal.ReadInt32(intPtr + 60);

                // Check WinNT header signature
                var signature = Marshal.ReadInt32(intPtr + ntHeaderAddressOffset);
                if (signature != 0x4550) return CompilationMode.Invalid;

                //Determine file bitness by reading magic from IMAGE_OPTIONAL_HEADER
                var magic = Marshal.ReadInt16(intPtr + ntHeaderAddressOffset + 24);

                var result = CompilationMode.Invalid;
                uint clrHeaderSize;
                if (magic == 0x10b)
                {
                    clrHeaderSize = (uint)Marshal.ReadInt32(intPtr + ntHeaderAddressOffset + 24 + 208 + 4);
                    result |= CompilationMode.Bit32;
                }
                else if (magic == 0x20b)
                {
                    clrHeaderSize = (uint)Marshal.ReadInt32(intPtr + ntHeaderAddressOffset + 24 + 224 + 4);
                    result |= CompilationMode.Bit64;
                }
                else return CompilationMode.Invalid;

                result |= clrHeaderSize != 0
                    ? CompilationMode.CLR
                    : CompilationMode.Native;

                return result;
            }
            finally
            {
                if (intPtr != IntPtr.Zero) Marshal.FreeHGlobal(intPtr);
            }
        }

        [Flags]
        public enum CompilationMode
        {
            Invalid = 0,
            Native = 0x1,
            CLR = Native << 1,
            Bit32 = CLR << 1,
            Bit64 = Bit32 << 1
        }

        private void ShowDetails()
        {
            throw new NotImplementedException();
        }

        private void CleanLoadedStartupItems()
        {
            m_StartupItems.Clear();
        }

        private void LoadStartupItems(bool userAddedItem = false, string loadingText = "")
        {
            try
            {
                m_LoadingForm.SetText(this, loadingText);
                m_LoadingForm.Show(this);
                if (!userAddedItem)
                {
                    CleanLoadedStartupItems();
                    LoadStartupItemsFromRegistry(Constants.REG_ROOT.HKCU, false);
                    LoadStartupItemsFromRegistry(Constants.REG_ROOT.HKCU, true);
                    LoadStartupItemsFromRegistry(Constants.REG_ROOT.HKLM, false);
                    LoadStartupItemsFromRegistry(Constants.REG_ROOT.HKLM, true);
                    LoadStartupItemsFromStartupFolders();
                    GetStartupItemsFromUWP();
                }
                UnsavedChanges = userAddedItem;
                PopulateListView();
            }
            finally
            {
                m_LoadingForm.Hide();
            }
        }

        private void EnableStartupItems()
        {
            throw new NotImplementedException();
        }

        private void DisableStartupItems()
        {
            throw new NotImplementedException();
        }

        private void CopyItemDetailsToClipboard()
        {
            throw new NotImplementedException();
        }

        private void ShowSetup()
        {
            throw new NotImplementedException();
        }

        #endregion Private methods
    }
}
