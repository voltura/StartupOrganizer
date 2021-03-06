
namespace StartupOrganizer
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Current User", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Local Machine", System.Windows.Forms.HorizontalAlignment.Left);
            this.listViewStartupItems = new System.Windows.Forms.ListView();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderPublisher = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderExecutable = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderParam = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderOS = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderStatus = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderType = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderState = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemEnable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDisable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnDetails = new System.Windows.Forms.Button();
            this.btnEnable = new System.Windows.Forms.Button();
            this.btnDisable = new System.Windows.Forms.Button();
            this.btnSetup = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // listViewStartupItems
            // 
            this.listViewStartupItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewStartupItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderPublisher,
            this.columnHeaderExecutable,
            this.columnHeaderParam,
            this.columnHeaderOS,
            this.columnHeaderStatus,
            this.columnHeaderType,
            this.columnHeaderState});
            this.listViewStartupItems.ContextMenuStrip = this.contextMenuStrip;
            this.listViewStartupItems.FullRowSelect = true;
            this.listViewStartupItems.GridLines = true;
            listViewGroup1.CollapsedState = System.Windows.Forms.ListViewGroupCollapsedState.Expanded;
            listViewGroup1.Footer = "";
            listViewGroup1.Header = "Current User";
            listViewGroup1.Name = "listViewGroupCurrentUser";
            listViewGroup1.Subtitle = "Runs only for current user";
            listViewGroup1.TaskLink = "";
            listViewGroup2.CollapsedState = System.Windows.Forms.ListViewGroupCollapsedState.Expanded;
            listViewGroup2.Footer = "";
            listViewGroup2.Header = "Local Machine";
            listViewGroup2.Name = "listViewGroupLocalMachine";
            listViewGroup2.Subtitle = "Runs for all users";
            listViewGroup2.TaskLink = "";
            this.listViewStartupItems.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.listViewStartupItems.HideSelection = false;
            this.listViewStartupItems.Location = new System.Drawing.Point(12, 105);
            this.listViewStartupItems.Name = "listViewStartupItems";
            this.listViewStartupItems.Size = new System.Drawing.Size(1920, 1050);
            this.listViewStartupItems.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewStartupItems.TabIndex = 0;
            this.listViewStartupItems.UseCompatibleStateImageBehavior = false;
            this.listViewStartupItems.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 260;
            // 
            // columnHeaderPublisher
            // 
            this.columnHeaderPublisher.Text = "Company/Publisher";
            this.columnHeaderPublisher.Width = 260;
            // 
            // columnHeaderExecutable
            // 
            this.columnHeaderExecutable.Text = "Executable";
            this.columnHeaderExecutable.Width = 260;
            // 
            // columnHeaderParam
            // 
            this.columnHeaderParam.Text = "Parameters";
            this.columnHeaderParam.Width = 260;
            // 
            // columnHeaderOS
            // 
            this.columnHeaderOS.Text = "OS";
            this.columnHeaderOS.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // columnHeaderStatus
            // 
            this.columnHeaderStatus.Text = "Status";
            this.columnHeaderStatus.Width = 160;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Type";
            this.columnHeaderType.Width = 120;
            // 
            // columnHeaderState
            // 
            this.columnHeaderState.Text = "State";
            this.columnHeaderState.Width = 120;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDetails,
            this.toolStripMenuItemEnable,
            this.toolStripMenuItemDisable,
            this.toolStripMenuItemCopy,
            this.toolStripMenuItemDelete});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(183, 224);
            this.contextMenuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ContextMenuStrip_ItemClicked);
            // 
            // toolStripMenuItemDetails
            // 
            this.toolStripMenuItemDetails.Name = "toolStripMenuItemDetails";
            this.toolStripMenuItemDetails.Size = new System.Drawing.Size(182, 44);
            this.toolStripMenuItemDetails.Tag = "Details";
            this.toolStripMenuItemDetails.Text = "Details";
            // 
            // toolStripMenuItemEnable
            // 
            this.toolStripMenuItemEnable.Name = "toolStripMenuItemEnable";
            this.toolStripMenuItemEnable.Size = new System.Drawing.Size(182, 44);
            this.toolStripMenuItemEnable.Tag = "Enable";
            this.toolStripMenuItemEnable.Text = "Enable";
            // 
            // toolStripMenuItemDisable
            // 
            this.toolStripMenuItemDisable.Name = "toolStripMenuItemDisable";
            this.toolStripMenuItemDisable.Size = new System.Drawing.Size(182, 44);
            this.toolStripMenuItemDisable.Tag = "Disable";
            this.toolStripMenuItemDisable.Text = "Disable";
            // 
            // toolStripMenuItemCopy
            // 
            this.toolStripMenuItemCopy.Name = "toolStripMenuItemCopy";
            this.toolStripMenuItemCopy.Size = new System.Drawing.Size(182, 44);
            this.toolStripMenuItemCopy.Tag = "Copy";
            this.toolStripMenuItemCopy.Text = "Copy";
            // 
            // toolStripMenuItemDelete
            // 
            this.toolStripMenuItemDelete.Name = "toolStripMenuItemDelete";
            this.toolStripMenuItemDelete.Size = new System.Drawing.Size(182, 44);
            this.toolStripMenuItemDelete.Tag = "Delete";
            this.toolStripMenuItemDelete.Text = "Delete";
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(10, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(141, 89);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.toolTip.SetToolTip(this.btnSave, "Save changes");
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.Save_Click);
            // 
            // btnBackup
            // 
            this.btnBackup.Location = new System.Drawing.Point(604, 10);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(141, 89);
            this.btnBackup.TabIndex = 2;
            this.btnBackup.Text = "&Backup";
            this.toolTip.SetToolTip(this.btnBackup, "Backup all startup item configuration");
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.Backup_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(753, 10);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(141, 89);
            this.btnRestore.TabIndex = 3;
            this.btnRestore.Text = "&Restore";
            this.toolTip.SetToolTip(this.btnRestore, "Restore a backup");
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.Restore_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(1792, 10);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(141, 89);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "E&xit";
            this.toolTip.SetToolTip(this.btnExit, "Exit the Startup Organizer application");
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.Exit_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(159, 10);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(141, 89);
            this.btnLoad.TabIndex = 5;
            this.btnLoad.Text = "Re&load";
            this.toolTip.SetToolTip(this.btnLoad, "Reload startup items");
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.Load_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(308, 10);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(141, 89);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "&Add";
            this.toolTip.SetToolTip(this.btnAdd, "Add new startup item");
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.Add_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(456, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(141, 89);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.toolTip.SetToolTip(this.btnDelete, "Delete selected startup items");
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(902, 10);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(141, 89);
            this.btnBrowse.TabIndex = 8;
            this.btnBrowse.Text = "&Browse";
            this.toolTip.SetToolTip(this.btnBrowse, "Browse file system or registry for selected item");
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // btnDetails
            // 
            this.btnDetails.Location = new System.Drawing.Point(1050, 10);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(141, 89);
            this.btnDetails.TabIndex = 9;
            this.btnDetails.Text = "&Details";
            this.toolTip.SetToolTip(this.btnDetails, "Display details for selected startup item");
            this.btnDetails.UseVisualStyleBackColor = true;
            this.btnDetails.Click += new System.EventHandler(this.Details_Click);
            // 
            // btnEnable
            // 
            this.btnEnable.Location = new System.Drawing.Point(1198, 10);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(141, 89);
            this.btnEnable.TabIndex = 10;
            this.btnEnable.Text = "&Enable";
            this.toolTip.SetToolTip(this.btnEnable, "Enable selected startup items");
            this.btnEnable.UseVisualStyleBackColor = true;
            this.btnEnable.Click += new System.EventHandler(this.Enable_Click);
            // 
            // btnDisable
            // 
            this.btnDisable.Location = new System.Drawing.Point(1347, 10);
            this.btnDisable.Name = "btnDisable";
            this.btnDisable.Size = new System.Drawing.Size(141, 89);
            this.btnDisable.TabIndex = 11;
            this.btnDisable.Text = "Disable";
            this.toolTip.SetToolTip(this.btnDisable, "Disable selected startup items");
            this.btnDisable.UseVisualStyleBackColor = true;
            this.btnDisable.Click += new System.EventHandler(this.Disable_Click);
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(1644, 10);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(141, 89);
            this.btnSetup.TabIndex = 12;
            this.btnSetup.Text = "Settings";
            this.toolTip.SetToolTip(this.btnSetup, "Show Startup Organizer settings");
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.Setup_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(1496, 10);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(141, 89);
            this.btnCopy.TabIndex = 13;
            this.btnCopy.Text = "Copy";
            this.toolTip.SetToolTip(this.btnCopy, "Copy selected startup item details to the clipboard");
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.Copy_Click);
            // 
            // toolTip
            // 
            this.toolTip.BackColor = System.Drawing.Color.Goldenrod;
            this.toolTip.ForeColor = System.Drawing.Color.White;
            this.toolTip.IsBalloon = true;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip.UseAnimation = false;
            this.toolTip.UseFading = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1942, 1169);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnSetup);
            this.Controls.Add(this.btnDisable);
            this.Controls.Add(this.btnEnable);
            this.Controls.Add(this.btnDetails);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.btnBackup);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.listViewStartupItems);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "Startup Organizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewStartupItems;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderPublisher;
        private System.Windows.Forms.ColumnHeader columnHeaderExecutable;
        private System.Windows.Forms.ColumnHeader columnHeaderParam;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnDetails;
        private System.Windows.Forms.ColumnHeader columnHeaderOS;
        private System.Windows.Forms.ColumnHeader columnHeaderStatus;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.Button btnEnable;
        private System.Windows.Forms.Button btnDisable;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDetails;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemEnable;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDisable;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCopy;
        private System.Windows.Forms.ColumnHeader columnHeaderState;
    }
}

