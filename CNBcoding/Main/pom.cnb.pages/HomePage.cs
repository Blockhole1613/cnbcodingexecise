using CommonComponents;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;

namespace CNBcoding.Main.pom.cnb.pages
{
    public class HomePage : BasePage
    {
        public HomePage(RemoteWebDriver driver) => _driver = driver;
        private IWebElement BankingOptions => _driver.FindElement(By.XPath("//ul[contains(@class, 'cmp-navigation__group')]"));

        IList<IWebElement> links => _driver.FindElements(By.XPath("//ul[contains(@class, 'cmp-navigation__group')]/li"));
        private IWebElement SelectBankingOptions(string option)
        {
            return _driver.FindElement(By.LinkText(option));
        }
        private IWebElement PageText => _driver.FindElement(By.XPath("//h1[contains(@class, 'cmp-hero__title')]"));

        public string ClickBankingOption(string option)
        {
            try
            {
                SelectBankingOptions(option).Click();
                return PageText.Text;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Error";
            }
        }

        public void ClickBankingOption(int count)
        {
            try
            {

                for (int i = 0; i <= count; i++)
                {
                    links[i].Click();
                    Wait.WaitForElementDispalyed(_driver, PageText, 10);
                    Assertions.Validate(PageText.Text, BankingTypes[links[i].Text], "Expected : " + PageText.Text + " - Actual : " + BankingTypes[links[i].Text], "Home Page Links", links[i].Text, _driver);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
