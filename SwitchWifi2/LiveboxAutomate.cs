using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace SeleniumTests
{
  
    public class LiveboxAutomate
    {
        private IWebDriver driver;
        private string baseURL;

        public LiveboxAutomate()
        {
            Console.WriteLine("Prepare a new firefox instance");
            driver = new FirefoxDriver();
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
        

        public void SwitchWifi()
        {
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
            if(WaitAndClickOnFirst(By.Id("bt_enable"), By.Id("bt_disable")))
            {
                WaitAndClick(By.Id("ct-msgbox-button1"));
            }

            CleanUp();

            Console.WriteLine("Exit in 3 seconds");
            Thread.Sleep(3000);
           
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

        /// <summary>
        /// Click on the first visible element
        /// </summary>
        /// <param name="criteriaEnable"></param>
        /// <param name="criteriaDisable"></param>
        /// <returns>true if the disable element was clicked</returns>
        private bool WaitAndClickOnFirst(By criteriaEnable, By criteriaDisable)
        {
            var controlEnable = StubbornFindElement(criteriaEnable);
            var controlDisable = StubbornFindElement(criteriaDisable);

            int i = 0;
            do
            {
                if (controlEnable.Displayed)
                {
                    Console.WriteLine("oOo Plug Wifi in oOo");
                    controlEnable.Click();
                    return false;
                }

                if (controlDisable.Displayed)
                {
                    Console.WriteLine("oOo Cut the Wifi oOo");
                    controlDisable.Click();
                    return true;
                }

                ++i;
                Thread.Sleep(500);
            }
            while (i < 10);


            throw new Exception(string.Format("could not click any of these controls: {0}and {1}", criteriaEnable.ToString(), criteriaDisable.ToString()));
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
