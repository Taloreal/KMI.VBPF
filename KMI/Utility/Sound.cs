namespace KMI.Utility
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class Sound
    {
        private static string ns = "";

        [DllImport("winmm.dll")]
        private static extern int mciSendString(string lpstrCommand, ref string lpstrReturnString, int uReturnLength, int hwndCallback);
        public static bool PlayMidiFromFile(string filename)
        {
            int num = -1;
            if (File.Exists(Application.StartupPath + @"\" + filename))
            {
                try
                {
                    string currentDirectory = Environment.CurrentDirectory;
                    Environment.CurrentDirectory = Application.StartupPath;
                    num = mciSendString("stop midi", ref ns, 0, 0);
                    num = mciSendString("close midi", ref ns, 0, 0);
                    num = mciSendString("open sequencer!" + filename + " alias midi", ref ns, 0, 0);
                    num = mciSendString("play midi", ref ns, 0, 0);
                    Environment.CurrentDirectory = currentDirectory;
                }
                catch
                {
                }
            }
            return (num == 0);
        }

        [DllImport("winmm.dll")]
        private static extern int PlaySound(byte[] pszSound, short hMod, long fdwSound);
        public static int PlaySoundFromFile(string sSoundFile)
        {
            return PlaySoundFromFile(sSoundFile, false, true, false, false, false);
        }

        private static int PlaySoundFromFile(string sSoundFile, bool bSynchronous, bool bIgnoreErrors, bool bNoDefault, bool bLoop, bool bNoStop)
        {
            sSoundFile = Application.StartupPath + @"\" + sSoundFile;
            if (File.Exists(sSoundFile))
            {
                int uFlags = 0;
                if (!bSynchronous)
                {
                    uFlags = 1;
                }
                if (bNoDefault)
                {
                    uFlags += 2;
                }
                if (bLoop)
                {
                    uFlags += 8;
                }
                if (bNoStop)
                {
                    uFlags += 0x10;
                }
                try
                {
                    return sndPlaySoundA(sSoundFile, uFlags);
                }
                catch
                {
                }
            }
            return 0;
        }

        [DllImport("winmm.dll")]
        private static extern int sndPlaySoundA(string lpszSoundName, int uFlags);
        public static bool StopMidi()
        {
            int num = -1;
            try
            {
                num = mciSendString("stop midi", ref ns, 0, 0);
                num = mciSendString("close midi", ref ns, 0, 0);
            }
            catch
            {
            }
            return (num == 0);
        }
    }
}

