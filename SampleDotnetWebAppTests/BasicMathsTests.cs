using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleDotnetWebApp;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleDotnetWebApp.Tests
{
    [TestClass()]
    public class BasicMathsTests
    {
        [TestMethod()]
        public void AddTest()
        {
            BasicMaths bm = new BasicMaths();
            double result = bm.Add(2, 3);
            Assert.AreEqual(result, 5, "Add method gave wrong answer");
        }

        [TestMethod()]
        public void SubtractTest()
        {
            BasicMaths bm = new BasicMaths();
            double result = bm.Subtract(3, 2);
            Assert.AreEqual(result, 1, "Subtract method gave wrong answer");
        }

        [TestMethod()]
        public void divideTest()
        {
            BasicMaths bm = new BasicMaths();
            double result = bm.divide(4, 2);
            Assert.AreEqual(result, 2, "Divide method gave wrong answer");
        }

        [TestMethod()]
        public void MultiplyTest()
        {
            BasicMaths bm = new BasicMaths();
            double result = bm.Multiply(2, 3);
            Assert.AreEqual(result, 6, "Multiply method gave wrong answer");
        }
    }
}