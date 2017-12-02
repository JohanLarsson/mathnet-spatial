namespace MathNet.Spatial.Euclidean
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using MathNet.Numerics.LinearAlgebra;
    using MathNet.Spatial.Units;

    /// <summary>
    /// A unit vector, this is used to describe a direction in 2D
    /// </summary>
    [Serializable]
    public struct UnitVector2D : IXmlSerializable, IEquatable<UnitVector2D>, IEquatable<Vector2D>, IFormattable
    {
        /// <summary>
        /// The x component.
        /// </summary>
        public readonly double X;

        /// <summary>
        /// The y component.
        /// </summary>
        public readonly double Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitVector2D"/> struct.
        ///  </summary>
        /// <param name="x">The x component.</param>
        /// <param name="y">The y component.</param>
        private UnitVector2D(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public static UnitVector2D Create(double x, double y)
        {
            var l = Math.Sqrt((x * x) + (y * y));
            if (l < float.Epsilon)
            {
                throw new ArgumentException("l < float.Epsilon");
            }

            return new UnitVector2D(x / l, y / l);
        }

        public static UnitVector2D XAxis => new UnitVector2D(1, 0);

        public static UnitVector2D YAxis => new UnitVector2D(0, 1);

        /// <summary>
        /// The length of the vector not the count of elements
        /// </summary>
        public double Length => 1;

        /// <summary>
        /// Creates a UnitVector2D from its string representation
        /// </summary>
        /// <param name="s">The string representation of the UnitVector2D</param>
        /// <returns></returns>
        public static UnitVector2D Parse(string s)
        {
            var doubles = Parser.ParseItem2D(s);
            return new UnitVector2D(doubles[0], doubles[1]);
        }

        public static bool operator ==(UnitVector2D left, UnitVector2D right)
        {
            return left.Equals(right);
        }

        public static bool operator ==(Vector2D left, UnitVector2D right)
        {
            return right.Equals(left);
        }

        public static bool operator ==(UnitVector2D left, Vector2D right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UnitVector2D left, UnitVector2D right)
        {
            return !left.Equals(right);
        }

        public static bool operator !=(Vector2D left, UnitVector2D right)
        {
            return !right.Equals(left);
        }

        public static bool operator !=(UnitVector2D left, Vector2D right)
        {
            return !left.Equals(right);
        }

        public static double operator *(UnitVector2D left, UnitVector2D right)
        {
            return left.DotProduct(right);
        }

        public static Vector2D operator +(UnitVector2D v1, UnitVector2D v2)
        {
            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2D operator +(Vector2D v1, UnitVector2D v2)
        {
            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2D operator +(UnitVector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2D operator -(UnitVector2D v1, UnitVector2D v2)
        {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2D operator -(Vector3D v1, UnitVector2D v2)
        {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2D operator -(UnitVector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static UnitVector2D operator -(UnitVector2D v)
        {
            return new UnitVector2D(-1 * v.X, -1 * v.Y);
        }

        public static Vector2D operator *(double d, UnitVector2D v)
        {
            return new Vector2D(d * v.X, d * v.Y);
        }

        public static Vector2D operator /(UnitVector2D v, double d)
        {
            return new Vector2D(v.X / d, v.Y / d);
        }

        public static implicit operator Vector2D(UnitVector2D v)
        {
            return new Vector2D(v.X, v.Y);
        }

        public static UnitVector2D ReadFrom(XmlReader reader)
        {
            var v = new UnitVector2D();
            v.ReadXml(reader);
            return v;
        }

        public override string ToString() => this.ToString(null, CultureInfo.InvariantCulture);

        public string ToString(IFormatProvider provider) => this.ToString(null, provider);

        public string ToString(string format, IFormatProvider provider = null)
        {
            var numberFormatInfo = provider != null ? NumberFormatInfo.GetInstance(provider) : CultureInfo.InvariantCulture.NumberFormat;
            var separator = numberFormatInfo.NumberDecimalSeparator == "," ? ";" : ",";
            return $"({this.X.ToString(format, numberFormatInfo)}{separator} {this.Y.ToString(format, numberFormatInfo)})";
        }

        public bool Equals(Vector2D other)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return this.X == other.X && this.Y == other.Y;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        public bool Equals(UnitVector2D other)
        {
            // ReSharper disable CompareOfFloatsByEqualityOperator
            return this.X == other.X && this.Y == other.Y;
            // ReSharper restore CompareOfFloatsByEqualityOperator
        }

        public bool Equals(UnitVector2D other, double tolerance)
        {
            if (tolerance < 0)
            {
                throw new ArgumentException("epsilon < 0");
            }

            return Math.Abs(other.X - this.X) < tolerance &&
                   Math.Abs(other.Y - this.Y) < tolerance;
        }

        public bool Equals(Vector2D other, double tolerance)
        {
            if (tolerance < 0)
            {
                throw new ArgumentException("epsilon < 0");
            }

            return Math.Abs(other.X - this.X) < tolerance &&
                   Math.Abs(other.Y - this.Y) < tolerance;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return (obj is UnitVector2D && this.Equals((UnitVector2D)obj)) ||
                   (obj is Vector2D && this.Equals((Vector2D)obj));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.X.GetHashCode() * 397) ^ this.Y.GetHashCode();
            }
        }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized. </param>
        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            var e = (XElement)XNode.ReadFrom(reader);

            // Hacking set readonly fields here, can't think of a cleaner workaround
            XmlExt.SetReadonlyField(ref this, x => x.X, XmlConvert.ToDouble(e.ReadAttributeOrElementOrDefault("X")));
            XmlExt.SetReadonlyField(ref this, x => x.Y, XmlConvert.ToDouble(e.ReadAttributeOrElementOrDefault("Y")));
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized. </param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttribute("X", this.X);
            writer.WriteAttribute("Y", this.Y);
        }

        public Vector2D ScaleBy(double scaleFactor)
        {
            return scaleFactor * this;
        }

        [Pure]
        public Vector2D ProjectOn(UnitVector2D uv)
        {
            return this.DotProduct(uv) * this;
        }

        /// <summary>
        /// Determine whether or not this unit vector is parallel to another unit vector within a given angle tolerance.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="tolerance"></param>
        /// <returns>true if the vectors are parallel within the angle tolerance, false if they are not</returns>
        [Pure]
        public bool IsParallelTo(UnitVector2D other, Angle tolerance)
        {
            return this.AngleTo(other) < tolerance;
        }

        /// <summary>
        /// Determine whether or not this unit vector is parallel to a vector within a given angle tolerance.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="tolerance"></param>
        /// <returns>true if the vectors are parallel within the angle tolerance, false if they are not</returns>
        [Pure]
        public bool IsParallelTo(Vector2D other, Angle tolerance)
        {
            return this.AngleTo(other) < tolerance;
        }

        [Pure]
        public bool IsPerpendicularTo(Vector2D other, Angle tolerance)
        {
            const double x = Math.PI / 2;
            return Math.Abs(this.AngleTo(other).Radians - x) < tolerance.Radians;
        }

        [Pure]
        public bool IsPerpendicularTo(UnitVector2D other, Angle tolerance)
        {
            const double x = Math.PI / 2;
            return Math.Abs(this.AngleTo(other).Radians - x) < tolerance.Radians;
        }

        [Pure]
        public UnitVector2D Negate()
        {
            return new UnitVector2D(-1 * this.X, -1 * this.Y);
        }

        [Pure]
        public double DotProduct(Vector2D v)
        {
            return (this.X * v.X) + (this.Y * v.Y);
        }

        [Pure]
        public double DotProduct(UnitVector2D v)
        {
            var dp = (this.X * v.X) + (this.Y * v.Y);
            return Math.Max(-1, Math.Min(dp, 1));
        }

        /// <summary>
        /// Returns signed angle
        /// </summary>
        /// <param name="other">The other vector</param>
        /// <returns>The angle between the vectors, with a range between -180° and 180°</returns>
        public Angle SignedAngleTo(Vector2D other)
        {
            return Angle.FromRadians(
                Math.Atan2(
                    this.X * other.Y - other.X * this.Y,
                    this.X * other.X + this.Y * other.Y));
        }

        /// <summary>
        /// Returns signed angle
        /// </summary>
        /// <param name="other">The other vector</param>
        /// <returns>The angle between the vectors, with a range between -180° and 180°</returns>
        public Angle SignedAngleTo(UnitVector2D other)
        {
            return Angle.FromRadians(
                Math.Atan2(
                    this.X * other.Y - other.X * this.Y,
                    this.X * other.X + this.Y * other.Y));
        }

        /// <summary>
        /// Compute the angle between this vector and <paramref name="other"/>
        /// </summary>
        /// <param name="other">The other vector</param>
        /// <returns>The angle between the vectors, with a range between 0° and 180°</returns>
        public Angle AngleTo(Vector2D other)
        {
            return Angle.FromRadians(
                Math.Abs(
                    Math.Atan2(
                        this.X * other.Y - other.X * this.Y,
                        this.X * other.X + this.Y * other.Y)));
        }

        /// <summary>
        /// Compute the angle between this vector and <paramref name="other"/>
        /// </summary>
        /// <param name="other">The other vector</param>
        /// <returns>The angle between the vectors, with a range between 0° and 180°</returns>
        public Angle AngleTo(UnitVector2D other)
        {
            return Angle.FromRadians(
                Math.Abs(
                    Math.Atan2(
                        this.X * other.Y - other.X * this.Y,
                        this.X * other.X + this.Y * other.Y)));
        }

        /// <summary>
        /// Returns a vector that is this vector rotated the signed angle around the about vector
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public UnitVector2D Rotate(Angle angle)
        {
            var ca = Math.Cos(angle.Radians);
            var sa = Math.Sin(angle.Radians);
            return new UnitVector2D(ca * this.X - sa * this.Y, sa * this.X + ca * this.Y);
        }

        /// <summary>
        /// Convert to a Math.NET Numerics dense vector of length 2.
        /// </summary>
        public Vector<double> ToVector()
        {
            return Vector<double>.Build.Dense(new[] { X, Y });
        }
    }
}
