using System;
using System.Windows.Forms;

namespace StartupOrganizer
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
        }

        public void SetText(Form parentForm, string text)
        {
            StartPosition = (string)parentForm.Tag == "Loaded" ? FormStartPosition.CenterParent : FormStartPosition.CenterScreen;
            lblLoading.Text = text;
        }

        private void LoadingForm_VisibleChanged(object sender, EventArgs e)
        {
            if (StartPosition != FormStartPosition.CenterScreen)
                CenterToParent();
        }
    }
}
