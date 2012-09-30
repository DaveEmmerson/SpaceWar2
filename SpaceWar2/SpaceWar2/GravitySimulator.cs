using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

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
            //TODO MW should probably either make sources IMassive, or put mass on GameObject.
            foreach (var source in _sources.OfType<IMassive>())
            {
                //TODO MW similar to the above should make Gameobject have acceleartion or make all participants Ships
                foreach (var participant in _participants.OfType<Ship>())
                {
                    participant.Acceleration += CalculateAccelerationDueToGravity(source, participant);
                }
            }
        }

        private static Vector2 CalculateAccelerationDueToGravity(IMassive source, IGameObject participant)
        {
            Vector2 diff = source.Position - participant.Position;

            //if (diff.Length() > smallObject.Radius + massiveObject.Radius)
            //{
                diff.Normalize();
                var acceleration = diff * source.Mass / diff.LengthSquared();
            //}

            return acceleration;
        }
    }
}
