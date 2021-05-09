// NPP plugin platform for .Net v0.94.00 by Kasper B. Graversen etc.
using System;
using System.Runtime.InteropServices;
using Kbg.NppPluginNET.PluginInfrastructure;
using NppPlugin.DllExport;

namespace Kbg.NppPluginNET
{
    class UnmanagedExports
    {
        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static bool isUnicode()
        {
            return true;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static void setInfo(NppData notepadPlusData)
        {
            PluginBase.nppData = notepadPlusData;
            Main.CommandMenuInit();
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static IntPtr getFuncsArray(ref int nbF)
        {
            nbF = PluginBase._funcItems.Items.Count;
            return PluginBase._funcItems.NativePointer;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static uint messageProc(uint Message, IntPtr wParam, IntPtr lParam)
        {
            return 1;
        }

        static IntPtr _ptrPluginName = IntPtr.Zero;
        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static IntPtr getName()
        {
            if (_ptrPluginName == IntPtr.Zero)
                _ptrPluginName = Marshal.StringToHGlobalUni(Main.PluginName);
            return _ptrPluginName;
        }

        [DllExport(CallingConvention = CallingConvention.Cdecl)]
        static void beNotified(IntPtr notifyCode)
        {
            ScNotification notification = (ScNotification)Marshal.PtrToStructure(notifyCode, typeof(ScNotification));
            if (notification.Header.Code == (uint)NppMsg.NPPN_TBMODIFICATION)
            {
                PluginBase._funcItems.RefreshItems();
                Main.SetToolBarIcon();
            }
            else if (notification.Header.Code == (uint)NppMsg.NPPN_SHUTDOWN)
            {
                Main.PluginCleanUp();
                Marshal.FreeHGlobal(_ptrPluginName);
            }
            else
            {
                Main.OnNotification(notification);
            }
        }
/*
        public static void SetupFolding()
        {
            //see here https://www.garybeene.com/code/gbsnippets_gbs_00669.htm
            Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_SETMARGINWIDTHN, 0, 20);//display line numbers
            //Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_SETMARGINWIDTHN, 1, 20);//no breakpoints
            Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_SETMARGINWIDTHN, 2, 20);//display folding in margin2
            Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_SETMARGINMASKN, 2, SciMsg.SC_MASK_FOLDERS);//margin2 set to hold folder symbols
            Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_SETMARGINSENSITIVEN, 2, 1); //margin2 set as sensitive to clicks
            Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_MARKERDEFINE, SciMsg.SC_MARKNUM_FOLDEROPEN, SciMsg.SC_MARK_ARROWDOWN);
            Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_MARKERDEFINE, SciMsg.SC_MARKNUM_FOLDER, SciMsg.SC_MARK_ARROW);
            Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_MARKERDEFINE, SciMsg.SC_MARKNUM_FOLDERSUB, SciMsg.SC_MARK_VLINE);
            Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_MARKERDEFINE, SciMsg.SC_MARKNUM_FOLDERTAIL, SciMsg.SC_MARK_LCORNER);
            //Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_MARKERDEFINE, SciMsg.SC_MARKNUM_FOLDERMIDTAIL, SciMsg.SC_MARK_EMPTY);
            //Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_MARKERDEFINE, SciMsg.SC_MARKNUM_FOLDEROPENMID, SciMsg.SC_MARK_EMPTY);
            //Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_MARKERDEFINE, SciMsg.SC_MARKNUM_FOLDEREND, SciMsg.SC_MARK_EMPTY);
            Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_MARKERSETBACK, SciMsg.SC_MARKNUM_FOLDERSUB, 0);
            Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_MARKERSETBACK, SciMsg.SC_MARKNUM_FOLDERTAIL, 0);
        }
        public static void SetupLexer()
        {
            // see https://www.garybeene.com/code/gbsnippets_gbs_00664.htm
            //this will enable the SCN_STYLENEEDED notification
            //??   Win32.SendMessage(PluginBase.GetCurrentScintilla(), SciMsg.SCI_SETLEXER, SciMsg.SCLEX_CONTAINER, 0);
        }
*/
    }
}
