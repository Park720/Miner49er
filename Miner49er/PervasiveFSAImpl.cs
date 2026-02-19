using System;
namespace KAI.FSA
{
	public class PervasiveFSAImpl : FSAImpl
	{
		List<Transition> pervasiveTransitions = new List<Transition>();

        // just call base constructor
        public PervasiveFSAImpl(string name) : base(name)
		{
		}

        // adds a pervasive transition to the FSA
        public override Transition addPervasiveTransition(String evt, ConditionDelegate[] conditions,
			ActionDelegate[] actions, State nextState, String postEvent = null)
		{
			Transition t = new TransitionImpl(evt, conditions, actions, nextState, postEvent);
			pervasiveTransitions.Add(t);
			return t;
		}

        //overrided public virtual bool DoEvent(String evt) ... to check current state then check pervasive transitions when current state cannot handle event
        public override bool DoEvent(String evt)
		{
			if (currentState != null)
			{
				if (currentState.doEvent(this, evt))
				{
					return true;
				}
			}
			return firePervasiveTransition(evt);
		}

        // Check pervasive transitions for unhandled events
        private bool firePervasiveTransition(String evt)
		{
			foreach (Transition t in pervasiveTransitions)
			{
				if (t.getEvent() == evt)
				{
					if (t.conditionTest(this))
					{
						t.doit(this);
						return true;
					}
				}
			}
			return false;
		}
	}
}