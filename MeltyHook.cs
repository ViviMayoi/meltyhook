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
        public string[][] CharacterNames = new string[3][];
        // Fill the character name array
        public MeltyBlood()
        {

            CharacterNames[0] = new string[0x64];
            CharacterNames[1] = new string[0x64];
            CharacterNames[2] = new string[0x64];

            #region Code Names
            CharacterNames[0][0x00] = "sion";
            CharacterNames[0][0x01] = "arc";
            CharacterNames[0][0x02] = "ciel";
            CharacterNames[0][0x03] = "akiha";
            CharacterNames[0][0x04] = "maids";
            CharacterNames[0][0x05] = "hisui";
            CharacterNames[0][0x06] = "kohak";
            CharacterNames[0][0x07] = "tohno";
            CharacterNames[0][0x08] = "miyak";
            CharacterNames[0][0x09] = "wara";
            CharacterNames[0][0x0A] = "nero";
            CharacterNames[0][0x0B] = "vsion";
            CharacterNames[0][0x0C] = "warc";
            CharacterNames[0][0x0D] = "vkiha";
            CharacterNames[0][0x0E] = "mech";
            CharacterNames[0][0x0F] = "nanay";
            CharacterNames[0][0x11] = "sacch";
            CharacterNames[0][0x12] = "len";
            CharacterNames[0][0x13] = "pciel";
            CharacterNames[0][0x14] = "neco";
            CharacterNames[0][0x16] = "aoko";
            CharacterNames[0][0x17] = "wlen";
            CharacterNames[0][0x19] = "nac";
            CharacterNames[0][0x1C] = "kouma";
            CharacterNames[0][0x1D] = "skiha";
            CharacterNames[0][0x1E] = "ries";
            CharacterNames[0][0x1F] = "roa";
            CharacterNames[0][0x21] = "ryoug";
            CharacterNames[0][0x22] = "nmech";
            CharacterNames[0][0x23] = "kmech";
            CharacterNames[0][0x33] = "hime";
            CharacterNames[0][0x63] = "random";
            #endregion

            #region Full Names
            CharacterNames[1][0x00] = "Sion Eltnam Atlasia";
            CharacterNames[1][0x01] = "Arcueid Brunestud";
            CharacterNames[1][0x02] = "Ciel";
            CharacterNames[1][0x03] = "Akiha Tohno";
            CharacterNames[1][0x04] = "Hisui & Kohaku";
            CharacterNames[1][0x05] = "Hisui";
            CharacterNames[1][0x06] = "Kohaku";
            CharacterNames[1][0x07] = "Shiki Tohno";
            CharacterNames[1][0x08] = "Miyako Arima";
            CharacterNames[1][0x09] = "Wallachia";
            CharacterNames[1][0x0A] = "Nrvnqsr Chaos";
            CharacterNames[1][0x0B] = "Sion Tatari";
            CharacterNames[1][0x0C] = "Red Arcueid";
            CharacterNames[1][0x0D] = "Akiha Vermillion";
            CharacterNames[1][0x0E] = "Mech-Hisui";
            CharacterNames[1][0x0F] = "Shiki Nanaya";
            CharacterNames[1][0x11] = "Satsuki Yumizuka";
            CharacterNames[1][0x12] = "Len";
            CharacterNames[1][0x13] = "Powered Ciel";
            CharacterNames[1][0x14] = "Neco-Arc";
            CharacterNames[1][0x16] = "Aoko Aozaki";
            CharacterNames[1][0x17] = "White Len";
            CharacterNames[1][0x19] = "Neco-Arc Chaos";
            CharacterNames[1][0x1C] = "Kouma Kishima";
            CharacterNames[1][0x1D] = "Seifuku Akiha";
            CharacterNames[1][0x1E] = "Riesbyfe Stridberg";
            CharacterNames[1][0x1F] = "Michael Roa Valdamjong";
            CharacterNames[1][0x21] = "Shiki Ryougi";
            CharacterNames[1][0x22] = "Neco & Mech";
            CharacterNames[1][0x23] = "Koha & Mech";
            CharacterNames[1][0x33] = "Archetype-Earth";
            CharacterNames[1][0x63] = "Random";
            #endregion

            #region Short Names
            CharacterNames[2][0x00] = "Sion";
            CharacterNames[2][0x01] = "Arcueid";
            CharacterNames[2][0x02] = "Ciel";
            CharacterNames[2][0x03] = "Akiha";
            CharacterNames[2][0x04] = "Maids";
            CharacterNames[2][0x05] = "Hisui";
            CharacterNames[2][0x06] = "Kohaku";
            CharacterNames[2][0x07] = "Tohno";
            CharacterNames[2][0x08] = "Miyako";
            CharacterNames[2][0x09] = "Warachia";
            CharacterNames[2][0x0A] = "Nero";
            CharacterNames[2][0x0B] = "VSion";
            CharacterNames[2][0x0C] = "WArc";
            CharacterNames[2][0x0D] = "VAkiha";
            CharacterNames[2][0x0E] = "Mech";
            CharacterNames[2][0x0F] = "Nanaya";
            CharacterNames[2][0x11] = "Satsuki";
            CharacterNames[2][0x12] = "Len";
            CharacterNames[2][0x13] = "PCiel";
            CharacterNames[2][0x14] = "Neco";
            CharacterNames[2][0x16] = "Aoko";
            CharacterNames[2][0x17] = "WLen";
            CharacterNames[2][0x19] = "NAC";
            CharacterNames[2][0x1C] = "Kouma";
            CharacterNames[2][0x1D] = "Seifuku";
            CharacterNames[2][0x1E] = "Riesbyfe";
            CharacterNames[2][0x1F] = "Roa";
            CharacterNames[2][0x21] = "Ryougi";
            CharacterNames[2][0x22] = "Necomech";
            CharacterNames[2][0x23] = "Kohamech";
            CharacterNames[2][0x33] = "Hime";
            CharacterNames[2][0x63] = "Random";
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