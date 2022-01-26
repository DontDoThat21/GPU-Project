// See https://aka.ms/new-console-template for more information
using GPUScalper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

StartScalper(args);

static async void StartScalper(string[] args)
{    

    string emailAddressForNewCartNotifications = "bobthegpuscalpiungbuilder@yandex.com";
    string emailPassForNewCartNotifications = "PASSEDINFROMPROG-CMD-ARGS";
    //int userSetBotCount = 0; start using this asap // "PASSEDINFROMPROG-CMD-ARGS";

    Console.WriteLine("-- WELCOME TO THE SCALPER -- ");
    if (args.Length > 0) // if user puts in a cmd arg, the first is the user, the second is the pass, the third will be the bot count
    {
        emailAddressForNewCartNotifications = args[0];
        emailPassForNewCartNotifications = args[1];

    }
    Console.WriteLine($"Email being used: {emailAddressForNewCartNotifications}.");
    Console.WriteLine($"Pass being used: {emailPassForNewCartNotifications}.");

    Console.WriteLine("-- Starting in 3 -- ");
    Thread.Sleep(1000);
    Console.WriteLine("-- Starting in 2 -- ");
    Thread.Sleep(1000);
    Console.WriteLine("-- Starting in 1 -- ");
    Thread.Sleep(1000); 

    List<string> bestBuyLinks = new List<string>();
    List<string> bnhLinks = new List<string>();
    List<string> neweggLinks = new List<string>();

    List<BestBuyScalper> bbScalpers = new List<BestBuyScalper>();

    StreamReader streamReaderBB = new StreamReader("LinksToSearch\\BestBuy1.txt");
    string dataBB = streamReaderBB.ReadToEnd();
    StringReader readerBB = new StringReader(dataBB);
    while ((dataBB = readerBB.ReadLine()) != null)
    {
        bestBuyLinks.Add(dataBB);
    };

    //StreamReader streamReaderBH = new StreamReader("LinksToSearch\\BnH1.txt");
    //string dataBH = streamReaderBH.ReadToEnd();
    //StringReader readerBH = new StringReader(dataBH);
    //while ((dataBH = readerBH.ReadLine()) != null)
    //{
    //    bnhLinks.Add(dataBH);
    //};

    //StreamReader streamReaderNE = new StreamReader("LinksToSearch\\BnH1.txt");
    //string dataNE = streamReaderNE.ReadToEnd();
    //StringReader readerNE = new StringReader(dataNE);
    //while ((dataNE = readerNE.ReadLine()) != null)
    //{
    //    neweggLinks.Add(dataNE);
    //};

    int bestBuyBots = bestBuyLinks.Count; //1 per for now 50; // how many bots to run at once
    //int bNhBots = 100; // how many bots to run at once
    //int neweggBots = 100; // how many bots to run at once

    int bestBuyBotsPerLink = (bestBuyBots / bestBuyLinks.Count);
    //int bNhBotsPerLink = (bestBuyBots / bestBuyLinks.Count);
    //int neweggBotsPerLink = (bestBuyBots / bestBuyLinks.Count);

    int tempBestBuyBotsRemainingToDelegate = (bestBuyBots / bestBuyLinks.Count);
    //int tempBnHBotsRemainingToDelegate = (bNhBots / bnhLinks.Count);
    //int tempNeweggBotsRemainingToDelegate = (neweggBots / neweggLinks.Count);
    List<Task> tasks = new List<Task>();

    // these loops are going to make X bots, (i < X), then spin up (unfortunately linearly) and altuomatically select the link based off of BOT COUNT / linkCount
    for (int i = 0; i < (bestBuyLinks.Count - 1); i++)
    {
        while (tempBestBuyBotsRemainingToDelegate > 0)
        {
            // do the gpu scalp thing.
            BestBuyScalper bestBuy = new BestBuyScalper();
            bestBuy.passedEmailAddressForNewCartNotifications = emailAddressForNewCartNotifications;
            bestBuy.passedEmailPassForNewCartNotifications = emailPassForNewCartNotifications;
            bestBuy.driver = new ChromeDriver();
            try
            {
                bestBuy.driver.Navigate().GoToUrl(bestBuyLinks[i]);
            }
            catch (Exception)
            {
                Thread.Sleep(3500);
                // im having really annyoing DNS_PROBE_FINISHED_NXDOMAIN issues so i am just going to let it try again. remove this for ur machine most likely fine for u.
                try
                {
                    bestBuy.driver.Navigate().GoToUrl(bestBuyLinks[i]);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            bbScalpers.Add(bestBuy);
            tasks.Add(Task.Run(() => bestBuy.GetGpu()));
            string test = "test";
            //await Task.Run(() => bestBuy.dostuff());
            //int x = 12;
            //await bestBuy.dostuff();
            //Thread.Sleep(1000);

            //bbScalpers.Add(bestBuy);            

            //try
            //{
            //    bestBuy.driver.FindElement(By.CssSelector(".c-button.c-button-primary.c-button-lg.c-button-block.c-button-icon.c-button-icon-leading.add-to-cart-button")).Click();

            //}
            //catch (Exception)
            //{
            //    Thread.Sleep(1000); // possible it didnt load yet; try again one more time
            //    try
            //    {
            //        bestBuy.driver.FindElement(By.CssSelector(".c-button.c-button-primary.c-button-lg.c-button-block.c-button-icon.c-button-icon-leading.add-to-cart-button")).Click();

            //    }
            //    catch (Exception)
            //    {
            //        break;
            //    }

            //}

            //try
            //{
            //    bestBuy.driver.FindElement(By.CssSelector(".cart-label")).Click();
            //}
            //catch (Exception)
            //{
            //    //.c-modal-window.email-submission-modal.active
            //    bestBuy.driver.FindElement(By.CssSelector(".size-l.c-overlay-fullscreen-is-open")).Click(); // so this happened because the modal view came up in chrome in best buy.
            //    bestBuy.driver.FindElement(By.CssSelector(".cart-label")).Click();
            //}
            //// a good idea would be to get the screenshot of the opage before clicking trhe button? or actually probably best to click cart?

            //EmailSender em = new EmailSender();
            //em.SendGPUAlertEmail("https://www.bestbuy.com/site/gigabyte-nvidia-geforce-rtx-3080-eagle-oc-10gb-gddr6x-pci-express-4-0-graphics-card/6430621.p?skuId=6430621");

            tempBestBuyBotsRemainingToDelegate -= 1;
     }
        tempBestBuyBotsRemainingToDelegate = bestBuyBotsPerLink; // ok, now reset so the next lnik will have success
    }

    for (int i = 0; i < bbScalpers.Count - 1; i++) // clean up current chromes before restarting
    {
        bbScalpers[i].driver.Close();
        bbScalpers[i].driver.Dispose();
    }
    bbScalpers = new List<BestBuyScalper>();
    StartScalper(args);
    //await Task.WhenAll(tasks);
    //string x = "buttajeg!";

    //for (int i = 0; i < bNhBotsPerLink; i++)
    //{
    //    while (tempBnHBotsRemainingToDelegate > 0)
    //    {
    //        // do the gpu scalp thing.

    //        tempBnHBotsRemainingToDelegate -= 1;
    //    }
    //    tempBnHBotsRemainingToDelegate = bNhBotsPerLink;
    //}
    //for (int i = 0; i < neweggBotsPerLink; i++)
    //{
    //    while (tempNeweggBotsRemainingToDelegate > 0)
    //    {
    //        // do the gpu scalp thing.


    //        tempNeweggBotsRemainingToDelegate -= 1;
    //    }
    //    tempNeweggBotsRemainingToDelegate = neweggBotsPerLink;
    //}

        Console.ReadLine();

}