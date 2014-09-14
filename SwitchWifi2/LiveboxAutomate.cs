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
            driver.Navigate().GoToUrl(baseURL + "/");
            driver.FindElement(By.Id("PopupPassword")).Clear();

            Console.WriteLine("Fetch the LiveBoxAdmin env variable content");
            string password = Environment.GetEnvironmentVariable("LiveBoxAdmin");

            Console.WriteLine("Connexion");
            driver.FindElement(By.Id("PopupPassword")).SendKeys(password);
            driver.FindElement(By.Id("bt_authenticate")).Click();
            driver.FindElement(By.XPath("//li[@id='hmenu-wifi']/a/span")).Click();

            Thread.Sleep(2000);

            // Disable wifi
            bool wifiEnabled = !driver.FindElement(By.Id("wifiEnabled")).GetAttribute("style").Contains("display: none");
            if (wifiEnabled)
            {
                Console.WriteLine("Cut Wifi");
                driver.FindElement(By.Id("bt_disable")).Click();
                Thread.Sleep(1000);
                driver.FindElement(By.Id("ct-msgbox-button1")).Click();
            }
            else
            {
                Console.WriteLine("Plug Wifi in");
                driver.FindElement(By.Id("bt_enable")).Click();
            }

            CleanUp();

            Console.WriteLine("Exit in 5 seconds");
            Thread.Sleep(5000);
           
        }
    }
}
