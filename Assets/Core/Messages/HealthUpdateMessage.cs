using Mirror;
namespace Core.Messages
{
    public class HealthUpdateMessage
    {
        public NetworkIdentity Target;
        public int MaxHealth;
        public int UpdatedHealth;
        public int Delta;

        public override string ToString()
        {
            return $"{nameof(Target)}: {Target.netId}, {nameof(MaxHealth)}: {MaxHealth}, {nameof(UpdatedHealth)}: {UpdatedHealth}, {nameof(Delta)}: {Delta}";
        }
    }
}
