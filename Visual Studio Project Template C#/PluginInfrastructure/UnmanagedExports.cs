// NPP plugin platform for .Net v0.94.00 by Kasper B. Graversen etc.
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
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

        #region LEXER specific
        [DllExport(CallingConvention = CallingConvention.StdCall)]
        static int GetLexerCount()
        {
            // function will be called twice, once by npp and once by scintilla
            return 1;  // this dll contains only one lexer
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        static void GetLexerName(uint index, IntPtr name, int buffer_length)
        {
            // function will be called twice, once by npp and once by scintilla
            // index is always 0 if this dll has only one lexer
            // name is a pointer to memory provided by npp and scintilla InsertMenuA is used, hence byte array
            // buffer_length is the size of the provided memory
            // use zero-terminated string to interface with C++
            byte[] lexer_name = Encoding.ASCII.GetBytes("EdifactLexer\0");
            Marshal.Copy(lexer_name, 0, name, lexer_name.Length);
        }

        [DllExport(CallingConvention = CallingConvention.StdCall)]
        static void GetLexerStatusText(uint index, IntPtr name, int buffer_length)
        {
            // function will be called by npp only, fills the first field of the statusbar
            // index is always 0 if this dll has only one lexer
            // name is a pointer to memory provided by npp and scintilla
            // buffer_length is the size of the provided memory
            // use zero-terminated string to interface with C++

            char[] lexer_status_text = "My Edifact Lexer example\0".ToCharArray(); // SendMessageW is used, hence ToCharArray as this returns utf16 strings
            Marshal.Copy(lexer_status_text, 0, name, lexer_status_text.Length);
        }

        // according to c# documentation delegates are used to simulate function pointers
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerImpDelegate();


        [DllExport(CallingConvention = CallingConvention.StdCall)]
        static Delegate GetLexerFactory(int index)
        {
            // function will be called by scintilla only
            // index is always 0 if this dll has only one lexer
            ILexerImpDelegate lexer_interface_implementation = new ILexerImpDelegate(ILexerImplementation);
            return lexer_interface_implementation;
        }



        // from here on these functions are not exported anymore - maybe another place makes more sense
        //public static IntPtr ILexerImplementation()
        //{
        //    return IntPtr.Zero;
        //}
        #endregion


        #region MORE LEXER code copied from messageboard
        // since cpp defines this as an interface with virtual functions, 
        // there is an implicit first parameter, the class instance

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerVersion(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ILexerRelease(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerPropertyNames(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerPropertyType(IntPtr instance, IntPtr name);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerDescribeProperty(IntPtr instance, IntPtr name);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerPropertySet(IntPtr instance, IntPtr key, IntPtr val);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerDescribeWordListSets(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerWordListSet(IntPtr instance, int kw_list_index, IntPtr key_word_list);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ILexerLex(IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ILexerFold(IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerPrivateCall(IntPtr instance, int operation, IntPtr pointer);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerLineEndTypesSupported(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerAllocateSubStyles(IntPtr instance, int style_base, int number_styles);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerSubStylesStart(IntPtr instance, int style_base);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerSubStylesLength(IntPtr instance, int style_base);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerStyleFromSubStyle(IntPtr instance, int sub_style);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerPrimaryStyleFromStyle(IntPtr instance, int style);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ILexerFreeSubStyles(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ILexerSetIdentifiers(IntPtr instance, int style, IntPtr identifiers);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerDistanceToSecondaryStyles(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerGetSubStyleBases(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int ILexerNamedStyles(IntPtr instance);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerNameOfStyle(IntPtr instance, int style);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerTagsOfStyle(IntPtr instance, int style);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate IntPtr ILexerDescriptionOfStyle(IntPtr instance, int style);


        // from here on these functions are not exported anymore - maybe another place makes more sense
        [StructLayout(LayoutKind.Sequential)]
        public struct ILexer4
        {
            public IntPtr Version;
            public IntPtr Release;
            public IntPtr PropertyNames;
            public IntPtr PropertyType;
            public IntPtr DescribeProperty;
            public IntPtr PropertySet;
            public IntPtr DescribeWordListSets;
            public IntPtr WordListSet;
            public IntPtr Lex;
            public IntPtr Fold;
            public IntPtr PrivateCall;
            public IntPtr LineEndTypesSupported;
            public IntPtr AllocateSubStyles;
            public IntPtr SubStylesStart;
            public IntPtr SubStylesLength;
            public IntPtr StyleFromSubStyle;
            public IntPtr PrimaryStyleFromStyle;
            public IntPtr FreeSubStyles;
            public IntPtr SetIdentifiers;
            public IntPtr DistanceToSecondaryStyles;
            public IntPtr GetSubStyleBases;
            public IntPtr NamedStyles;
            public IntPtr NameOfStyle;
            public IntPtr TagsOfStyle;
            public IntPtr DescriptionOfStyle;
        }

        public static IntPtr ILexerImplementation()
        {
            // simulate a c++ vtable by creating an array of 25 function pointers
            ILexer4 ilexer = new ILexer4
            {
                Version = Marshal.GetFunctionPointerForDelegate(new ILexerVersion(Version)),
                Release = Marshal.GetFunctionPointerForDelegate(new ILexerRelease(Release)),
                PropertyNames = Marshal.GetFunctionPointerForDelegate(new ILexerPropertyNames(PropertyNames)),
                PropertyType = Marshal.GetFunctionPointerForDelegate(new ILexerPropertyType(PropertyType)),
                DescribeProperty = Marshal.GetFunctionPointerForDelegate(new ILexerDescribeProperty(DescribeProperty)),
                PropertySet = Marshal.GetFunctionPointerForDelegate(new ILexerPropertySet(PropertySet)),
                DescribeWordListSets = Marshal.GetFunctionPointerForDelegate(new ILexerDescribeWordListSets(DescribeWordListSets)),
                WordListSet = Marshal.GetFunctionPointerForDelegate(new ILexerWordListSet(WordListSet)),
                Lex = Marshal.GetFunctionPointerForDelegate(new ILexerLex(Lex)),
                Fold = Marshal.GetFunctionPointerForDelegate(new ILexerFold(Fold)),
                PrivateCall = Marshal.GetFunctionPointerForDelegate(new ILexerPrivateCall(PrivateCall)),
                LineEndTypesSupported = Marshal.GetFunctionPointerForDelegate(new ILexerLineEndTypesSupported(LineEndTypesSupported)),
                AllocateSubStyles = Marshal.GetFunctionPointerForDelegate(new ILexerAllocateSubStyles(AllocateSubStyles)),
                SubStylesStart = Marshal.GetFunctionPointerForDelegate(new ILexerSubStylesStart(SubStylesStart)),
                SubStylesLength = Marshal.GetFunctionPointerForDelegate(new ILexerSubStylesLength(SubStylesLength)),
                StyleFromSubStyle = Marshal.GetFunctionPointerForDelegate(new ILexerStyleFromSubStyle(StyleFromSubStyle)),
                PrimaryStyleFromStyle = Marshal.GetFunctionPointerForDelegate(new ILexerPrimaryStyleFromStyle(PrimaryStyleFromStyle)),
                FreeSubStyles = Marshal.GetFunctionPointerForDelegate(new ILexerFreeSubStyles(FreeSubStyles)),
                SetIdentifiers = Marshal.GetFunctionPointerForDelegate(new ILexerSetIdentifiers(SetIdentifiers)),
                DistanceToSecondaryStyles = Marshal.GetFunctionPointerForDelegate(new ILexerDistanceToSecondaryStyles(DistanceToSecondaryStyles)),
                GetSubStyleBases = Marshal.GetFunctionPointerForDelegate(new ILexerGetSubStyleBases(GetSubStyleBases)),
                NamedStyles = Marshal.GetFunctionPointerForDelegate(new ILexerNamedStyles(NamedStyles)),
                NameOfStyle = Marshal.GetFunctionPointerForDelegate(new ILexerNameOfStyle(NameOfStyle)),
                TagsOfStyle = Marshal.GetFunctionPointerForDelegate(new ILexerTagsOfStyle(TagsOfStyle)),
                DescriptionOfStyle = Marshal.GetFunctionPointerForDelegate(new ILexerDescriptionOfStyle(DescriptionOfStyle))
            };

            IntPtr vtable = Marshal.AllocHGlobal(Marshal.SizeOf(ilexer));

            Marshal.StructureToPtr(ilexer, vtable, false);
            IntPtr vtable_pointer = Marshal.AllocHGlobal(Marshal.SizeOf(vtable));
            Marshal.StructureToPtr(vtable, vtable_pointer, false);
            return vtable_pointer;  // return the address of the fake vtable
        }

        // virtual int SCI_METHOD Version() const = 0
        public static int Version(IntPtr instance)
        {
            return 2;
        }


        // virtual void SCI_METHOD Release() = 0
        public static void Release(IntPtr instance)
        {
            // ??
        }


        // virtual const char * SCI_METHOD PropertyNames() = 0
        public static IntPtr PropertyNames(IntPtr instance)
        {
            return IntPtr.Zero;
        }


        // virtual int SCI_METHOD PropertyType(const char *name) = 0
        public static int PropertyType(IntPtr instance, IntPtr name)
        {
            return 0;
        }


        // virtual const char * SCI_METHOD DescribeProperty(const char *name) = 0
        public static IntPtr DescribeProperty(IntPtr instance, IntPtr name)
        {
            return IntPtr.Zero;
        }

        // virtual i64 SCI_METHOD PropertySet(const char *key, const char *val) = 0
        public static IntPtr PropertySet(IntPtr instance, IntPtr key, IntPtr val)
        {
            return (IntPtr)0;
        }


        // virtual const char * SCI_METHOD DescribeWordListSets() = 0
        public static IntPtr DescribeWordListSets(IntPtr instance)
        {
            return IntPtr.Zero;
        }

        // virtual i64 SCI_METHOD WordListSet(int n, const char *wl) = 0
        public static IntPtr WordListSet(IntPtr instance, int kw_list_index, IntPtr key_word_list)
        {
            // Read demo.xml and return the configured keywords
            return (IntPtr)0;
        }

        // virtual void SCI_METHOD Lex(Sci_PositionU startPos, i64 lengthDoc, int initStyle, IDocument *pAccess) = 0;
        public static void Lex(IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access)
        {
            // TODO: input tags from user window
            var segs1 = new List<string> { "UNA", "UNB", "UNH", "UNT", "UNZ", "BGM" };
            var segs2 = new List<string> { "DTM", "MOA" };
            var segs3 = new List<string> { "NAD", "RFF", "CUX", "PRI", "TAX", "UNS", "CNT" };
            var segs4 = new List<string> { "LIN", "PIA", "IMD", "QTY" };

            /*
             * Note
             * Code must be added to distinguish between different buffers, for example, 
             * if a user has both views open and is scrolling in the inactive view, 
             * then in this case the lex method is called with the parameters from the inactive view.
             */
            IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

            // convert variable types
            int start = (int)start_pos;
            int length = (int)length_doc;

            // start position
            //int style_used = editor.GetStyleAt(start);
            //editor.StartStyling(start, 0);
            //editor.SetStyling((int)length_doc, style_used == 0 ? 3 : 0);

            // get line positions
            int line = editor.LineFromPosition(start);

            int LineStart = editor.PositionFromLine(line);
            int LineEnd = editor.GetLineEndPosition(line);

            // loop through all lines
            while ((LineEnd <= length) && (LineStart != LineEnd))
            {
                // style this line
                string tag = "";
                for (var i = 0; i < LineEnd; i++)
                {
                    char c = (char)(editor.GetCharAt(LineStart + i));
                    tag += c;

                    if (i >= 2) break;
                };

                // line belong to segment?
                int idx = 0;
                if (segs1.Contains(tag)) idx = 3;
                if (segs2.Contains(tag)) idx = 5;
                if (segs3.Contains(tag)) idx = 6;
                if (segs4.Contains(tag)) idx = 7;

                // style this line
                editor.StartStyling(LineStart, 0);
                editor.SetStyling(LineEnd - LineStart, idx);

                // next line
                line++;
                LineStart = editor.PositionFromLine(line);
                LineEnd = editor.GetLineEndPosition(line);
            };
        }

        // virtual void SCI_METHOD Fold(Sci_PositionU startPos, i64 lengthDoc, int initStyle, IDocument *pAccess) = 0;
        public static void Fold(IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access)
        {
            /* 
             * Lessons I have learned so far are
             * - do not start with a base level of 0 to simplify the arithmetic int calculation
             * - scintilla recommends to use 0x400 as a base level
             * - when the value becomes smaller than the base value, set the base value
             * - create an additional margin in which you set the levels of the respective lines, 
             *      so it is easy to see when something breaks.
             */
        }

        // virtual void * SCI_METHOD PrivateCall(int operation, void *pointer) = 0;
        public static IntPtr PrivateCall(IntPtr instance, int operation, IntPtr pointer)
        {
            return IntPtr.Zero;
        }

        // virtual int SCI_METHOD LineEndTypesSupported() = 0;
        public static int LineEndTypesSupported(IntPtr instance)
        {
            return 0;
        }

        // virtual int SCI_METHOD AllocateSubStyles(int styleBase, int numberStyles) = 0;
        public static int AllocateSubStyles(IntPtr instance, int style_base, int number_styles)
        {
            // used for sub styles - not needed/supported by this lexer
            return -1;
        }

        // virtual int SCI_METHOD SubStylesStart(int styleBase) = 0;
        public static int SubStylesStart(IntPtr instance, int style_base)
        {
            // used for sub styles - not needed/supported by this lexer
            return -1;
        }

        // virtual int SCI_METHOD SubStylesLength(int styleBase) = 0;
        public static int SubStylesLength(IntPtr instance, int style_base)
        {
            // used for sub styles - not needed/supported by this lexer
            return 0;
        }

        // virtual int SCI_METHOD StyleFromSubStyle(int subStyle) = 0;
        public static int StyleFromSubStyle(IntPtr instance, int sub_style)
        {
            return 0;
        }

        // virtual int SCI_METHOD PrimaryStyleFromStyle(int style) = 0;
        public static int PrimaryStyleFromStyle(IntPtr instance, int style)
        {
            // used for sub styles - not needed/supported by this lexer
            return 0;
        }

        // virtual void SCI_METHOD FreeSubStyles() = 0;
        public static void FreeSubStyles(IntPtr instance)
        {
            //
        }

        // virtual void SCI_METHOD SetIdentifiers(int style, const char *identifiers) = 0;
        public static void SetIdentifiers(IntPtr instance, int style, IntPtr identifiers)
        {
            //
        }

        // virtual int SCI_METHOD DistanceToSecondaryStyles() = 0;
        public static int DistanceToSecondaryStyles(IntPtr instance)
        {
            return 0;
        }

        // virtual const char * SCI_METHOD GetSubStyleBases() = 0;
        public static IntPtr GetSubStyleBases(IntPtr instance)
        {
            return IntPtr.Zero;
        }

        // virtual int SCI_METHOD NamedStyles() = 0;
        public static int NamedStyles(IntPtr instance)
        {
            return 0;
        }

        // virtual const char * SCI_METHOD NameOfStyle(int style) = 0;
        public static IntPtr NameOfStyle(IntPtr instance, int style)
        {
            return IntPtr.Zero;
        }

        // virtual const char * SCI_METHOD TagsOfStyle(int style) = 0;
        public static IntPtr TagsOfStyle(IntPtr instance, int style)
        {
            return IntPtr.Zero;
        }

        // virtual const char * SCI_METHOD DescriptionOfStyle(int style) = 0;
        public static IntPtr DescriptionOfStyle(IntPtr instance, int style)
        {
            return IntPtr.Zero;
        }

        #endregion
    }
}