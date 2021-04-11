#region Using statements

using System;
using System.Windows.Forms;

#endregion Using statements

namespace StartupOrganizer
{
    public partial class MainForm : Form
    {
        #region Private variables

        private readonly StartupHandler m_StartupHandler = new();
        private readonly AddForm m_AddForm = new();
        private readonly LoadingForm m_LoadingForm = new();

        #endregion Private variables

        #region Constructor

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Private Events

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
                    m_StartupHandler.DeleteStartupItems();
                    break;
                case "Enable":
                    m_StartupHandler.EnableStartupItems();
                    break;
                case "Disable":
                    m_StartupHandler.DisableStartupItems();
                    break;
                case "Copy":
                    m_StartupHandler.CopyItemDetailsToClipboard();
                    break;
            }
        }

        private void Details_Click(object sender, EventArgs e)
        {
            ShowDetails();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            m_StartupHandler.SaveStartupItems();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            AddStartupItem();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            m_StartupHandler.DeleteStartupItems();
        }

        private void Backup_Click(object sender, EventArgs e)
        {
            m_StartupHandler.BackupStartupItems();
        }

        private void Restore_Click(object sender, EventArgs e)
        {
            RestoreBackedUpStartupItems();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            // get first selected ID of list via tag
            ListViewItem item = listViewStartupItems.SelectedItems[0];
            m_StartupHandler.BrowseItem(item);
        }

        private void Enable_Click(object sender, EventArgs e)
        {
            m_StartupHandler.EnableStartupItems();
        }

        private void Disable_Click(object sender, EventArgs e)
        {
            m_StartupHandler.DisableStartupItems();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            m_StartupHandler.CopyItemDetailsToClipboard();
        }

        private void Setup_Click(object sender, EventArgs e)
        {
            ShowSetup();
        }

        #endregion Private Events

        #region Private methods

        private bool UserConfirmedPerformOperation()
        {
            if (m_StartupHandler.UnsavedChanges &&
                DialogResult.No == MessageBox.Show(this, "Unsaved changes will be lost", "Continue?", MessageBoxButtons.YesNo))
                return false;
            return true;
        }

        private void RestoreBackedUpStartupItems()
        {
            m_StartupHandler.RestoreBackedUpStartupItems(listViewStartupItems);
        }

        private void AddStartupItem()
        {
            if (DialogResult.OK != m_AddForm.ShowDialog(this)) return;
            StartupItem itemToAdd = m_AddForm.StartupItemToAdd;
            if (m_StartupHandler.ItemAlreadyExist(itemToAdd))
            {
                MessageBox.Show(this, "A startup item with same file path and parameters already exist. Modify existing instead.", "Item not added", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            m_StartupHandler.AddStartupItem(itemToAdd);
            LoadStartupItems(true, @"Updating Startup Item list
Please stand by...");
        }

        private void PopulateListView()
        {
            m_StartupHandler.PopulateListView(listViewStartupItems);
            MakeListViewColumnsFitContent();
            MakeFormWithFitListViewWidth();
            btnSave.Enabled = m_StartupHandler.UnsavedChanges;
        }

        private void MakeFormWithFitListViewWidth()
        {
            int formWidth = 100;
            foreach (ColumnHeader columnHeader in listViewStartupItems.Columns)
                formWidth += columnHeader.Width;
            Width = formWidth > Width ? formWidth : Width;
        }

        private void MakeListViewColumnsFitContent()
        {
            listViewStartupItems.BeginUpdate();
            listViewStartupItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listViewStartupItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewStartupItems.EndUpdate();
        }

        private void ShowDetails()
        {
            throw new NotImplementedException();
        }

        private void LoadStartupItems(bool userAddedItem = false, string loadingText = "")
        {
            try
            {
                m_LoadingForm.SetText(this, loadingText);
                m_LoadingForm.Show(this);
                if (!userAddedItem)
                {
                    m_StartupHandler.LoadStartupItems();
                }
                PopulateListView();
            }
            finally
            {
                m_LoadingForm.Hide();
            }
        }

        private void ShowSetup()
        {
            throw new NotImplementedException();
        }

        #endregion Private methods
    }
}
