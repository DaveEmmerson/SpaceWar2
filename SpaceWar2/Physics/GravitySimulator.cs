using System.Collections.Generic;
using DEMW.SpaceWar2.GameObjects;
using Microsoft.Xna.Framework;

namespace DEMW.SpaceWar2.Physics
{
    public class GravitySimulator
    {
        private readonly IList<IGameObject> _sources = new List<IGameObject>();
        private readonly IList<IGameObject> _participants = new List<IGameObject>();

        public void RegisterSource(IGameObject source)
        {
            _sources.Add(source);
        }

        public void RegisterParticipant(IGameObject participant)
        {
            _participants.Add(participant);
        }

        public void UnRegister(IGameObject item)
        {
            _participants.Remove(item);
            _sources.Remove(item);
        }

        public void Simulate()
        {
            foreach (var source in _sources)
            {
                foreach (var participant in _participants)
                {
                    var force = CalculateAccelerationDueToGravity(source, participant);
                    participant.ApplyExternalForce(force);
                }
            }
        }

        private static Force CalculateAccelerationDueToGravity(IGameObject source, IGameObject participant)
        {
            const int gravitationalConstant = 100;

            var unitVector = DirectionBetween(source, participant);
            var diff = (participant.Position - source.Position);

            //TODO remove this when proper collision detection has been implemented
            if (diff.Length() <= source.Radius + participant.Radius)
            {
                unitVector = Vector2.Negate(unitVector);
            }

            var lengthSquared = diff.LengthSquared();
            return new Force(gravitationalConstant * (source.Mass * participant.Mass / lengthSquared) * unitVector);
        }

        private static Vector2 DirectionBetween(IGameObject source, IGameObject participant)
        {
            var unitVector = source.Position - participant.Position;
            unitVector.Normalize();
            return unitVector;
        }
    }
}
