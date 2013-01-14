using System;
using System.Collections.Generic;
using DEMW.SpaceWar2.Core.Utils;
using NSubstitute;
using NUnit.Framework;

namespace DEMW.SpaceWar2Tests.Utils
{
    class TestBaseClass { }
    class TestSubClass : TestBaseClass { }
    class OtherSubClass : TestBaseClass { }
        
    [TestFixture]
    class ExtensionsTests
    {
        private IList<TestBaseClass> _list;
        private Action<TestSubClass> _action;

        [SetUp]
        public void SetUp()
        {
            _list = new List<TestBaseClass>();
            _action = Substitute.For<Action<TestSubClass>>();
        }

        [Test]
        public void ForEach_runs_without_exception_on_empty_list()
        {
            _list.ForEach(_action);
            _action.DidNotReceive().Invoke(Arg.Any<TestSubClass>());
        }

        [Test]
        public void ForEach_applies_only_to_specified_subtype()
        {
            var t1 = new TestSubClass();
            var t2 = new TestSubClass();
            _list.Add(new TestBaseClass());
            _list.Add(new TestBaseClass());
            _list.Add(t1);
            _list.Add(new TestBaseClass());
            _list.Add(new OtherSubClass());
            _list.Add(new TestBaseClass());
            _list.Add(t2);

            _list.ForEach(_action);

            _action.Received(1).Invoke(t1);
            _action.Received(1).Invoke(t2);
            _action.DidNotReceive().Invoke(Arg.Is<TestSubClass>(x => !x.Equals(t1) && !x.Equals(t2)));
        }
    }
}
