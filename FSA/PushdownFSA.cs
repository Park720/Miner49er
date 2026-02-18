namespace KAI.FSA;

public interface PushdownFSA : FSA
{
    		/// <summary>
		/// Pushes a state ontoi this FSA's state stack
		/// </summary>
		/// <param name="state">
		/// the state to push <see cref="State"/>
		/// </param>
		void PushState(State state);
		
		/// <summary>
		/// Pops the last pushed state and returns it
		/// </summary>
		/// <returns>
		/// the popped State or null if the stack is empty <see cref="State"/>
		/// </returns>
		State PopState();
	
}

