using System;

namespace chfanghr.UnityFSM
{
    public interface IStateReflector<T> where T: struct, IConvertible, IComparable, IFormattable
    {
        State<T>[] GetStates();
    }
}