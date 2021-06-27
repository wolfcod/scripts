using System;
using System.Text;
using Microsoft.Win32;

namespace miosync
{
    public class config
    {
        internal static string MIOSync = "Software\\miosync";

        private bool _RequireSync = false;

        private string _DownloadFolder;
        private bool _Convert;
        private AdapterType _Type;

        public string DownloadFolder
        {
            get { return _DownloadFolder; }
            set
            {
                if (this._DownloadFolder != value)
                     _RequireSync = true;  
                this._DownloadFolder = value;
            }
        }

        public bool Convert
        {
            get { return _Convert; }
            
            set 
            { 
                if (value != this._Convert)
                    _RequireSync = true; 
                this._Convert = value ; 
            }
        }

        public AdapterType Type
        {
            get { return _Type; }
            set {
                if (value != this._Type)
                    _RequireSync = true;
                this._Type = value; _RequireSync = true; }
        }

        /**
         * Private constructor!
         **/
        private config()
        {
        }

        private static config _instance = null;

        public static config getConfiguration()
        {
            if (config._instance == null)
            {
                config._instance = new config();
                config._instance.Load();

            }

            return config._instance;
        }

        /**
         * Write configuration on registry!
         **/
        public void Update()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(config.MIOSync, true);

            if (reg == null)
            {
                reg = Registry.CurrentUser.CreateSubKey(config.MIOSync);
            }

            if (this._RequireSync == false)
            {
                reg.Close();
                return;
            }

            reg.SetValue("Download Folder", this._DownloadFolder, RegistryValueKind.String);
            reg.SetValue("Convert", this._Convert, RegistryValueKind.DWord);
            reg.SetValue("Type", this._Type, RegistryValueKind.DWord);

            reg.Close();
        }

        protected void Load()
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(config.MIOSync);

            if (reg == null)
            {
                this._Type = AdapterType.NONE;
                this._Convert = false;
                this._DownloadFolder = "";
                this._RequireSync = false;
            }

            this._Type = (AdapterType) reg.GetValue("Type");
            int x = (int) reg.GetValue("Convert");
            this._Convert = (x == 1) ? true : false;
            this._DownloadFolder = (string) reg.GetValue("Download Folder");
            reg.Close();
        }

    }

}
