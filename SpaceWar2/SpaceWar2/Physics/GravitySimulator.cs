using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Physics
{
    internal class GravitySimulator
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

        internal void UnRegister(IGameObject item)
        {
            _participants.Remove(item);
            _sources.Remove(item);
        }

        internal void Simulate()
        {
            foreach (var source in _sources)
            {
                foreach (var participant in _participants)
                {
                    var force = CalculateAccelerationDueToGravity(source, participant);
                    participant.ApplyForce(force);
                }
            }
        }

        private static Vector2 CalculateAccelerationDueToGravity(IGameObject source, IGameObject participant)
        {
            const int gravitationalConstant = -100;

            var unitVector = DirectionBetween(source, participant);
            var diff = (participant.Position - source.Position);
            
            if (diff.Length() > source.Radius)
            {
                var lengthSquared = diff.LengthSquared();
                return gravitationalConstant * (source.Mass * participant.Mass / lengthSquared) * unitVector;
            }

            return new Vector2(0f,0f);
        }

        private static Vector2 DirectionBetween(IGameObject source, IGameObject participant)
        {
            Vector2 unitVector = participant.Position - source.Position;
            unitVector.Normalize();
            return unitVector;
        }
    }
}
