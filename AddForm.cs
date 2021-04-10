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
            m_StartupItem.Type = rbtnFolderCurrentUser.Checked || rbtnFolderMachine.Checked ? StartupItem.TYPE.FOLDER : rbtnRegistryHKCU.Checked || rbtnRegistryHKLM.Checked ? StartupItem.TYPE.REGISTRY : StartupItem.TYPE.UWP;
            if (rbtnRegistryHKCU.Checked)
                m_StartupItem.RegRoot = Constants.REG_ROOT.HKCU;
            if (rbtnRegistryHKLM.Checked)
                m_StartupItem.RegRoot = Constants.REG_ROOT.HKLM;
            m_StartupItem.State = StartupItem.MODIFIED_STATE.NEW;
            m_StartupItem.Parameters = txtParameters.Text;
            if (rbtnRegistryHKCU.Checked || rbtnRegistryHKLM.Checked)
                m_StartupItem.ValueData = $"\"{txtFileName.Text}\" {txtParameters.Text}";
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
