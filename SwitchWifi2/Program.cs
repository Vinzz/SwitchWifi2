using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using SwitchWifi2.Properties;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace SwitchWifi2
{
    class SwitchWifi : Form
    {
        static void Main(string[] args)
        {
            Application.Run(new SwitchWifi());
        }

        private NotifyIcon  trayIcon;
        private ContextMenu trayMenu;
        private System.Timers.Timer timer;


        public SwitchWifi()
        {
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add(Resources.Exit, OnExit);
           
            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon      = new NotifyIcon();
            trayIcon.Text = Resources.Loading;
            trayIcon.Icon = Resources.LookingForWifi;
 
            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible     = true;

            // Check every 60 minutes, in case wifi state was changed from elsewhere
            timer = new System.Timers.Timer(3600 * 1000);
            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CheckWifiState();
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                Visible = false; // Hide form window.
                ShowInTaskbar = false; // Remove from taskbar.

                base.OnLoad(e);

                CheckWifiState();
                trayMenu.MenuItems.Add(Resources.Switch, OnSwitchWifi);
                trayIcon.ContextMenu = trayMenu;

                // Switch on double click
                trayIcon.DoubleClick += OnSwitchWifi;
            }
            catch (Exception ex)
            {
                ProcessExp(ex);
            }
        }

        private void CheckWifiState()
        {
            IModemAutomate myServiceInstance = UnityResolver.BuildUnityContainer().Resolve(typeof(IModemAutomate)) as IModemAutomate;

            HandleIcon(myServiceInstance.IsWifiEnabled());
        }

        private static void ProcessExp(Exception ex)
        {
            MessageBox.Show(string.Format(Resources.Error, ex.Message));
        }
 
        private void OnSwitchWifi(object sender, EventArgs e)
        {
            try
            {
                trayIcon.Text = Resources.InProgress;
                trayIcon.Icon = Resources.LookingForWifi;

                IModemAutomate myServiceInstance = UnityResolver.BuildUnityContainer().Resolve(typeof(IModemAutomate)) as IModemAutomate;
                bool isWifiEnabled = myServiceInstance.SwitchWifi();

                HandleIcon(isWifiEnabled);
            }
            catch (Exception ex)
            {

                ProcessExp(ex);
            }
        }

        private void HandleIcon(bool isWifiEnabled)
        {
           if (isWifiEnabled)
           {
               trayIcon.Icon = Resources.Wifi;
               trayIcon.Text = Resources.WifiOn;
           }
           else
           {
                trayIcon.Icon = Resources.NoWifi;
                trayIcon.Text = Resources.WifiOff;
           }
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }
 
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }
 
            base.Dispose(isDisposing);
        }  
    }
}
