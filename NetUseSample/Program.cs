using Pinvoke;

namespace NetUseSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string remoteUNC = @"\\reddog\builds\branches\git_ad_msods_core_master\1.0.9050.0";
            string deviceName = "H:";
            string username = @"redmond\gustavoa";
            string password = "";

            // remote connection without mapping to local drive
            PinvokeNetUse.Disconnect(remoteUNC);
            PinvokeNetUse.Connect(
                remoteUNC,
                username,
                password);

            // remote connection mapping to a local drive
            PinvokeNetUse.Disconnect(deviceName);
            PinvokeNetUse.Connect(
                remoteUNC,
                deviceName,
                username,
                password);
        }
    }
}
