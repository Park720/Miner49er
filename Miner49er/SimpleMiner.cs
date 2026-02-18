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

            miningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.parched) },
                new ActionDelegate[] { new ActionDelegate(this.moving) }, drinkingState);

            miningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.exhausted) },
                new ActionDelegate[] { }, sleepingState);

            miningState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.pocketsFull) },
                new ActionDelegate[] { new ActionDelegate(this.moving) }, bankingState);
            
            miningState.addTransition("tick",
                new ConditionDelegate[] { }, 
                new ActionDelegate[] { new ActionDelegate(this.dig) }, miningState);

            // set drinking transitions
            drinkingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.thirsty) },
                new ActionDelegate[] { new ActionDelegate(this.takeDrink) }, drinkingState);

            drinkingState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.moving) }, miningState);

            // set sleeping transitions
            sleepingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.isTired) },
                new ActionDelegate[] { new ActionDelegate(this.takeSleep) }, sleepingState);

            sleepingState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.moving) }, miningState);

            // set banking transitions
            bankingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.pocketsNotEmpty) },
                new ActionDelegate[] { new ActionDelegate(this.depositGold) }, bankingState);

            bankingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.parched) },
                new ActionDelegate[] { }, drinkingState);

            bankingState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.exhausted) },
                new ActionDelegate[] { }, sleepingState);

            bankingState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.moving) }, miningState);

            // set resting transitions (while mine is closed)
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

            // *** PERVASIVE TRANSITIONS ***
            // MineClose can interrupt ANY state -> go to Resting
            addPervasiveTransition("MineClose",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.mineClosing) }, unemployedState);

            // MineOpen can interrupt ANY state -> go back to Mining
            addPervasiveTransition("MineOpen",
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

        private void withdrawGold(FSA fsa)
        {
            gold +=2;
            bank -=2;
            Console.WriteLine("withdraw a gold nugget");
        }

        private void cantWithdraw(FSA fsa)
        {
            if (bank <= 0)
            {
                Console.WriteLine("No gold in the bank to withdraw.");
            }
        }
        private void cantAfford(FSA fsa)
        {
            Console.WriteLine("Can't afford it! Need more money...");
        }
        private void unemployed(FSA fsa)
        {
            Console.WriteLine("Saddly unemployed while mine is closed...");
        }
        private void mineClosing(FSA fsa)
        {
            Console.WriteLine("Mine is closing! Time to rest.");
        }

        private void mineOpening(FSA fsa)
        {
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