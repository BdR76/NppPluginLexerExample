// NPP plugin platform for .Net v0.91.57 by Kasper B. Graversen etc.
using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Kbg.NppPluginNET.PluginInfrastructure;
using static Kbg.NppPluginNET.PluginInfrastructure.Win32;
using Kbg.NppPluginNET;

namespace Kbg.NppPluginNET
{
    /// <summary>
    /// Integration layer as the demo app uses the pluginfiles as soft-links files.
    /// This is different to normal plugins that would use the project template and get the files directly.
    /// </summary>
    class Main
    {
        static internal void CommandMenuInit()
        {
            Kbg.Demo.Namespace.Main.CommandMenuInit();
        }

        static internal void PluginCleanUp()
        {
            Kbg.Demo.Namespace.Main.PluginCleanUp();
        }

        static internal void SetToolBarIcon()
        {
            Kbg.Demo.Namespace.Main.SetToolBarIcon();
        }

        public static void OnNotification(ScNotification notification)
        {
            //if (notification.Header.Code == (uint)SciMsg.SCN_CHARADDED)
            //{
            //    Kbg.Demo.Namespace.Main.doInsertHtmlCloseTag((char)notification.Character);
            //}
        }

        internal static string PluginName { get { return Kbg.Demo.Namespace.Main.PluginName; }}
    }
}

namespace Kbg.Demo.Namespace
{
    class Main
    {
        #region " Fields "
        internal const string PluginName = "EdifactLexer";
        static string iniFilePath = null;
        static string sectionName = "Insert Extension";
        static string keyName = "NumericHighlight";
        static bool bNumericHighlight = false;
        static string sessionFilePath = @"C:\text.session";
        static frmLexerSettings frmLexerSettings = null;
        static internal int idToggle = -1;
        static internal int idFrmLexer = -1;
        static Bitmap tbBmp = Properties.Resources.lexer;
        static Bitmap tbBmp_tbTab = Properties.Resources.lexer_bmp;
        static Icon tbIcon = null;
        static IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());
        static INotepadPPGateway notepad = new NotepadPPGateway();
        
        #endregion

        #region " Startup/CleanUp "

        static internal void CommandMenuInit()
        {
            // Initialization of your plugin commands
            // You should fill your plugins commands here
 
            //
            // Firstly we get the parameters from your plugin config file (if any)
            //

            // get path of plugin configuration
            StringBuilder sbIniFilePath = new StringBuilder(Win32.MAX_PATH);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, sbIniFilePath);
            iniFilePath = sbIniFilePath.ToString();

            // if config path doesn't exist, we create it
            if (!Directory.Exists(iniFilePath))
            {
                Directory.CreateDirectory(iniFilePath);
            }

            // make your plugin config file full file path name
            iniFilePath = Path.Combine(iniFilePath, PluginName + ".ini");

            // get the parameter value from plugin config
            bNumericHighlight = (Win32.GetPrivateProfileInt(sectionName, keyName, 0, iniFilePath) != 0);

            // with function :
            // SetCommand(int index,                            // zero based number to indicate the order of command
            //            string commandName,                   // the command name that you want to see in plugin menu
            //            NppFuncItemDelegate functionPointer,  // the symbol of function (function pointer) associated with this command. The body should be defined below. See Step 4.
            //            ShortcutKey *shortcut,                // optional. Define a shortcut to trigger this command
            //            bool check0nInit                      // optional. Make this menu item be checked visually
            //            );
            PluginBase.SetCommand(0, "Hello Notepad++", DummyEDIfile);
            PluginBase.SetCommand(1, "Highlight numeric values", ToggleMenuItem1, bNumericHighlight); ; idToggle = 1;

            PluginBase.SetCommand(2, "Dockable Dialog Demo", DockableDlgDemo); idFrmLexer = 2;

            // Here you insert a separator
            PluginBase.SetCommand(3, "---", null);

            // Shortcut :
            // Following makes the command bind to the shortcut Alt-F
            PluginBase.SetCommand(4, "About", aboutmessage);
        }

        static internal void SetToolBarIcon()
        {
            toolbarIcons tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = tbBmp.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_ADDTOOLBARICON, PluginBase._funcItems.Items[idFrmLexer]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }

        static internal void PluginCleanUp()
        {
            Win32.WritePrivateProfileString(sectionName, keyName, bNumericHighlight ? "1" : "0", iniFilePath);
        }
        #endregion

        #region " Menu functions "
        static void DummyEDIfile()
        {
            notepad.FileNew();
            editor.SetText("Hello, Notepad++...from.NET!");
            var rest = editor.GetLine(0);
            editor.SetText(rest + rest + rest);
        }

        static void ToggleMenuItem1()
        {
            //Debug.WriteLine("ToggleMenuItem1()");

            //PluginBase.ToggleMenuItem(menuitems.menuitemToggleCheckXML, bNumericHighlight);
            PluginBase.ToggleMenuItem(idToggle, ref bNumericHighlight);
        }

        static void aboutmessage()
        {
            var about = new AboutForm();
            about.ShowDialog();
            about.Dispose();
        }

        static void DockableDlgDemo()
        {
            // Dockable Dialog Demo
            // 
            // This demonstration shows you how to do a dockable dialog.
            // You can create your own non dockable dialog - in this case you don't nedd this demonstration.
            if (frmLexerSettings == null)
            {
                frmLexerSettings = new frmLexerSettings(editor);

                using (Bitmap newBmp = new Bitmap(16, 16))
                {
                    Graphics g = Graphics.FromImage(newBmp);
                    ColorMap[] colorMap = new ColorMap[1];
                    colorMap[0] = new ColorMap();
                    colorMap[0].OldColor = Color.Fuchsia;
                    colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetRemapTable(colorMap);
                    g.DrawImage(tbBmp_tbTab, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                    tbIcon = Icon.FromHandle(newBmp.GetHicon());
                }
                
                NppTbData _nppTbData = new NppTbData();
                _nppTbData.hClient = frmLexerSettings.Handle;
                _nppTbData.pszName = "Lexer Example Test";
                // the dlgDlg should be the index of funcItem where the current function pointer is in
                // this case is 15.. so the initial value of funcItem[15]._cmdID - not the updated internal one !
                _nppTbData.dlgID = idFrmLexer;
                // define the default docking behaviour
                _nppTbData.uMask = NppTbMsg.DWS_DF_CONT_RIGHT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR;
                _nppTbData.hIconTab = (uint)tbIcon.Handle;
                _nppTbData.pszModuleName = PluginName;
                IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
                Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);
                // Following message will toogle both menu item state and toolbar button
                Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_SETMENUITEMCHECK, PluginBase._funcItems.Items[idFrmLexer]._cmdID, 1);
            }
            else
            {
                if (!frmLexerSettings.Visible)
                {
                    Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_DMMSHOW, 0, frmLexerSettings.Handle);
                    Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_SETMENUITEMCHECK, PluginBase._funcItems.Items[idFrmLexer]._cmdID, 1);
                }
                else
                {
                    Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_DMMHIDE, 0, frmLexerSettings.Handle);
                    Win32.SendMessage(PluginBase.nppData._nppHandle, (uint) NppMsg.NPPM_SETMENUITEMCHECK, PluginBase._funcItems.Items[idFrmLexer]._cmdID, 0);
                }
            }
        }
        #endregion
    }
}   
