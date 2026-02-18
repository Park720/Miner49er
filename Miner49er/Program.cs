using System;

namespace Miner49er
{
    class MainClass
    {
        public static void Main(string[] args) {
            //Set up the variables 
            Miner miner = new SimpleMiner();
            Mine mine = new Mine(miner);
            int secsPerTick = 1;
            Random myRandom = new Random(Environment.TickCount);
            int gameLengthInTics = (int)(myRandom.NextSingle() * 10) + 20;
            // run the mineM9er• loop 
            for (int tick = 0; tick < gameLengthInTics; tick++)
            {
                Console.WriteLine("Tick # " + tick);
                mine.DoEvent("tick");
                miner.DoEvent("tick");
                miner.printStatus();
                Console.WriteLine();
                Console.WriteLine("");
                System.Threading.Thread.Sleep(secsPerTick * 1000);
            }

            Console.WriteLine("Ending Wealth: " + miner.getCurrentWealth());
        }
    }
}