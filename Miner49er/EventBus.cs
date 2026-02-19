using System;
using KAI.FSA;

namespace Miner49er
{
    public class EventBus
    {
        List<FSA> fsaList = new List<FSA>();
        public EventBus()
        {
        }

        public void AddEventListener(FSA fsa){
            fsaList.Add(fsa);
        }

        public void RemoveEventListener(FSA fsa){
            fsaList.Remove(fsa);
        }

        public void DoEvent(string evt){
            foreach (FSA fsa in fsaList){
                fsa.DoEvent(evt);
            }
        }
    }
}