using System.Runtime.InteropServices;

namespace WirelessDisplayConnector;

class Program
{
    const uint KEYEVENTF_KEYDOWN = 0x0000;
    const uint KEYEVENTF_KEYUP = 0x0002;

    [DllImport("user32.dll", SetLastError = true)]
    static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

    public static void Main()
    {
        // Press the left Win key down
        keybd_event((byte)Keys.LWin, 0, KEYEVENTF_KEYDOWN, 0);

        // Press the K key down
        keybd_event((byte)Keys.K, 0, KEYEVENTF_KEYDOWN, 0);

        // Release the K key
        keybd_event((byte)Keys.K, 0, KEYEVENTF_KEYUP, 0);

        // Release the left Win key
        keybd_event((byte)Keys.LWin, 0, KEYEVENTF_KEYUP, 0);
    }
}

