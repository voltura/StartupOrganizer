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

        private void Enabled_CheckedChanged(object sender, EventArgs e)
        {
            rbtnDisabled.Checked = !rbtnEnabled.Checked;
        }

        private void Disabled_CheckedChanged(object sender, EventArgs e)
        {
            rbtnEnabled.Checked = !rbtnDisabled.Checked;
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
                this.txtFileName.Text = ofdSelectStartupItemFile.FileName;
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
            m_StartupItem.Executable = Path.GetFileName(txtFileName.Text);
            m_StartupItem.Folder = Path.GetDirectoryName(txtFileName.Text);
            m_StartupItem.Type = rbtnFolderCurrentUser.Checked || rbtnFolderMachine.Checked ? StartupItem.TYPE.FOLDER : rbtnRegistryHKCU.Checked || rbtnRegistryHKLM.Checked ? StartupItem.TYPE.REGISTRY : StartupItem.TYPE.UWP;
            m_StartupItem.State = StartupItem.MODIFIED_STATE.NEW;
            m_StartupItem.Parameters = txtParameters.Text;
            DialogResult = txtFileName.Text.Length > 0 && File.Exists(txtFileName.Text) ? DialogResult.OK : DialogResult.Cancel;
            Close();
        }

        private void RegistryHKCU_CheckedChanged(object sender, EventArgs e)
        {
            ToogleTypeChecked((RadioButton)sender);
        }

        private void ToogleTypeChecked(RadioButton rb)
        {
            if (!rb.Equals(rbtnFolderCurrentUser)) rbtnFolderCurrentUser.Checked = !rb.Checked;
            if (!rb.Equals(rbtnFolderMachine)) rbtnFolderMachine.Checked = !rb.Checked;
            if (!rb.Equals(rbtnRegistryHKCU)) rbtnRegistryHKCU.Checked = !rb.Checked;
            if (!rb.Equals(rbtnRegistryHKLM)) rbtnRegistryHKLM.Checked = !rb.Checked;
            if (!rb.Equals(rbtnUwp)) rbtnUwp.Checked = !rb.Checked;
        }

        private void RegistryHKLM_CheckedChanged(object sender, EventArgs e)
        {
            ToogleTypeChecked((RadioButton)sender);
        }

        private void FolderCurrentUser_CheckedChanged(object sender, EventArgs e)
        {
            ToogleTypeChecked((RadioButton)sender);
        }

        private void FolderMachine_CheckedChanged(object sender, EventArgs e)
        {
            ToogleTypeChecked((RadioButton)sender);
        }

        private void Uwp_CheckedChanged(object sender, EventArgs e)
        {
            ToogleTypeChecked((RadioButton)sender);
        }
    }
}
