using System;

namespace chfanghr.UnityFSM
{
    public interface IState<out T> where T : struct, IConvertible, IComparable, IFormattable
    {
        T ID { get; }

        void Enter();

        void Exit();

        void Update(float deltaTime);
    } 
}