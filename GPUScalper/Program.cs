// See https://aka.ms/new-console-template for more information
using GPUScalper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

List<string> bestBuyLinks;
List<string> bnhLinks;
List<string> neweggLinks;
List<Task> tasks;

int tempBestBuyBotsRemainingToDelegate;
int bestBuyBotsPerLink;
int bestBuyBots;
string emailPassForNewCartNotifications;
string emailAddressForNewCartNotifications;

List<BestBuyScalper> bbScalpers;

await StartScalper(args);

async Task StartScalper(string[] args)
{    

    emailAddressForNewCartNotifications = "bobthegpuscalpiungbuilder@yandex.com";
    emailPassForNewCartNotifications = "PASSEDINFROMPROG-CMD-ARGS";
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

    bestBuyLinks = new List<string>();
    bnhLinks = new List<string>();
    neweggLinks = new List<string>();

    bbScalpers = new List<BestBuyScalper>();

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

    bestBuyBots = 50;// '' bestBuyLinks.Count; //1 per for now 50; // how many bots to run at once
    //int bNhBots = 100; // how many bots to run at once
    //int neweggBots = 100; // how many bots to run at once

    bestBuyBotsPerLink = (bestBuyBots / bestBuyLinks.Count);
    //int bNhBotsPerLink = (bestBuyBots / bestBuyLinks.Count);
    //int neweggBotsPerLink = (bestBuyBots / bestBuyLinks.Count);

    tempBestBuyBotsRemainingToDelegate = (bestBuyBots / bestBuyLinks.Count);
    //int tempBnHBotsRemainingToDelegate = (bNhBots / bnhLinks.Count);
    //int tempNeweggBotsRemainingToDelegate = (neweggBots / neweggLinks.Count);
    tasks = new List<Task>();

    // these loops are going to make X bots, (i < X), then spin up (unfortunately linearly) and altuomatically select the link based off of BOT COUNT / linkCount
    for (int i = 0; i <= (bestBuyLinks.Count - 1); i++)
    {
        await DoTheGPUThing(i);
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
        tasks.Add(Task.Run(async () => await bestBuy.GetGpu()));
        string test = "test";
        tempBestBuyBotsRemainingToDelegate -= 1;
        tempBestBuyBotsRemainingToDelegate = bestBuyBotsPerLink; // ok, now reset so the next lnik will have success
    }

    for (int i = 0; i <= bbScalpers.Count - 1; i++) // clean up current chromes before restarting
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

}

async Task DoTheGPUThing(int gpuListNumber)
{
    // do the gpu scalp thing.
    BestBuyScalper bestBuy = new BestBuyScalper();
    bestBuy.passedEmailAddressForNewCartNotifications = emailAddressForNewCartNotifications;
    bestBuy.passedEmailPassForNewCartNotifications = emailPassForNewCartNotifications;
    bestBuy.driver = new ChromeDriver();
    try
    {
        bestBuy.driver.Navigate().GoToUrl(bestBuyLinks[gpuListNumber]);
    }
    catch (Exception)
    {
        Thread.Sleep(3500);
        // im having really annyoing DNS_PROBE_FINISHED_NXDOMAIN issues so i am just going to let it try again. remove this for ur machine most likely fine for u.
        try
        {
            bestBuy.driver.Navigate().GoToUrl(bestBuyLinks[gpuListNumber]);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    bbScalpers.Add(bestBuy);
    tasks.Add(Task.Run(async () => await bestBuy.GetGpu()));
    string test = "test";
    tempBestBuyBotsRemainingToDelegate -= 1;
    tempBestBuyBotsRemainingToDelegate = bestBuyBotsPerLink; // ok, now reset so the next lnik will have success
}