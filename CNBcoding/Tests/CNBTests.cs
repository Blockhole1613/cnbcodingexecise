using CNBcoding.Main.pom.cnb.pages;
using CommonComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;

namespace CNBcoding
{
    [TestClass]
    public class CNBTests : BasePage
    {
        MortgagesPage mortgagesPage = new MortgagesPage(_driver);
        HomePage homePage = new HomePage(_driver);

        [TestMethod, TestCategory("Mortgage")]
        public void TestMethod_01_Validate_CNBMortgageEnquiry()
        {
            HTMLReports.CreateTest(MethodBase.GetCurrentMethod().Name.ToString().Split('_')[3] + "--->" + string.Join(" ", MethodBase.GetCurrentMethod().Name.ToString().Split('_').Skip(2)), "Mortgage");
            var success = mortgagesPage.MortgageEnquiry("CNBFirst_", "CNBLast_");
            Assertions.Validate(success, "Thanks For Getting In Touch!\r\nA Relationship Manager will be reaching out to you shortly. In the meantime, check out our latest news and insights.", "Expected", "Test", "test", _driver);
        }

        [TestMethod, TestCategory("HomePage")]
        public void TestMethod_02_Validate_HomePageLinks()
        {
            HTMLReports.CreateTest(MethodBase.GetCurrentMethod().Name.ToString().Split('_')[3] + "--->" + string.Join(" ", MethodBase.GetCurrentMethod().Name.ToString().Split('_').Skip(2)), "Mortgage");
            //POLYMORPHISM used here 
            homePage.ClickBankingOption(Constants.rand.Next(0, 2));
            //Assertions.Validate(success, "Thanks For Getting In Touch!\r\nA Relationship Manager will be reaching out to you shortly. In the meantime, check out our latest news and insights.", "Expected", "Test", "test", _driver);
        }

        [TestMethod, TestCategory("HomePage")]
        public void TestMethod_03_Validate_HomePageLinks_By_LinkText()
        {
            HTMLReports.CreateTest(MethodBase.GetCurrentMethod().Name.ToString().Split('_')[3] + "--->" + string.Join(" ", MethodBase.GetCurrentMethod().Name.ToString().Split('_').Skip(2)), "Mortgage");
            //POLYMORPHISM used here
            string[] types = { "Personal", "Business & Commercial", "Private Banking" };
            var option = types[Constants.rand.Next(types.Length)];
            var returntext = homePage.ClickBankingOption(option);
            Assertions.Validate(returntext, BankingTypes[option], "Expected : " + returntext + " - Actual : " + BankingTypes[option], "Home Page Links", BankingTypes[option], _driver);
        }
    }
}
