﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class TestsObserver
    {
        [Test, Author("Vitaliy Dobriyan")]
        public void TestTestsObserverSearchTestSuites()
        {
            IEnumerable<Type> foundSuites = Taf.Core.Engine.TestsObserver.ObserveTestSuites(Assembly.GetExecutingAssembly());
            Assert.AreEqual(18, foundSuites.Count());
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestTestsObserverSearchTests()
        {
            IEnumerable<MethodInfo> foundTests = Taf.Core.Engine.TestsObserver.ObserveTests(Assembly.GetExecutingAssembly());
            Assert.AreEqual(70, foundTests.Count());
        }
    }
}
