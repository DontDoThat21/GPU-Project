// See https://aka.ms/new-console-template for more information
using GPUScalper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;


delegate bool EventHandler(CtrlType sig);

enum CtrlType
{
    CTRL_C_EVENT = 0,
    CTRL_BREAK_EVENT = 1,
    CTRL_CLOSE_EVENT = 2,
    CTRL_LOGOFF_EVENT = 5,
    CTRL_SHUTDOWN_EVENT = 6
}
namespace GPUScalper
{


    internal class GPUScalper
    {
        [DllImport("Kernel32")]
        static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);


        static List<string> bestBuyLinks;
        static List<string> bnhLinks;
        static List<string> neweggLinks;
        static List<Task> tasks;

        static int bestBuyBotsPerLink;
        static int bestBuyBots = 10;
        static string emailPassForNewCartNotifications;
        static string emailAddressForNewCartNotifications;
        static EventHandler _applicationCrashedHandler;

        static List<BestBuyScalper> bbScalpers = new List<BestBuyScalper>();


        static bool ApplicationCrashedHandler(CtrlType sig)
        {
            DisposeScalpers();

            switch (sig)
            {
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                default:
                    return false;
            }
        }

        static async Task Main(string[] args)
        {
            
            SetupLinks();
            GetEmailCredentials(args);
            SetupScalpers();
            ShowConsoleWelcomer();
            
            await StartScalper(args);
            _applicationCrashedHandler += new EventHandler(ApplicationCrashedHandler);
            SetConsoleCtrlHandler(_applicationCrashedHandler, true);
            // CLEAN UP ALL CHROME SESSIONS
            // DisposeDetachedChromeApps();
        }


        static void SetupScalpers()
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

        /// <summary>
        ///  only called if the app crashes. for some reason, it seems to do that. a lot.
        /// </summary>
        static void DisposeScalpers()
        {
            for (int i = 0; i <= (bbScalpers.Count - 1); i++)
            {
                //await DoTheGPUThing(i);
                bbScalpers[i].driver.Close();
                bbScalpers[i].driver.Dispose();
            }
            bbScalpers = new List<BestBuyScalper>();
        }

        static void SetupLinks()
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

        static void GetEmailCredentials(string[] args)
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

        static void ShowConsoleWelcomer()
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

        static async Task StartScalper(string[] args)
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

        static async Task DoTheGPUThing(int gpuListNumber)
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

    }
}


