namespace Launcher
{
    partial class DownloadViewer
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.Instructions = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 388);
            this.progressBar1.Maximum = 35000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(288, 29);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(197, 423);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cancel Download";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.CancelDownloadButton_Click);
            // 
            // Instructions
            // 
            this.Instructions.AllowNavigation = false;
            this.Instructions.AllowWebBrowserDrop = false;
            this.Instructions.IsWebBrowserContextMenuEnabled = false;
            this.Instructions.Location = new System.Drawing.Point(12, 12);
            this.Instructions.MinimumSize = new System.Drawing.Size(291, 370);
            this.Instructions.Name = "Instructions";
            this.Instructions.ScriptErrorsSuppressed = true;
            this.Instructions.Size = new System.Drawing.Size(291, 370);
            this.Instructions.TabIndex = 3;
            this.Instructions.Url = new System.Uri("", System.UriKind.Relative);
            this.Instructions.WebBrowserShortcutsEnabled = false;
            // 
            // DownloadViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(315, 486);
            this.ControlBox = false;
            this.Controls.Add(this.Instructions);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressBar1);
            this.Location = new System.Drawing.Point(30, 30);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(323, 494);
            this.Name = "DownloadViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Downloading Viewer";
            this.Load += new System.EventHandler(this.DownloadViewer_Load);
            this.ResumeLayout(false);

        }

        #endregion

       public System.Windows.Forms.ProgressBar progressBar1;
       private System.Windows.Forms.Button button1;
       private System.Windows.Forms.WebBrowser Instructions;
    }
}