using UnityEngine;

public class GameSystem
{

    public class ClientInfo
    {
        public string AppVersion;
        public string Locale;
        public string Platform;
        public string DeviceModel;
        public string DeviceMake;
        public string TimeZone;
    }

    public static ClientInfo GetClientInfo()
    {
        ClientInfo clientInfo = new ClientInfo();
        clientInfo.AppVersion = GetClientVersion();
        clientInfo.DeviceModel = SystemInfo.deviceModel;
        clientInfo.DeviceMake = GetDeviceMake();
        clientInfo.Locale = GetClientLocale();
        clientInfo.Platform = SystemInfo.deviceType.Equals(DeviceType.Handheld) ? "Mobile" : SystemInfo.deviceType.ToString();
        clientInfo.TimeZone = GetClientTimezone();
        return clientInfo;
    }

    private static string GetClientVersion()
    {
        // TODO : this should be implemented to return correct Client Version.
        return "1.0.1";
    }

    private static string GetDeviceMake()
    {
        // TODO : this should be implemented to get Client Device vendor.
        return "Samsung";
    }

    private static string GetClientLocale()
    {
        // TODO : this should be implemented to get Client Locale from device information.
        return "US";
    }

    private static string GetClientTimezone()
    {
        // TODO : this should be implemented depending on service design.
        return "UTC";
    }
}
