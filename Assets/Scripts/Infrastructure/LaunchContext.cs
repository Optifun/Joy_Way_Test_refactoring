namespace JoyWay.Infrastructure
{
    public class LaunchContext : ILaunchContext
    {
        public bool IsClient { get; set; }
        public bool IsServer { get; set; }

        public void Update(bool isClient, bool isServer)
        {
            IsClient = isClient;
            IsServer = isServer;
        }
    }
}
