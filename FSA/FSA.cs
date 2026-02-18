using System;
namespace KAI.FSA
{
	/// <summary>
	/// This interface defines the publicly visible interface to a Finite State Automata
	/// Author: Jeffrey P. Kesselman
	/// </summary>
	/// 
	public interface FSA
	{
		/// <summary>
		/// This call trigger's the first transition in the current state whose
		/// event is equal to evt (case sensative) and whose conditions all resolve to true.
		/// </summary>
		/// <param name="evt">
		///  The event to process. <see cref="String"/>
		/// </param>
		/// <returns>
		/// The Transition that fired, or null if none fired
		/// </returns>
		/// 
		bool DoEvent(String evt);
		
		/// <summary>
		/// This sets the current state of the FSA
		/// </summary>
		/// <param name="state">
		/// The current state <see cref="setCurrentState"/>
		/// </param>
		void SetCurrentState(State state);
		
		/// <summary>
		/// Creates a new state that is part of this FSA
		/// </summary>
		State MakeNewState(string name=null);

		/// <summary>
		/// Creates a new state of the passed type that is part of this FSA
		/// </summary>
		T MakeNewState<T>(string name=null) where T : State;
		
		/// <summary>
		/// Gets the current state of this FSA
		/// </summary>
		/// <returns>
		/// the current state <see cref="State"/>
		/// </returns>
		State GetCurrentState();
		

		string GetName();
		
		Transition addPervasiveTransition(String evt, ConditionDelegate[] conditions, ActionDelegate[] actions,State nextState, 
			String postEvent=null);
	}
}

