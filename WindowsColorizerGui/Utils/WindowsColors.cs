using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media;
using Microsoft.Win32;

namespace WindowsColorizerGui.Utils
{
    public static class WindowsColors
    {

        private const string AccentPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Accent";
        private static readonly string _accentPalette = "AccentPalette";


        public static void SetExplorerColors(Color color)
        {
            var key = Registry.CurrentUser.OpenSubKey(AccentPath, true);
            if (key != null)
            {
                var registered = (byte[]) key.GetValue(_accentPalette, null);
                //split by rgba
                var colorPal = registered.Split(4);
                var enumerable = colorPal as byte[][] ?? colorPal.ToArray();
                for (var j = 0; j < enumerable.Length; j++)
                {
                    //Console.WriteLine(@"Section {0}", j);
                    var section = enumerable[j];
                    if (j == 0)
                    {
                        //unknown
                    }
                    if (j == 1)
                    {
                        //unknown
                    }
                    if (j == 2)
                    {
                        //start orb hover
                        Console.WriteLine($"Colors for Orb Hover {section[0]} {section[1]} {section[2]}");
                        section[0] = color.R;
                        section[1] = color.G;
                        section[2] = color.B;
                    }
                    if (j == 3)
                    {
                        //tile colors for metro
                        Console.WriteLine($"Colors for Metro {section[0]} {section[1]} {section[2]}");
                        section[0] = color.R;
                        section[1] = color.G;
                        section[2] = color.B;
                    }
                    if (j == 4)
                    {
                        //icon tray color
                        Console.WriteLine($"Colors for tray {section[0]} {section[1]} {section[2]}");
                        section[0] = color.R;
                        section[1] = color.G;
                        section[2] = color.B;
                    }
                    if (j == 5)
                    {
                        //transparency off task bar color
                        Console.WriteLine($"Colors for taskbar t-off {section[0]} {section[1]} {section[2]}");
                        section[0] = color.R;
                        section[1] = color.G;
                        section[2] = color.B;
                    }
                    if (j == 6)
                    {
                        //transparency on task bar color
                        Console.WriteLine($"Colors for taskbar t-on {section[0]} {section[1]} {section[2]}");
                        section[0] = color.R;
                        section[1] = color.G;
                        section[2] = color.B;
                    }
                    if (j == 7)
                    {
                        //unknown
                    }

                    var newPallet = ByteUtils.Combine(enumerable);
                    key.SetValue(_accentPalette, newPallet, RegistryValueKind.Binary);
                    //this is needed to make the system update
                    ForceRefresh();
                    //TODO Force Explorer/Taskbar refresh
                }
            }
        }


        public static void SetTitleBarColor(Color color)
        {
            //Title bar use AABBGGRR    
            var systemColor = ByteUtils.ConvertHex($"{color.A:X2}{color.B:X2}{color.G:X2}{color.R:X2}");
            Console.WriteLine(systemColor);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\DWM", "AccentColor", systemColor,
                RegistryValueKind.DWord);
        }

        public static void ForceRefresh()
        {
            var key = Registry.CurrentUser.OpenSubKey(AccentPath, true);
            var rand = new Random();
            var rDword = -1 - rand.Next(0, 0xFFFFFF);
            key.SetValue("AccentColorMenu", rDword, RegistryValueKind.DWord);
            key.SetValue("AccentColor", rDword, RegistryValueKind.DWord);
            key.SetValue("StartColor", rDword, RegistryValueKind.DWord);
            key.SetValue("StartColorMenu", rDword, RegistryValueKind.DWord);
            key.Close();
        }
    }
}