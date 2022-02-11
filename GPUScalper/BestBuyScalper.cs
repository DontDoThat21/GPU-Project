using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GPUScalper
{    

    internal class BestBuyScalper
    {
        public List<Tuple<Screenshot, string>> cartAddedScreenShots = new List<Tuple<Screenshot, string>>();
        public volatile string passedEmailAddressForNewCartNotifications = "";
        public volatile string passedEmailPassForNewCartNotifications = "";

        public volatile IWebDriver driver;// = new ChromeDriver();

        public async Task GetGpu()
        {
            Thread.Sleep(1000);

            try { 
                // try to click the add to cart button
               this.driver.FindElement(By.CssSelector(".c-button.c-button-primary.c-button-lg.c-button-block.c-button-icon.c-button-icon-leading.add-to-cart-button")).Click();
            }
            catch (Exception ex)
            {
                if (ex.Message == "")
                {
                    return;
                }
                Thread.Sleep(1000); // possible it didnt load yet; try again one more time

                try // going to try to kill the survery menu/bot detection? menu after a refresh
                {
                    this.driver.FindElement(By.CssSelector("#survey_invite_no")).Click(); // tested and works; closes the annoying survery popup that would make the bot slower
                    //string x = "yea";
                }
                catch
                {
                    Console.WriteLine(Environment.NewLine + "Couldn't find survey.");
                    // dont care if didnt exist then continue
                }

                try
                {
                    this.driver.FindElement(By.CssSelector(".c-button.c-button-primary.c-button-lg.c-button-block.c-button-icon.c-button-icon-leading.add-to-cart-button")).Click();

                }
                catch (Exception)
                {                    
                    this.driver.Navigate().GoToUrl(this.driver.Url);
                    await GetGpu();
                }

            }

            try
            {
                this.driver.FindElement(By.CssSelector(".cart-label")).Click();                
            }
            catch (Exception exO)
            {
                try
                {
                    //.c-modal-window.email-submission-modal.active
                    this.driver.FindElement(By.CssSelector(".size-l.c-overlay-fullscreen-is-open")).Click(); // so this happened because the modal view came up in chrome in best buy.
                    Thread.Sleep(500);
                    this.driver.FindElement(By.CssSelector(".cart-label")).Click();
                }
                catch (Exception exI)
                {                    
                    string msg = exI.Message;
                    // no idea when this would fail? should i maybe check for the bot detector?
                }
                
            }
            // a good idea would be to get the screenshot of the opage before clicking trhe button? or actually probably best to click cart?

            EmailSender em = new EmailSender();
            em.passedSenderEmailAddressForNewCartNotifications = passedEmailAddressForNewCartNotifications;
            em.passedSenderEmailPassForNewCartNotifications = passedEmailPassForNewCartNotifications;

            //byte[] screenShot =
            GetScreenShotFromSeleniumDriver();
            //screenShot.
            Attachment screenShotAtt = new Attachment(cartAddedScreenShots.LastOrDefault().Item2);
            em.SendGPUAlertEmail(this.driver.Url, screenShotAtt);

            //tempBestBuyBotsRemainingToDelegate -= 1;
        }

        private byte[] GetScreenShotFromSeleniumDriver()
        {
            //throw new NotImplementedException();
            Screenshot ss = ((ITakesScreenshot) driver).GetScreenshot();
            byte[] ssBytes = ss.AsByteArray;
            string now = DateTime.Now.ToString().Replace("/", "-").Replace(" ", "-").Replace(":", "-");
            string ssSavedPath = $"CartScreenshots\\screenshot-cart-{now}.png";
            ss.SaveAsFile(ssSavedPath, ScreenshotImageFormat.Png);
            cartAddedScreenShots.Add(new Tuple<Screenshot, string>(ss, ssSavedPath));
            return ssBytes;


        }
    }

}
