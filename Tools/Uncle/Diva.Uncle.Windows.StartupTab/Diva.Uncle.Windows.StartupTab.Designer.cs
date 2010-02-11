namespace Diva.Uncle.Windows.StartupTab
{
    partial class StartupTab
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_saveCrashesCheckBox = new System.Windows.Forms.CheckBox();
            this.m_nonPhysicalPrimMax = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_saveCrashesCheckBox
            // 
            this.m_saveCrashesCheckBox.AutoSize = true;
            this.m_saveCrashesCheckBox.Location = new System.Drawing.Point(15, 26);
            this.m_saveCrashesCheckBox.Name = "m_saveCrashesCheckBox";
            this.m_saveCrashesCheckBox.Size = new System.Drawing.Size(89, 17);
            this.m_saveCrashesCheckBox.TabIndex = 0;
            this.m_saveCrashesCheckBox.Text = "save crashes";
            this.m_saveCrashesCheckBox.ThreeState = true;
            this.m_saveCrashesCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_nonPhysicalPrimMax
            // 
            this.m_nonPhysicalPrimMax.Location = new System.Drawing.Point(15, 50);
            this.m_nonPhysicalPrimMax.Name = "m_nonPhysicalPrimMax";
            this.m_nonPhysicalPrimMax.Size = new System.Drawing.Size(100, 20);
            this.m_nonPhysicalPrimMax.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "NonPhysicalPrimMax";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(40, 93);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // StartupTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.m_saveCrashesCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_nonPhysicalPrimMax);
            this.Name = "StartupTab";
            this.Size = new System.Drawing.Size(248, 150);
            this.Load += new System.EventHandler(this.StartupTab_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox m_saveCrashesCheckBox;
        private System.Windows.Forms.TextBox m_nonPhysicalPrimMax;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button saveButton;
    }
}