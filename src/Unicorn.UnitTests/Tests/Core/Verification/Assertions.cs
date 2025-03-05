﻿using NUnit.Framework;
using System;
using Unicorn.UnitTests.BO;
using Um = Unicorn.Taf.Core.Verification.Matchers;
using Uv = Unicorn.Taf.Core.Verification;

namespace Unicorn.UnitTests.Tests.Core.Verification
{
    [TestFixture]
    public class Assertions
    {
        [Test, Author("Vitaliy Dobriyan")]
        public void TestSoftAssertThat()
        {
            Assert.Throws<Uv.AssertionException>(delegate
            {
                new Uv.ChainAssert()
                    .That("asd", Um.Is.EqualTo("asd"))
                    .That(2, Um.Is.EqualTo(2))
                    .That(new SampleObject(), Um.Is.EqualTo(new SampleObject("ds", 234)), "Sample objects comparison fail")
                    .That(new SampleObject(), Um.Is.EqualTo(new SampleObject()))
                    .That(new int[] { 2 }, Um.Collection.IsTheSameAs(new int[] { 1 }), "Collections comparison fail")
                    .AssertChain();
            });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestAssertThatPositive() =>
                Uv.Assert.That(1, Um.Is.EqualTo(1));

        [Test, Author("Vitaliy Dobriyan")]
        public void TestAssertThatNegative() =>
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.That("as2d", Um.Is.EqualTo("asd"));
            });

        [Test, Author("Vitaliy Dobriyan")]
        public void TestAssertIsTruePositive()
        {
            var value = "value";
            Uv.Assert.IsTrue(value.Equals(value));
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestAssertIsTrueNegative()
        {
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.IsTrue(1 == 2);
            });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestAssertIsFalsePositive() =>
            Uv.Assert.IsFalse(1 == 2);

        [Test, Author("Vitaliy Dobriyan")]
        public void TestAssertIsFalseNegative()
        {
            var value = "value";
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.IsFalse(value.Equals(value));
            });
        }

        #region Throws

        [Test, Author("Vitaliy Dobriyan")]
        public void TestAssertThrowsPositive() =>
            Uv.Assert.Throws<ArgumentException>(() => throw new ArgumentException());

        [Test, Author("Vitaliy Dobriyan")]
        public void TestAssertThrowsNegative()
        {
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.Throws<ArgumentException>(() => throw new ArithmeticException());
            });
        }

        [Test, Author("Vitaliy Dobriyan")]
        public void TestAssertThrowsNegativeNoException()
        {
            Assert.Throws<Uv.AssertionException>(delegate
            {
                Uv.Assert.Throws<ArgumentException>(() => Assert.IsTrue(true));
            });
        }

        #endregion
    }
}
