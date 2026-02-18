using System; 
using KAI.FSA;

namespace Miner49er
{
    /// <summary>
//This interface defines the methods on a Miner that are callable by the game framework 
//</summary>
    public interface Miner : FSA
    {
        /// <summary
        /// This method prints a line describing the Status Of the miner's internal variables. 
        /// </summary> 
        void printStatus();

        //    <sumarry> 
        // This call returns the total of the nunebr of gold nuggets the miner has in the bank and
        // in his pockets. Its is the "score" for this simple game 
        // </summary> 
        //  <returns>
        /// A <see cref="System.Int32" /> <
        int getCurrentWealth();
    }
}