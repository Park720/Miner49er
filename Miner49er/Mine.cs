using System;
using KAI.FSA;

namespace Miner49er
{
    /// <summary>
    /// The Mine is also a simple FSA
    /// Uses EventBus to send events to the miner when the mine opens and closes.
    /// </summary>
    /// 
    public class Mine : FSAImpl
    {
        // durability of mine
        public int durability = 0;
        // restoration of mine
        public int restoration = 0;
        // is mine ready to open or not
        public Boolean readyToOpen = false;

        // reference to event bus
        EventBus eventBus;

        // The following variables are each oen of the defiend states the mine cna be in.
        State openState;
        State restoringState;

        public Mine(EventBus eb) : base("Mine")
        {
            eventBus = eb;

            openState = MakeNewState("Open");
            restoringState = MakeNewState("Restoring");

            // set open transitions
            // close mine under certaint conditions
            openState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.needsRestoration) },
                new ActionDelegate[] { new ActionDelegate(this.closeMine) }, restoringState);
            // mine damage every tick
            openState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.mineDamage) }, openState);

            // set restoring transitions
            // ready to open to open mine
            restoringState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.isReadyToOpen) },
                new ActionDelegate[] { new ActionDelegate(this.reopenMine) }, openState);

            // restoration to ready to open
            restoringState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.finishedRestoration) },
                new ActionDelegate[] { new ActionDelegate(this.prepareToOpen) }, restoringState);

            // tick restore 
            restoringState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.restore) }, restoringState);

            SetCurrentState(openState);
        }

        /// <summary>
        /// This is a condition that tests to see if the mine is safe to enter 
        /// </summary>
        // durability hits 10 -> close mine
        private Boolean needsRestoration(FSA fsa)
        {
            return durability >= 10;
        }
        // restoration hits 3 -> ready to open
        private Boolean finishedRestoration(FSA fsa)
        {
            return restoration >= 3;
        }
        // readyToOpen step before reopen mine
        private Boolean isReadyToOpen(FSA fsa)
        {
            return readyToOpen;
        }

        /// <summary>
        /// An action that repeats the process of wearing and fixing
        /// </summary>
        private void mineDamage(FSA fsa)
        {
            durability++;
            Console.WriteLine("Mine: limit " + durability + "/10");
        }

        private void closeMine(FSA fsa)
        {
            restoration = 0;
            readyToOpen = false;
            Console.WriteLine("Mine: Needs fixing. Please leave the mine.");
            eventBus.DoEvent("MineClose");
        }

        private void restore(FSA fsa)
        {
            restoration++;
            Console.WriteLine("Mine: Fixing... (" + restoration + "/3)");
        }

        private void prepareToOpen(FSA fsa) // kept showing 4/3 so I just added in one more step to make it look better -- could be developed
        {
            readyToOpen = true;
            Console.WriteLine("Mine: Restoration almost done...");
        }

        private void reopenMine(FSA fsa)
        {
            durability = 0;
            restoration = 0;
            readyToOpen = false;
            Console.WriteLine("Mine: Restoration complete. Mine open.");
            eventBus.DoEvent("MineOpen");
        }
    }
}