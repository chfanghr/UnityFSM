using System;
using System.Reflection;
using UnityEngine.Assertions;

namespace chfanghr.UnityFSM
{
    public class StateReflector<T>: IStateReflector<T> where T: struct,IConvertible, IFormattable, IComparable
    {
        private readonly object _instanceToReflect;

        private readonly string _enterMethodPattern;
        private readonly string _exitMethodPattern;
        private readonly string _updateMethodPattern;

        public StateReflector(object instanceToReflect, 
            string enterMethodPattern = "Enter{0}", 
            string exitMethodPattern = "Exit{0}", 
            string updateMethodPattern = "Update{0}")
        {
            _instanceToReflect = instanceToReflect;
            _enterMethodPattern = enterMethodPattern;
            _exitMethodPattern = exitMethodPattern;
            _updateMethodPattern = updateMethodPattern;
        }


        public State<T>[] GetStates()
        {
            var enumValues = Enum.GetValues(typeof(T));
            var enumNames = Enum.GetNames(typeof(T));

            var states = new State<T>[enumNames.Length];

            for (var i = 0; i < states.Length; i++)
            {
                states[i] = CreateStateByName(enumNames[i], (T)enumValues.GetValue(i));
            }

            return states;
        }

        private State<T> CreateStateByName(string enumName, T enumValue)
        {
            var enterMethodName = string.Format(_enterMethodPattern, enumName);
            var enterMethod = FindEnterDelegateByName(_instanceToReflect, enterMethodName);
            var exitMethodName = string.Format(_exitMethodPattern, enumName);
            var exitMethod = FindExitDelegateByName(_instanceToReflect, exitMethodName);
            var updateMethodName = string.Format(_updateMethodPattern, enumName);
            var updateMethod = FindUpdateDelegateByName(_instanceToReflect, updateMethodName);
            
            return new State<T>(enumValue, enterMethod, exitMethod, updateMethod);
        }

        private static State<T>.EnterMethod FindEnterDelegateByName(object classInstanceToReflect, string methodName)
        {
            return CreateDelegateForMethodByName(classInstanceToReflect, typeof(State<T>.EnterMethod), methodName) as
                State<T>.EnterMethod;
        }
        
        private static State<T>.ExitMethod FindExitDelegateByName(object classInstanceToReflect, string methodName)
        {
            return CreateDelegateForMethodByName(classInstanceToReflect, typeof(State<T>.ExitMethod), methodName) as
                State<T>.ExitMethod;
        }
        
        private static State<T>.UpdateMethod FindUpdateDelegateByName(object classInstanceToReflect, string methodName)
        {
            return CreateDelegateForMethodByName(classInstanceToReflect, typeof(State<T>.UpdateMethod), methodName) as
                State<T>.UpdateMethod;
        }
        
        private static Delegate CreateDelegateForMethodByName(object classInstanceToReflect, Type delegateType, string methodName)
        {
            var methodInfo = classInstanceToReflect.GetType().GetMethod(methodName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return methodInfo == null
                ? null
                : Delegate.CreateDelegate(delegateType, classInstanceToReflect, methodInfo);
        }
        
    }
}