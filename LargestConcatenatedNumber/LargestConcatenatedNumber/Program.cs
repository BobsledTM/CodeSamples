using System;
using System.Collections.Generic;

/// <summary>
/// Problem
/// Write a function that given a list of non negative integers, 
/// arranges them such that they form the largest possible number.
/// For example, given[50, 2, 1, 9], the largest formed number is 95021.
/// </summary>
namespace LargestConcatenatedNumber
{
	class Program
	{
		static void Main(string[] args)
		{
			uint[] numList = new uint[] { 420, 42, 423 };
			uint[] numList2 = new uint[] { 12, 121, 120 };
			uint[] numList3 = new uint[] { 9, 99, 990, 919, 91, 911 };

			Console.WriteLine(LargestNumberConcatenation.GetLargestNumberConcatenation(numList));
			Console.WriteLine(LargestNumberConcatenation.GetLargestNumberConcatenation(numList2));
			Console.WriteLine(LargestNumberConcatenation.GetLargestNumberConcatenation(numList3));

			/*
			Output
			42423420
			12121120
			99999091991911
			*/
		}
	}

	/// <summary>
	/// Class that gets the largest concatenation of a list of numbers.
	/// For example, given[50, 2, 1, 9], the largest formed number is 95021.
	/// </summary>
	public static class LargestNumberConcatenation
	{
		/// <summary>
		/// Class that keeps track of the relavent information for individual ints when solving
		/// concatenation problem.
		/// </summary>
		private class IntInfo
		{
			public uint OriginalInt { get; private set; }
			public uint OriginalNumberOfDigits { get; private set; }
			public ulong ZeroPaddedNum { get; private set; }
			public ulong SortingNum { get; private set; }

			public IntInfo(uint num)
			{
				OriginalInt = num;
				ulong originalUlong = num;
				ZeroPaddedNum = originalUlong;
				SortingNum = ZeroPaddedNum;
				OriginalNumberOfDigits = Numbers.GetNumberOfDigits(originalUlong);
			}

			/// <summary>
			/// Pads the zero padded num with zeros on the end for the zero padded num.
			/// Also pads the sorting num with 9 on the end.
			/// </summary>
			public void PadNums(uint numDigitsToPad)
			{
				ZeroPaddedNum = Numbers.ConcatenateNumber(ZeroPaddedNum, 0, numDigitsToPad);

				// Note - Padding the sorting number with the first number solves the issue of 42, 420, 423
				// where using a zero padding to sort would make the final result 42342420 instead
				// of 42423420.
				uint? firstDigitInSortingNum = Numbers.GetDigitInNumber(SortingNum, 0);
				if (firstDigitInSortingNum != null)
				{
					SortingNum = Numbers.ConcatenateNumber(SortingNum, (ulong)firstDigitInSortingNum, numDigitsToPad);
				}
				else
				{
					SortingNum = ZeroPaddedNum;
				}
			}

			/// <summary>
			/// Removes the last digits of a given int.
			/// </summary>
			public void RemoveLastDigits(uint numDigitsToRemove)
			{
				ZeroPaddedNum /= (ulong)Math.Pow(10, numDigitsToRemove);
				SortingNum /= (ulong)Math.Pow(10, numDigitsToRemove);
			}
		}

		/// <summary>
		/// Comparer that returns the highest IntInfo SortingNum.
		/// </summary>
		private class IntInfoComparer : Comparer<IntInfo>
		{
			public override int Compare(IntInfo x, IntInfo y)
			{
				if (x.SortingNum < y.SortingNum)
				{
					return 1;
				}
				else if (x.SortingNum > y.SortingNum)
				{
					return -1;
				}
				else
				{
					// If the sorting numbers are equal,
					// calculate which combination of the two original numbers has a larger concatenation.
					ulong concat1 = Numbers.ConcatenateNumber(x.OriginalInt, y.OriginalInt);
					ulong concat2 = Numbers.ConcatenateNumber(y.OriginalInt, x.OriginalInt);

					if (concat1 < concat2)
					{
						return 1;
					}
					else if (concat1 > concat2)
					{
						return -1;
					}
					else
					{
						return 0;
					}
				}
			}
		}

		private static IntInfoComparer _intInfoComparer = new IntInfoComparer();

		/// <summary>
		/// Get the largest concatenation of a list of numbers.
		/// For example, given[50, 2, 1, 9], the largest formed number is 95021.
		/// </summary>
		public static ulong GetLargestNumberConcatenation(uint[] ints)
		{
			ulong largestNumber = 0;

			// First get the total number of digits and keep track of how many digits each number contains.
			uint totalNumberOfDigits = 0;

			// Create a list of information about the integers from the original array.
			List<IntInfo> intInfo = new List<IntInfo>(ints.Length);

			for (int i = 0; i < ints.Length; ++i)
			{
				uint tempInt = ints[i];

				IntInfo tempIntInfo = new IntInfo(ints[i]);
				totalNumberOfDigits += tempIntInfo.OriginalNumberOfDigits;
				intInfo.Add(tempIntInfo);
			}

			// Now pad all of the integers with 0 so they have digits equal to their combined total number of digits.
			for (int i = 0; i < intInfo.Count; ++i)
			{
				intInfo[i].PadNums(totalNumberOfDigits - intInfo[i].OriginalNumberOfDigits);
			}

			// Sort the int info from largest to smallest.
			intInfo.Sort(_intInfoComparer);

			// Loop through the ints and remove the padded numbers equal to the number of digits in the numbers before
			// and then add that number to the largest number total.
			largestNumber += intInfo[0].ZeroPaddedNum;

			uint numZerosToRemove = 0;
			for (int i = 1; i < intInfo.Count; ++i)
			{
				numZerosToRemove += intInfo[i - 1].OriginalNumberOfDigits;

				intInfo[i].RemoveLastDigits(numZerosToRemove);

				largestNumber += intInfo[i].ZeroPaddedNum;
			}

			return largestNumber;
		}
	}

	/// <summary>
	/// Provides static methods that help modify or get information about numbers
	/// used when calculating the largest combination of number concatenation.
	/// </summary>
	public static class Numbers
	{
		/// <summary>
		/// Gets the number of digits that are in a number.
		/// </summary>
		public static uint GetNumberOfDigits(ulong num)
		{
			uint longSize = 0;

			do
			{
				num /= 10;
				++longSize;
			} while (num > 0);

			return longSize;
		}

		/// <summary>
		/// Concatenates two numbers together.
		/// Ex: 12 and 345 becomes 12345.
		/// </summary>
		public static ulong ConcatenateNumber(ulong num, ulong concatNum, uint numTimesToConcat = 1)
		{
			uint concatNumDigits = GetNumberOfDigits(concatNum);

			// Pad the first number with zeros equal to the number of digits of the second number.
			// Ex: 12 and 345 becomes 12000 and 345.
			for (int i = 0; i < numTimesToConcat; ++i)
			{
				num *= (ulong)Math.Pow(10, concatNumDigits);
				num += concatNum;
			}

			return num;
		}

		/// <summary>
		/// Gets a specific digit in a number.
		/// For example in the digit 3592, the digit with an index of 2 is 9.
		/// Returns null if the digit index is too large for the digit tryig to be found.
		/// </summary>
		public static uint? GetDigitInNumber(ulong num, uint digitIndex)
		{
			uint numberOfDigits = GetNumberOfDigits(num);

			// Modify the index so it is the number of digits into the number.
			digitIndex++;

			// Validate the passed in values.
			if (numberOfDigits < digitIndex)
			{
				return null;
			}

			// First remove all numbers to the right of the desired number.
			num /= (ulong)Math.Pow(10, numberOfDigits - digitIndex);

			// Next remove all numbers to the left of the remaining number.
			num %= 10;

			return (uint)num;
		}
	}
}
