using System;
using System.Collections.Generic;

namespace KAI.FSA
{
	
	/// <summary>
	/// This class implements the Transition logic. TrabnsitionIMpls are immutable once
	/// created.
	/// Author: Jeffrey Kesselman
	/// </summary>
	public class TransitionImpl : Transition
	{
		protected ConditionDelegate[] conditions;
		protected ActionDelegate[] actions;
		protected State newState;
		protected String evt;
		protected String postEvent = null;
		
		public TransitionImpl (String evt, ConditionDelegate[] conditions,
		                       ActionDelegate[] actions, State newState, String postEvent=null)
		{
			this.newState = newState;
			this.evt=evt;
			this.actions= actions;
			this.conditions = conditions;
			this.postEvent = postEvent;
		
		}
		
		
		public String getEvent(){
			return evt;
		}
		
		public Boolean conditionTest(FSA fsa){
			if (this.conditions != null) {
				foreach (ConditionDelegate condition in conditions) {
					if (!condition (fsa)) {
						return false;	
					}
				}
			}
			return true;
		}
		public virtual void doit(FSA fsa){
			if (this.actions != null) {
				foreach (ActionDelegate action in actions) {
					action (fsa);
				}
			}
			if (newState != null) {
				fsa.SetCurrentState (newState);
			}
			if (postEvent!=null){
				fsa.DoEvent(postEvent);	
			}
		}
		
		
		
		
	}
}


		
		