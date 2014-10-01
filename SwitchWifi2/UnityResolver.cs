using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace SwitchWifi2
{
    static class UnityResolver
    {
        private static IUnityContainer container;

        public static IUnityContainer BuildUnityContainer()
        {
            if (container == null)
            { 
                container = new UnityContainer();
                container.LoadConfiguration();
            }
            return container;
        }
    }
}
