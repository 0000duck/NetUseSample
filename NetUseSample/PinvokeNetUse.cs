using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Pinvoke
{
    public class PinvokeNetUse
    {
        const int CONNECT_UPDATE_PROFILE = 0x00000001;
        const int RESOURCE_CONNECTED = 0x00000001;
        const int RESOURCETYPE_DISK = 0x00000001;
        const int RESOURCEDISPLAYTYPE_SHARE = 0x00000003;
        const int RESOURCEUSAGE_CONNECTABLE = 0x00000001;

        [StructLayout(LayoutKind.Sequential)]
        private class NETRESOURCE
        {
            public int dwScope;
            public int dwType;
            public int dwDisplayType;
            public int dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        }

        [DllImport("Mpr.dll")]
        private static extern int WNetUseConnection(
            IntPtr hwndOwner,
            NETRESOURCE lpNetResource,
            string lpPassword,
            string lpUserID,
            int dwFlags,
            string lpAccessName,
            string lpBufferSize,
            string lpResult);

        [DllImport("Mpr.dll")]
        public static extern int WNetCancelConnection2(
            string lpName,
            int dwFlags,
            bool fForce);

        public static void Connect(
            string remoteUNC,
            string username,
            string password)
        {
            Connect(remoteUNC, null, username, password);
        }

        public static void Connect(
            string remoteUNC,
            string deviceName,
            string username,
            string password
            )
        {
            NETRESOURCE nr = new NETRESOURCE();
            nr.dwScope = RESOURCE_CONNECTED;
            nr.dwType = RESOURCETYPE_DISK;
            nr.dwDisplayType = RESOURCEDISPLAYTYPE_SHARE;
            nr.dwUsage = RESOURCEUSAGE_CONNECTABLE;
            nr.lpLocalName = deviceName;
            nr.lpRemoteName = remoteUNC;
            nr.lpComment = null;
            nr.lpProvider = null;
            int dwFlags = CONNECT_UPDATE_PROFILE;
            string lpAccessName = null;
            string lpBufferSize = null;
            string lpResult = null;

            int errorCode = WNetUseConnection(IntPtr.Zero, nr, password, username, dwFlags, lpAccessName, lpBufferSize, lpResult);
            Win32Exception exception = new Win32Exception(errorCode);
            string errorMessage = string.Format("{0}: {1}", errorCode, exception.Message);
            if (errorCode == 0)
            {
                Console.WriteLine(errorMessage);
            }
            else
            {
                throw new Win32Exception(errorMessage);
            }            
        }

        public static void Disconnect(string deviceName)
        {
            int errorCode = WNetCancelConnection2(deviceName, CONNECT_UPDATE_PROFILE, true);
            Win32Exception exception = new Win32Exception(errorCode);
            string errorMessage = string.Format("{0}: {1}", errorCode, exception.Message);
            if (errorCode == 0 || errorCode == 2250)
            {
                Console.WriteLine(errorMessage);
            }
            else
            {
                throw new Win32Exception(errorMessage);
            }
        }
    }
}
