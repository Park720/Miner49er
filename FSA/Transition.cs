using System;
namespace KAI.FSA
{
	public delegate Boolean ConditionDelegate(FSA fsa );
	public delegate void ActionDelegate(FSA fsa);	
	/// <summary>
	/// This interface represents a single state transition
	/// </summary>
	public interface Transition
	{
		/// <summary>
		/// This method returns the event that fires this transition
		/// </summary>
		/// <returns>
		/// The event this transition responds to.  It is iused by the state to see if this is a
		///  transition for a specific event <see cref="String"/>
		/// </returns>
		String getEvent();
		/// <summary>
		/// This method tests to see if all the transition's conditions return true
		/// It is used  by the state to see if this transition can be fired
		/// </summary>
		/// <param name="fsa">
		///  The FSA that this condition's owning state belongs to.  It is passed
		///  into conditiosn for their use. <see cref="FSA"/>
		/// </param>
		/// <returns>
		/// true if all conditions resolve to true, else false <see cref="Boolean"/>
		/// </returns>
		Boolean conditionTest(FSA fsa); 
		/// <summary>
		/// This call causes all actions of this transition to occur sequentially in the
		/// order they were opassed in in this transition's constructor.  Then the passed in
		/// FSA's state is set to this transition's new state.
		/// </summary>
		/// <param name="fsa">
		///  The FSA that this condition's owning state belongs to.  It is passed
		///  into actions for their use, then fas.setCurrentState(0 is invoked with this
		/// Transition's new state. <see cref="FSA"/>
		/// </param>
		void doit(FSA fsa);
	}
}

