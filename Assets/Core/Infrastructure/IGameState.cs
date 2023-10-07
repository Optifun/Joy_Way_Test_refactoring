using JoyWay.Core.Model;

namespace JoyWay.Core.Infrastructure
{
    public interface IGameState
    {
        public bool IsHost => IsClient && IsServer;
        public bool IsClient { get; }
        public bool IsServer { get; }
        GameStateType State { get; }
    }
}
