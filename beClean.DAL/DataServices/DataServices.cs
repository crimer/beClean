
using beClean.Services.DataServices.BluetoothLE;
using beClean.Services.DataServices.BClassic;
using beClean.Services.DataServices.Notifications;

namespace beClean.Services.DataServices
{
    public static class DataServices
    {
        public static IBluetoothLEService BluetoothLE { get; set; }
        public static IBClassic BClassic { get; set; }
        public static INotificationService Notifications { get; set; }
        public static void Init()
        {
            BluetoothLE = new BluetoothLEService();
            BClassic = new BClassicService();
            //Notifications = new NotificationService();
        }
    }
}
