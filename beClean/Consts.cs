namespace beClean
{
    public static class Consts
    {
        // Navigation
        public const string NavigationPushMessage = @"NavigationPushMessage";
        public const string NavigationPopMessage = @"NavigationPopMessage";

        // Commands:
        // Получение истории за промежуток - GET_HISTORY@16:00-20:00\n
        // Старт непрерывной отправки - ALWAYS_SEND
        // Прекратить непрерывно отправлять - STOP_SEND
        // Интервал считывания - TIME_CHECK@20  (20 - это типа 20 секунд)
        public const string GET_HISTORY_COMMAND = @"GET_HISTORY";
        public const string ALWAYS_SEND_COMMAND = @"ALWAYS_SEND";
        public const string STOP_SEND_COMMAND = @"STOP_SEND";
        public const string TIME_CHECK_COMMAND = @"TIME_CHECK";
    }
}
