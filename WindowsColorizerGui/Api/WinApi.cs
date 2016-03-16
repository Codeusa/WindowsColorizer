using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowsColorizerGui.Api
{
    public class WinApi
    {
        public class WindowHandleInfo
        {
            private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

            [DllImport("user32")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

            private IntPtr _MainHandle;

            public WindowHandleInfo(IntPtr handle)
            {
                this._MainHandle = handle;
            }

            public List<IntPtr> GetAllChildHandles()
            {
                List<IntPtr> childHandles = new List<IntPtr>();

                GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
                IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

                try
                {
                    EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                    EnumChildWindows(this._MainHandle, childProc, pointerChildHandlesList);
                }
                finally
                {
                    gcChildhandlesList.Free();
                }

                return childHandles;
            }

            private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
            {
                GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

                if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
                {
                    return false;
                }

                List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
                childHandles.Add(hWnd);

                return true;
            }
        }
        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();
        public static  uint SPI_SETNONCLIENTMETRICS = 0x002A;
        #region public enum HChangeNotifyFlags

        /// <summary>
        ///     Flags that indicate the meaning of the <i>dwItem1</i> and <i>dwItem2</i> parameters.
        ///     The uFlags parameter must be one of the following values.
        /// </summary>
        [Flags]
        public enum HChangeNotifyFlags
        {
            /// <summary>
            ///     The <i>dwItem1</i> and <i>dwItem2</i> parameters are DWORD values.
            /// </summary>
            SHCNF_DWORD = 0x0003,

            /// <summary>
            ///     <i>dwItem1</i> and <i>dwItem2</i> are the addresses of ITEMIDLIST structures that
            ///     represent the item(s) affected by the change.
            ///     Each ITEMIDLIST must be relative to the desktop folder.
            /// </summary>
            SHCNF_IDLIST = 0x0000,

            /// <summary>
            ///     <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings of
            ///     maximum length MAX_PATH that contain the full path names
            ///     of the items affected by the change.
            /// </summary>
            SHCNF_PATHA = 0x0001,

            /// <summary>
            ///     <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings of
            ///     maximum length MAX_PATH that contain the full path names
            ///     of the items affected by the change.
            /// </summary>
            SHCNF_PATHW = 0x0005,

            /// <summary>
            ///     <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings that
            ///     represent the friendly names of the printer(s) affected by the change.
            /// </summary>
            SHCNF_PRINTERA = 0x0002,

            /// <summary>
            ///     <i>dwItem1</i> and <i>dwItem2</i> are the addresses of null-terminated strings that
            ///     represent the friendly names of the printer(s) affected by the change.
            /// </summary>
            SHCNF_PRINTERW = 0x0006,

            /// <summary>
            ///     The function should not return until the notification
            ///     has been delivered to all affected components.
            ///     As this flag modifies other data-type flags, it cannot by used by itself.
            /// </summary>
            SHCNF_FLUSH = 0x1000,

            /// <summary>
            ///     The function should begin delivering notifications to all affected components
            ///     but should return as soon as the notification process has begun.
            ///     As this flag modifies other data-type flags, it cannot by used by itself.
            /// </summary>
            SHCNF_FLUSHNOWAIT = 0x2000
        }

        #endregion // enum HChangeNotifyFlags

        [Flags]
        public enum RedrawWindowFlags : uint
        {
            /// <summary>
            ///     Invalidates the rectangle or region that you specify in lprcUpdate or hrgnUpdate.
            ///     You can set only one of these parameters to a non-NULL value. If both are NULL, RDW_INVALIDATE invalidates the
            ///     entire window.
            /// </summary>
            Invalidate = 0x1,

            /// <summary>
            ///     Causes the OS to post a WM_PAINT message to the window regardless of whether a portion of the window is
            ///     invalid.
            /// </summary>
            InternalPaint = 0x2,

            /// <summary>
            ///     Causes the window to receive a WM_ERASEBKGND message when the window is repainted.
            ///     Specify this value in combination with the RDW_INVALIDATE value; otherwise, RDW_ERASE has no effect.
            /// </summary>
            Erase = 0x4,

            /// <summary>
            ///     Validates the rectangle or region that you specify in lprcUpdate or hrgnUpdate.
            ///     You can set only one of these parameters to a non-NULL value. If both are NULL, RDW_VALIDATE validates the entire
            ///     window.
            ///     This value does not affect internal WM_PAINT messages.
            /// </summary>
            Validate = 0x8,

            NoInternalPaint = 0x10,

            /// <summary>Suppresses any pending WM_ERASEBKGND messages.</summary>
            NoErase = 0x20,

            /// <summary>Excludes child windows, if any, from the repainting operation.</summary>
            NoChildren = 0x40,

            /// <summary>Includes child windows, if any, in the repainting operation.</summary>
            AllChildren = 0x80,

            /// <summary>
            ///     Causes the affected windows, which you specify by setting the RDW_ALLCHILDREN and RDW_NOCHILDREN values, to
            ///     receive WM_ERASEBKGND and WM_PAINT messages before the RedrawWindow returns, if necessary.
            /// </summary>
            UpdateNow = 0x100,

            /// <summary>
            ///     Causes the affected windows, which you specify by setting the RDW_ALLCHILDREN and RDW_NOCHILDREN values, to receive
            ///     WM_ERASEBKGND messages before RedrawWindow returns, if necessary.
            ///     The affected windows receive WM_PAINT messages at the ordinary time.
            /// </summary>
            EraseNow = 0x200,

            Frame = 0x400,

            NoFrame = 0x800
        }

        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
            SMTO_ERRORONEXIT = 0x20
        }

        public enum WindowsMessage
        {
            WM_NULL = 0x0000,
            WM_CREATE = 0x0001,
            WM_DESTROY = 0x0002,
            WM_MOVE = 0x0003,
            WM_SIZE = 0x0005,
            WM_ACTIVATE = 0x0006,
            WM_SETFOCUS = 0x0007,
            WM_KILLFOCUS = 0x0008,
            WM_ENABLE = 0x000A,
            WM_SETREDRAW = 0x000B,
            WM_SETTEXT = 0x000C,
            WM_GETTEXT = 0x000D,
            WM_GETTEXTLENGTH = 0x000E,
            WM_PAINT = 0x000F,
            WM_CLOSE = 0x0010,
            WM_QUERYENDSESSION = 0x0011,
            WM_QUERYOPEN = 0x0013,
            WM_ENDSESSION = 0x0016,
            WM_QUIT = 0x0012,
            WM_ERASEBKGND = 0x0014,
            WM_SYSCOLORCHANGE = 0x0015,
            WM_SHOWWINDOW = 0x0018,
            WM_WININICHANGE = 0x001A,
            WM_SETTINGCHANGE = WM_WININICHANGE,
            WM_DEVMODECHANGE = 0x001B,
            WM_ACTIVATEAPP = 0x001C,
            WM_FONTCHANGE = 0x001D,
            WM_TIMECHANGE = 0x001E,
            WM_CANCELMODE = 0x001F,
            WM_SETCURSOR = 0x0020,
            WM_MOUSEACTIVATE = 0x0021,
            WM_CHILDACTIVATE = 0x0022,
            WM_QUEUESYNC = 0x0023,
            WM_GETMINMAXINFO = 0x0024,
            WM_PAINTICON = 0x0026,
            WM_ICONERASEBKGND = 0x0027,
            WM_NEXTDLGCTL = 0x0028,
            WM_SPOOLERSTATUS = 0x002A,
            WM_DRAWITEM = 0x002B,
            WM_MEASUREITEM = 0x002C,
            WM_DELETEITEM = 0x002D,
            WM_VKEYTOITEM = 0x002E,
            WM_CHARTOITEM = 0x002F,
            WM_SETFONT = 0x0030,
            WM_GETFONT = 0x0031,
            WM_SETHOTKEY = 0x0032,
            WM_GETHOTKEY = 0x0033,
            WM_QUERYDRAGICON = 0x0037,
            WM_COMPAREITEM = 0x0039,
            WM_GETOBJECT = 0x003D,
            WM_COMPACTING = 0x0041,
            WM_COMMNOTIFY = 0x0044,
            WM_WINDOWPOSCHANGING = 0x0046,
            WM_WINDOWPOSCHANGED = 0x0047,
            WM_POWER = 0x0048,
            WM_COPYDATA = 0x004A,
            WM_CANCELJOURNAL = 0x004B,
            WM_NOTIFY = 0x004E,
            WM_INPUTLANGCHANGEREQUEST = 0x0050,
            WM_INPUTLANGCHANGE = 0x0051,
            WM_TCARD = 0x0052,
            WM_HELP = 0x0053,
            WM_USERCHANGED = 0x0054,
            WM_NOTIFYFORMAT = 0x0055,
            WM_CONTEXTMENU = 0x007B,
            WM_STYLECHANGING = 0x007C,
            WM_STYLECHANGED = 0x007D,
            WM_DISPLAYCHANGE = 0x007E,
            WM_GETICON = 0x007F,
            WM_SETICON = 0x0080,
            WM_NCCREATE = 0x0081,
            WM_NCDESTROY = 0x0082,
            WM_NCCALCSIZE = 0x0083,
            WM_NCHITTEST = 0x0084,
            WM_NCPAINT = 0x0085,
            WM_NCACTIVATE = 0x0086,
            WM_GETDLGCODE = 0x0087,
            WM_SYNCPAINT = 0x0088,


            WM_NCMOUSEMOVE = 0x00A0,
            WM_NCLBUTTONDOWN = 0x00A1,
            WM_NCLBUTTONUP = 0x00A2,
            WM_NCLBUTTONDBLCLK = 0x00A3,
            WM_NCRBUTTONDOWN = 0x00A4,
            WM_NCRBUTTONUP = 0x00A5,
            WM_NCRBUTTONDBLCLK = 0x00A6,
            WM_NCMBUTTONDOWN = 0x00A7,
            WM_NCMBUTTONUP = 0x00A8,
            WM_NCMBUTTONDBLCLK = 0x00A9,
            WM_NCXBUTTONDOWN = 0x00AB,
            WM_NCXBUTTONUP = 0x00AC,
            WM_NCXBUTTONDBLCLK = 0x00AD,

            WM_INPUT_DEVICE_CHANGE = 0x00FE,
            WM_INPUT = 0x00FF,

            WM_KEYFIRST = 0x0100,
            WM_KEYDOWN = 0x0100,
            WM_KEYUP = 0x0101,
            WM_CHAR = 0x0102,
            WM_DEADCHAR = 0x0103,
            WM_SYSKEYDOWN = 0x0104,
            WM_SYSKEYUP = 0x0105,
            WM_SYSCHAR = 0x0106,
            WM_SYSDEADCHAR = 0x0107,
            WM_UNICHAR = 0x0109,
            WM_KEYLAST = 0x0109,

            WM_IME_STARTCOMPOSITION = 0x010D,
            WM_IME_ENDCOMPOSITION = 0x010E,
            WM_IME_COMPOSITION = 0x010F,
            WM_IME_KEYLAST = 0x010F,

            WM_INITDIALOG = 0x0110,
            WM_COMMAND = 0x0111,
            WM_SYSCOMMAND = 0x0112,
            WM_TIMER = 0x0113,
            WM_HSCROLL = 0x0114,
            WM_VSCROLL = 0x0115,
            WM_INITMENU = 0x0116,
            WM_INITMENUPOPUP = 0x0117,
            WM_MENUSELECT = 0x011F,
            WM_MENUCHAR = 0x0120,
            WM_ENTERIDLE = 0x0121,
            WM_MENURBUTTONUP = 0x0122,
            WM_MENUDRAG = 0x0123,
            WM_MENUGETOBJECT = 0x0124,
            WM_UNINITMENUPOPUP = 0x0125,
            WM_MENUCOMMAND = 0x0126,

            WM_CHANGEUISTATE = 0x0127,
            WM_UPDATEUISTATE = 0x0128,
            WM_QUERYUISTATE = 0x0129,

            WM_CTLCOLORMSGBOX = 0x0132,
            WM_CTLCOLOREDIT = 0x0133,
            WM_CTLCOLORLISTBOX = 0x0134,
            WM_CTLCOLORBTN = 0x0135,
            WM_CTLCOLORDLG = 0x0136,
            WM_CTLCOLORSCROLLBAR = 0x0137,
            WM_CTLCOLORSTATIC = 0x0138,
            MN_GETHMENU = 0x01E1,

            WM_MOUSEFIRST = 0x0200,
            WM_MOUSEMOVE = 0x0200,
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            WM_RBUTTONDBLCLK = 0x0206,
            WM_MBUTTONDOWN = 0x0207,
            WM_MBUTTONUP = 0x0208,
            WM_MBUTTONDBLCLK = 0x0209,
            WM_MOUSEWHEEL = 0x020A,
            WM_XBUTTONDOWN = 0x020B,
            WM_XBUTTONUP = 0x020C,
            WM_XBUTTONDBLCLK = 0x020D,
            WM_MOUSEHWHEEL = 0x020E,

            WM_PARENTNOTIFY = 0x0210,
            WM_ENTERMENULOOP = 0x0211,
            WM_EXITMENULOOP = 0x0212,

            WM_NEXTMENU = 0x0213,
            WM_SIZING = 0x0214,
            WM_CAPTURECHANGED = 0x0215,
            WM_MOVING = 0x0216,

            WM_POWERBROADCAST = 0x0218,

            WM_DEVICECHANGE = 0x0219,

            WM_MDICREATE = 0x0220,
            WM_MDIDESTROY = 0x0221,
            WM_MDIACTIVATE = 0x0222,
            WM_MDIRESTORE = 0x0223,
            WM_MDINEXT = 0x0224,
            WM_MDIMAXIMIZE = 0x0225,
            WM_MDITILE = 0x0226,
            WM_MDICASCADE = 0x0227,
            WM_MDIICONARRANGE = 0x0228,
            WM_MDIGETACTIVE = 0x0229,


            WM_MDISETMENU = 0x0230,
            WM_ENTERSIZEMOVE = 0x0231,
            WM_EXITSIZEMOVE = 0x0232,
            WM_DROPFILES = 0x0233,
            WM_MDIREFRESHMENU = 0x0234,

            WM_IME_SETCONTEXT = 0x0281,
            WM_IME_NOTIFY = 0x0282,
            WM_IME_CONTROL = 0x0283,
            WM_IME_COMPOSITIONFULL = 0x0284,
            WM_IME_SELECT = 0x0285,
            WM_IME_CHAR = 0x0286,
            WM_IME_REQUEST = 0x0288,
            WM_IME_KEYDOWN = 0x0290,
            WM_IME_KEYUP = 0x0291,

            WM_MOUSEHOVER = 0x02A1,
            WM_MOUSELEAVE = 0x02A3,
            WM_NCMOUSEHOVER = 0x02A0,
            WM_NCMOUSELEAVE = 0x02A2,

            WM_WTSSESSION_CHANGE = 0x02B1,

            WM_TABLET_FIRST = 0x02c0,
            WM_TABLET_LAST = 0x02df,

            WM_CUT = 0x0300,
            WM_COPY = 0x0301,
            WM_PASTE = 0x0302,
            WM_CLEAR = 0x0303,
            WM_UNDO = 0x0304,
            WM_RENDERFORMAT = 0x0305,
            WM_RENDERALLFORMATS = 0x0306,
            WM_DESTROYCLIPBOARD = 0x0307,
            WM_DRAWCLIPBOARD = 0x0308,
            WM_PAINTCLIPBOARD = 0x0309,
            WM_VSCROLLCLIPBOARD = 0x030A,
            WM_SIZECLIPBOARD = 0x030B,
            WM_ASKCBFORMATNAME = 0x030C,
            WM_CHANGECBCHAIN = 0x030D,
            WM_HSCROLLCLIPBOARD = 0x030E,
            WM_QUERYNEWPALETTE = 0x030F,
            WM_PALETTEISCHANGING = 0x0310,
            WM_PALETTECHANGED = 0x0311,
            WM_HOTKEY = 0x0312,

            WM_PRINT = 0x0317,
            WM_PRINTCLIENT = 0x0318,

            WM_APPCOMMAND = 0x0319,

            WM_THEMECHANGED = 0x031A,

            WM_CLIPBOARDUPDATE = 0x031D,

            WM_DWMCOMPOSITIONCHANGED = 0x031E,
            WM_DWMNCRENDERINGCHANGED = 0x031F,
            WM_DWMCOLORIZATIONCOLORCHANGED = 0x0320,
            WM_DWMWINDOWMAXIMIZEDCHANGE = 0x0321,

            WM_GETTITLEBARINFOEX = 0x033F,

            WM_HANDHELDFIRST = 0x0358,
            WM_HANDHELDLAST = 0x035F,

            WM_AFXFIRST = 0x0360,
            WM_AFXLAST = 0x037F,

            WM_PENWINFIRST = 0x0380,
            WM_PENWINLAST = 0x038F,

            WM_APP = 0x8000,

            WM_USER = 0x0400,

            WM_REFLECT = WM_USER + 0x1C00
        }
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SendNotifyMessage(IntPtr hWnd, uint Msg, UIntPtr wParam,
   IntPtr lParam);

        public const uint WM_SETTINGCHANGE = 0x001A;
        public const uint SMTO_ABORTIFHUNG = 0x2;
        public static IntPtr HWND_BROADCAST = new IntPtr(0xffff);
        [DllImport("shell32.dll")]
        public static extern void SHChangeNotify(int eventID, uint flags, IntPtr item1, IntPtr item2);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint PostMessage(IntPtr hWnd, int Msg, uint wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(
            IntPtr hWnd,
            uint Msg,
            UIntPtr wParam,
            IntPtr lParam,
            SendMessageTimeoutFlags fuFlags,
            uint uTimeout,
            out UIntPtr lpdwResult);

        [DllImport("shell32.dll")]
        public static extern void SHChangeNotify(HChangeNotifyEventID wEventId,
            HChangeNotifyFlags uFlags,
            IntPtr dwItem1,
            IntPtr dwItem2);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(
            IntPtr windowHandle,
            uint Msg,
            IntPtr wParam,
            IntPtr lParam,
            SendMessageTimeoutFlags flags,
            uint timeout,
            out IntPtr result);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg,
                                    UIntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint wMsg,
                                  UIntPtr wParam, UIntPtr lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SendNotifyMessage(IntPtr hWnd, uint Msg,
            UIntPtr wParam, string lParam);
        /* Version specifically setup for use with WM_GETTEXT message */

        [DllImport("user32.dll", EntryPoint = "SendMessageTimeout", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint SendMessageTimeoutText(
            IntPtr hWnd,
            int Msg, // Use WM_GETTEXT
            int countOfChars,
            StringBuilder text,
            SendMessageTimeoutFlags flags,
            uint uTImeoutj,
            out IntPtr result);

        /* Version for a message which returns an int, such as WM_GETTEXTLENGTH. */

        [DllImport("user32.dll", EntryPoint = "SendMessageTimeout", CharSet = CharSet.Auto)]
        public static extern int SendMessageTimeout(
            IntPtr hwnd,
            uint Msg,
            int wParam,
            int lParam,
            uint fuFlags,
            uint uTimeout,
            out int lpdwResult);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, [In] ref RECT lprcUpdate, IntPtr hrgnUpdate,
            RedrawWindowFlags flags);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate,
            RedrawWindowFlags flags);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool InvalidateRect(IntPtr hWnd, IntPtr rect, bool bErase);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UpdateWindow(IntPtr hWnd);

        public bool PaintWindow(IntPtr hWnd)
        {
            InvalidateRect(hWnd, IntPtr.Zero, true);
            if (UpdateWindow(hWnd))
            {
                return true;
            }
            return false;
        }

        #region enum HChangeNotifyEventID

        /// <summary>
        ///     Describes the event that has occurred.
        ///     Typically, only one event is specified at a time.
        ///     If more than one event is specified, the values contained
        ///     in the <i>dwItem1</i> and <i>dwItem2</i>
        ///     parameters must be the same, respectively, for all specified events.
        ///     This parameter can be one or more of the following values.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <b>Windows NT/2000/XP:</b> <i>dwItem2</i> contains the index
        ///         in the system image list that has changed.
        ///         <i>dwItem1</i> is not used and should be <see langword="null" />.
        ///     </para>
        ///     <para>
        ///         <b>Windows 95/98:</b> <i>dwItem1</i> contains the index
        ///         in the system image list that has changed.
        ///         <i>dwItem2</i> is not used and should be <see langword="null" />.
        ///     </para>
        /// </remarks>
        [Flags]
        public enum HChangeNotifyEventID
        {
            /// <summary>
            ///     All events have occurred.
            /// </summary>
            SHCNE_ALLEVENTS = 0x7FFFFFFF,

            /// <summary>
            ///     A file type association has changed. <see cref="HChangeNotifyFlags.SHCNF_IDLIST" />
            ///     must be specified in the <i>uFlags</i> parameter.
            ///     <i>dwItem1</i> and <i>dwItem2</i> are not used and must be <see langword="null" />.
            /// </summary>
            SHCNE_ASSOCCHANGED = 0x08000000,

            /// <summary>
            ///     The attributes of an item or folder have changed.
            ///     <see cref="HChangeNotifyFlags.SHCNF_IDLIST" /> or
            ///     <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the item or folder that has changed.
            ///     <i>dwItem2</i> is not used and should be <see langword="null" />.
            /// </summary>
            SHCNE_ATTRIBUTES = 0x00000800,

            /// <summary>
            ///     A nonfolder item has been created.
            ///     <see cref="HChangeNotifyFlags.SHCNF_IDLIST" /> or
            ///     <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the item that was created.
            ///     <i>dwItem2</i> is not used and should be <see langword="null" />.
            /// </summary>
            SHCNE_CREATE = 0x00000002,

            /// <summary>
            ///     A nonfolder item has been deleted.
            ///     <see cref="HChangeNotifyFlags.SHCNF_IDLIST" /> or
            ///     <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the item that was deleted.
            ///     <i>dwItem2</i> is not used and should be <see langword="null" />.
            /// </summary>
            SHCNE_DELETE = 0x00000004,

            /// <summary>
            ///     A drive has been added.
            ///     <see cref="HChangeNotifyFlags.SHCNF_IDLIST" /> or
            ///     <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the root of the drive that was added.
            ///     <i>dwItem2</i> is not used and should be <see langword="null" />.
            /// </summary>
            SHCNE_DRIVEADD = 0x00000100,

            /// <summary>
            ///     A drive has been added and the Shell should create a new window for the drive.
            ///     <see cref="HChangeNotifyFlags.SHCNF_IDLIST" /> or
            ///     <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the root of the drive that was added.
            ///     <i>dwItem2</i> is not used and should be <see langword="null" />.
            /// </summary>
            SHCNE_DRIVEADDGUI = 0x00010000,

            /// <summary>
            ///     A drive has been removed. <see cref="HChangeNotifyFlags.SHCNF_IDLIST" /> or
            ///     <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the root of the drive that was removed.
            ///     <i>dwItem2</i> is not used and should be <see langword="null" />.
            /// </summary>
            SHCNE_DRIVEREMOVED = 0x00000080,

            /// <summary>
            ///     Not currently used.
            /// </summary>
            SHCNE_EXTENDED_EVENT = 0x04000000,

            /// <summary>
            ///     The amount of free space on a drive has changed.
            ///     <see cref="HChangeNotifyFlags.SHCNF_IDLIST" /> or
            ///     <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the root of the drive on which the free space changed.
            ///     <i>dwItem2</i> is not used and should be <see langword="null" />.
            /// </summary>
            SHCNE_FREESPACE = 0x00040000,

            /// <summary>
            ///     Storage media has been inserted into a drive.
            ///     <see cref="HChangeNotifyFlags.SHCNF_IDLIST" /> or
            ///     <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the root of the drive that contains the new media.
            ///     <i>dwItem2</i> is not used and should be <see langword="null" />.
            /// </summary>
            SHCNE_MEDIAINSERTED = 0x00000020,

            /// <summary>
            ///     Storage media has been removed from a drive.
            ///     <see cref="HChangeNotifyFlags.SHCNF_IDLIST" /> or
            ///     <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the root of the drive from which the media was removed.
            ///     <i>dwItem2</i> is not used and should be <see langword="null" />.
            /// </summary>
            SHCNE_MEDIAREMOVED = 0x00000040,

            /// <summary>
            ///     A folder has been created. <see cref="HChangeNotifyFlags.SHCNF_IDLIST" />
            ///     or <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the folder that was created.
            ///     <i>dwItem2</i> is not used and should be <see langword="null" />.
            /// </summary>
            SHCNE_MKDIR = 0x00000008,

            /// <summary>
            ///     A folder on the local computer is being shared via the network.
            ///     <see cref="HChangeNotifyFlags.SHCNF_IDLIST" /> or
            ///     <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the folder that is being shared.
            ///     <i>dwItem2</i> is not used and should be <see langword="null" />.
            /// </summary>
            SHCNE_NETSHARE = 0x00000200,

            /// <summary>
            ///     A folder on the local computer is no longer being shared via the network.
            ///     <see cref="HChangeNotifyFlags.SHCNF_IDLIST" /> or
            ///     <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the folder that is no longer being shared.
            ///     <i>dwItem2</i> is not used and should be <see langword="null" />.
            /// </summary>
            SHCNE_NETUNSHARE = 0x00000400,

            /// <summary>
            ///     The name of a folder has changed.
            ///     <see cref="HChangeNotifyFlags.SHCNF_IDLIST" /> or
            ///     <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the previous pointer to an item identifier list (PIDL) or name of the folder.
            ///     <i>dwItem2</i> contains the new PIDL or name of the folder.
            /// </summary>
            SHCNE_RENAMEFOLDER = 0x00020000,

            /// <summary>
            ///     The name of a nonfolder item has changed.
            ///     <see cref="HChangeNotifyFlags.SHCNF_IDLIST" /> or
            ///     <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the previous PIDL or name of the item.
            ///     <i>dwItem2</i> contains the new PIDL or name of the item.
            /// </summary>
            SHCNE_RENAMEITEM = 0x00000001,

            /// <summary>
            ///     A folder has been removed.
            ///     <see cref="HChangeNotifyFlags.SHCNF_IDLIST" /> or
            ///     <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the folder that was removed.
            ///     <i>dwItem2</i> is not used and should be <see langword="null" />.
            /// </summary>
            SHCNE_RMDIR = 0x00000010,

            /// <summary>
            ///     The computer has disconnected from a server.
            ///     <see cref="HChangeNotifyFlags.SHCNF_IDLIST" /> or
            ///     <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the server from which the computer was disconnected.
            ///     <i>dwItem2</i> is not used and should be <see langword="null" />.
            /// </summary>
            SHCNE_SERVERDISCONNECT = 0x00004000,

            /// <summary>
            ///     The contents of an existing folder have changed,
            ///     but the folder still exists and has not been renamed.
            ///     <see cref="HChangeNotifyFlags.SHCNF_IDLIST" /> or
            ///     <see cref="HChangeNotifyFlags.SHCNF_PATH" /> must be specified in <i>uFlags</i>.
            ///     <i>dwItem1</i> contains the folder that has changed.
            ///     <i>dwItem2</i> is not used and should be <see langword="null" />.
            ///     If a folder has been created, deleted, or renamed, use SHCNE_MKDIR, SHCNE_RMDIR, or
            ///     SHCNE_RENAMEFOLDER, respectively, instead.
            /// </summary>
            SHCNE_UPDATEDIR = 0x00001000,

            /// <summary>
            ///     An image in the system image list has changed.
            ///     <see cref="HChangeNotifyFlags.SHCNF_DWORD" /> must be specified in <i>uFlags</i>.
            /// </summary>
            SHCNE_UPDATEIMAGE = 0x00008000
        }

        #endregion // enum HChangeNotifyEventID

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom)
            {
            }

            public int X
            {
                get { return Left; }
                set
                {
                    Right -= Left - value;
                    Left = value;
                }
            }

            public int Y
            {
                get { return Top; }
                set
                {
                    Bottom -= Top - value;
                    Top = value;
                }
            }

            public int Height
            {
                get { return Bottom - Top; }
                set { Bottom = value + Top; }
            }

            public int Width
            {
                get { return Right - Left; }
                set { Right = value + Left; }
            }

            public Point Location
            {
                get { return new Point(Left, Top); }
                set
                {
                    X = value.X;
                    Y = value.Y;
                }
            }

            public Size Size
            {
                get { return new Size(Width, Height); }
                set
                {
                    Width = value.Width;
                    Height = value.Height;
                }
            }

            public static implicit operator Rectangle(RECT r)
            {
                return new Rectangle(r.Left, r.Top, r.Width, r.Height);
            }

            public static implicit operator RECT(Rectangle r)
            {
                return new RECT(r);
            }

            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r)
            {
                return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT)
                    return Equals((RECT) obj);
                if (obj is Rectangle)
                    return Equals(new RECT((Rectangle) obj));
                return false;
            }

            public override int GetHashCode()
            {
                return ((Rectangle) this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top,
                    Right, Bottom);
            }
        }
    }
}