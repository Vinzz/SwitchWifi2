using SeleniumTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using SwitchWifi2.Properties;

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

        public SwitchWifi()
        {
            // Create a simple tray menu with only one item.
            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);
           
            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            trayIcon      = new NotifyIcon();
            trayIcon.Text = "Switch Wifi, loading informations";
            trayIcon.Icon = new Icon(SystemIcons.Question, 40, 40);
 
            // Add menu to tray icon and show it.
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible     = true;
        }
 
        protected override void OnLoad(EventArgs e)
        {
            Visible       = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.
 
            base.OnLoad(e);
            HandleIcon(new LiveboxAutomate().IsWifiEnabled());
            trayMenu.MenuItems.Add("Switch Wifi", OnSwitchWifi);
            trayIcon.ContextMenu = trayMenu;
        }
 
        private void OnSwitchWifi(object sender, EventArgs e)
        {
            trayIcon.Text = "Switching Wifi... Please wait";
            trayIcon.Icon = new Icon(SystemIcons.Question, 40, 40);
            bool isWifiEnabled = new LiveboxAutomate().SwitchWifi();

            HandleIcon(isWifiEnabled);
        }

        private void HandleIcon(bool isWifiEnabled)
        {
           if (isWifiEnabled)
           {
               trayIcon.Icon = Resources.Wifi;
               trayIcon.Text = "Wifi is Enabled";
           }
           else
           {
                trayIcon.Icon = Resources.NoWifi;
                trayIcon.Text = "Wifi is Disabled";
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
