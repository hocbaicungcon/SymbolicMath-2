﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SymbolicMath;
using SymbolicMath.Simplification;
using static SymbolicMath.ExpressionHelper;
using System.Collections.Generic;

namespace SymMathTests
{
    [TestClass]
    public class Variables
    {

        [TestMethod]
        public void Comstruction()
        {
            var names = new[] { "x", "X", "y", "Y", "z", "Z", "daysPerYear", "gallons" };
            foreach (var name in names)
            {
                Expression e = name;
                Assert.IsInstanceOfType(e, typeof(Variable));
                Assert.AreEqual(name, e.ToString());
            }
        }

        [TestMethod]
        public void Combination()
        {
            var names = new[] { "x", "X", "y", "Y", "z", "Z", "daysPerYear", "gallons" };
            foreach (var name1 in names)
            {
                Expression A = name1;
                foreach (var name2 in names)
                {
                    Expression B = name2;
                    Assert.AreEqual($"({name1} + {name2})", (A + B).ToString());
                    Assert.AreEqual($"({name1} - {name2})", (A - B).ToString());
                    Assert.AreEqual($"({name1} * {name2})", (A * B).ToString());
                    Assert.AreEqual($"({name1} / {name2})", (A / B).ToString());
                }
            }
        }

        [TestMethod]
        public void Derivation()
        {
            var names = new[] { "x", "X", "y", "Y", "z", "Z", "daysPerYear", "gallons" };
            foreach (var name1 in names)
            {
                Expression A = name1;
                Assert.AreEqual(new Constant(1), A.Derivative(name1));
                Assert.AreEqual(new Constant(0), A.Derivative(name1 + "_"));
            }
        }

        [TestMethod]
        public void Simplification()
        {
            Expression x = var("x");
            Expression e = (x + (1 + x));
            Expression I = 1;
            Expression II = 2;
            Assert.AreEqual((1 + (x + x)), new Simplifier().Simplify(e));

            Assert.AreEqual(5 + x, new Simplifier().Simplify(1 + ((1 + ((x + 1) + 1)) + 1)));

            Assert.AreEqual(2 + (I / 5 * x), new Simplifier().Simplify(x / 5 + 1 + I));

            Assert.AreEqual(II / 5, new Simplifier().Simplify((I + I) / 5));

            //Constant is always on the left
            Assert.AreEqual(x - 5, new Simplifier().Simplify(x - (1 + 2 + 2)));

            Assert.AreEqual(5 - x, new Simplifier().Simplify((1 + 2 + 2) - x));
        }

        [TestMethod]
        public void Evaluation()
        {
            Expression x = "x";
            Dictionary<string, double> values = new Dictionary<string, double>() { ["x"] = 100.45 };
            Assert.AreEqual(100.45, x.Evaluate(values));
            Assert.AreEqual(200.9, (x + x).Evaluate(values));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NonConstantValue()
        {
            Expression x = "x";
            Assert.AreEqual(0, x.Value);
        }
    }
}
