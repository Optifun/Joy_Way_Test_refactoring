using System;
using JoyWay.Core.Model;
namespace JoyWay.Core.Messages
{
    public class GameEvent : EventArgs
    {
        public readonly object Sender;
        public readonly GameEventType Event;
        public GameEvent(object sender, GameEventType e)
        {
            Sender = sender;
            Event = e;
        }
    }

}
