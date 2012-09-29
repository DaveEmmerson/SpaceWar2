using System;
using System.Collections.Generic;

namespace SpaceWar2
{
    public class GravitySimulator
    {
        private readonly IList<IGameObject> _sources = new List<IGameObject>();
        private readonly IList<IGameObject> _participants = new List<IGameObject>();

        internal void RegisterSource(IGameObject source)
        {
            _sources.Add(source);
        }

        internal void RegisterParticipant(IGameObject participant)
        {
            _participants.Add(participant);
        }

        internal void Simulate()
        {
            foreach (var source in _sources)
            {
                foreach (var participant in _participants)
                {
                    var gravity = CalculateGravity
                    participant.DoSoemthing
                }
            }
        }
    }
}
