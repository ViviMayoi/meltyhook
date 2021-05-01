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
            if(MBProcesses.Length != 0) {
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
        public string[] CharNames = new string[0x64];

        public MeltyBlood()
        {
            // Set character names
            CharNames[0x00] = "sion";
            CharNames[0x01] = "arc";
            CharNames[0x02] = "ciel";
            CharNames[0x03] = "akiha";
            CharNames[0x04] = "maids";
            CharNames[0x05] = "hisui";
            CharNames[0x06] = "kohak";
            CharNames[0x07] = "tohno";
            CharNames[0x08] = "miyak";
            CharNames[0x09] = "wara";
            CharNames[0x0A] = "nero";
            CharNames[0x0B] = "vsion";
            CharNames[0x0C] = "warc";
            CharNames[0x0D] = "vkiha";
            CharNames[0x0E] = "mech";
            CharNames[0x0F] = "nanay";
            CharNames[0x11] = "sacch";
            CharNames[0x12] = "len";
            CharNames[0x13] = "pciel";
            CharNames[0x14] = "neco";
            CharNames[0x16] = "aoko";
            CharNames[0x17] = "wlen";
            CharNames[0x19] = "nac";
            CharNames[0x1C] = "kouma";
            CharNames[0x1D] = "skiha";
            CharNames[0x1E] = "ries";
            CharNames[0x1F] = "roa";
            CharNames[0x21] = "ryoug";
            CharNames[0x22] = "nmech";
            CharNames[0x23] = "kmech";
            CharNames[0x33] = "hime";
            CharNames[0x63] = "random";
        }
    }

    public enum MeltyMem: int
    {
        // Values taken from https://github.com/pluot-mb/CCCaster/blob/master/netplay/Constants.hpp

        // Character Select Data
        CC_P1_SELECTOR_MODE_ADDR        = 0x74D8EC,
        CC_P1_CHARA_SELECTOR_ADDR       = 0x74D8F8,
        CC_P1_CHARACTER_ADDR            = 0x74D8FC,
        CC_P1_MOON_SELECTOR_ADDR        = 0x74D900,
        CC_P1_COLOR_SELECTOR_ADDR       = 0x74D904,

        CC_P2_SELECTOR_MODE_ADDR        = 0x74D910,
        CC_P2_CHARA_SELECTOR_ADDR       = 0x74D91C,
        CC_P2_CHARACTER_ADDR            = 0x74D920,
        CC_P2_MOON_SELECTOR_ADDR        = 0x74D924,
        CC_P2_COLOR_SELECTOR_ADDR       = 0x74D928,

        CC_STAGE_SELECTOR_ADDR          = 0x74FD98,

        // Total size of a single player structure.
        CC_PLR_STRUCT_SIZE              = 0xAFC,

        CC_P1_HEAT_ADDR                 = 0x555214,
        CC_P1_PUPPET_STATE_ADDR         = 0x5552A8,

        CC_P2_HEAT_ADDR                 = CC_P1_HEAT_ADDR + CC_PLR_STRUCT_SIZE,
        CC_P2_PUPPET_STATE_ADDR         = CC_P1_PUPPET_STATE_ADDR + CC_PLR_STRUCT_SIZE,

        // Values found by ViviMayoi
        CC_P1_SCORE_ADDR = 0x76FC14,
        CC_P2_SCORE_ADDR = 0x770EB4
    }
}