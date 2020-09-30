using System;
using System.Collections.Generic;
using System.Linq;

namespace chfanghr.UnityFSM.Tests.Editor.Stub
{
    public class StateMachineDiagnosticsLog<T> where T: struct, IConvertible, IComparable, IFormattable
    {
        public struct LogEntry
        {
            public enum Callback
            {
                Enter,
                Exit,
                Update
            }
            
            public T State { get; set; }
            
            public Callback Call { get; set; }

            public float FloatParam { get; set; }
            
            public override string ToString()
            {
                return $"LogEntry: {{ Callback: {Call}, State: {State}, FloatParam: {FloatParam} }}";
            }
        }
        
        private List<LogEntry> Entries { get; }

        public StateMachineDiagnosticsLog()
        {
            Entries=new List<LogEntry>();
        }

        public void AddEntry(LogEntry.Callback call, T state, float floatParam = -1.0f)
        {
            Entries.Add(new LogEntry{
                State = state,
                Call = call,
                FloatParam = floatParam
            });
        }

        public override string ToString()
        {
            var fullLog = Entries.Aggregate("StateMachineLog:\n", 
                (current, entry) => string.Concat(current, entry, "\n"));
            fullLog += "End of Log";
            return fullLog;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherStateMachineDiagnosticsLog = (StateMachineDiagnosticsLog<T>)obj;
            if (otherStateMachineDiagnosticsLog.Entries.Count != Entries.Count)
            {
                return false;
            }

            for (var i = 0; i < Entries.Count; i++)
            {
                if (!otherStateMachineDiagnosticsLog.Entries[i].Equals(Entries[i]))
                {
                    return false;
                }
            }

            return true;
        }

        protected bool Equals(StateMachineDiagnosticsLog<T> other)
        {
            return Equals(Entries, other.Entries);
        }

        public override int GetHashCode()
        {
            return Entries.GetHashCode();
        }
    }
}