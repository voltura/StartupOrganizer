using System;
using System.IO;
using System.Windows.Forms;

namespace StartupOrganizer
{
    public partial class AddForm : Form
    {
        internal StartupItem StartupItemToAdd
        {
            get
            {
                return m_StartupItem;
            }
            private set { }
        }

        private StartupItem m_StartupItem;
        public AddForm()
        {
            InitializeComponent();
        }

        private void AddForm_Load(object sender, EventArgs e)
        {
            ofdSelectStartupItemFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            m_StartupItem = new StartupItem();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == ofdSelectStartupItemFile.ShowDialog(this))
            {
                txtFileName.Text = ofdSelectStartupItemFile.FileName;
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            m_StartupItem.Enabled = rbtnEnabled.Checked;
            m_StartupItem.File = Path.GetFileName(txtFileName.Text);
            m_StartupItem.Folder = Path.GetDirectoryName(txtFileName.Text);
            m_StartupItem.Type = rbtnFolderCurrentUser.Checked || rbtnFolderMachine.Checked ? 
                StartupItem.TYPE.FOLDER : rbtnRegistryHKCU.Checked || rbtnRegistryHKLM.Checked ? 
                StartupItem.TYPE.REGISTRY : StartupItem.TYPE.UWP;
            if (rbtnRegistryHKCU.Checked || rbtnFolderCurrentUser.Checked)
                m_StartupItem.RegRoot = Constants.REG_ROOT.HKCU;
            else if (rbtnRegistryHKLM.Checked || rbtnFolderMachine.Checked)
                m_StartupItem.RegRoot = Constants.REG_ROOT.HKLM;
            // Folder
            if (rbtnFolderCurrentUser.Checked)
            {
                //%USERPROFILE%\AppData\Roaming\Microsoft\Windows\Start Menu
                string usersAppDataStartMenuFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    @"Microsoft\Windows\Start Menu\Programs\Startup");
                m_StartupItem.LinkFolder = usersAppDataStartMenuFolder;
            } else if (rbtnFolderMachine.Checked)
            {
                //%ProgramData%\Microsoft\Windows\Start Menu\Programs\Startup 
                string systemAppDataStartMenuFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    @"Microsoft\Windows\Start Menu\Programs\Startup");
                m_StartupItem.LinkFolder = systemAppDataStartMenuFolder;
            }
            if (m_StartupItem.Type == StartupItem.TYPE.FOLDER)
            {
                m_StartupItem.LinkName = Path.GetFileNameWithoutExtension(m_StartupItem.File) + ".lnk";
                m_StartupItem.RegistryKey = Constants.APPROVED_STARTUP_FOLDER_SUBKEY_REG;
                m_StartupItem.Kind = Microsoft.Win32.RegistryValueKind.Binary;
                m_StartupItem.ValueName = m_StartupItem.LinkName;
                m_StartupItem.BinaryValueData = new byte[] { (byte)(m_StartupItem.Enabled ? Constants.STATUS.ENABLED : Constants.STATUS.DISABLED), 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            }
            m_StartupItem.State = StartupItem.MODIFIED_STATE.NEW;
            m_StartupItem.Parameters = txtParameters.Text;
            if (rbtnRegistryHKCU.Checked || rbtnRegistryHKLM.Checked)
            {
                m_StartupItem.Kind = Microsoft.Win32.RegistryValueKind.String;
                m_StartupItem.ValueData = $"\"{txtFileName.Text}\" {txtParameters.Text}";
            }
            m_StartupItem.GroupIndex = rbtnRegistryHKCU.Checked || rbtnFolderCurrentUser.Checked || rbtnUwp.Checked ? 0 : 1;
            DialogResult = txtFileName.Text.Length > 0 && File.Exists(txtFileName.Text) ? DialogResult.OK : DialogResult.Cancel;
            Close();
        }

        private void FileName_TextChanged(object sender, EventArgs e)
        {
            btnAdd.Enabled = txtFileName.TextLength > 3;
        }
    }
}
