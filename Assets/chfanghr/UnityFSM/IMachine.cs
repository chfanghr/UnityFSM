using System;

namespace chfanghr.UnityFSM
{
    public interface IMachine<in T> where T: struct, IConvertible, IComparable, IFormattable
    {
        void ChangeTo(T desiredState);

        void Update(float deltaTime);
    }
}