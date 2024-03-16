using System.Runtime.InteropServices;
using Microsoft.Management.Infrastructure;
using System.Text.RegularExpressions;

namespace App;

class Program
{

    public static void Main()
    {
        Connector.Launch();
    }
}

partial class Connector
{

    const uint KEYEVENTF_KEYDOWN = 0x0000;
    const uint KEYEVENTF_KEYUP = 0x0002;

    const uint MiracastSupportMajorVersion = 6;
    const uint MiracastSupportMinorVersion = 6;

    [LibraryImport("user32.dll", SetLastError = true)]
    static partial void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

    public class MiracastNotSupportedhException : SystemException
    {
        public MiracastNotSupportedhException(string message) : base(message) { }
    }

    /// <summary>
    /// Miracastへの接続をおこないます。
    /// </summary>
    /// <exception cref="MiracastNotSupportedhException">MiracastをサポートするNetowkAdapterがない場合にスローされます。</exception>
    /// <remarks>
    /// この関数は、現在のNDISバージョンと指定されたバージョンを比較します。
    /// MiracastをサポートするNetowkAdapterがない場合にMiracastNotSupportedhExceptionがスローされます。
    /// </remarks>
    unsafe public static void Launch()
    {
        // error

        if (!CheckMiracastSupport())
        {
            MessageBox.Show(
                $"This computer not support Miracast. Miracast require NdisVersion {MiracastSupportMajorVersion}.{MiracastSupportMinorVersion} or higher.",
                "Network Driver Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            throw new MiracastNotSupportedhException("Network Driver Error");
        }

        // Press the left Win key down
        keybd_event((byte)Keys.LWin, 0, KEYEVENTF_KEYDOWN, 0);

        // Press the K key down
        keybd_event((byte)Keys.K, 0, KEYEVENTF_KEYDOWN, 0);

        // Release the K key
        keybd_event((byte)Keys.K, 0, KEYEVENTF_KEYUP, 0);

        // Release the left Win key
        keybd_event((byte)Keys.LWin, 0, KEYEVENTF_KEYUP, 0);
    }

    /// <summary>
    /// MiracastをサポートするNetworkAdapterを持っているか確認します。
    /// </summary>
    /// <returns>MiracastをサポートするNetworkAdapterを持っている場合はtrue、それ以外はfalse</returns>
    public static bool CheckMiracastSupport()
    {

        var judge = false;
        // wifi-2とか。
        string search = "Wi-Fi";

        using (var session = CimSession.Create(null))
        {
            foreach (var instance in session.EnumerateInstances(@"root\standardcimv2", "MSFT_NetAdapter"))
            {
                var name = instance.CimInstanceProperties["Name"].Value as string;
                if (name is null) {
                    continue;
                }
                var major = instance.CimInstanceProperties["DriverMajorNdisVersion"].Value as byte?;
                var minor = instance.CimInstanceProperties["DriverMinorNdisVersion"].Value as byte?;

                // name Wifiでかつmiracast対応のものを探す。
                if (name.IndexOf(search) < 0)
                {
                    if ((major == MiracastSupportMajorVersion && minor >= MiracastSupportMinorVersion ) || (major > MiracastSupportMajorVersion))
                    {
                        judge = true;
                    }
                }
                
            }
        }

        return judge;
    }
}
