namespace JoyWay.Core.Services
{
    public interface ILaunchContext
    {
        public bool IsHost => IsClient && IsServer;
        public bool IsClient { get; }
        public bool IsServer { get; }
    }
}
