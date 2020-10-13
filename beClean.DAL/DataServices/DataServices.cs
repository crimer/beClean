
using beClean.DAL.DataServices.BluetoothLE;
using beClean.DAL.DataServices.BClassic;

namespace beClean.DAL.DataServices
{
    public static class DataServices
    {
        public static IBluetoothLEService BluetoothLE { get; set; }
        public static IBClassic BClassic { get; set; }
        public static void Init()
        {
            BluetoothLE = new BluetoothLEService();
            BClassic = new BClassicService();
        }
    }
}
