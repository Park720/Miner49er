namespace KAI.FSA;

public class PushdownTransitionImpl: TransitionImpl
{
    public enum PushPop { Push, Pop }
    private PushPop pushPop;


    // note that in pop newState is ignored
    public PushdownTransitionImpl(PushPop pushPop,string evt, ConditionDelegate[] conditions, 
        ActionDelegate[] actions, State newState, string postEvent = null) : 
            base(evt, conditions, actions, newState, postEvent)
    {
        this.pushPop = pushPop;
    }

    public override void doit(FSA fsa)
    {
        if (fsa is not PushdownFSA)
        {
            throw new ArgumentException("Pushdown transition passed an incompatagble FSA");
        }

        PushdownFSA pfsa = (PushdownFSA)fsa;
        if (pushPop == PushPop.Push)
        {
            pfsa.PushState(fsa.GetCurrentState());
            base.doit(fsa);
        }
        else
        {
            base.doit(fsa);
            pfsa.SetCurrentState(pfsa.PopState());
        }
    }

    

    
}