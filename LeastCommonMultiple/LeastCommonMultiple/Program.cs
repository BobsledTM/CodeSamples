using System;

namespace MathOperations
{
	/// <summary>
	/// Handles calculations for the least common multiple.
	/// </summary>
	public static class LeastCommonMultiple
	{
		/// <summary>
		/// Returns the least common multiple of the two given numbers using the least common multiple formula
		/// |a*b|/gcd(a,b).
		/// </summary>
		/// <param name="num1">The first given number.</param>
		/// <param name="num2">The second given number.</param>
		/// <returns>The least common multiple.</returns>
		public static long Calculate(long num1, long num2)
		{
			long gcd = GreatestCommonDivisor.Calculate(num1, num2);
			long lcm = Math.Abs(num1 * num2) / gcd;
			return lcm;
		}

		/// <summary>
		/// Returns the least common multiple for all numbers within the inclusive range of int "from" to int "to".
		/// Returns -1 if invalid "from" and "to" or if numbers are negative.
		/// </summary>
		/// <param name="from">The inclusive beginning of the range for the least common multiple.</param>
		/// <param name="to">The inclusive end of range for the least common multiple.</param>
		/// <returns>The least common multiple for all numbers within the range.</returns>
		public static long CalculateRange(long from, long to)
		{
			// Check for valid numbers.
			if (from <= 0
				|| to <= 0
				|| from > to)
			{
				return -1;
			}

			long lcm = from;

			// Calculate the least common multiple each time a new least common multiple is found.
			for (long i = from; i <= to; ++i)
			{
				lcm = Calculate(lcm, i);
			}

			return lcm;
		}
	}

	/// <summary>
	/// Handles calculations for the greatest common divisor.
	/// </summary>
	public static class GreatestCommonDivisor
	{
		/// <summary>
		/// Returns the greatest common denominator of the two given numbers.
		/// </summary>
		/// <param name="num1">Any long.</param>
		/// <param name="num2">Any long.</param>
		/// <returns>The greatest common denominator of any two numbers.</returns>
		public static long Calculate(long num1, long num2)
		{
			long larger;
			long smaller;

			// Determine the larger number of the two.
			if (num1 > num2)
			{
				larger = num1;
				smaller = num2;
			}
			else
			{
				larger = num2;
				smaller = num1;
			}

			// Calculate the GCD by finding the remainder between the larger and smaller number
			// then taking the smaller number and dividing it by the remainder until there is no remainder.
			// When there is no remainder, then the number that was about to be divided(the larger number)
			// is the GCD.
			long remainder;
			do
			{
				remainder = larger % smaller;
				larger = smaller;
				smaller = remainder;
			} while (remainder != 0);

			// The number that was about to be divided by the smaller (the larger) is the GCD
			return larger;
		}
	}
}

class MathOperationsProgram
{
	static void Main()
	{
		Console.WriteLine(MathOperations.LeastCommonMultiple.CalculateRange(1, 20));
		Console.WriteLine(MathOperations.LeastCommonMultiple.Calculate(12, 8));
		Console.WriteLine(MathOperations.GreatestCommonDivisor.Calculate(12, 8));

		/*
		 * Output
		 * 232792560
		 * 24
		 * 4
		 */
	}
}