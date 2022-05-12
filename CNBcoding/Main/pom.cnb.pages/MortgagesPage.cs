using CommonComponents;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;

namespace CNBcoding.Main.pom.cnb.pages
{

    /* ** Mortgage Page Extends BasePage here (INHERITANCE)
     * Mortgage Page has  all the Elements(data) and Methods(Actions with the Elements) (ENCAPSULATION)
     * We Use CommonCompents.dll as project dependencies where Helper classes and methods are defined which are being used throught the Tests (ABSTARCTION)
    */
    public class MortgagesPage : BasePage // ENCAPSULATION priciple is used Here
    {
        public MortgagesPage(RemoteWebDriver driver) => _driver = driver;

        private IWebElement Personal => _driver.FindElement(By.LinkText("Personal"));
        private IWebElement Mortgages => _driver.FindElement(By.LinkText("Mortgages"));

        private IWebElement MortgageTitle => _driver.FindElement(By.XPath("//h1[contains(@class, 'cmp-hero__title')]"));

        public IWebElement FirstName => _driver.FindElement(By.Id("first_name"));
        public IWebElement LastName => _driver.FindElement(By.Id("last_name"));
        public IWebElement FullAddress => _driver.FindElement(By.Id("addressfull"));
        public IWebElement Zip => _driver.FindElement(By.Id("zip"));
        public IWebElement Phone => _driver.FindElement(By.Id("phone"));
        public IWebElement Email => _driver.FindElement(By.Id("email"));
        public IWebElement Submit => _driver.FindElement(By.XPath("//button[contains(.,'SUBMIT')]"));

        private IWebElement SuccessMessage => _driver.FindElement(By.XPath("//div[contains(@class, 'cmp-form-container__message--message')]"));
        public string MortgageEnquiry(string firstname, string lastname)
        {
            try
            {
                var timestamp = DateTimeStamp.getTimeStamp("yyyyMMddHHmmss");
                Actions action = new Actions(_driver);
                Wait.WaitForElementDispalyed(_driver, Personal, 10);
                action.MoveToElement(Personal).Perform();
                Wait.WaitForElementDispalyed(_driver, Mortgages, 10);
                Mortgages.Click();
                Wait.WaitForElementDispalyed(_driver, MortgageTitle, 10);
                Assertions.Equals(MortgageTitle.Text, "MORTGAGE LOANS", "Actual : " + MortgageTitle.Text + " - Expected :  MORTGAGE LOANS ", "Personal", "Mortgages", _driver);
                FirstName.SendKeys(firstname + RandomChar.Char(5));
                LastName.SendKeys(lastname + RandomChar.Char(5));
                var RandState = Address.RandomState();
                var addressfull = Address.GetAddress()[RandState].Item1 + " " + Address.GetAddress()[RandState].Item2 + " " + Address.GetAddress()[RandState].Item3 + " " + RandState + " United States";
                FullAddress.SendKeys(addressfull);
                Zip.SendKeys(Address.GetAddress()[RandState].Item4.ToString());
                Phone.SendKeys(PhoneNumberGenerator.PhoneNumber());
                Email.SendKeys("cnb_" + timestamp + "@blackhole.com");
                Submit.Click();
                Wait.WaitForElementDispalyed(_driver, SuccessMessage, 10);
                return SuccessMessage.Text;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

    }
}

