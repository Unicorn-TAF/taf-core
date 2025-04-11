using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Utility;
using Unicorn.UnitTests.BO;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Tests.Core.Utility
{
    [TestFixture]
    public class DataSetGeneratorTests : NUnitTestRunner
    {
        [Test]
        public void TestGenerateDataSetFromItemsList()
        {
            string[] items = new string[] { "1", "2" };
            List<DataSet> dataSets = DataSetGenerator.FromItems(items);

            Assert.That(dataSets.Select(ds => ds.Name), Is.EquivalentTo(items));
            Assert.That(dataSets.Select(ds => ds.Parameters[0]), Is.EquivalentTo(items));
            Assert.That(dataSets.Select(ds => ds.Parameters.Count), Has.All.EqualTo(1));
        }

        [Test]
        public void TestGenerateDataSetFromCombinationOf2Lists()
        {
            string[] items1 = new string[] { "1", "2" };
            List<int> items2 = new List<int> { 3, 4 };
            List<DataSet> dataSets = DataSetGenerator.CombinationOf(items1, items2);

            Assert.That(dataSets, Has.Count.EqualTo(4));
            Assert.That(dataSets.Select(ds => ds.Parameters.Count), Has.All.EqualTo(2));

            int index = 0;

            for(int i = 0; i < items1.Length; i++)
            {
                for (int j = 0; j < items2.Count; j++)
                {
                    Assert.That(dataSets[index].Name, Is.EqualTo($"[{items1[i]} x {items2[j]}]"));
                    Assert.That(dataSets[index].Parameters[0], Is.EqualTo(items1[i]));
                    Assert.That(dataSets[index].Parameters[1], Is.EqualTo(items2[j]));
                    index++;
                }
            }
        }

        [Test]
        public void TestGenerateDataSetFromCombinationOf3Lists()
        {
            string[] items1 = new string[] { "1", "2" };
            List<int> items2 = new List<int> { 3, 4 };
            SampleEnum[] items3 = (SampleEnum[]) Enum.GetValues(typeof(SampleEnum));

            List<DataSet> dataSets = DataSetGenerator.CombinationOf(items1, items2, items3);

            Assert.That(dataSets, Has.Count.EqualTo(8));
            Assert.That(dataSets.Select(ds => ds.Parameters.Count), Has.All.EqualTo(3));

            int index = 0;

            for (int i = 0; i < items1.Length; i++)
            {
                for (int j = 0; j < items2.Count; j++)
                {
                    for (int k = 0; k < items3.Length; k++)
                    {
                        Assert.That(dataSets[index].Name, Is.EqualTo($"[{items1[i]} x {items2[j]} x {items3[k]}]"));
                        Assert.That(dataSets[index].Parameters[0], Is.EqualTo(items1[i]));
                        Assert.That(dataSets[index].Parameters[1], Is.EqualTo(items2[j]));
                        Assert.That(dataSets[index].Parameters[2], Is.EqualTo(items3[k]));
                        index++;
                    }
                }
            }
        }
    }
}
