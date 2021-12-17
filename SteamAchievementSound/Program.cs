using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace SteamAchievementSound
{
    class Program
    {
        // Do not edit! This is a constant value of the Steam64ID Identifier
        // Provided by valve on https://developer.valvesoftware.com/wiki/SteamID
        const long Steam64IDIdentifier = 0x0110000100000000;

        private static AchievementSound achievementSound;

        static void Main(string[] args)
        {
            long steamID3 = Convert.ToInt64(
                    Registry.GetValue(
                        @"HKEY_CURRENT_USER\Software\Valve\Steam\ActiveProcess",
                        "ActiveUser",
                        0));
            // To convert we just need to add the Identifier to the existing SteamID3
            long steamID64 = steamID3 + Steam64IDIdentifier;

            achievementSound = new AchievementSound(steamID64.ToString());

            achievementSound.Run();

            Console.ReadKey();
        }
    }
}
