using System;
using KAI.FSA;

namespace Miner49er
{

    public class Mine : FSAImpl
    {
        public int durability = 0;
        public int restoration = 0;

        State openState;
        State restoringState;

        EventBus eventBus;

        public Mine(EventBus eb) : base("Mine")
        {
            eventBus = eb;

            openState = MakeNewState("Open");
            restoringState = MakeNewState("Restoring");

            // open transitions
            openState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.needsRestoration) },
                new ActionDelegate[] { new ActionDelegate(this.closeMine) }, restoringState);

            openState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.mineDamage) }, openState);

            // restoring transitions
            restoringState.addTransition("tick",
                new ConditionDelegate[] { new ConditionDelegate(this.finishedRestoration) },
                new ActionDelegate[] { new ActionDelegate(this.reopenMine) }, openState);

            restoringState.addTransition("tick",
                new ConditionDelegate[] { },
                new ActionDelegate[] { new ActionDelegate(this.restore) }, restoringState);

            SetCurrentState(openState);
        }

        // conditions
        private Boolean needsRestoration(FSA fsa)
        {
            return durability >= 10;
        }

        private Boolean finishedRestoration(FSA fsa)
        {
            return restoration >= 3;
        }

        // actions
        private void mineDamage(FSA fsa)
        {
            durability++;
            Console.WriteLine("  Mine: limit " + durability + "/10");
        }

        private void closeMine(FSA fsa)
        {
            restoration = 0;
            Console.WriteLine("Mine needs restoratoin. Please Leave");
            eventBus.DoEvent("MineClose");
        }

        private void restore(FSA fsa)
        {
            restoration++;
            Console.WriteLine("Mine restoring... (" + restoration + "/3)");
        }

        private void reopenMine(FSA fsa)
        {
            durability = 0;
            restoration = 0;
            Console.WriteLine("Mine restoration complete. Mine open.");
            eventBus.DoEvent("MineOpen");
        }
    }
}