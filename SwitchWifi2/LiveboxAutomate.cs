using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using SwitchWifi2;
using System.Drawing;

namespace SwitchWifi2
{
  
    public class LiveboxAutomate : IModemAutomate
    {
        private IWebDriver driver;
        private string baseURL;

        public LiveboxAutomate()
        {
            Console.WriteLine("Prepare a new browser instance");

            var driverService = PhantomJSDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            driver = new PhantomJSDriver(driverService);

            driver.Manage().Window.Position = new Point(-2000, 0);


            baseURL = "http://livebox/";
        }
        
  
        private void CleanUp()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }
        

        public bool SwitchWifi()
        {
            bool isWifiEnabled = false;
            Console.WriteLine("Go to the livebox admin page");
            driver.Navigate().GoToUrl(baseURL);
            StubbornFindElement(By.Id("PopupPassword")).Clear();

            Console.WriteLine("Fetch the LiveBoxAdmin env variable content");
            string password = Environment.GetEnvironmentVariable("LiveBoxAdmin");

            Console.WriteLine("Connexion");
            StubbornFindElement(By.Id("PopupPassword")).SendKeys(password);
            StubbornFindElement(By.Id("bt_authenticate")).Click();
            StubbornFindElement(By.XPath("//li[@id='hmenu-wifi']/a/span")).Click();

            // Wait for the controls to appear
            if (WaitAndClickOnFirst(By.Id("bt_enable"), By.Id("bt_disable")).Criteria == By.Id("bt_disable"))
            {
                isWifiEnabled = false;
            }
            else
            {
                isWifiEnabled = true;
            }

            CleanUp();

            return isWifiEnabled;
        }

        public bool IsWifiEnabled()
        {
            bool ans = false;

            Console.WriteLine("Go to the livebox admin page");
            driver.Navigate().GoToUrl(baseURL);
            StubbornFindElement(By.Id("PopupPassword")).Clear();

            Console.WriteLine("Fetch the LiveBoxAdmin env variable content");
            string password = Environment.GetEnvironmentVariable("LiveBoxAdmin");

            Console.WriteLine("Connexion");
            StubbornFindElement(By.Id("PopupPassword")).SendKeys(password);
            StubbornFindElement(By.Id("bt_authenticate")).Click();
            StubbornFindElement(By.XPath("//li[@id='hmenu-wifi']/a/span")).Click();

            // Wait for the controls to appear
            if (WaitForFirstVisible(By.Id("bt_enable"), By.Id("bt_disable")).Criteria.ToString() == By.Id("bt_disable").ToString())
            {
                // Wifi enabled
                Console.WriteLine("Wifi is enabled");
                ans =  true;
            }
            else
            {
                Console.WriteLine("Wifi is not enabled");
                ans = false;
            }

            CleanUp();

            return ans;
        }

        /// <summary>
        /// Wait for an element to be visible, before clicking on it
        /// </summary>
        /// <param name="criteria"></param>
        private void WaitAndClick(By criteria)
        {
            var control = StubbornFindElement(criteria);

            int i = 0;
            do
            {
                if (control.Displayed)
                {
                    control.Click();
                    return;
                }

                ++i;
                Thread.Sleep(500);
            }
            while (i < 10);


            throw new Exception(string.Format("could not click on this control: {0}", criteria.ToString()));

        }

        private WebElementWithCriteria WaitForFirstVisible(By criteriaEnable, By criteriaDisable)
        {
            var controlEnable = StubbornFindElement(criteriaEnable);
            var controlDisable = StubbornFindElement(criteriaDisable);

            int i = 0;
            do
            {
                if (controlEnable.Displayed)
                    return new WebElementWithCriteria()
                        {
                            Control = controlEnable,
                            Criteria = criteriaEnable
                        };

                if (controlDisable.Displayed)
                    return new WebElementWithCriteria()
                    {
                        Control = controlDisable,
                        Criteria = criteriaDisable
                    };

                ++i;
                Thread.Sleep(500);
            }
            while (i < 10);

            throw new Exception(string.Format("could not find any of these controls: {0} and {1}", criteriaEnable.ToString(), criteriaDisable.ToString()));
        }
        /// <summary>
        /// Click on the first visible element
        /// </summary>
        /// <param name="criteriaEnable"></param>
        /// <param name="criteriaDisable"></param>
        /// <returns>true if the disable element was clicked</returns>
        private WebElementWithCriteria WaitAndClickOnFirst(By criteriaEnable, By criteriaDisable)
        {
            var control = WaitForFirstVisible(criteriaEnable, criteriaDisable);

            if (control.Criteria.ToString() == criteriaEnable.ToString())
            {
                 Console.WriteLine("oOo Plug Wifi in oOo");
            }
            else
            {
                 Console.WriteLine("oOo Cut the Wifi oOo");
            }

            control.Control.Click();
            return control;
        }

        /// <summary>
        /// Find an element, and wait for some time if it is not present
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        private IWebElement StubbornFindElement(By criteria)
        {
            IWebElement ans = null;
            int i = 0;
            
            do
            {
                try
                {
                    ans = driver.FindElement(criteria);
                    i = 10;
                }
                catch (System.Exception ex)
                {
                    if (ex is OpenQA.Selenium.NoSuchElementException || ex is OpenQA.Selenium.ElementNotVisibleException)
                    {
                        Thread.Sleep(500);
                        ++i;
                    }
                    else
                    { 
                        throw;
                    }
                }
            }
            while (i < 10);
            return ans;
        }
    }
}
