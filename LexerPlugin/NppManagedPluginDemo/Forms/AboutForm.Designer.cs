namespace Kbg.NppPluginNET
{
    partial class AboutForm
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
            this.btnOk = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblInfo1 = new System.Windows.Forms.Label();
            this.lnkGithub = new System.Windows.Forms.LinkLabel();
            this.linkEkopalypse = new System.Windows.Forms.LinkLabel();
            this.linkBdR76 = new System.Windows.Forms.LinkLabel();
            this.lblInfo2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(107, 158);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(76, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Location = new System.Drawing.Point(8, 8);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(272, 13);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Lexer Example plug-in NPP v";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblInfo1
            // 
            this.lblInfo1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo1.Location = new System.Drawing.Point(8, 32);
            this.lblInfo1.Name = "lblInfo1";
            this.lblInfo1.Size = new System.Drawing.Size(272, 64);
            this.lblInfo1.TabIndex = 2;
            this.lblInfo1.Text = "Lex() and Fold() example plug-in, adding syntax highlighting and folding options " +
    "to a EDIFACT data files. This is an example project or starting point for progra" +
    "mming C# plug-ins.";
            this.lblInfo1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lnkGithub
            // 
            this.lnkGithub.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkGithub.Location = new System.Drawing.Point(8, 123);
            this.lnkGithub.Name = "lnkGithub";
            this.lnkGithub.Size = new System.Drawing.Size(272, 13);
            this.lnkGithub.TabIndex = 3;
            this.lnkGithub.TabStop = true;
            this.lnkGithub.Tag = "";
            this.lnkGithub.Text = "https://github.com/BdR76/NppPluginLexerExample";
            this.lnkGithub.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lnkGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // linkEkopalypse
            // 
            this.linkEkopalypse.AutoSize = true;
            this.linkEkopalypse.Location = new System.Drawing.Point(196, 96);
            this.linkEkopalypse.Name = "linkEkopalypse";
            this.linkEkopalypse.Size = new System.Drawing.Size(73, 13);
            this.linkEkopalypse.TabIndex = 4;
            this.linkEkopalypse.TabStop = true;
            this.linkEkopalypse.Text = "@Ekopalypse";
            this.linkEkopalypse.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // linkBdR76
            // 
            this.linkBdR76.AutoSize = true;
            this.linkBdR76.Location = new System.Drawing.Point(74, 96);
            this.linkBdR76.Name = "linkBdR76";
            this.linkBdR76.Size = new System.Drawing.Size(51, 13);
            this.linkBdR76.TabIndex = 4;
            this.linkBdR76.TabStop = true;
            this.linkBdR76.Text = "@BdR76";
            this.linkBdR76.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnLinkClicked);
            // 
            // lblInfo2
            // 
            this.lblInfo2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo2.Location = new System.Drawing.Point(8, 96);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Size = new System.Drawing.Size(272, 13);
            this.lblInfo2.TabIndex = 1;
            this.lblInfo2.Text = "Created by @BdR76 with input from @Ekopalypse";
            this.lblInfo2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 193);
            this.Controls.Add(this.linkBdR76);
            this.Controls.Add(this.linkEkopalypse);
            this.Controls.Add(this.lnkGithub);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblInfo1);
            this.Controls.Add(this.lblInfo2);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblInfo1;
        private System.Windows.Forms.LinkLabel lnkGithub;
        private System.Windows.Forms.LinkLabel linkEkopalypse;
        private System.Windows.Forms.LinkLabel linkBdR76;
        private System.Windows.Forms.Label lblInfo2;
    }
}