using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace _3_Uzd
{
    public class Tests
    {
        private IWebDriver _driver;
        private IJavaScriptExecutor _js;
        private readonly string email = "cernovas14@mail.com";
        private readonly string password = "cernovas1";
        
        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://demowebshop.tricentis.com/");
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
            
            _js = (IJavaScriptExecutor) _driver;
        }

        [Test]
        public void CreateAccountTest()
        {
            var loginNavigation =
                _driver.FindElement(By.XPath("//div[contains(@class, 'header-links')]//a[contains(text(), 'Log i')]"));
            loginNavigation.Click();
        
            var registerNavigation =
                _driver.FindElement(By.XPath(
                    "//div[contains(@class, 'customer-blocks')]//input[contains(@class, 'register-button')]"));
            registerNavigation.Click();
        
            var gener = _driver.FindElement(By.Id("gender-male"));
            gener.Click();
            
            var firstName = _driver.FindElement(By.Id("FirstName"));
            firstName.SendKeys("John");
            
            var lastName = _driver.FindElement(By.Id("LastName"));
            lastName.SendKeys("Doe");
            
            var emailElement = _driver.FindElement(By.Id("Email"));
            emailElement.SendKeys(this.email);
        
            var passwordElement = _driver.FindElement(By.Id("Password"));
            passwordElement.SendKeys(this.password);
            
            var confirmPasswordElement = _driver.FindElement(By.Id("ConfirmPassword"));
            confirmPasswordElement.SendKeys(this.password);
            
            var registerButton = _driver.FindElement(By.Id("register-button"));
            registerButton.Click();
            
            var explicitWaitContinue = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            var continueButton = explicitWaitContinue.Until(driver =>
                driver.FindElement(By.XPath("//div[contains(@class, 'page-body')]//input")));
        
            continueButton.Click();
            
            Assert.Pass();
        }

        [Test]
        public void Test1()
        {
            // Get files data
            string[] items = File.ReadAllLines("/Users/dominykas.cernovas/Documents/Vilnius University/3 year/Programu\u0328 sistemu\u0328 testavimas/1 Dalis/3_Uzd/3_Uzd/data1.txt").Select(x => x.Trim()).ToArray();
            
            if (items.Length == 0)
            {
                Assert.Fail("No data found in the file");
            }
            
            var loginNavigation =
                    _driver.FindElement(By.XPath("//div[contains(@class, 'header-links')]//a[contains(text(), 'Log i')]"));
            loginNavigation.Click();
        
            var emailElement = _driver.FindElement(By.Id("Email"));
            emailElement.SendKeys(this.email);
            
            var passwordElement = _driver.FindElement(By.Id("Password"));
            passwordElement.SendKeys(this.password);
        
            var loginButton =
                _driver.FindElement(By.XPath("//div[contains(@class, 'form-fields')]//input[contains(@class, 'login-button')]"));
            loginButton.Click();
        
            var explicitWaitDigitalDownloads = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            var digitalDownloads = explicitWaitDigitalDownloads.Until(driver =>
                driver.FindElement(
                    By.XPath("//div[contains(@class, 'listbox')]//a[contains(text(), 'Digital downlo')]")));
            digitalDownloads.Click();
        
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            
            foreach (var item in items)
            {
                try
                {
                    // Find all product elements
                    var products = _driver.FindElements(By.CssSelector(".product-grid .item-box"));
        
                    bool itemAdded = false;
        
                    foreach (var product in products)
                    {
                        // Get the product title
                        var titleElement = product.FindElement(By.CssSelector(".product-title a"));
                        string productName = titleElement.Text.Trim();
        
                        // If the product name matches the file entry, add to cart
                        if (productName.Equals(item, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"Adding {productName} to cart...");
        
                            // Click "Add to cart" button
                            var addToCartButton = product.FindElement(By.CssSelector(".product-box-add-to-cart-button"));
                            
                            // Get the cart quantity before adding the item
                            var cartElement = _driver.FindElement(By.CssSelector(".cart-qty"));
                            string oldCartValue = cartElement.Text;
        
                            // Click "Add to Cart" button
                            addToCartButton.Click();
        
                            // Wait for cart quantity to update (without relying on specific numbers)
                            wait.Until(d => _driver.FindElement(By.CssSelector(".cart-qty")).Text != oldCartValue);
        
                            itemAdded = true;
                            break; // Stop searching once the item is added
                        }
                    }
        
                    if (!itemAdded)
                    {
                        Console.WriteLine($"Item '{item}' not found on the page.");
                    }
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine($"Error: Could not find elements for '{item}'.");
                }
            }
        
            var cartNavigation = _driver.FindElement(By.CssSelector(".cart-label"));
            cartNavigation.Click();
        
            var agreeTerms = _driver.FindElement(By.Id("termsofservice"));
            agreeTerms.Click();
            
            var checkoutButton = _driver.FindElement(By.Id("checkout"));
            checkoutButton.Click();
        
            FillAddressForm();
            
            var continueButton =
                _driver.FindElement(By.XPath("//div[contains(@id, 'billing-buttons-container')]/input"));
            continueButton.Click();
            
            var paymentMethodButton =
                _driver.FindElement(By.XPath("//div[contains(@id, 'payment-method-buttons-container')]/input"));
            paymentMethodButton.Click();
            
            var paymentInformationButton =
                _driver.FindElement(By.XPath("//div[contains(@id, 'payment-info-buttons-container')]/input"));
            paymentInformationButton.Click();
            
            var confirmButton =
                _driver.FindElement(By.XPath("//div[contains(@id, 'confirm-order-buttons-container')]/input"));
            confirmButton.Click();
        
            var orderCompletedMessage = _driver.FindElement(By.XPath("//div[contains(@class, 'order-completed')]/div[contains(@class, 'title')]"));
            Assert.That(orderCompletedMessage.Text, Is.EqualTo("Your order has been successfully processed!"), "Order was not successful.");
        }

        [Test]
        public void Test2()
        {
            // Get files data
            string[] items = File.ReadAllLines("/Users/dominykas.cernovas/Documents/Vilnius University/3 year/Programu\u0328 sistemu\u0328 testavimas/1 Dalis/3_Uzd/3_Uzd/data2.txt").Select(x => x.Trim()).ToArray();
            
            if (items.Length == 0)
            {
                Assert.Fail("No data found in the file");
            }
            
            var loginNavigation =
                    _driver.FindElement(By.XPath("//div[contains(@class, 'header-links')]//a[contains(text(), 'Log i')]"));
            loginNavigation.Click();

            var emailElement = _driver.FindElement(By.Id("Email"));
            emailElement.SendKeys(this.email);
            
            var passwordElement = _driver.FindElement(By.Id("Password"));
            passwordElement.SendKeys(this.password);

            var loginButton =
                _driver.FindElement(By.XPath("//div[contains(@class, 'form-fields')]//input[contains(@class, 'login-button')]"));
            loginButton.Click();

            var explicitWaitDigitalDownloads = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
            var digitalDownloads = explicitWaitDigitalDownloads.Until(driver =>
                driver.FindElement(
                    By.XPath("//div[contains(@class, 'listbox')]//a[contains(text(), 'Digital downlo')]")));
            digitalDownloads.Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            
            foreach (var item in items)
            {
                try
                {
                    // Find all product elements
                    var products = _driver.FindElements(By.CssSelector(".product-grid .item-box"));

                    bool itemAdded = false;

                    foreach (var product in products)
                    {
                        // Get the product title
                        var titleElement = product.FindElement(By.CssSelector(".product-title a"));
                        string productName = titleElement.Text.Trim();

                        // If the product name matches the file entry, add to cart
                        if (productName.Equals(item, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine($"Adding {productName} to cart...");

                            // Click "Add to cart" button
                            var addToCartButton = product.FindElement(By.CssSelector(".product-box-add-to-cart-button"));
                            
                            // Get the cart quantity before adding the item
                            var cartElement = _driver.FindElement(By.CssSelector(".cart-qty"));
                            string oldCartValue = cartElement.Text;

                            // Click "Add to Cart" button
                            addToCartButton.Click();

                            // Wait for cart quantity to update (without relying on specific numbers)
                            wait.Until(d => _driver.FindElement(By.CssSelector(".cart-qty")).Text != oldCartValue);

                            itemAdded = true;
                            break; // Stop searching once the item is added
                        }
                    }

                    if (!itemAdded)
                    {
                        Console.WriteLine($"Item '{item}' not found on the page.");
                    }
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine($"Error: Could not find elements for '{item}'.");
                }
            }

            var cartNavigation = _driver.FindElement(By.CssSelector(".cart-label"));
            cartNavigation.Click();

            var agreeTerms = _driver.FindElement(By.Id("termsofservice"));
            agreeTerms.Click();
            
            var checkoutButton = _driver.FindElement(By.Id("checkout"));
            checkoutButton.Click();

            var continueButtonAddress =
                _driver.FindElement(By.XPath("//div[contains(@id, 'billing-buttons-container')]/input"));
            continueButtonAddress.Click();
            
            var continueButton =
                _driver.FindElement(By.XPath("//div[contains(@id, 'billing-buttons-container')]/input"));
            continueButton.Click();
            
            var paymentMethodButton =
                _driver.FindElement(By.XPath("//div[contains(@id, 'payment-method-buttons-container')]/input"));
            paymentMethodButton.Click();
            
            var paymentInformationButton =
                _driver.FindElement(By.XPath("//div[contains(@id, 'payment-info-buttons-container')]/input"));
            paymentInformationButton.Click();
            
            var confirmButton =
                _driver.FindElement(By.XPath("//div[contains(@id, 'confirm-order-buttons-container')]/input"));
            confirmButton.Click();

            var orderCompletedMessage = _driver.FindElement(By.XPath("//div[contains(@class, 'order-completed')]/div[contains(@class, 'title')]"));
            Assert.That(orderCompletedMessage.Text, Is.EqualTo("Your order has been successfully processed!"), "Order was not successful.");
        }
        
        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }
        
        private void FillAddressForm()
        {
            var firstName = _driver.FindElement(By.Id("BillingNewAddress_FirstName"));
            firstName.Clear();
            firstName.SendKeys("John");
            
            var lastName = _driver.FindElement(By.Id("BillingNewAddress_LastName"));
            lastName.Clear();
            lastName.SendKeys("Doe");
            
            var emailField = _driver.FindElement(By.Id("BillingNewAddress_Email"));
            emailField.Clear();
            emailField.SendKeys(this.email);
            
            var countryField = _driver.FindElement(By.Id("BillingNewAddress_CountryId"));
            var countrySelect = new SelectElement(countryField);
            countrySelect.SelectByText("Lithuania");
            
            var cityField = _driver.FindElement(By.Id("BillingNewAddress_City"));
            cityField.Clear();
            cityField.SendKeys("Vilnius");
            
            var addressField = _driver.FindElement(By.Id("BillingNewAddress_Address1"));
            addressField.Clear();
            addressField.SendKeys("Gedimino pr. 9");
            
            var zipField = _driver.FindElement(By.Id("BillingNewAddress_ZipPostalCode"));
            zipField.Clear();
            zipField.SendKeys("01103");
            
            var phoneField = _driver.FindElement(By.Id("BillingNewAddress_PhoneNumber"));
            phoneField.Clear();
            phoneField.SendKeys("+37060000000");
        }
    }
}