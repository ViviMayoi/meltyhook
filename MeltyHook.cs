using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MeltyHook
{
    public class MeltyBlood
    {
        static string ProcName = "MBAA";        // The name of the executable
        const int PROCESS_WM_READ = 0x0010;

        public Process MeltyBloodProc;                 // The process for the hooked Melty Blood
        public Process MeltyBloodProc2;
        public bool FoundMelty = false;         // A bool for finding the Melty Blood executable
        public bool UseSecondMelty = false; // A bool for deciding which active Melty process to hook onto

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        public byte[] ReadMem(int adr, int size)
        {
            // Get the process handle
            IntPtr ProcessHandle;
            if (!UseSecondMelty)
                ProcessHandle = OpenProcess(PROCESS_WM_READ, false, MeltyBloodProc.Id);
            else
                ProcessHandle = OpenProcess(PROCESS_WM_READ, false, MeltyBloodProc2.Id);


            // Create a buffer
            byte[] Buffer = new byte[size];
            int BytesRead = 0;

            // Read the memory
            ReadProcessMemory((int)ProcessHandle, adr, Buffer, size, ref BytesRead);

            // Return the buffer
            return Buffer;
        }

        public bool SwapActiveProcess()
        {
            GetMB();
            if (MeltyBloodProc2 != null)
            {
                UseSecondMelty = !UseSecondMelty;
                return true;
            }
            return false;
        }

        public bool GetMB()
        {
            Process[] MBProcesses = Process.GetProcessesByName(ProcName);
            if (MBProcesses.Length != 0)
            {
                MeltyBloodProc = MBProcesses[0];

                if (MBProcesses.Length > 1)
                    MeltyBloodProc2 = MBProcesses[1];
                else
                    MeltyBloodProc2 = null;

                // Return if the header for this process is correct
                return ReadMem(0x400000, 1)[0] != 0x00;
            }

            return false;
        }

        public bool SearchForMelty()
        {
            // Check if the Melty Blood process actually exists
            FoundMelty = MeltyBloodProc != null && ReadMem(0x400000, 1)[0] != 0x00;

            // Return if Melty Blood was actually found
            return FoundMelty;
        }

        // Character Names
        public string[,] CharacterNames = new string[0x64, 3];

        // Fill the character name array
        public MeltyBlood()
        {
            #region Code Names
            CharacterNames[0x00, 0] = "sion";
            CharacterNames[0x01, 0] = "arc";
            CharacterNames[0x02, 0] = "ciel";
            CharacterNames[0x03, 0] = "akiha";
            CharacterNames[0x04, 0] = "maids";
            CharacterNames[0x05, 0] = "hisui";
            CharacterNames[0x06, 0] = "kohak";
            CharacterNames[0x07, 0] = "tohno";
            CharacterNames[0x08, 0] = "miyak";
            CharacterNames[0x09, 0] = "wara";
            CharacterNames[0x0A, 0] = "nero";
            CharacterNames[0x0B, 0] = "vsion";
            CharacterNames[0x0C, 0] = "warc";
            CharacterNames[0x0D, 0] = "vkiha";
            CharacterNames[0x0E, 0] = "mech";
            CharacterNames[0x0F, 0] = "nanay";
            CharacterNames[0x11, 0] = "sacch";
            CharacterNames[0x12, 0] = "len";
            CharacterNames[0x13, 0] = "pciel";
            CharacterNames[0x14, 0] = "neco";
            CharacterNames[0x16, 0] = "aoko";
            CharacterNames[0x17, 0] = "wlen";
            CharacterNames[0x19, 0] = "nac";
            CharacterNames[0x1C, 0] = "kouma";
            CharacterNames[0x1D, 0] = "skiha";
            CharacterNames[0x1E, 0] = "ries";
            CharacterNames[0x1F, 0] = "roa";
            CharacterNames[0x21, 0] = "ryoug";
            CharacterNames[0x22, 0] = "nmech";
            CharacterNames[0x23, 0] = "kmech";
            CharacterNames[0x33, 0] = "hime";
            CharacterNames[0x63, 0] = "random";
            #endregion

            #region Full Names
            CharacterNames[0x00, 1] = "Sion Eltnam Atlasia";
            CharacterNames[0x01, 1] = "Arcueid Brunestud";
            CharacterNames[0x02, 1] = "Ciel";
            CharacterNames[0x03, 1] = "Akiha Tohno";
            CharacterNames[0x04, 1] = "Hisui & Kohaku";
            CharacterNames[0x05, 1] = "Hisui";
            CharacterNames[0x06, 1] = "Kohaku";
            CharacterNames[0x07, 1] = "Shiki Tohno";
            CharacterNames[0x08, 1] = "Miyako Arima";
            CharacterNames[0x09, 1] = "Wallachia";
            CharacterNames[0x0A, 1] = "Nrvnqsr Chaos";
            CharacterNames[0x0B, 1] = "Sion Tatari";
            CharacterNames[0x0C, 1] = "Red Arcueid";
            CharacterNames[0x0D, 1] = "Akiha Vermillion";
            CharacterNames[0x0E, 1] = "Mech-Hisui";
            CharacterNames[0x0F, 1] = "Shiki Nanaya";
            CharacterNames[0x11, 1] = "Satsuki Yumizuka";
            CharacterNames[0x12, 1] = "Len";
            CharacterNames[0x13, 1] = "Powered Ciel";
            CharacterNames[0x14, 1] = "Neco-Arc";
            CharacterNames[0x16, 1] = "Aoko Aozaki";
            CharacterNames[0x17, 1] = "White Len";
            CharacterNames[0x19, 1] = "Neco-Arc Chaos";
            CharacterNames[0x1C, 1] = "Kouma Kishima";
            CharacterNames[0x1D, 1] = "Seifuku Akiha";
            CharacterNames[0x1E, 1] = "Riesbyfe Stridberg";
            CharacterNames[0x1F, 1] = "Michael Roa Valdamjong";
            CharacterNames[0x21, 1] = "Shiki Ryougi";
            CharacterNames[0x22, 1] = "Neco & Mech";
            CharacterNames[0x23, 1] = "Koha & Mech";
            CharacterNames[0x33, 1] = "Archetype-Earth";
            CharacterNames[0x63, 1] = "Random";
            #endregion

            #region Short Names
            CharacterNames[0x00, 2] = "Sion";
            CharacterNames[0x01, 2] = "Arcueid";
            CharacterNames[0x02, 2] = "Ciel";
            CharacterNames[0x03, 2] = "Akiha";
            CharacterNames[0x04, 2] = "Maids";
            CharacterNames[0x05, 2] = "Hisui";
            CharacterNames[0x06, 2] = "Kohaku";
            CharacterNames[0x07, 2] = "Tohno";
            CharacterNames[0x08, 2] = "Miyako";
            CharacterNames[0x09, 2] = "Warachia";
            CharacterNames[0x0A, 2] = "Nero";
            CharacterNames[0x0B, 2] = "VSion";
            CharacterNames[0x0C, 2] = "WArc";
            CharacterNames[0x0D, 2] = "VAkiha";
            CharacterNames[0x0E, 2] = "Mech";
            CharacterNames[0x0F, 2] = "Nanaya";
            CharacterNames[0x11, 2] = "Satsuki";
            CharacterNames[0x12, 2] = "Len";
            CharacterNames[0x13, 2] = "PCiel";
            CharacterNames[0x14, 2] = "Neco";
            CharacterNames[0x16, 2] = "Aoko";
            CharacterNames[0x17, 2] = "WLen";
            CharacterNames[0x19, 2] = "NAC";
            CharacterNames[0x1C, 2] = "Kouma";
            CharacterNames[0x1D, 2] = "Seifuku";
            CharacterNames[0x1E, 2] = "Riesbyfe";
            CharacterNames[0x1F, 2] = "Roa";
            CharacterNames[0x21, 2] = "Ryougi";
            CharacterNames[0x22, 2] = "Necomech";
            CharacterNames[0x23, 2] = "Kohamech";
            CharacterNames[0x33, 2] = "Hime";
            CharacterNames[0x63, 2] = "Random";
            #endregion
        }
    }

    public enum MeltyMem : Int32
    {
        // Most values taken from https://github.com/pluot-mb/CCCaster/blob/master/netplay/Constants.hpp

        // Character Select Data
        CC_P1_SELECTOR_MODE_ADDR = 0x74D8EC,
        CC_P1_CHARA_SELECTOR_ADDR = 0x74D8F8,
        CC_P1_CHARACTER_ADDR = 0x74D8FC,
        CC_P1_MOON_SELECTOR_ADDR = 0x74D900,
        CC_P1_COLOR_SELECTOR_ADDR = 0x74D904,

        CC_P2_SELECTOR_MODE_ADDR = 0x74D910,
        CC_P2_CHARA_SELECTOR_ADDR = 0x74D91C,
        CC_P2_CHARACTER_ADDR = 0x74D920,
        CC_P2_MOON_SELECTOR_ADDR = 0x74D924,
        CC_P2_COLOR_SELECTOR_ADDR = 0x74D928,

        CC_STAGE_SELECTOR_ADDR = 0x74FD98,

        // Total size of a single player structure.
        CC_PLR_STRUCT_SIZE = 0xAFC,

        CC_P1_HEAT_ADDR = 0x555214,
        CC_P1_PUPPET_STATE_ADDR = 0x5552A8,

        CC_P2_HEAT_ADDR = CC_P1_HEAT_ADDR + CC_PLR_STRUCT_SIZE,
        CC_P2_PUPPET_STATE_ADDR = CC_P1_PUPPET_STATE_ADDR + CC_PLR_STRUCT_SIZE,

        // Player Score Values
        CC_P1_SCORE_ADDR = 0x76FC14,
        CC_P2_SCORE_ADDR = 0x770EB4,

        // Game States
        CC_INTRO_STATE_ADDR = 0x55D20B,
        CC_GAME_MODE_ADDR = 0x54EEE8
    }

    public enum NameType : Int32
    {
        Code = 0,
        Full = 1,
        Short = 2
    }
}