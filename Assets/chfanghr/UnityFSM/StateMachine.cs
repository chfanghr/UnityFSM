using System;
using System.Collections.Generic;
using System.Linq;

namespace chfanghr.UnityFSM
{
    public class StateMachine<T>: IMachine<T> where T: struct, IConvertible, IFormattable, IComparable
    {
        private Dictionary<T, IState<T>> _states;

        private IState<T> CurrentState => _states[CurrentStateID];

        private static void VerifyStatesRepresentAllEntriesOfT(IState<T>[] states)
        {
            VerifyStatesArentMissing(states);
            VerifyNoStatesAreDuplicated(states);
        }
        
        private static void VerifyTIsEnum()
        {
            if(typeof(T).IsEnum) return;
            
            throw new ArgumentException($"StateMachine trying to initialize with an invalid generic type. Generic type (T) is not an Enum. Type: {typeof(T)}");
        }

        private static void VerifyStatesArentMissing(IState<T>[] states)
        {
            var missingEntries = GetMissingIDs(states);
            if (missingEntries.Length == 0) return;
            
            var message =
                "StateMachine trying to initialize with an invalid set of states. Not enough states passed in. Missing states: ";

            for (var i = 0; i < missingEntries.Length; i++)
            {
                if (i != 0)
                {
                    message += ", ";
                }
                message += missingEntries[i].ToString();

            }

            throw new ArgumentException(message);
        }

        private static void VerifyNoStatesAreDuplicated(IState<T>[] states)
        {
            var duplicateIDs = GetDuplicateIDs(states);
            if (duplicateIDs.Length == 0) return;
            
            var message =
                "StateMachine trying to initialize with an invalid set of states. Duplicate states passed in. Duplicate states: ";

            for (var i = 0; i < duplicateIDs.Length; i++)
            {
                if (i != 0)
                {
                    message += ", ";
                }

                message += duplicateIDs[i].ToString();
            }
            
            throw new ArgumentException(message);
        }
        
        private static T[] GetMissingIDs(IState<T>[] states)
        {
            var foundTs = states.Select(state => state.ID).ToList();
            var entriesTs = Enum.GetValues(typeof(T));

            return entriesTs
                .Cast<object>()
                .Select((t, i) => (T) entriesTs.GetValue(i))
                .Where(entry => !foundTs.Contains(entry)).ToArray();
        }

        private static T[] GetDuplicateIDs(IState<T>[] states)
        {
            var extraStates= states.Select(state => state.ID).ToList();
            var entryInTs = Enum.GetValues(typeof(T));
           
            for (var i = 0; i < entryInTs.Length; i++)
            {
                var entry = (T) entryInTs.GetValue(i);
                if (extraStates.Contains(entry))
                {
                    extraStates.Remove(entry);
                }
            }

            return extraStates.ToArray();
        }

        private void Initialize(IState<T>[] states, T initialStateID)
        {
            VerifyTIsEnum();
            VerifyStatesRepresentAllEntriesOfT(states);
            
            _states=new Dictionary<T, IState<T>>();
            
            foreach(var state in states)
            {
                _states.Add(state.ID, state);
            }

            CurrentStateID = initialStateID;
            CurrentState.Enter();
        }

        public StateMachine(IState<T>[] states, T initialStateID)
        {
            Initialize(states,initialStateID);
        }
        
        public void ChangeTo(T desiredState)
        {
            if (desiredState.Equals(CurrentStateID))
            {
                return;
            }
            
            CurrentState.Exit();
            CurrentStateID = desiredState;
            CurrentState.Enter();
        }

        public void Update(float deltaTime)
        {
            CurrentState.Update(deltaTime);
        }

        public T CurrentStateID { get; private set; }
    }
}