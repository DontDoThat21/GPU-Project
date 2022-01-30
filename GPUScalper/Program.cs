// See https://aka.ms/new-console-template for more information
using GPUScalper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

List<string> bestBuyLinks;
List<string> bnhLinks;
List<string> neweggLinks;
List<Task> tasks;

int bestBuyBotsPerLink;
int bestBuyBots = 10;
string emailPassForNewCartNotifications;
string emailAddressForNewCartNotifications;

List<BestBuyScalper> bbScalpers = new List<BestBuyScalper>();

SetupLinks();
GetEmailCredentials();
SetupScalpers();
ShowConsoleWelcomer();

await StartScalper(args);

void SetupScalpers()
{
    for (int i = 0; i <= (bestBuyLinks.Count - 1); i++)
    {
        //await DoTheGPUThing(i);
        // do the gpu scalp thing.
        BestBuyScalper bestBuy = new BestBuyScalper();
        bestBuy.passedEmailAddressForNewCartNotifications = emailAddressForNewCartNotifications;
        bestBuy.passedEmailPassForNewCartNotifications = emailPassForNewCartNotifications;
        bestBuy.driver = new ChromeDriver();

        bbScalpers.Add(bestBuy);
    }
}

 void SetupLinks()
{
    bestBuyLinks = new List<string>();
    bnhLinks = new List<string>();
    neweggLinks = new List<string>();

    //bbScalpers = new List<BestBuyScalper>();

    StreamReader streamReaderBB = new StreamReader("LinksToSearch\\BestBuy1.txt");
    string dataBB = streamReaderBB.ReadToEnd();
    StringReader readerBB = new StringReader(dataBB);
    while ((dataBB = readerBB.ReadLine()) != null)
    {
        bestBuyLinks.Add(dataBB);
    };
}

void GetEmailCredentials()
{
    emailAddressForNewCartNotifications = "bobthegpuscalpiungbuilder@yandex.com";
    emailPassForNewCartNotifications = "PASSEDINFROMPROG-CMD-ARGS";
    //int userSetBotCount = 0; start using this asap // "PASSEDINFROMPROG-CMD-ARGS";

    if (args.Length > 0) // if user puts in a cmd arg, the first is the user, the second is the pass, the third will be the bot count
    {
        emailAddressForNewCartNotifications = args[0];
        emailPassForNewCartNotifications = args[1];

    }
}

void ShowConsoleWelcomer()
{
    Console.WriteLine("-- WELCOME TO THE SCALPER -- ");
    Console.WriteLine($"Email being used: {emailAddressForNewCartNotifications}.");
    Console.WriteLine($"Pass being used: {emailPassForNewCartNotifications}.");

    Console.WriteLine("-- Starting in 3 -- ");
    Thread.Sleep(1000);
    Console.WriteLine("-- Starting in 2 -- ");
    Thread.Sleep(1000);
    Console.WriteLine("-- Starting in 1 -- ");
    Thread.Sleep(1000);
    bestBuyBotsPerLink = (bestBuyBots / bestBuyLinks.Count);
    tasks = new List<Task>();

}

async Task StartScalper(string[] args)
{    

    //bestBuyBots = 50;// '' bestBuyLinks.Count; //1 per for now 50; // how many bots to run at once


    for (int i = 0; i <= (bestBuyLinks.Count - 1); i++)
    {
        await DoTheGPUThing(i);

        try
        {
            bbScalpers[i].driver.Navigate().GoToUrl(bestBuyLinks[i]);
        }
        catch (Exception ex1)
        {
            Thread.Sleep(3500);
            // im having really annyoing DNS_PROBE_FINISHED_NXDOMAIN issues so i am just going to let it try again. remove this for ur machine most likely fine for u.
            try
            {
                bbScalpers[i].driver.Navigate().GoToUrl(bestBuyLinks[i]);

            }
            catch (Exception ex2)
            {
                throw ex2;
            }
        }

    }
        StartScalper(args);    

}

async Task DoTheGPUThing(int gpuListNumber)
{
    // do the gpu scalp thing.
    //BestBuyScalper bestBuy = new BestBuyScalper();
    //bestBuy.passedEmailAddressForNewCartNotifications = emailAddressForNewCartNotifications;
    //bestBuy.passedEmailPassForNewCartNotifications = emailPassForNewCartNotifications;
    //bestBuy.driver = new ChromeDriver();

    //bbScalpers[i]

    try
    {
        bbScalpers[gpuListNumber].driver.Navigate().GoToUrl(bbScalpers[gpuListNumber].driver.Url);
    }
    catch (Exception)
    {
        Thread.Sleep(3500);
        // im having really annyoing DNS_PROBE_FINISHED_NXDOMAIN issues so i am just going to let it try again. remove this for ur machine most likely fine for u.
        try
        {
            bbScalpers[gpuListNumber].driver.Navigate().GoToUrl(bestBuyLinks[gpuListNumber]);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //bbScalpers.Add(bestBuy);
    tasks.Add(Task.Run(async () => await bbScalpers[gpuListNumber].GetGpu()));
}