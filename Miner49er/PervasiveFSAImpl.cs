namespace KAI.FSA

public interface PervasiveFSA : FSA
{
    Transition addPervasiveTransition(String evt, ConditionDelegate[] conditions, ActionDelegate[] actions, State nextState, String postEvent = null);
}
    public class PervasiveFSAImpl : FSAImpl, PervasiveFSA
    {
        List<Transition> pervasiveTransitions = new List<Transition>();

        public PervasiveFSAImpl() : base()
        {

        }

        public Transition addPervasiveTransition(String evt, ConditionDelegate[] conditions, ActionDelegate[] actions, State nextState, String postEvent = null){
            Transition t = new TransitionImpl(evt, conditions, actions, nextState, postEvent);
            pervasiveTransitions.Add(t);
            return t;
        }
        override public Transition DoEvent(String evt){
            Transition t = base.DoEvent(evt);
            if (t==null){
			    return firePervasiveTransition(evt);
		}
        return t;
	}

        private Transition firePervasiveTransition(String evt){
            foreach (Transition t in pervasiveTransitions){
                if (t.getEvent() == evt){
                    if (t.conditionTest(this)){
                        t.doit(this);
                        return t;
                    }
                }
            }
            return null;
        }
    }
}