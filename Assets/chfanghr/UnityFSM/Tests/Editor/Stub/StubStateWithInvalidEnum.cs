namespace chfanghr.UnityFSM.Tests.Editor.Stub
{
    public class StubStateWithInvalidEnum: IState<NotAnEnum>
    {
        public NotAnEnum ID { get; }
        public void Enter()
        {
            throw new System.NotImplementedException();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }

        public void Update(float deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public StubStateWithInvalidEnum(NotAnEnum id)
        {
            ID = id;
        }
    }
}