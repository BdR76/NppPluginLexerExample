using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Kbg.NppPluginNET;
using Kbg.NppPluginNET.PluginInfrastructure;

namespace Kbg.Demo.Namespace
{
    partial class frmLexerSettings : Form
    {
        private readonly IScintillaGateway editor;

        public frmLexerSettings(IScintillaGateway editor)
        {
            this.editor = editor;
            InitializeComponent();
        }

        private void frmLexerSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Return) || (e.Alt && (e.KeyCode == Keys.G)))
            {
                btnApply.PerformClick();
                e.Handled = true;
            }
            else if (e.KeyData == Keys.Escape)
            {
                editor.GrabFocus();
            }
            else if (e.KeyCode == Keys.Tab)
            {
                Control next = GetNextControl((Control)sender, !e.Shift);
                while ((next == null) || (!next.TabStop)) next = GetNextControl(next, !e.Shift);
                next.Focus();
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar)
                && (e.KeyChar != '\b')
                && (e.KeyChar != '\t')) 
                e.Handled = true;
        }
        
        void FrmLexerSettingsVisibleChanged(object sender, EventArgs e)
        {
            if (!Visible)
            {
                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_SETMENUITEMCHECK,
                                  PluginBase._funcItems.Items[Main.idFrmLexer]._cmdID, 0);
            }
        }

    }
}
