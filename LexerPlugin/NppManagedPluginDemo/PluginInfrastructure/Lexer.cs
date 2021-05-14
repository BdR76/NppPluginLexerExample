using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Kbg.NppPluginNET.PluginInfrastructure;

namespace NppPluginNET.PluginInfrastructure
{
    class ILexer
    {
        public static readonly string Name = "EdifactLexer\0";
        public static readonly string StatusText = "My Edifact Lexer example\0";

        static IScintillaGateway editor = new ScintillaGateway(PluginBase.GetCurrentScintilla());

        static List<string> Segs1; // = new List<string> { "UNA", "UNB", "UNH", "UNT", "UNZ", "BGM" };
        static List<string> Segs2; // = new List<string> { "DTM", "MOA" };
        static List<string> Segs3; // = new List<string> { "NAD", "RFF", "CUX", "PRI", "TAX", "UNS", "CNT" };
        static List<string> Segs4; // = new List<string> { "LIN", "PIA", "IMD", "QTY" };


        // 1. since cpp defines this as an interface with virtual functions, 
        //      there is an implicit first parameter, the class instance
        // 2. according to c# documentation delegates are used to simulate function pointers

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

        [StructLayout(LayoutKind.Sequential)]
        public struct ILexer4
        {
            public ILexerVersion Version;
            public ILexerRelease Release;
            public ILexerPropertyNames PropertyNames;
            public ILexerPropertyType PropertyType;
            public ILexerDescribeProperty DescribeProperty;
            public ILexerPropertySet PropertySet;
            public ILexerDescribeWordListSets DescribeWordListSets;
            public ILexerWordListSet WordListSet;
            public ILexerLex Lex;
            public ILexerFold Fold;
            public ILexerPrivateCall PrivateCall;
            public ILexerLineEndTypesSupported LineEndTypesSupported;
            public ILexerAllocateSubStyles AllocateSubStyles;
            public ILexerSubStylesStart SubStylesStart;
            public ILexerSubStylesLength SubStylesLength;
            public ILexerStyleFromSubStyle StyleFromSubStyle;
            public ILexerPrimaryStyleFromStyle PrimaryStyleFromStyle;
            public ILexerFreeSubStyles FreeSubStyles;
            public ILexerSetIdentifiers SetIdentifiers;
            public ILexerDistanceToSecondaryStyles DistanceToSecondaryStyles;
            public ILexerGetSubStyleBases GetSubStyleBases;
            public ILexerNamedStyles NamedStyles;
            public ILexerNameOfStyle NameOfStyle;
            public ILexerTagsOfStyle TagsOfStyle;
            public ILexerDescriptionOfStyle DescriptionOfStyle;
        }

        static ILexer4 ilexer = new ILexer4 { };

        public static IntPtr ILexerImplementation()
        {
            // simulate a c++ vtable by creating an array of 25 function pointers
            ilexer.Version = new ILexerVersion(Version);
            ilexer.Release = new ILexerRelease(Release);
            ilexer.PropertyNames = new ILexerPropertyNames(PropertyNames);
            ilexer.PropertyType = new ILexerPropertyType(PropertyType);
            ilexer.DescribeProperty = new ILexerDescribeProperty(DescribeProperty);
            ilexer.PropertySet = new ILexerPropertySet(PropertySet);
            ilexer.DescribeWordListSets = new ILexerDescribeWordListSets(DescribeWordListSets);
            ilexer.WordListSet = new ILexerWordListSet(WordListSet);
            ilexer.Lex = new ILexerLex(Lex);
            ilexer.Fold = new ILexerFold(Fold);
            ilexer.PrivateCall = new ILexerPrivateCall(PrivateCall);
            ilexer.LineEndTypesSupported = new ILexerLineEndTypesSupported(LineEndTypesSupported);
            ilexer.AllocateSubStyles = new ILexerAllocateSubStyles(AllocateSubStyles);
            ilexer.SubStylesStart = new ILexerSubStylesStart(SubStylesStart);
            ilexer.SubStylesLength = new ILexerSubStylesLength(SubStylesLength);
            ilexer.StyleFromSubStyle = new ILexerStyleFromSubStyle(StyleFromSubStyle);
            ilexer.PrimaryStyleFromStyle = new ILexerPrimaryStyleFromStyle(PrimaryStyleFromStyle);
            ilexer.FreeSubStyles = new ILexerFreeSubStyles(FreeSubStyles);
            ilexer.SetIdentifiers = new ILexerSetIdentifiers(SetIdentifiers);
            ilexer.DistanceToSecondaryStyles = new ILexerDistanceToSecondaryStyles(DistanceToSecondaryStyles);
            ilexer.GetSubStyleBases = new ILexerGetSubStyleBases(GetSubStyleBases);
            ilexer.NamedStyles = new ILexerNamedStyles(NamedStyles);
            ilexer.NameOfStyle = new ILexerNameOfStyle(NameOfStyle);
            ilexer.TagsOfStyle = new ILexerTagsOfStyle(TagsOfStyle);
            ilexer.DescriptionOfStyle = new ILexerDescriptionOfStyle(DescriptionOfStyle);

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
            switch (kw_list_index)
            {
                case 0:
                    //ILexer.Segs1.AddRange("UNA UNB UNH UNT UNZ BGM".Split(' '));
                    ILexer.Segs1 = new List<string>(Marshal.PtrToStringAnsi(key_word_list).Split(' '));
                    break;
                case 1:
                    ILexer.Segs2 = new List<string>(Marshal.PtrToStringAnsi(key_word_list).Split(' '));
                    break;
                case 2:
                    ILexer.Segs3 = new List<string>(Marshal.PtrToStringAnsi(key_word_list).Split(' '));
                    break;
                case 3:
                    ILexer.Segs4 = new List<string>(Marshal.PtrToStringAnsi(key_word_list).Split(' '));
                    break;
                default:
                    break;
            }
            return (IntPtr)kw_list_index;
        }

        // virtual void SCI_METHOD Lex(Sci_PositionU startPos, i64 lengthDoc, int initStyle, IDocument *pAccess) = 0;
        public static void Lex(IntPtr instance, UIntPtr start_pos, IntPtr length_doc, int init_style, IntPtr p_access)
        {
            int start = (int)start_pos;
            int length = (int)length_doc;
            IntPtr range_ptr = editor.GetRangePointer(start, length);
            string content = Marshal.PtrToStringAnsi(range_ptr, length);

            for (int i = 0; i < length; i++)
            {
                int start_position = i;
                string tag = "";
                int idx = 0;

                if (i + 2 < length) { tag = content.Substring(i, 3); }

                if (Segs1.Contains(tag)) idx = 3;
                else if (Segs2.Contains(tag)) idx = 5;
                else if (Segs3.Contains(tag)) idx = 6;
                else if (Segs4.Contains(tag)) idx = 7;

                while (i < length)
                {
                    // read rest of the line
                    if (content[i] == '\n') { break; }
                    i++;
                }
                // style this line
                editor.StartStyling(start + start_position, 0);
                editor.SetStyling(i - start_position, idx);
            }
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


    }
}
