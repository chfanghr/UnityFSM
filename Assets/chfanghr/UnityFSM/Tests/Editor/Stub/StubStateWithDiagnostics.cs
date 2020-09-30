using System;

namespace chfanghr.UnityFSM.Tests.Editor.Stub
{
    public class StubStateWithDiagnostics<T>: IState<T> where T: struct, IConvertible, IComparable, IFormattable
    {
        public T ID { get; }
        
        public StateMachineDiagnosticsLog<T> Log { get; set; }

        public StubStateWithDiagnostics(T id, StateMachineDiagnosticsLog<T> log)
        {
            ID = id;
            Log = log;
        }
        
        public void Enter()
        {
            Log.AddEntry(StateMachineDiagnosticsLog<T>.LogEntry.Callback.Enter,ID);
        }

        public void Exit()
        {
            Log.AddEntry(StateMachineDiagnosticsLog<T>.LogEntry.Callback.Exit,ID);
        }

        public void Update(float deltaTime)
        {
            Log.AddEntry(StateMachineDiagnosticsLog<T>.LogEntry.Callback.Enter,ID, deltaTime);
        }
    }
}