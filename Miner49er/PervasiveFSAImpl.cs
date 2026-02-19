using System;
namespace KAI.FSA
{
	/// <summary>
	/// PervasiveFSAImpl extends FSAImpl to properly support pervasive transitions.
	/// When the current state cannot handle an event, pervasive transitions are checked.
	/// </summary>
	public class PervasiveFSAImpl : FSAImpl
	{
		List<Transition> pervasiveTransitions = new List<Transition>();

		public PervasiveFSAImpl(string name) : base(name)
		{
		}

		/// <summary>
		/// This method adds a pervasive transition to the FSA
		/// </summary>
		public override Transition addPervasiveTransition(String evt, ConditionDelegate[] conditions,
			ActionDelegate[] actions, State nextState, String postEvent = null)
		{
			Transition t = new TransitionImpl(evt, conditions, actions, nextState, postEvent);
			pervasiveTransitions.Add(t);
			return t;
		}

		/// <summary>
		/// Override DoEvent: try current state first, then pervasive transitions
		/// </summary>
		public override bool DoEvent(String evt)
		{
			// First try the current state
			if (currentState != null)
			{
				if (currentState.doEvent(this, evt))
				{
					return true;
				}
			}
			// If current state didn't handle it, try pervasive transitions
			return firePervasiveTransition(evt);
		}

		/// <summary>
		/// Fire the first matching pervasive transition
		/// </summary>
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