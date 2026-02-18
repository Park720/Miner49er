using System;
using KAI.FSA; // Use the namespace from your FSAImpl definition

namespace Miner49er
{
    /// <summary>
    /// This class implements the Miner as described by the first state transition table
    /// </summary>
    public class SimpleMiner : FSAImpl, Miner
    {
        /// Amount of gold nuggest in the miner's pockets ...
        public int gold = 0;
        /// How thirsty the miner is ...
        public int thirst = 0;
        /// How many gold nuggets the miner has in the bank ...
        public int bank = 0;
        //How tired the miner is ...
        public int tired = 0;
        // Whether the mine is open or closed (if closed, the miner is unemployed)
        public Boolean isMineOpen = true;


        // The following variables are each oen of the defiend states the miner cna be in.
        State miningState;
        State drinkingState;
        State bankingState;
        State sleepingState;
        State unemployedState;

        // FIXED: Added : base("SimpleMiner") to resolve the FSAImpl constructor error
        public SimpleMiner() : base("SimpleMiner")
        {
            // FIXED: Using PascalCase to match your FSAImpl.MakeNewState method
            miningState = MakeNewState("Mining");
            drinkingState = MakeNewState("Drinking");
            bankingState = MakeNewState("Banking");
            sleepingState = MakeNewState("Sleeping");
            unemployedState = MakeNewState("No Job");


            // set mining transitions
            //Mine -> Drink
            miningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.parched) },
                new ActionDelegate[] { new ActionDelegate(this.moving) }, drinkingState);
            //Mine -> Sleep
            miningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.exhausted) },
                new ActionDelegate[] { }, sleepingState);
            //Mine -> Bank
            miningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.pocketsFull) },
                new ActionDelegate[] { new ActionDelegate(this.moving) }, bankingState);
            //Mine -> Unemployed
            miningState.addTransition("MineClose",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.closeMine) }, unemployedState);
            //Mine -> Mine (reopen)
            miningState.addTransition("MineOpen",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.openMine) }, miningState);
            //Keep Mining
            miningState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.dig) }, miningState);


            // set drinking transitions
            // Keep Drinking
            drinkingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.thirsty) },
                new ActionDelegate[] { new ActionDelegate(this.takeDrink) }, drinkingState);
            //Drink -> Sleep
            drinkingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.exhausted) },
                new ActionDelegate[] { new ActionDelegate(this.moving) }, sleepingState);
            // Drink -> Mine(Unemployed)
            drinkingState.addTransition("CloseMine",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.mineClosing) }, unemployedState);
            // Drink -> Mine (reopen)
            drinkingState.addTransition("OpenMine",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.mineOpening) }, miningState);
            // Drink -> Mine
            drinkingState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.moving) }, miningState);



            // set sleeping transitions
            // Keep sleeping
            sleepingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.isTired) },
                new ActionDelegate[] { new ActionDelegate(this.takeSleep) }, sleepingState);
            // Sleep -> Drink
            sleepingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.thirsty) },
                new ActionDelegate[] { new ActionDelegate(this.moving) }, drinkingState);
            // Sleep -> Mine(Unemployed)
            sleepingState.addTransition("MineClose",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.mineClosing) }, unemployedState);
            // Sleep -> Mine (reopen)
            sleepingState.addTransition("MineOpen",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.mineOpening) }, miningState);
            // Sleep -> Mine
            sleepingState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.moving) }, miningState);


            // set banking transitions
            //Keep Saving
            bankingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.pocketsNotEmpty) },
                new ActionDelegate[] { new ActionDelegate(this.depositGold) }, bankingState);
            // Bank -> Drink
            bankingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.parched) },
                new ActionDelegate[] { new ActionDelegate(this.moving) }, drinkingState);
            // Bank -> Sleep
            bankingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.exhausted) },
                new ActionDelegate[] { new ActionDelegate(this.moving) }, sleepingState);
            // Bank -> Mine (Unemployed)
            bankingState.addTransition("MineClose",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.mineClosing) }, unemployedState);
            // Bank -> Mine (reopen)
            bankingState.addTransition("MineOpen",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.mineOpening) }, miningState);
            // Bank -> Mine
            bankingState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.moving) }, miningState);


            // set unemployed transitions (while mine is closed)
            unemployedState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.parched) },
                new ActionDelegate[] { }, drinkingState);

            unemployedState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.exhausted) },
                new ActionDelegate[] { }, sleepingState);

            unemployedState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.pocketsNotEmpty) },
                new ActionDelegate[] { }, bankingState);

            unemployedState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.unemployed) }, unemployedState);

            unemployedState.addTransition("MineOpen",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.mineOpening) }, miningState);

            // FIXED: Using PascalCase to match your FSAImpl.SetCurrentState method
            SetCurrentState(miningState);
        }

        /// <summary>
        /// This is a condition that tests to see if the miner is so thirsty that he cannot dig
        /// </summary>
        private Boolean parched(FSA fsa)
        {
            if (thirst >= 5)
            {
                Console.WriteLine("Too thirsty too work.");
            }
            return thirst >= 5;
        }

        /// <summary>
        /// An action that decrements the miner's thirst ...
        /// </summary>
        private void takeDrink(FSA fsa)
        {
            thirst -=2;
            gold --;
            Console.WriteLine("Glug glug glug");
        }

        private Boolean exhausted(FSA fsa)
        {
            if (tired >= 8)
            {
                Console.WriteLine("Too tired to work.");
            }
            return tired >= 8;
        }

        private void takeSleep(FSA fsa)
        {
            tired --;
            Console.WriteLine("Zzz...");
        }

        /// <summary>
        /// An action that decrements the gold in the miner's pockets and increments the gold in the bank ...
        /// </summary>
        private void depositGold(FSA fsa)
        {
            gold -= 2;
            bank +=2;
            Console.WriteLine("deposit a gold nugget");
        }
        private void unemployed(FSA fsa)
        {
            Console.WriteLine("Saddly unemployed while mine is closed...");
        }
        private void closeMine(FSA fsa)
        {
            isMineOpen = false; 
            Console.WriteLine("Mine is closing! Time to rest.");
        }
        private void openMine(FSA fsa)
        {
            isMineOpen = true; 
            Console.WriteLine("Mine is open again! Back to work!");
        }

        /// <summary>
        /// This implements the Miner.getCurrentWealth() call ...
        /// </summary>
        public int getCurrentWealth()
        {
            return bank + gold;
        }

        // --- Previously extracted methods ---

        private void dig(FSA fsa)
        {
            gold+= 2;
            thirst++;
            tired++;
            Console.WriteLine("Miner is digging.");
        }

        private void moving(FSA fsa)
        {
            thirst++;
            Console.WriteLine("Walk Walk Wlak");

        }

        private Boolean pocketsFull(FSA fsa) => gold >= 10;

        private Boolean pocketsNotEmpty(FSA fsa) => gold > 5;

        private Boolean thirsty(FSA fsa) => thirst > 0;

        private Boolean isTired(FSA fsa) => tired > 0;

        private Boolean hasGoldInHand(FSA fsa) => gold >= 0;

        private Boolean hasGoldInBank(FSA fsa) => bank >= 0;



        public void printStatus()
        {
            Console.WriteLine("Thirst: "+thirst+ " Tired: "+tired +" Gold: "+gold+" Bank: "+bank);
        }
    }
}