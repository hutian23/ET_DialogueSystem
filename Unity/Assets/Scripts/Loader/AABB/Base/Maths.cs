using System;

namespace AABB
{
    /// <summary>
    /// math helper
    /// </summary>
    public static class Maths
    {
        public const float E = (float)Math.E;

        public const float Log10E = 0.4342945f;

        public const float Log2E = 1.442695f;

        public const float Pi = (float)Math.PI;

        public const float PiOver2 = (float)(Math.PI / 2.0);

        public const float PiOver4 = (float)(Math.PI / 4.0);

        public const float TwoPi = (float)(Math.PI * 2.0);

        public static float Barycentric(float value1, float value2, float value3, float amount1, float amount2)
        {
            return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
        }

        public static float CatmullRom(float value1, float value2, float value3, float value4, float amount)
        {
            // Using formula from http://www.mvps.org/directx/articles/catmull/
            // Internally using doubles not to lose precission
            double amountSquared = amount * amount;
            double amountCubed = amountSquared * amount;
            return (float)(0.5 * (2.0 * value2) + (value3 - value1) * amount + (2.0 * value1 - 5.0 * value2 + 4.0 * value3 - value4) * amountSquared +
                (3.0 * value2 - value1 - 3.0 * value3 + value4) * amountCubed);
        }

        /// <summary>
        /// Restrict a value to be within a specified range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Clamp(float value, float min, float max)
        {
            //First we check to see if we're greater than the max
            value = (value > max)? max : value;
            // Then we check to see if we're less than the min.
            value = (value < min)? min : value;

            // There's no check to see if min > max.
            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            value = (value > max)? max : value;
            value = (value < min)? min : value;
            return value;
        }

        public static float Distance(float value1, float value2)
        {
            return Math.Abs(value1 - value2);
        }

	/// <summary>
		/// Performs a Hermite spline interpolation.
		/// </summary>
		/// <param name="value1">Source position.</param>
		/// <param name="tangent1">Source tangent.</param>
		/// <param name="value2">Source position.</param>
		/// <param name="tangent2">Source tangent.</param>
		/// <param name="amount">Weighting factor.</param>
		/// <returns>The result of the Hermite spline interpolation.</returns>
		public static float Hermite(float value1, float tangent1, float value2, float tangent2, float amount)
		{
			// All transformed to double not to lose precission
			// Otherwise, for high numbers of param:amount the result is NaN instead of Infinity
			double v1 = value1, v2 = value2, t1 = tangent1, t2 = tangent2, s = amount, result;
			double sCubed = s * s * s;
			double sSquared = s * s;

			if (amount == 0f)
				result = value1;
			else if (amount == 1f)
				result = value2;
			else
				result = (2 * v1 - 2 * v2 + t2 + t1) * sCubed +
					(3 * v2 - 3 * v1 - 2 * t1 - t2) * sSquared +
					t1 * s +
					v1;
			return (float)result;
		}


		/// <summary>
		/// Linearly interpolates between two values.
		/// </summary>
		/// <param name="value1">Source value.</param>
		/// <param name="value2">Destination value.</param>
		/// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
		/// <returns>Interpolated value.</returns> 
		/// <remarks>This method performs the linear interpolation based on the following formula:
		/// <code>value1 + (value2 - value1) * amount</code>.
		/// Passing amount a value of 0 will cause value1 to be returned, a value of 1 will cause value2 to be returned.
		/// See <see cref="Maths.LerpPrecise"/> for a less efficient version with more precision around edge cases.
		/// </remarks>
		public static float Lerp(float value1, float value2, float amount)
		{
			return value1 + (value2 - value1) * amount;
		}


		/// <summary>
		/// Linearly interpolates between two values.
		/// This method is a less efficient, more precise version of <see cref="Maths.Lerp"/>.
		/// See remarks for more info.
		/// </summary>
		/// <param name="value1">Source value.</param>
		/// <param name="value2">Destination value.</param>
		/// <param name="amount">Value between 0 and 1 indicating the weight of value2.</param>
		/// <returns>Interpolated value.</returns>
		/// <remarks>This method performs the linear interpolation based on the following formula:
		/// <code>((1 - amount) * value1) + (value2 * amount)</code>.
		/// Passing amount a value of 0 will cause value1 to be returned, a value of 1 will cause value2 to be returned.
		/// This method does not have the floating point precision issue that <see cref="Maths.Lerp"/> has.
		/// i.e. If there is a big gap between value1 and value2 in magnitude (e.g. value1=10000000000000000, value2=1),
		/// right at the edge of the interpolation range (amount=1), <see cref="Maths.Lerp"/> will return 0 (whereas it should return 1).
		/// This also holds for value1=10^17, value2=10; value1=10^18,value2=10^2... so on.
		/// For an in depth explanation of the issue, see below references:
		/// Relevant Wikipedia Article: https://en.wikipedia.org/wiki/Linear_interpolation#Programming_language_support
		/// Relevant StackOverflow Answer: http://stackoverflow.com/questions/4353525/floating-point-linear-interpolation#answer-23716956
		/// </remarks>
		public static float LerpPrecise(float value1, float value2, float amount)
		{
			return ((1 - amount) * value1) + (value2 * amount);
		}

		/// <summary>
		/// Returns the greater of two values.
		/// </summary>
		/// <param name="value1">Source value.</param>
		/// <param name="value2">Source value.</param>
		/// <returns>The greater value.</returns>
		public static float Max(float value1, float value2)
		{
			return value1 > value2 ? value1 : value2;
		}

		/// <summary>
		/// Returns the greater of two values.
		/// </summary>
		/// <param name="value1">Source value.</param>
		/// <param name="value2">Source value.</param>
		/// <returns>The greater value.</returns>
		public static int Max(int value1, int value2)
		{
			return value1 > value2 ? value1 : value2;
		}

		/// <summary>
		/// Returns the lesser of two values.
		/// </summary>
		/// <param name="value1">Source value.</param>
		/// <param name="value2">Source value.</param>
		/// <returns>The lesser value.</returns>
		public static float Min(float value1, float value2)
		{
			return value1 < value2 ? value1 : value2;
		}

		/// <summary>
		/// Returns the lesser of two values.
		/// </summary>
		/// <param name="value1">Source value.</param>
		/// <param name="value2">Source value.</param>
		/// <returns>The lesser value.</returns>
		public static int Min(int value1, int value2)
		{
			return value1 < value2 ? value1 : value2;
		}

		/// <summary>
		/// Interpolates between two values using a cubic equation.
		/// </summary>
		/// <param name="value1">Source value.</param>
		/// <param name="value2">Source value.</param>
		/// <param name="amount">Weighting value.</param>
		/// <returns>Interpolated value.</returns>
		public static float SmoothStep(float value1, float value2, float amount)
		{
			// It is expected that 0 < amount < 1
			// If amount < 0, return value1
			// If amount > 1, return value2
			float result = Maths.Clamp(amount, 0f, 1f);
			result = Maths.Hermite(value1, 0f, value2, 0f, result);

			return result;
		}

		/// <summary>
		/// Converts radians to degrees.
		/// </summary>
		/// <param name="radians">The angle in radians.</param>
		/// <returns>The angle in degrees.</returns>
		/// <remarks>
		/// This method uses double precission internally,
		/// though it returns single float
		/// Factor = 180 / pi
		/// </remarks>
		public static float ToDegrees(float radians)
		{
			return (float)(radians * 57.295779513082320876798154814105);
		}

		/// <summary>
		/// Converts degrees to radians.
		/// </summary>
		/// <param name="degrees">The angle in degrees.</param>
		/// <returns>The angle in radians.</returns>
		/// <remarks>
		/// This method uses double precission internally,
		/// though it returns single float
		/// Factor = pi / 180
		/// </remarks>
		public static float ToRadians(float degrees)
		{
			return (float)(degrees * 0.017453292519943295769236907684886);
		}

		/// <summary>
		/// Reduces a given angle to a value between π and -π.
		/// </summary>
		/// <param name="angle">The angle to reduce, in radians.</param>
		/// <returns>The new angle, in radians.</returns>
		public static float WrapAngle(float angle)
		{
			angle = (float)Math.IEEERemainder((double)angle, 6.2831854820251465);
			if (angle <= -3.14159274f)
			{
				angle += 6.28318548f;
			}
			else
			{
				if (angle > 3.14159274f)
				{
					angle -= 6.28318548f;
				}
			}
			return angle;
		}

		/// <summary>
		/// Determines if value is powered by two.
		/// </summary>
		/// <param name="value">A value.</param>
		/// <returns><c>true</c> if <c>value</c> is powered by two; otherwise <c>false</c>.</returns>
		public static bool IsPowerOfTwo(int value)
		{
			return (value > 0) && ((value & (value - 1)) == 0);
		}
    }
}