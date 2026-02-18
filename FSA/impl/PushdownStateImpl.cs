namespace KAI.FSA;

public class PushdownStateImpl: StateImpl
{
    public PushdownStateImpl(FSA parent, string name) : base(parent, name)
    {
    }

    public Transition addPushdownTransition(PushdownTransitionImpl.PushPop pushPop, String evt, ConditionDelegate[] conditions, ActionDelegate[] actions,State nextState, 
        String postEvent=null){
        Console.WriteLine ("Make transition");
        Transition t = new PushdownTransitionImpl(pushPop, evt,conditions,actions,nextState, postEvent);	
        Console.WriteLine ("Add transition to list");
        transitionList.Add(t);
        return t;
    }
}
    
    