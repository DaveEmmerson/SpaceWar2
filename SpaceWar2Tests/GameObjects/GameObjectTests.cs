using System;
using System.Collections.Generic;
using DEMW.SpaceWar2.GameObjects;
using DEMW.SpaceWar2.Physics;
using DEMW.SpaceWar2Tests.TestUtils;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.GameObjects
{
    [TestFixture]
    internal class GameObjectTests
    {
        private TestGameObject _gameObject;
        private Vector2 _position;
        
        private class TestGameObject : GameObject
        {
            public TestGameObject(Vector2 position, float radius, float mass) : base(position, radius, mass)
            {
            }

            protected override void UpdateInternal(float deltaT) { }
            public override void Draw() { }

            public new IEnumerable<Force> Forces
            {
                get { return base.Forces; }
            }
        }

        [SetUp]
        public void SetUp()
        {
            _position = new Vector2(12f, 5.5f);
            _gameObject = new TestGameObject(_position, 30f, 200f);
        }

        [Test]
        public void GameObject_can_be_created_and_its_properties_are_set_in_the_constructor()
        {
            Assert.IsFalse(_gameObject.Expired);

            Assert.AreEqual(200f, _gameObject.Mass);
            Assert.AreEqual(_position, _gameObject.Position);
            Assert.AreEqual(new Vector2(0f, 0f), _gameObject.Velocity);
            
            Assert.AreEqual(72000f, _gameObject.MomentOfInertia);
            Assert.AreEqual(0f, _gameObject.Rotation);
            Assert.AreEqual(0f, _gameObject.AngularVelocity);

            Assert.AreEqual(30f, _gameObject.Radius);
            Assert.IsNull(_gameObject.Model);
            Assert.AreEqual(Color.Transparent, _gameObject.Color);

            Assert.That(_gameObject.TotalForce.Matches(new Force()));
            Assert.AreEqual(0f, _gameObject.TotalMoment);
        
            Assert.IsEmpty(_gameObject.Forces);
        }

        [Test]
        public void Mass_can_be_set()
        {
            _gameObject.Mass = 110f;

            Assert.AreEqual(110f, _gameObject.Mass);
        }

        [Test]
        public void Position_can_be_set()
        {
            _gameObject.Position = new Vector2(5f, 10f);
            
            Assert.AreEqual(new Vector2(5f, 10f), _gameObject.Position);
        }

        [Test]
        public void Velocity_can_be_set()
        {
            _gameObject.Velocity = new Vector2(5f, 10f);

            Assert.AreEqual(new Vector2(5f, 10f), _gameObject.Velocity);
        }

        [Test]
        public void MomentOfInertia_can_be_set()
        {
            _gameObject.MomentOfInertia = 300f;

            Assert.AreEqual(300f, _gameObject.MomentOfInertia);
        }

        [Test]
        public void Rotation_can_be_set()
        {
            _gameObject.Rotation = 3f;

            Assert.AreEqual(3f, _gameObject.Rotation);
        }

        [Test]
        public void AngularVelocity_can_be_set()
        {
            _gameObject.AngularVelocity = 1.23f;

            Assert.AreEqual(1.23f, _gameObject.AngularVelocity);
        }

        [Test]
        public void Radius_can_be_set()
        {
            _gameObject.Radius = 253f;

            Assert.AreEqual(253f, _gameObject.Radius);
        }

        [Test]
        public void Color_can_be_set()
        {
            _gameObject.Color = Color.BurlyWood;

            Assert.AreEqual(Color.BurlyWood, _gameObject.Color);
        }

        [Test]
        public void Update_causes_Position_to_update_in_line_with_velocity()
        {
            _gameObject.Velocity = new Vector2(1f,2f);
            _gameObject.Position = Vector2.Zero;

            var gametime = new GameTime(new TimeSpan(0, 0, 10, 0, 0), new TimeSpan(0, 0, 0, 0, 10));
            _gameObject.Update(gametime);

            var expectedPosition = new Vector2(0.01f, 0.02f);
            Assert.AreEqual(expectedPosition, _gameObject.Position);
        }

        [Test]
        public void Update_causes_Rotation_to_update_in_line_with_AngularVelocity()
        {
            _gameObject.AngularVelocity = 1f;
            _gameObject.Rotation = 0f;

            var gametime = new GameTime(new TimeSpan(0, 0, 10, 0, 0), new TimeSpan(0, 0, 0, 0, 10));
            _gameObject.Update(gametime);

            Assert.AreEqual(0.01f, _gameObject.Rotation);
        }
        
        [Test]
        public void ApplyExternalForce_does_nothing_when_zero_force_is_added()
        {
            var force = new Force(Vector2.Zero, Vector2.One);

            _gameObject.ApplyExternalForce(force);
            _gameObject.Update(new GameTime(new TimeSpan(10000000), new TimeSpan(200000)));
           
            Assert.IsEmpty(_gameObject.Forces);
        }

        [Test]
        public void ApplyInternalForce_does_nothing_when_zero_force_is_added()
        {
            var force = new Force(Vector2.Zero, Vector2.One);

            _gameObject.ApplyInternalForce(force);
            _gameObject.Update(new GameTime(new TimeSpan(10000000), new TimeSpan(200000)));

            Assert.IsEmpty(_gameObject.Forces);
        }

        //TODO Test accelerations work as expected with forces applied

        //TODO Test AddInternal adds force and takes into account rotation
        //Todo Test addexternal just adds the force.

        [Test]
        public void Teleport_updates_the_position_to_the_specified_location()
        {
            var destination = new Vector2(43f, -20f);
            _gameObject.Teleport(destination);

            Assert.AreEqual(destination, _gameObject.Position);
        }

    }
}
