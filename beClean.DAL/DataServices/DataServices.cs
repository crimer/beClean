
using beClean.DAL.DataServices.BluetoothLE;
using beClean.DAL.DataServices.BluetoothClassic;

namespace beClean.DAL.DataServices
{
    public static class DataServices
    {
        public static IBluetoothLEService BluetoothLE { get; set; }
        public static IBluetoothClassicService BluetoothClassic { get; set; }
        public static void Init()
        {
            BluetoothLE = new BluetoothLEService();
            BluetoothClassic = new BluetoothClassicService();
        }
    }
}
