using Plugin.Settings;
using Plugin.Settings.Abstractions;
namespace AssetTracking.Helpers
{
    /// <summary>  
    /// This is the Settings static class that can be used in your Core solution or in any  
    /// of your client applications. All settings are laid out the same exact way with getters  
    /// and setters.   
    /// </summary>  
    public static class UserSettings
    {
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        public static string OperatorId
        {
            get => AppSettings.GetValueOrDefault(nameof(OperatorId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(OperatorId), value);
        }
        public static string PlantId
        {
            get => AppSettings.GetValueOrDefault(nameof(PlantId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(PlantId), value);
        }
        public static string DeptId
        {
            get => AppSettings.GetValueOrDefault(nameof(DeptId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(DeptId), value);
        }
        public static string ModeId
        {
            get => AppSettings.GetValueOrDefault(nameof(ModeId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(ModeId), value);
        }
        public static string ActionItemId
        {
            get => AppSettings.GetValueOrDefault(nameof(ActionItemId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(ActionItemId), value);
        }
        public static string StationId
        {
            get => AppSettings.GetValueOrDefault(nameof(StationId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(StationId), value);
        }




        public static string UseGPS
        {
            get => AppSettings.GetValueOrDefault(nameof(UseGPS), bool.TrueString);
            set => AppSettings.AddOrUpdateValue(nameof(UseGPS), value);
        }
        public static string WorkOffline
        {
            get => AppSettings.GetValueOrDefault(nameof(WorkOffline), bool.FalseString);
            set => AppSettings.AddOrUpdateValue(nameof(WorkOffline), value);
        }
        public static string SoundEnabled
        {
            get => AppSettings.GetValueOrDefault(nameof(SoundEnabled), bool.FalseString);
            set => AppSettings.AddOrUpdateValue(nameof(SoundEnabled), value);
        }
        public static string UseTimer
        {
            get => AppSettings.GetValueOrDefault(nameof(UseTimer), bool.FalseString);
            set => AppSettings.AddOrUpdateValue(nameof(UseTimer), value);
        }
        public static string UserName
        {
            get => AppSettings.GetValueOrDefault(nameof(UserName), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(UserName), value);
        }

        public static string Password
        {
            get => AppSettings.GetValueOrDefault(nameof(Password), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Password), value);
        }

        public static string MobileNumber
        {
            get => AppSettings.GetValueOrDefault(nameof(MobileNumber), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(MobileNumber), value);
        }

        public static string ShiftStart
        {
            get => AppSettings.GetValueOrDefault(nameof(ShiftStart), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(ShiftStart), value);
        }
        public static string ShiftEnd
        {
            get => AppSettings.GetValueOrDefault(nameof(ShiftEnd), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(ShiftEnd), value);
        }


        public static void ClearAllData()
        {
            AppSettings.Clear();
        }

    }

}
