using System;
using  System.Collections.Generic;
namespace KAI.FSA
{
	/// <summary>
	/// This cAlass implements a fintie state machien that matches the FSA interface.
	/// It is intended that this class be sub-classed by variosu kinds of machines to
	/// suit their own needs
	/// </summary>
	public class FSAImpl : FSA
	{
		private List<State> stateList =
			new List<State>();
		public State currentState;
		public Stack<State> stateStack = new Stack<State>();
		public List<Transition> pervasiveTransitionList = new List<Transition>();
		private string name;
		private Boolean traceStates=false;

		public FSAImpl (string name)
		{
			this.name=name;
			
		}
		
		/// This call trigger's the first transition in the current state whose
		/// event is equal to evt (case sensative) and whose conditions all resolve to true.
		/// </summary>
		/// <param name="evt">
		///  The event to process. <see cref="String"/>
		/// </param>
		/// <returns>
		/// The transition that fire or null if no transition fired
		/// </returns>
		public virtual bool DoEvent(String evt)
		{
			if (currentState != null)
			{
				if (currentState.doEvent(this, evt))
				{
					return true;
				}
			}
			else // try pervasive
			{
				foreach (Transition t in pervasiveTransitionList)
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
			}
			return false;
		}
			
		public virtual State MakeNewState(string name=null){
			return MakeNewState<StateImpl> (name);
		}

		public T MakeNewState<T>(string name=null) where T : State{
			T newState =  (T)Activator.CreateInstance(typeof(T), new object[] { this, name});
			stateList.Add(newState);
			return newState;
		}
		
		public void SetCurrentState(State state){
			if (traceStates){
				Console.WriteLine("FSA "+name+" set to state "+state.GetName());
			}
			currentState = state;
		}
		protected void AddToStateList(State state){
			stateList.Add(state);	
		}
		
		/// <summary>
		/// Gets the current state of this FSA
		/// </summary>
		/// <returns>
		/// the current state <see cref="State"/>
		/// </returns>
		public State GetCurrentState(){
			return currentState;	
		}
		
	
		
		public string GetName(){
			return name;
		}

		public virtual Transition addPervasiveTransition(string evt, ConditionDelegate[] conditions, ActionDelegate[] actions, State nextState,
			string postEvent = null)
		{
			Transition t = new TransitionImpl(evt, conditions, actions, nextState, postEvent);
			pervasiveTransitionList.Add(t);
			return t;
		}
	}
}

