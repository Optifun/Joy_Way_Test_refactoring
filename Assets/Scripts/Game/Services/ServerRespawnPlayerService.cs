﻿using System;
using JoyWay.Game.Character;
using JoyWay.Game.Messages;
using MessagePipe;
using Zenject;

namespace JoyWay.Game.Services
{
    public class ServerRespawnPlayerService : IInitializable, IDisposable
    {
        private readonly IPublisher<SpawnCharacterServerMessage> _spawnCharacter;
        private readonly ISubscriber<DeathMessage> _deathMessage;
        private IDisposable _subscription;

        public ServerRespawnPlayerService(ISubscriber<DeathMessage> deathMessage,
            IPublisher<SpawnCharacterServerMessage> spawnCharacter)
        {
            _deathMessage = deathMessage;
            _spawnCharacter = spawnCharacter;
        }

        public void Initialize()
        {
            _subscription = _deathMessage.Subscribe(RespawnCharacter, message => 
                message.Target.gameObject.GetComponent<NetworkCharacter>() != null);
        }

        private void RespawnCharacter(DeathMessage message)
        {
            _spawnCharacter.Publish(new SpawnCharacterServerMessage()
            {
                Connection = message.Target.connectionToClient
            });
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}