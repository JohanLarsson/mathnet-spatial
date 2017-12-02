namespace MathNet.Spatial.UnitTests.Euclidean
{
    using System;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    using MathNet.Spatial.Euclidean;
    using MathNet.Spatial.Units;
    using NUnit.Framework;

    [TestFixture]
    public class UnitVector2DTests
    {
        [TestCase(1, 0, 1, 0)]
        [TestCase(2, 0, 1, 0)]
        [TestCase(-1, 0, -1, 0)]
        [TestCase(-2, 0, -1, 0)]
        [TestCase(0, 1, 0, 1)]
        [TestCase(0, 2, 0, 1)]
        [TestCase(0, -1, 0, -1)]
        [TestCase(0, -2, 0, -1)]
        [TestCase(1, 1, 0.70710678118654746, 0.70710678118654746)]
        [TestCase(0.1, 0.1, 0.70710678118654746, 0.70710678118654746)]
        [TestCase(2, 2, 0.70710678118654746, 0.70710678118654746)]
        public void Create(double x, double y, double expectedX, double expectedY)
        {
            var uv = UnitVector2D.Create(x, y);
            Assert.AreEqual(expectedX, uv.X);
            Assert.AreEqual(expectedY, uv.Y);
        }

        [Test]
        public void CreateThrowsWhenPassedZeroes()
        {
            Assert.Throws<ArgumentException>(() => UnitVector2D.Create(0, 0));
        }

        [TestCase("1, 2", "1, 2", 1e-4, true)]
        [TestCase("1, 2", "4, 5", 1e-4, false)]
        public void Equals(string p1s, string p2s, double tolerance, bool expected)
        {
            var uv1 = UnitVector2D.Parse(p1s);
            var uv2 = UnitVector2D.Parse(p2s);
            var v1 = Vector2D.Parse(p1s);
            var v2 = Vector2D.Parse(p2s);
            Assert.IsTrue(uv1 == v1);
            Assert.IsTrue(v1 == uv1);
            Assert.IsTrue(uv2 == v2);
            Assert.IsTrue(v2 == uv2);

            Assert.IsFalse(uv1 != v1);
            Assert.IsFalse(v1 != uv1);
            Assert.IsFalse(uv2 != v2);
            Assert.IsFalse(v2 != uv2);

            Assert.IsTrue(uv1.Equals(uv1));
            Assert.IsTrue(uv1.Equals(v1));
            Assert.IsTrue(v1.Equals(uv1));

            Assert.AreEqual(expected, uv1 == uv2);
            Assert.AreEqual(expected, uv1 == v2);
            Assert.AreEqual(expected, v2 == uv1);

            Assert.AreNotEqual(expected, uv1 != uv2);
            Assert.AreNotEqual(expected, uv1 != v2);
            Assert.AreNotEqual(expected, v2 != uv1);

            Assert.AreEqual(expected, uv1.Equals(uv2));
            Assert.AreEqual(expected, v1.Equals(uv2));
            Assert.AreEqual(expected, uv1.Equals(v2));

            Assert.AreEqual(expected, uv1.Equals((object)uv2));
            Assert.AreEqual(expected, uv1.Equals((object)v2));
            Assert.AreEqual(expected, Equals(uv1, uv2));

            Assert.AreEqual(expected, uv1.Equals(uv2, tolerance));
            Assert.AreEqual(expected, uv1.Equals(v2, tolerance));
        }

        [TestCase("1; 0", 5, "5; 0")]
        [TestCase("1; 0", -5, "-5; 0")]
        [TestCase("-1; 0", 5, "-5; 0")]
        [TestCase("-1; 0", -5, "5; 0")]
        [TestCase("0; 1", 5, "0; 5")]
        public void Scale(string ivs, double s, string exs)
        {
            var uv = UnitVector2D.Parse(ivs);
            var v = uv.ScaleBy(s);
            AssertGeometry.AreEqual(Vector2D.Parse(exs), v, float.Epsilon);
        }

        [TestCase("1; 0", "1; 0", 1)]
        [TestCase("1; 0", "-1; 0", -1)]
        [TestCase("1; 0", "0; -1", 0)]
        public void DotProduct(string v1s, string v2s, double expected)
        {
            var uv1 = UnitVector2D.Parse(v1s);
            var uv2 = UnitVector2D.Parse(v2s);
            var dp = uv1.DotProduct(uv2);
            Assert.AreEqual(dp, expected, 1e-9);
            Assert.IsTrue(dp <= 1);
            Assert.IsTrue(dp >= -1);
        }

        [TestCase("1; 0", "1; 0", "0°")]
        [TestCase("1; 0", "0; 1", "90°")]
        [TestCase("1; 0", "0; -1", "90°")]
        [TestCase("1; 0", "-1; 0", "180°")]
        public void AngleTo(string v1s, string v2s, string expected)
        {
            var uv1 = UnitVector2D.Parse(v1s);
            var v1 = Vector2D.Parse(v1s);
            var uv2 = UnitVector2D.Parse(v2s);
            var v2 = Vector2D.Parse(v2s);
            Assert.AreEqual(Angle.Parse(expected), uv1.AngleTo(uv2));
            Assert.AreEqual(Angle.Parse(expected), uv2.AngleTo(uv1));
            Assert.AreEqual(Angle.Parse(expected), v1.AngleTo(uv2));
            Assert.AreEqual(Angle.Parse(expected), uv2.AngleTo(v1));
            Assert.AreEqual(Angle.Parse(expected), v2.AngleTo(uv1));
            Assert.AreEqual(Angle.Parse(expected), uv1.AngleTo(v2));
        }

        [TestCase("1; 0", "1; 0", "0°")]
        [TestCase("1; 0", "0; 1", "90°")]
        [TestCase("1; 0", "0; -1", "90°")]
        [TestCase("1; 0", "-1; 0", "180°")]
        public void SignedAngleTo(string v1s, string v2s, string expected)
        {
            var uv1 = UnitVector2D.Parse(v1s);
            var v1 = Vector2D.Parse(v1s);
            var uv2 = UnitVector2D.Parse(v2s);
            var v2 = Vector2D.Parse(v2s);
            Assert.AreEqual(Angle.Parse(expected), uv1.SignedAngleTo(uv2));
            Assert.AreEqual(-Angle.Parse(expected), uv2.SignedAngleTo(uv1));
            Assert.Fail("Missing overloads below.");
            //Assert.AreEqual(Angle.Parse(expected), v1.SignedAngleTo(uv2));
            Assert.AreEqual(-Angle.Parse(expected), uv2.SignedAngleTo(v1));
            Assert.AreEqual(Angle.Parse(expected), uv1.SignedAngleTo(v2));
            //Assert.AreEqual(-Angle.Parse(expected), v2.SignedAngleTo(uv1));
        }

        [TestCase("1; 0", "0°", "1; 0")]
        [TestCase("1; 0", "360°", "1; 0")]
        [TestCase("1; 0", "90°", "0; 1")]
        [TestCase("1; 0", "-90°", "0; -1")]
        [TestCase("1; 0", "180°", "-1; 0")]
        public void Rotate(string uvs, string degs, string expected)
        {
            var uv = UnitVector2D.Parse(uvs);
            var actual = uv.Rotate(Angle.Parse(degs));
            AssertGeometry.AreEqual(UnitVector2D.Parse(expected), actual, 1E-6);
        }

        [TestCase("-1, 0", null, "(-1, 0)", 1e-4)]
        [TestCase("-1, 0", "F2", "(-1.00, 0.00)", 1e-3)]
        public void ToString(string vs, string format, string expected, double tolerance)
        {
            var v = UnitVector2D.Parse(vs);
            string actual = v.ToString(format);
            Assert.AreEqual(expected, actual);
            AssertGeometry.AreEqual(v, UnitVector2D.Parse(actual), tolerance);
        }

        [Test]
        public void XmlRoundTrips()
        {
            var uv = UnitVector2D.Create(0.447213595499958, -0.894427190999916);
            var xml = @"<UnitVector2D X=""0.447213595499958"" Y=""-0.894427190999916"" />";
            var elementXml = @"<UnitVector2D><X>0.447213595499958</X><Y>-0.894427190999916</Y></UnitVector2D>";

            AssertXml.XmlRoundTrips(uv, xml, (e, a) => AssertGeometry.AreEqual(e, a));
            var serializer = new XmlSerializer(typeof(UnitVector2D));
            var actuals = new[]
            {
                UnitVector2D.ReadFrom(XmlReader.Create(new StringReader(xml))),
                (UnitVector2D)serializer.Deserialize(new StringReader(xml)),
                (UnitVector2D)serializer.Deserialize(new StringReader(elementXml))
            };
            foreach (var actual in actuals)
            {
                AssertGeometry.AreEqual(uv, actual);
            }
        }

        [TestCase("1,0,0", 3, "3,0,0")]
        public void MultiplyTest(string unitVectorAsString, double multiplier, string expected)
        {
            UnitVector2D unitVector2D = UnitVector2D.Parse(unitVectorAsString);
            Assert.AreEqual(Vector2D.Parse(expected), multiplier * unitVector2D);
        }
    }
}
