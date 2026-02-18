using System;
namespace KAI.FSA
{
	
	/// <summary>
	/// This interface defines the publicly vidible  interface to an FSA state. 
	/// Each object that implements this interface represents a single unique
	/// state in its owning FiniteStateMachine
	/// </summary>
	public interface State
	{


		/// <summary>
		/// This adds a new Standard transition to the end of the state's transition list
		/// </summary>
		/// <param name="evt">
		/// The event to which the transition repsonds <see cref="String"/>
		/// </param>
		/// <param name="conditions">
		/// A list of conditiosn that must all evaluate to true to fire this transitions
		///  <see cref="ConditionDelegate[]"/>
		/// </param>
		/// <param name="actions">
		/// A list of actions to take when the transition fires<see cref="ActionDelegate[]"/>
		/// </param>
		/// <param name="nextState">
		/// The new state to which to set this state's owning FSA's current state <see cref="State"/>
		/// </param>
		/// <returns>
		/// An object that represents this transition. <see cref="Transition"/>
		/// </returns>
		/// 
		
		Transition addTransition(String evt, ConditionDelegate[] conditions, ActionDelegate[] actions,State nextState, 
			String postEvent=null);
		
		
		
		
		
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
		/// <returns>
		/// The transition that fired or null if none fired
		/// </returns>
	    Boolean doEvent(FSA fsa, String evt);
		
		string GetName();

	}
}


