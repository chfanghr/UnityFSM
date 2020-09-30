using System.Collections;
using UnityEditor;
using NUnit.Framework;
using chfanghr.UnityFSM.Tests.Editor.Stub;

namespace chfanghr.UnityFSM.Tests.Editor
{
    public class StateMachineTests
    {
        [Test]
        public void CtorNoReflection_OneState_EnterOnlyInitialState()
        {
            // Arrange
            var log=new StateMachineDiagnosticsLog<TwoStatesID>();
            var stubStates = new StubStateWithDiagnostics<TwoStatesID>[]
            {
                new StubStateWithDiagnostics<TwoStatesID>(TwoStatesID.One, log),
                new StubStateWithDiagnostics<TwoStatesID>(TwoStatesID.Two, log)
            };
            
            var expectedLog = new StateMachineDiagnosticsLog<TwoStatesID>();
            expectedLog.AddEntry(StateMachineDiagnosticsLog<TwoStatesID>.LogEntry.Callback.Enter, TwoStatesID.One);

            // Act
            // ReSharper disable once CoVariantArrayConversion
            // ReSharper disable once ObjectCreationAsStatement
            new StateMachine<TwoStatesID>(stubStates, TwoStatesID.One);

            // Assert
            Assert.That(log, Is.EqualTo(expectedLog));
        }
        
        [Test]
        public void CtorNoReflection_NotEnoughStates_Throws()
        {
            // Arrange
            var log = new StateMachineDiagnosticsLog<TwoStatesID>();
            var stubStates = new StubStateWithDiagnostics<TwoStatesID>[]
            {
                new StubStateWithDiagnostics<TwoStatesID>(TwoStatesID.One, log)
            };

            var expectedMessage = 
                    "StateMachine trying to initialize with an invalid set of states. Not enough states passed in. Missing states: Two";

            // Act / Assert
            var exception = Assert.Throws<System.ArgumentException>(
                // ReSharper disable once ObjectCreationAsStatement
                // ReSharper disable once CoVariantArrayConversion
                () => new StateMachine<TwoStatesID>(stubStates, TwoStatesID.One));
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void CtorNoReflection_TooManyStates_Throws()
        {
            // Arrange
            var log = new StateMachineDiagnosticsLog<TwoStatesID>();
            var stubStates = new StubStateWithDiagnostics<TwoStatesID>[]
            {
                new StubStateWithDiagnostics<TwoStatesID>(TwoStatesID.One, log),
                new StubStateWithDiagnostics<TwoStatesID>(TwoStatesID.One, log),
                new StubStateWithDiagnostics<TwoStatesID>(TwoStatesID.Two, log),
            };

            var expectedMessage = 
                    "StateMachine trying to initialize with an invalid set of states. Duplicate states passed in. Duplicate states: One";

            // Act / Assert
            var exception = Assert.Throws<System.ArgumentException>(
                // ReSharper disable once ObjectCreationAsStatement
                // ReSharper disable once CoVariantArrayConversion
                () => new StateMachine<TwoStatesID>(stubStates, TwoStatesID.One));
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void CtorNoReflection_CorrectNumberOfStatesButDuplicates_Throws()
        {
            // Arrange
            var log = new StateMachineDiagnosticsLog<TwoStatesID>();
            var stubStates = new StubStateWithDiagnostics<TwoStatesID>[]
            {
                new StubStateWithDiagnostics<TwoStatesID>(TwoStatesID.One, log),
                new StubStateWithDiagnostics<TwoStatesID>(TwoStatesID.One, log),
            };

            // Both "DuplicateStates" and "NotEnoughStates" would be valid errors
            var notEnoughMessage = 
                    "StateMachine trying to initialize with an invalid set of states. Not enough states passed in. Missing states: Two";

            var duplicatesMessage =
                "StateMachine trying to initialize with an invalid set of states. Duplicate states passed in. Duplicate states: One";

            // Act / Assert
            var exception = Assert.Throws<System.ArgumentException>(
                // ReSharper disable once ObjectCreationAsStatement
                // ReSharper disable once CoVariantArrayConversion
                () => new StateMachine<TwoStatesID>(stubStates, TwoStatesID.One));
            Assert.That(exception.Message,
                        Is.EqualTo(notEnoughMessage).Or.EqualTo(duplicatesMessage));
        }

        [Test]
        public void CtorNoReflection_NotAnEnum_Throws()
        {
            // Arrange
            var stubStates = new StubStateWithInvalidEnum[] { };

            var expectedMessage = 
                    "StateMachine trying to initialize with an invalid generic type. Generic type (T) is not an Enum. Type: chfanghr.UnityFSM.Tests.Editor.Stub.NotAnEnum";

            // Act / Assert
            var exception = Assert.Throws<System.ArgumentException>(
                // ReSharper disable once ObjectCreationAsStatement
                // ReSharper disable once CoVariantArrayConversion
                () => new StateMachine<NotAnEnum>(stubStates, default));
            Assert.That(exception.Message, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void ChangeState_ValidState_ExitsThenEnters()
        {
            // Arrange
            var log = new StateMachineDiagnosticsLog<TwoStatesID>();
            var stubStates = new StubStateWithDiagnostics<TwoStatesID>[]
            {
                new StubStateWithDiagnostics<TwoStatesID>(TwoStatesID.One, log),
                new StubStateWithDiagnostics<TwoStatesID>(TwoStatesID.Two, log),
            };

            var stateMachine = new StateMachine<TwoStatesID>(stubStates, TwoStatesID.One);
            var expectedLog = new StateMachineDiagnosticsLog<TwoStatesID>();
            expectedLog.AddEntry(StateMachineDiagnosticsLog<TwoStatesID>.LogEntry.Callback.Enter, TwoStatesID.One);
            expectedLog.AddEntry(StateMachineDiagnosticsLog<TwoStatesID>.LogEntry.Callback.Exit, TwoStatesID.One);
            expectedLog.AddEntry(StateMachineDiagnosticsLog<TwoStatesID>.LogEntry.Callback.Enter, TwoStatesID.Two);

            // Act
            stateMachine.ChangeTo(TwoStatesID.Two);

            // Assert
            Assert.That(log, Is.EqualTo(expectedLog));
        }

        [Test]
        public void ChangeState_CurrentState_DoesNothing()
        {
            // Arrange
            var log = new StateMachineDiagnosticsLog<TwoStatesID>();
            var stubStates = new StubStateWithDiagnostics<TwoStatesID>[]
            {
                new StubStateWithDiagnostics<TwoStatesID>(TwoStatesID.One, log),
                new StubStateWithDiagnostics<TwoStatesID>(TwoStatesID.Two, log),
            };

            var stateMachine = new StateMachine<TwoStatesID>(stubStates, TwoStatesID.One);
            var expectedLog = new StateMachineDiagnosticsLog<TwoStatesID>();
            expectedLog.AddEntry(StateMachineDiagnosticsLog<TwoStatesID>.LogEntry.Callback.Enter, TwoStatesID.One);

            // Act
            stateMachine.ChangeTo(TwoStatesID.One);

            // Assert
            Assert.That(log, Is.EqualTo(expectedLog));
        }
    }
}