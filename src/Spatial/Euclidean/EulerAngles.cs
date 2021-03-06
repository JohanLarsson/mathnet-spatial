﻿namespace MathNet.Spatial.Euclidean
{
    using System;
    using MathNet.Numerics;
    using MathNet.Spatial.Units;

    /// <summary>
    /// A means of representing spatial orientation of any reference frame.
    /// More information can be found https://en.wikipedia.org/wiki/Euler_angles
    /// </summary>
    public struct EulerAngles : IEquatable<EulerAngles>
    {
        /// <summary>
        /// Alpha (or phi) is the rotation around the z axis
        /// </summary>
        public readonly Angle Alpha; // phi
        /// <summary>
        /// Beta (or theta) is the rotation around the N axis
        /// </summary>
        public readonly Angle Beta; // theta
        /// <summary>
        /// Gamma (or psi) is the rotation around the Z axis
        /// </summary>
        public readonly Angle Gamma; // psi

        /// <summary>
        /// Constructs a EulerAngles from three provided angles
        /// </summary>
        /// <param name="alpha">an angle</param>
        /// <param name="beta">an angle</param>
        /// <param name="gamma">an angle</param>
        public EulerAngles(Angle alpha, Angle beta, Angle gamma)
        {
            this.Alpha = alpha;
            this.Beta = beta;
            this.Gamma = gamma;
        }

        /// <summary>
        /// Checks if the EulerAngles are empty
        /// </summary>
        /// <returns>true if the angles have not been set</returns>
        public bool IsEmpty()
        {
            return double.IsNaN(this.Alpha.Radians) && double.IsNaN(this.Beta.Radians) && double.IsNaN(this.Gamma.Radians);
        }

        /// <summary>
        /// Compares two EularAngles
        /// </summary>
        /// <param name="other">the other EularAngles to compare</param>
        /// <returns>true if the angles are equal to within a fixed error of 0.000001</returns>
        public bool Equals(EulerAngles other)
        {
            const double defaultAbsoluteError = 0.000001;
            return this.Alpha.Radians.AlmostEqual(other.Alpha.Radians, defaultAbsoluteError) &&
                   this.Beta.Radians.AlmostEqual(other.Beta.Radians, defaultAbsoluteError) &&
                   this.Gamma.Radians.AlmostEqual(other.Gamma.Radians, defaultAbsoluteError);
        }
    }
}
