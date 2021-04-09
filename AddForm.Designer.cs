
namespace StartupOrganizer
{
    partial class AddForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.rbtnRegistryHKCU = new System.Windows.Forms.RadioButton();
            this.groupBoxType = new System.Windows.Forms.GroupBox();
            this.rbtnFolderMachine = new System.Windows.Forms.RadioButton();
            this.rbtnRegistryHKLM = new System.Windows.Forms.RadioButton();
            this.rbtnUwp = new System.Windows.Forms.RadioButton();
            this.rbtnFolderCurrentUser = new System.Windows.Forms.RadioButton();
            this.lblFileName = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.groupBoxState = new System.Windows.Forms.GroupBox();
            this.rbtnDisabled = new System.Windows.Forms.RadioButton();
            this.rbtnEnabled = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ofdSelectStartupItemFile = new System.Windows.Forms.OpenFileDialog();
            this.lblParameters = new System.Windows.Forms.Label();
            this.txtParameters = new System.Windows.Forms.TextBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxType.SuspendLayout();
            this.groupBoxState.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbtnRegistryHKCU
            // 
            this.rbtnRegistryHKCU.AutoSize = true;
            this.rbtnRegistryHKCU.Checked = true;
            this.rbtnRegistryHKCU.Location = new System.Drawing.Point(19, 42);
            this.rbtnRegistryHKCU.Name = "rbtnRegistryHKCU";
            this.rbtnRegistryHKCU.Size = new System.Drawing.Size(309, 41);
            this.rbtnRegistryHKCU.TabIndex = 0;
            this.rbtnRegistryHKCU.TabStop = true;
            this.rbtnRegistryHKCU.Text = "Registry (Current user)";
            this.toolTip.SetToolTip(this.rbtnRegistryHKCU, "Run when you login to Windows");
            this.rbtnRegistryHKCU.UseVisualStyleBackColor = true;
            // 
            // groupBoxType
            // 
            this.groupBoxType.Controls.Add(this.rbtnFolderMachine);
            this.groupBoxType.Controls.Add(this.rbtnRegistryHKLM);
            this.groupBoxType.Controls.Add(this.rbtnUwp);
            this.groupBoxType.Controls.Add(this.rbtnFolderCurrentUser);
            this.groupBoxType.Controls.Add(this.rbtnRegistryHKCU);
            this.groupBoxType.Location = new System.Drawing.Point(12, 111);
            this.groupBoxType.Name = "groupBoxType";
            this.groupBoxType.Size = new System.Drawing.Size(543, 272);
            this.groupBoxType.TabIndex = 1;
            this.groupBoxType.TabStop = false;
            this.groupBoxType.Text = "Type";
            // 
            // rbtnFolderMachine
            // 
            this.rbtnFolderMachine.AutoSize = true;
            this.rbtnFolderMachine.Location = new System.Drawing.Point(19, 186);
            this.rbtnFolderMachine.Name = "rbtnFolderMachine";
            this.rbtnFolderMachine.Size = new System.Drawing.Size(346, 41);
            this.rbtnFolderMachine.TabIndex = 4;
            this.rbtnFolderMachine.Text = "Start menu link (All users)";
            this.toolTip.SetToolTip(this.rbtnFolderMachine, "Run when any user login to Windows");
            this.rbtnFolderMachine.UseVisualStyleBackColor = true;
            // 
            // rbtnRegistryHKLM
            // 
            this.rbtnRegistryHKLM.AutoSize = true;
            this.rbtnRegistryHKLM.Location = new System.Drawing.Point(19, 89);
            this.rbtnRegistryHKLM.Name = "rbtnRegistryHKLM";
            this.rbtnRegistryHKLM.Size = new System.Drawing.Size(263, 41);
            this.rbtnRegistryHKLM.TabIndex = 3;
            this.rbtnRegistryHKLM.Text = "Registry (All users)";
            this.toolTip.SetToolTip(this.rbtnRegistryHKLM, "Run when any user login to Windows");
            this.rbtnRegistryHKLM.UseVisualStyleBackColor = true;
            // 
            // rbtnUwp
            // 
            this.rbtnUwp.AutoSize = true;
            this.rbtnUwp.Enabled = false;
            this.rbtnUwp.Location = new System.Drawing.Point(19, 233);
            this.rbtnUwp.Name = "rbtnUwp";
            this.rbtnUwp.Size = new System.Drawing.Size(517, 41);
            this.rbtnUwp.TabIndex = 2;
            this.rbtnUwp.Text = "Universal Windows Platform (UWP) app";
            this.toolTip.SetToolTip(this.rbtnUwp, "Run when you login to Windows");
            this.rbtnUwp.UseVisualStyleBackColor = true;
            // 
            // rbtnFolderCurrentUser
            // 
            this.rbtnFolderCurrentUser.AutoSize = true;
            this.rbtnFolderCurrentUser.Location = new System.Drawing.Point(19, 136);
            this.rbtnFolderCurrentUser.Name = "rbtnFolderCurrentUser";
            this.rbtnFolderCurrentUser.Size = new System.Drawing.Size(392, 41);
            this.rbtnFolderCurrentUser.TabIndex = 1;
            this.rbtnFolderCurrentUser.Text = "Start menu link (Current user)";
            this.toolTip.SetToolTip(this.rbtnFolderCurrentUser, "Run when you login to Windows");
            this.rbtnFolderCurrentUser.UseVisualStyleBackColor = true;
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(12, 16);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(137, 37);
            this.lblFileName.TabIndex = 2;
            this.lblFileName.Text = "File name:";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(172, 13);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(383, 43);
            this.txtFileName.TabIndex = 1;
            this.toolTip.SetToolTip(this.txtFileName, "Full file path including file name, use Browse button");
            this.txtFileName.TextChanged += new System.EventHandler(this.FileName_TextChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(572, 13);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(169, 92);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "&Browse...";
            this.toolTip.SetToolTip(this.btnBrowse, "Select application or script to be executed at startup");
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Enabled = false;
            this.btnAdd.Location = new System.Drawing.Point(572, 449);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(169, 89);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "&Add";
            this.toolTip.SetToolTip(this.btnAdd, "Add startup item");
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.Add_Click);
            // 
            // groupBoxState
            // 
            this.groupBoxState.Controls.Add(this.rbtnDisabled);
            this.groupBoxState.Controls.Add(this.rbtnEnabled);
            this.groupBoxState.Location = new System.Drawing.Point(12, 391);
            this.groupBoxState.Name = "groupBoxState";
            this.groupBoxState.Size = new System.Drawing.Size(543, 146);
            this.groupBoxState.TabIndex = 6;
            this.groupBoxState.TabStop = false;
            this.groupBoxState.Text = "State";
            // 
            // rbtnDisabled
            // 
            this.rbtnDisabled.AutoSize = true;
            this.rbtnDisabled.Location = new System.Drawing.Point(6, 90);
            this.rbtnDisabled.Name = "rbtnDisabled";
            this.rbtnDisabled.Size = new System.Drawing.Size(152, 41);
            this.rbtnDisabled.TabIndex = 1;
            this.rbtnDisabled.Text = "Disabled";
            this.toolTip.SetToolTip(this.rbtnDisabled, "File will NOT be executed on startup");
            this.rbtnDisabled.UseVisualStyleBackColor = true;
            // 
            // rbtnEnabled
            // 
            this.rbtnEnabled.AutoSize = true;
            this.rbtnEnabled.Checked = true;
            this.rbtnEnabled.Location = new System.Drawing.Point(7, 43);
            this.rbtnEnabled.Name = "rbtnEnabled";
            this.rbtnEnabled.Size = new System.Drawing.Size(144, 41);
            this.rbtnEnabled.TabIndex = 0;
            this.rbtnEnabled.TabStop = true;
            this.rbtnEnabled.Text = "Enabled";
            this.toolTip.SetToolTip(this.rbtnEnabled, "File will be executed during startup");
            this.rbtnEnabled.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(572, 354);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(169, 89);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.toolTip.SetToolTip(this.btnCancel, "Do NOT add a startup item");
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ofdSelectStartupItemFile
            // 
            this.ofdSelectStartupItemFile.Title = "Select file to add as startup item";
            // 
            // lblParameters
            // 
            this.lblParameters.AutoSize = true;
            this.lblParameters.Location = new System.Drawing.Point(12, 65);
            this.lblParameters.Name = "lblParameters";
            this.lblParameters.Size = new System.Drawing.Size(154, 37);
            this.lblParameters.TabIndex = 7;
            this.lblParameters.Text = "Parameters:";
            // 
            // txtParameters
            // 
            this.txtParameters.Location = new System.Drawing.Point(172, 62);
            this.txtParameters.Name = "txtParameters";
            this.txtParameters.Size = new System.Drawing.Size(383, 43);
            this.txtParameters.TabIndex = 8;
            this.toolTip.SetToolTip(this.txtParameters, "Parameters for file to run at startup");
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 500;
            this.toolTip.ShowAlways = true;
            this.toolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // AddForm
            // 
            this.AcceptButton = this.btnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(754, 550);
            this.ControlBox = false;
            this.Controls.Add(this.txtParameters);
            this.Controls.Add(this.lblParameters);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBoxState);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.groupBoxType);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add startup item";
            this.Load += new System.EventHandler(this.AddForm_Load);
            this.groupBoxType.ResumeLayout(false);
            this.groupBoxType.PerformLayout();
            this.groupBoxState.ResumeLayout(false);
            this.groupBoxState.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtnRegistryHKCU;
        private System.Windows.Forms.GroupBox groupBoxType;
        private System.Windows.Forms.RadioButton rbtnFolderMachine;
        private System.Windows.Forms.RadioButton rbtnRegistryHKLM;
        private System.Windows.Forms.RadioButton rbtnUwp;
        private System.Windows.Forms.RadioButton rbtnFolderCurrentUser;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox groupBoxState;
        private System.Windows.Forms.RadioButton rbtnDisabled;
        private System.Windows.Forms.RadioButton rbtnEnabled;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog ofdSelectStartupItemFile;
        private System.Windows.Forms.Label lblParameters;
        private System.Windows.Forms.TextBox txtParameters;
        private System.Windows.Forms.ToolTip toolTip;
    }
}