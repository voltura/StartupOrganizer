
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
            this.rbtnRegistryHKCU = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnFolderMachine = new System.Windows.Forms.RadioButton();
            this.rbtnRegistryHKLM = new System.Windows.Forms.RadioButton();
            this.rbtnUwp = new System.Windows.Forms.RadioButton();
            this.rbtnFolderCurrentUser = new System.Windows.Forms.RadioButton();
            this.lblFileName = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtnDisabled = new System.Windows.Forms.RadioButton();
            this.rbtnEnabled = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.ofdSelectStartupItemFile = new System.Windows.Forms.OpenFileDialog();
            this.lblParameters = new System.Windows.Forms.Label();
            this.txtParameters = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.rbtnRegistryHKCU.UseVisualStyleBackColor = true;
            this.rbtnRegistryHKCU.CheckedChanged += new System.EventHandler(this.RegistryHKCU_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtnFolderMachine);
            this.groupBox1.Controls.Add(this.rbtnRegistryHKLM);
            this.groupBox1.Controls.Add(this.rbtnUwp);
            this.groupBox1.Controls.Add(this.rbtnFolderCurrentUser);
            this.groupBox1.Controls.Add(this.rbtnRegistryHKCU);
            this.groupBox1.Location = new System.Drawing.Point(12, 111);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(543, 272);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Type";
            // 
            // rbtnFolderMachine
            // 
            this.rbtnFolderMachine.AutoSize = true;
            this.rbtnFolderMachine.Location = new System.Drawing.Point(19, 186);
            this.rbtnFolderMachine.Name = "rbtnFolderMachine";
            this.rbtnFolderMachine.Size = new System.Drawing.Size(346, 41);
            this.rbtnFolderMachine.TabIndex = 4;
            this.rbtnFolderMachine.Text = "Start menu link (All users)";
            this.rbtnFolderMachine.UseVisualStyleBackColor = true;
            this.rbtnFolderMachine.CheckedChanged += new System.EventHandler(this.FolderMachine_CheckedChanged);
            // 
            // rbtnRegistryHKLM
            // 
            this.rbtnRegistryHKLM.AutoSize = true;
            this.rbtnRegistryHKLM.Location = new System.Drawing.Point(19, 89);
            this.rbtnRegistryHKLM.Name = "rbtnRegistryHKLM";
            this.rbtnRegistryHKLM.Size = new System.Drawing.Size(263, 41);
            this.rbtnRegistryHKLM.TabIndex = 3;
            this.rbtnRegistryHKLM.Text = "Registry (All users)";
            this.rbtnRegistryHKLM.UseVisualStyleBackColor = true;
            this.rbtnRegistryHKLM.CheckedChanged += new System.EventHandler(this.RegistryHKLM_CheckedChanged);
            // 
            // rbtnUwp
            // 
            this.rbtnUwp.AutoSize = true;
            this.rbtnUwp.Location = new System.Drawing.Point(19, 233);
            this.rbtnUwp.Name = "rbtnUwp";
            this.rbtnUwp.Size = new System.Drawing.Size(517, 41);
            this.rbtnUwp.TabIndex = 2;
            this.rbtnUwp.Text = "Universal Windows Platform (UWP) app";
            this.rbtnUwp.UseVisualStyleBackColor = true;
            this.rbtnUwp.CheckedChanged += new System.EventHandler(this.Uwp_CheckedChanged);
            // 
            // rbtnFolderCurrentUser
            // 
            this.rbtnFolderCurrentUser.AutoSize = true;
            this.rbtnFolderCurrentUser.Location = new System.Drawing.Point(19, 136);
            this.rbtnFolderCurrentUser.Name = "rbtnFolderCurrentUser";
            this.rbtnFolderCurrentUser.Size = new System.Drawing.Size(392, 41);
            this.rbtnFolderCurrentUser.TabIndex = 1;
            this.rbtnFolderCurrentUser.Text = "Start menu link (Current user)";
            this.rbtnFolderCurrentUser.UseVisualStyleBackColor = true;
            this.rbtnFolderCurrentUser.CheckedChanged += new System.EventHandler(this.FolderCurrentUser_CheckedChanged);
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
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(572, 13);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(169, 92);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "&Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(572, 449);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(169, 89);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.Add_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbtnDisabled);
            this.groupBox2.Controls.Add(this.rbtnEnabled);
            this.groupBox2.Location = new System.Drawing.Point(12, 391);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(543, 146);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "State";
            // 
            // rbtnDisabled
            // 
            this.rbtnDisabled.AutoSize = true;
            this.rbtnDisabled.Location = new System.Drawing.Point(6, 90);
            this.rbtnDisabled.Name = "rbtnDisabled";
            this.rbtnDisabled.Size = new System.Drawing.Size(152, 41);
            this.rbtnDisabled.TabIndex = 1;
            this.rbtnDisabled.Text = "Disabled";
            this.rbtnDisabled.UseVisualStyleBackColor = true;
            this.rbtnDisabled.CheckedChanged += new System.EventHandler(this.Disabled_CheckedChanged);
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
            this.rbtnEnabled.UseVisualStyleBackColor = true;
            this.rbtnEnabled.CheckedChanged += new System.EventHandler(this.Enabled_CheckedChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(572, 354);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(169, 89);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
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
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add startup item";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AddForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbtnRegistryHKCU;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtnFolderMachine;
        private System.Windows.Forms.RadioButton rbtnRegistryHKLM;
        private System.Windows.Forms.RadioButton rbtnUwp;
        private System.Windows.Forms.RadioButton rbtnFolderCurrentUser;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtnDisabled;
        private System.Windows.Forms.RadioButton rbtnEnabled;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.OpenFileDialog ofdSelectStartupItemFile;
        private System.Windows.Forms.Label lblParameters;
        private System.Windows.Forms.TextBox txtParameters;
    }
}