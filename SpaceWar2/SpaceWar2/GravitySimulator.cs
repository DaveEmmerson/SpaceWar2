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
            const int gravitationalConstant = -10000;

            var unitVector = NormalizeVectorBetween(source, participant);
            var diff = (participant.Position - source.Position);
            
            if (diff.Length() > source.Radius)
            {
                var lengthSquared = diff.LengthSquared();

                return gravitationalConstant * (source.Mass / lengthSquared) * unitVector;
            }

            return new Vector2(0f,0f);
        }

        private static Vector2 NormalizeVectorBetween(IMassive source, IGameObject participant)
        {
            Vector2 unitVector = participant.Position - source.Position;
            unitVector.Normalize();
            return unitVector;
        }

        internal void Clear()
        {
            _participants.Clear();
            _sources.Clear();
        }
    }
}
