using System;

namespace chfanghr.UnityFSM
{
    public class State<T>: IState<T> where T: struct, IConvertible, IComparable, IFormattable
    {
        public delegate void EnterMethod();
        public delegate void ExitMethod();
        public delegate void UpdateMethod(float deltaTime);

        private EnterMethod _enter;
        private ExitMethod _exit;
        private UpdateMethod _update;
        
        public T ID { get; }
        
        public State(T id, EnterMethod enter, ExitMethod exit, UpdateMethod update)
        {
            ID = id;
            _enter = enter;
            _exit = exit;
            _update = update;
        }

        public void Enter()
        {
            _enter?.Invoke();
        }

        public void Exit()
        {
            _exit?.Invoke();
        }

        public void Update(float deltaTime)
        {
            _update?.Invoke(deltaTime);
        }
    }
}