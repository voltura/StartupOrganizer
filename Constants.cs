namespace StartupOrganizer
{
    internal static class Constants
    {
        internal const string UNKNOWN = "Unknown";
        internal const string CURRENT_USER_RUN_REG =
            @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        internal const string CURRENT_USER_APPROVED_RUN_REG =
            @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";
        internal const string CURRENT_USER_APPROVED_STARTUP_FOLDER_REG =
            @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\StartupFolder";
        internal const string CURRENT_USER_RUN_REG32 =
            @"HKEY_CURRENT_USER\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run";
        internal const string LOCAL_MACHINE_RUN_REG =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        internal const string LOCAL_MACHINE_APPROVED_RUN_REG =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";
        internal const string LOCAL_MACHINE_APPROVED_STARTUP_FOLDER_REG =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\StartupFolder";
        internal const string LOCAL_MACHINE_RUN_REG32 =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run";
        internal const string RUN_SUBKEY_REG =
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        internal const string RUN_SUBKEY_REG32 =
            @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Run";
        internal const string APPROVED_RUN_SUBKEY_REG =
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";
        internal const string APPROVED_RUN_SUBKEY_REG32 =
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run32";
        internal const string APPROVED_STARTUP_FOLDER_SUBKEY_REG =
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\StartupFolder";
        internal const string UWP_APP_SUBKEY_REG =
            @"SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData";
        internal const string WINDOWS_OS_PRODUCT_NAME =
            @"Microsoft® Windows® Operating System";

        public enum STATUS
        {
            ENABLED = 0x02,
            DISABLED = 0x03
        }

        public enum REG_ROOT
        {
            HKLM,
            HKCU
        }

        public enum FILE_TYPE
        {
            EXECUTABLE,
            SHORTCUT,
            DLL,
            OTHER
        }
    }
}
