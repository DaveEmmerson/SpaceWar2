using System;
using System.Collections.Generic;
using System.Linq;
using DEMW.SpaceWar2.Core.GameObjects;
using DEMW.SpaceWar2.Core.Physics;
using DEMW.SpaceWar2.Core.Utils.XnaWrappers;
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
        private GameTime _gameTime;

        private class TestGameObject : GameObject
        {
            public TestGameObject(Vector2 position, float radius, float mass) : base(position, radius, mass)
            {
            }

            protected override void UpdateInternal(float deltaT) { }
            public override void Draw(IGraphicsDevice graphicsDevice) { }

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
            _gameTime = new GameTime(new TimeSpan(10000000), new TimeSpan(200000));
        }

        [Test]
        public void GameObject_can_be_created_and_its_properties_are_set_in_the_constructor()
        {
            Assert.IsFalse(_gameObject.Expired);

            Assert.AreEqual(200f, _gameObject.Mass);
            Assert.AreEqual(_position, _gameObject.Position);
            Assert.AreEqual(new Vector2(0f, 0f), _gameObject.Velocity);
            
            Assert.AreEqual(0f, _gameObject.Rotation);
            Assert.AreEqual(0f, _gameObject.AngularVelocity);

            Assert.AreEqual(30f, _gameObject.Radius);
            Assert.IsNull(_gameObject.Model);
            Assert.AreEqual(Color.Transparent, _gameObject.Color);

            _gameObject.TotalForce.AssertAreEqualWithinTolerance(Vector2.Zero);
            Assert.AreEqual(0f, _gameObject.TotalMoment);
        
            Assert.IsEmpty(_gameObject.Forces);

            //Just to get 100% coverage
            _gameObject.Draw(null);
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
        public void ApplyExternalForce_does_nothing_when_force_argument_is_null()
        {
            _gameObject.ApplyExternalForce(null);
            _gameObject.Update(_gameTime);

            Assert.IsEmpty(_gameObject.Forces);
        }

        [Test]
        public void ApplyExternalForce_does_nothing_when_zero_force_is_specified()
        {
            var force = new Force(Vector2.Zero, Vector2.One);

            _gameObject.ApplyExternalForce(force);
            _gameObject.Update(_gameTime);
           
            Assert.IsEmpty(_gameObject.Forces);
        }

        [Test]
        public void ApplyExternalForce_adds_force_when_non_zero_force_is_specified()
        {
            var direction = new Vector2(10f, 0f);
            var force = new Force(direction);

            _gameObject.ApplyExternalForce(force);
            _gameObject.Update(_gameTime);

            Assert.AreEqual(1, _gameObject.Forces.Count());
            var actualForce = _gameObject.Forces.First();
            actualForce.AssertAreEqualWithinTolerance(force);
            _gameObject.TotalForce.AssertAreEqualWithinTolerance(direction);

            Assert.AreEqual(0f, _gameObject.TotalMoment);
        }

        [Test]
        public void ApplyInternalForce_does_nothing_when_zero_force_is_added()
        {
            var force = new Force(Vector2.Zero, Vector2.One);

            _gameObject.ApplyInternalForce(force);
            _gameObject.Update(_gameTime);

            Assert.IsEmpty(_gameObject.Forces);
        }

        [Test]
        public void ApplyInternalForce_adds_force_relative_to_objects_rotation_when_non_zero_force_is_specified()
        {
            var force = new Force(new Vector2(10f, 0f));
            _gameObject.Rotation = MathHelper.Pi/2;

            _gameObject.ApplyInternalForce(force);
            _gameObject.Update(_gameTime);

            Assert.AreEqual(1, _gameObject.Forces.Count());
            var expectedForce = new Force(new Vector2(-4.37113897E-07f, 10f));
            var actualForce = _gameObject.Forces.First();
            actualForce.AssertAreEqualWithinTolerance(expectedForce);
            _gameObject.TotalForce.AssertAreEqualWithinTolerance(expectedForce.Vector);

            Assert.AreEqual(0f, _gameObject.TotalMoment);
        }

        [Test]
        public void Update_resolves_multiple_forces_into_TotalForce()
        {
            var force1 = new Force(new Vector2(1f, 0f));
            var force2 = new Force(new Vector2(2f, 4f));
            var force3 = new Force(new Vector2(0f, 8f));

            _gameObject.ApplyExternalForce(force1);
            _gameObject.ApplyExternalForce(force2);
            _gameObject.ApplyExternalForce(force3);
            _gameObject.Update(_gameTime);

            Assert.AreEqual(3, _gameObject.Forces.Count());

            var actualForce1 = _gameObject.Forces.ElementAt(0);
            var actualForce2 = _gameObject.Forces.ElementAt(1);
            var actualForce3 = _gameObject.Forces.ElementAt(2);

            actualForce1.AssertAreEqualWithinTolerance(force1);
            actualForce2.AssertAreEqualWithinTolerance(force2);
            actualForce3.AssertAreEqualWithinTolerance(force3);

            var expectedTotalForce = new Vector2(3f, 12f);
            _gameObject.TotalForce.AssertAreEqualWithinTolerance(expectedTotalForce);

            Assert.AreEqual(0f, _gameObject.TotalMoment);
        }

        [Test]
        public void Update_calculates_zero_moment_for_parallel_force_offset_from_centre_of_object()
        {
            var force = new Force(new Vector2(10f, 0f), new Vector2(1f, 0f));

            _gameObject.ApplyExternalForce(force);
            _gameObject.Update(_gameTime);

            Assert.AreEqual(0f, _gameObject.TotalMoment);
        }

        [Test]
        public void Update_calculates_moment_for_perpendicular_force_offset_from_centre_of_object()
        {
            var force = new Force(new Vector2(10f, 0f), new Vector2(0f, 1f));

            _gameObject.ApplyExternalForce(force);
            _gameObject.Update(_gameTime);

            Assert.AreEqual(-10f, _gameObject.TotalMoment);
        }

        [Test]
        public void Update_calculates_moment_for_non_perpendicular_force_offset_from_centre_of_object()
        {
            var force = new Force(new Vector2(6f, -2f), new Vector2(0f, -1f));

            _gameObject.ApplyExternalForce(force);
            _gameObject.Update(_gameTime);

            Assert.AreEqual(6f, _gameObject.TotalMoment);
        }

        [Test]
        public void Update_aggregates_mulitple_moments_together()
        {
            var force1 = new Force(new Vector2(10f, 0f), new Vector2(0f, 1f));
            var force2 = new Force(new Vector2(6f, -2f), new Vector2(0f, -1f));

            _gameObject.ApplyExternalForce(force1);
            _gameObject.ApplyExternalForce(force2);
            _gameObject.Update(_gameTime);

            Assert.AreEqual(-4f, _gameObject.TotalMoment);
        }

        [Test]
        public void Update_calculates_acceleration_from_force_and_causes_Velocity_and_Position_to_be_updated()
        {
            _gameObject.Velocity = new Vector2(0f, 0f);
            _gameObject.Position = new Vector2(2f, 2f);

            var force = new Force(new Vector2(10f, 0f));
            _gameObject.ApplyExternalForce(force);
            _gameObject.Update(_gameTime);

            _gameObject.Velocity.AssertAreEqualWithinTolerance(new Vector2(0.001f, 0f), 0.0000001f);
            _gameObject.Position.AssertAreEqualWithinTolerance(new Vector2(2.00001f, 2.00f));
        }

        [Test]
        public void Update_calculates_angularAcceleration_from_force_and_causes_AngularVelocity_and_Rotation_to_be_updated()
        {
            _gameObject.AngularVelocity = 0f;
            _gameObject.Rotation = 3f;

            var force = new Force(new Vector2(100f, 0f), new Vector2(0f, -20f));
            _gameObject.ApplyExternalForce(force);
            _gameObject.Update(_gameTime);

            Assert.AreEqual(0.000555555569f, _gameObject.AngularVelocity);
            Assert.AreEqual(3.00000548f, _gameObject.Rotation);
        }

        [Test]
        public void Teleport_updates_the_position_to_the_specified_location()
        {
            var destination = new Vector2(43f, -20f);
            _gameObject.Teleport(destination);

            Assert.AreEqual(destination, _gameObject.Position);
        }
    }
}
