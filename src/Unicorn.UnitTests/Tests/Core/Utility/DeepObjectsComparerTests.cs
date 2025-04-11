using NUnit.Framework;
using Unicorn.Taf.Core.Utility;
using Unicorn.UnitTests.BO;

namespace Unicorn.UnitTests.Tests.Core.Utility
{
    [TestFixture]
    public class DeepObjectsComparerTests
    {
        [Test]
        public void TestDeepComparerAllEqual()
        {
            var complexObject1 = new ComplexObject();
            var complexObject2 = new ComplexObject();

            var result = new DeepObjectsComparer().CompareObjects(complexObject1, complexObject2);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void TestDeepComparerNonPublicDiffer()
        {
            var complexObject1 = new ComplexObject();
            complexObject1.SetProtectedStrings("dsf");
            var complexObject2 = new ComplexObject();

            var result = new DeepObjectsComparer().CompareObjects(complexObject1, complexObject2);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void TestDeepComparerPublicDiffer()
        {
            var complexObject1 = new ComplexObject();
            complexObject1.PublicInnerObject.PublicDouble = 0.5;
            var complexObject2 = new ComplexObject();

            var result = new DeepObjectsComparer().CompareObjects(complexObject1, complexObject2);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void TestDeepComparerPublicFieldsDiffer()
        {
            var complexObject1 = new ComplexObject();
            complexObject1.PublicNullField = new InnerObject() { PublicDouble = 0.1 };
            var complexObject2 = new ComplexObject();
            complexObject2.PublicNullField = new InnerObject() { PublicDouble = 0.2 };

            var result = new DeepObjectsComparer().CompareObjects(complexObject1, complexObject2);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void TestDeepComparerListsDiffer()
        {
            var complexObject1 = new ComplexObject();
            complexObject1.PublicStringsListProperty.Add("asd");
            var complexObject2 = new ComplexObject();

            var result = new DeepObjectsComparer().CompareObjects(complexObject1, complexObject2);

            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void TestDeepComparerListItemsDiffer()
        {
            var complexObject1 = new ComplexObject();
            complexObject1.PublicObjectsListProperty.Add(new InnerObject { PublicString = "a" });
            var complexObject2 = new ComplexObject();
            complexObject2.PublicObjectsListProperty.Add(new InnerObject { PublicString = "b" });

            var result = new DeepObjectsComparer().CompareObjects(complexObject1, complexObject2);

            Assert.That(result, Is.Not.Empty);
        }


        [Test]
        public void TestDeepComparerSeveralDiffer()
        {
            var complexObject1 = new ComplexObject();
            complexObject1.PublicObjectsListProperty.Add(new InnerObject { PublicString = "a" });
            complexObject1.PublicInnerObject.PublicDouble = 0.5;
            var complexObject2 = new ComplexObject();
            complexObject2.PublicObjectsListProperty.Add(new InnerObject { PublicString = "b" });
            complexObject2.PublicInnerObject.PublicString = "2";

            var result = new DeepObjectsComparer().CompareObjects(complexObject1, complexObject2);

            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public void TestDeepComparerListItemsDifferSecondListLonger()
        {
            var complexObject1 = new ComplexObject();
            var complexObject2 = new ComplexObject();
            complexObject2.PublicStringsListProperty.Add("asd");

            var result = new DeepObjectsComparer().CompareObjects(complexObject1, complexObject2);

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestDeepComparerIterfacePropertiesDifferByType()
        {
            var complexObject1 = new ComplexObject() { PublicObjectWithInterface = new Object1() };
            var complexObject2 = new ComplexObject() { PublicObjectWithInterface = new Object2() };

            var result = new DeepObjectsComparer().CompareObjects(complexObject1, complexObject2);

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void TestDeepComparerIterfacePropertiesDifferWithExclusions()
        {
            var complexObject1 = new ComplexObject() { PublicObjectWithInterface = new Object1() };
            var complexObject2 = new ComplexObject() { PublicObjectWithInterface = new Object2() };

            var result = new DeepObjectsComparer().IgnorePaths("ComplexObject.PublicObjectWithInterface")
                .CompareObjects(complexObject1, complexObject2);

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void TestDeepComparerListPropertiesDifferWithExclusions()
        {
            var complexObject1 = new ComplexObject();
            complexObject1.PublicObjectsListProperty.Add(new InnerObject { PublicString = "a" });
            complexObject1.PublicObjectsListProperty.Add(new InnerObject { PublicString = "b" });
            complexObject1.PublicObjectsListProperty.Add(new InnerObject { PublicString = "c" });
            var complexObject2 = new ComplexObject();

            var result = new DeepObjectsComparer().IgnorePaths("PublicObjectsListProperty")
                .CompareObjects(complexObject1, complexObject2);

            Assert.That(result, Is.Empty);
        }
    }
}
