using System;
using System.Windows.Forms;

namespace Launcher
{
    internal partial class DownloadViewer : Form
    {
        private readonly string instructionsUrl;

        public bool CancelDownloadViewer { get; set; }

        public DownloadViewer(string instructionsUrl)
        {
            InitializeComponent();
            this.instructionsUrl = instructionsUrl;
        }

        private void CancelDownloadButton_Click(object sender, EventArgs e)
        {
            CancelDownloadViewer = true;
        }

        private void DownloadViewer_Load(object sender, EventArgs e)
        {
            Instructions.Url = new Uri( instructionsUrl );
        }

        internal delegate void InvokeDelegate();

        internal void Invoke(InvokeDelegate todo)
        {
            base.Invoke(todo);
        }
    }
}
