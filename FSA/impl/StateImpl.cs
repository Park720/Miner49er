using System;
using System.Collections.Generic;
namespace KAI.FSA
{
	/// <summary>
	/// This class is used internally by the FSAImpl and contains the actual state logic
	/// Author: Jeffrey P. Kesselman
	/// </summary>
	public class StateImpl : State
	{
		string name;
		FSA parent;
		
		/// <summary>
		/// This holds the states list of transition in evaluation order
		/// </summary>
		protected List<Transition> transitionList = new List<Transition>();
		/// <summary>
		/// This is an accessor that allows the transitionList to be acessed as a psuedo variable
		/// StateImpl.transitions
		/// </summary>
		public List<Transition> transitions {
			/// <summary>
			/// Sets the state's transition list
			/// </summary>
			set {transitionList = value;}
			// Gets the state's transition list
			get {return transitionList;}
		}
		
		public StateImpl(FSA parent, string name){
			this.name=name;	
			this.parent = parent;
		}
		
		/// This method adds a new atrsnitio nto the end of the transition list
		public virtual Transition addTransition(String evt, ConditionDelegate[] conditions, ActionDelegate[] actions,State nextState, 
			String postEvent=null){
			Console.WriteLine ("Make transition");
			Transition t = new TransitionImpl(evt,conditions,actions,nextState, postEvent);	
			Console.WriteLine ("Add transition to list");
			transitionList.Add(t);
			return t;
		}
	
		
		/// <summary>
		/// This method sends an event to the state.  The state will execute the
		/// first transition it finds which matches the event and whose conditions
		/// all resolve to true.
		/// </summary>
		/// <param name="fsa">
		/// The fsa that is executing this state <see cref="FSA"/>
		/// </param>
		/// <param name="evt">
		/// The event represented as a case-sensative string <see cref="String"/>
		/// </param>
		public virtual bool doEvent(FSA fsa, String evt){
			foreach (Transition t in transitionList){
				if (t.getEvent()==evt){
					if (t.conditionTest(fsa)){
						t.doit(fsa);
						return true;
					}
				}
			}
			return false;
		}
		
		public string GetName(){
			return name;
		}


	}
}

