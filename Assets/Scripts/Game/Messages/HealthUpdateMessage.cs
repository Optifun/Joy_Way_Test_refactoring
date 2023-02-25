using Mirror;
namespace JoyWay.Game.Messages
{
    public class HealthUpdateMessage
    {
        public NetworkIdentity Target;
        public int MaxHealth;
        public int UpdatedHealth;
        public int Delta;
    }
}
