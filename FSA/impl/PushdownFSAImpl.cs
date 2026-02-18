using System.Data;

namespace KAI.FSA;

public class PushdownFSAImpl(string name): FSAImpl(name), PushdownFSA
{
    private Stack<State> stateStack = new Stack<State>();
    /// <summary>
    /// Pushes a state ontoi this FSA's state stack
    /// </summary>
    /// <param name="state">
    /// the state to push <see cref="State"/>
    /// </param>
    public void PushState(State state){
        stateStack.Push(state);	
    }

    /// <summary>
    /// Pops the last pushed state and returns it
    /// </summary>
    /// <returns>
    /// the popped State or null if the stack is empty <see cref="State"/>
    /// </returns>
    public State PopState()
    {
        if (stateStack.Count == 0)
        {
            return null;
        }
        else
        {
            return stateStack.Pop();
        }
    }

    public override State MakeNewState(string name=null){
        return MakeNewState<PushdownStateImpl> (name);
    }
}
