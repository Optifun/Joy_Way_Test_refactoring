using JoyWay.Core.Model;

namespace JoyWay.Core.Infrastructure
{
    public class GameState : IGameState
    {
        public bool IsClient { get; set; }
        public bool IsServer { get; set; }
        public GameStateType State { get; private set; }

        public void SetState(GameStateType state)
        {
            State = state;
        }
    }
}
