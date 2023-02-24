namespace JoyWay.Infrastructure
{
	public class LaunchParameters
	{
		public bool IsHost => IsClient && IsServer;
		public bool IsClient;
		public bool IsServer;
	}
}