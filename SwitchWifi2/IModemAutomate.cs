using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchWifi2
{
    interface IModemAutomate
    {
        /// <summary>
        /// Switch the wifi state
        /// </summary>
        /// <returns>True if the wifi was enabled</returns>
        bool SwitchWifi();

        /// <summary>
        /// Checks the wifi state
        /// </summary>
        /// <returns>True if the wifi is enabled</returns>
        bool IsWifiEnabled();
    }
}
