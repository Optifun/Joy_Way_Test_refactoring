using System;
using Mirror;

namespace JoyWay.Games.Shooter.Character
{
    public class NetworkCharacter : NetworkBehaviour
    {
        public event Action<NetworkCharacter> OnDestroyed;

        private void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
        }
    }
}
