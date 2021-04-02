
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
            this.listViewStartupItems = new System.Windows.Forms.ListView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderPublisher = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderExecutable = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderParam = new System.Windows.Forms.ColumnHeader();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnDetails = new System.Windows.Forms.Button();
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
            this.columnHeaderParam});
            this.listViewStartupItems.FullRowSelect = true;
            this.listViewStartupItems.GridLines = true;
            this.listViewStartupItems.HideSelection = false;
            this.listViewStartupItems.Location = new System.Drawing.Point(12, 105);
            this.listViewStartupItems.Name = "listViewStartupItems";
            this.listViewStartupItems.Size = new System.Drawing.Size(1128, 784);
            this.listViewStartupItems.TabIndex = 0;
            this.listViewStartupItems.UseCompatibleStateImageBehavior = false;
            this.listViewStartupItems.View = System.Windows.Forms.View.Details;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(121, 87);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnBackup
            // 
            this.btnBackup.Location = new System.Drawing.Point(520, 12);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(121, 87);
            this.btnBackup.TabIndex = 2;
            this.btnBackup.Text = "&Backup";
            this.btnBackup.UseVisualStyleBackColor = true;
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(647, 12);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(121, 87);
            this.btnRestore.TabIndex = 3;
            this.btnRestore.Text = "&Restore";
            this.btnRestore.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(1019, 12);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(121, 87);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "&Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(139, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(121, 87);
            this.btnLoad.TabIndex = 5;
            this.btnLoad.Text = "&Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.Load_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(266, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(121, 87);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(393, 12);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(121, 87);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 260;
            // 
            // columnHeaderPublisher
            // 
            this.columnHeaderPublisher.Text = "Publisher";
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
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(774, 12);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(121, 87);
            this.btnBrowse.TabIndex = 8;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // btnDetails
            // 
            this.btnDetails.Location = new System.Drawing.Point(901, 12);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(121, 87);
            this.btnDetails.TabIndex = 9;
            this.btnDetails.Text = "&Details";
            this.btnDetails.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1151, 902);
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
            this.Name = "MainForm";
            this.Text = "Startup Organizer";
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
    }
}

