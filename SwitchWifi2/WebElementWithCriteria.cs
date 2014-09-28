using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchWifi2
{
    class WebElementWithCriteria
    {
        public IWebElement Control { get; set; }
        public By Criteria { get; set; }
    }
}
