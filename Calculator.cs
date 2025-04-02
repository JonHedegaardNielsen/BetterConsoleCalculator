using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsoleCalculator;

internal class Calculator
{
	private readonly char[] operators = { '*', '/', '+', '-' };

	public decimal PeformCalculation(string calculation)
	{
		decimal result = FindNumberInString(calculation, 0);

		List<NumberOperatorPair> numberOperatorPairs = GetNumberOperatorPairsOrdered(ref calculation);


		foreach (NumberOperatorPair numberOperatorPair in numberOperatorPairs)
		{
			switch (numberOperatorPair.Operator)
			{
				case '+':
					result += numberOperatorPair.Number;
					break;
				case '-':
					result -= numberOperatorPair.Number;
					break;
				case '*':
					result *= numberOperatorPair.Number;
					break;
				case '/':
					result /= numberOperatorPair.Number;
					break;
			}
		}


		return result;
	}

	private decimal FindNumberInString(string calculation, int index)
	{
		string numberText = "";
		while (index < calculation.Length)
		{
			if (int.TryParse(calculation[index].ToString(), out int _) || calculation[index] == ',' || calculation[index] == '.')
			{
				numberText += calculation[index];
			}
			else
			{
				break;
			}
			index++;
		}

		return decimal.Parse(numberText);
	}

	private List<NumberOperatorPair> GetNumberOperatorPairsOrdered(ref string calculation)
	{
		List<NumberOperatorPair> numberOperatorPairs = new List<NumberOperatorPair>();

		while (calculation.Contains('('))
		{
			int startIndex = calculation.IndexOf('(');
			int subStringLength = calculation.IndexOf(')') - startIndex;
			string substring = calculation.Substring(startIndex + 1, subStringLength - 1);

			var subNumberOperatorPairs = GetNumberOperatorPairsOrdered(ref substring);

			decimal startValue = FindNumberInString(calculation, startIndex + 1);
			calculation += $"{calculation[startIndex - 1]}{CalculateNumberOperatorPairSum(subNumberOperatorPairs.ToArray(), startValue)}";
			calculation = calculation.Remove(startIndex - 1, subStringLength + 2);
		}

		for (int i = 1; i < calculation.Length; i++)
		{
			if (operators.Contains(calculation[i]))
			{
				NumberOperatorPair numberOperatorPair = new NumberOperatorPair();

				numberOperatorPair.Number = FindNumberInString(calculation, i + 1);
				numberOperatorPair.Operator = calculation[i];
				numberOperatorPairs.Add(numberOperatorPair);
			}
		}

		List<NumberOperatorPair> orderedNumberOperatorPairs = new();
		foreach (var op in operators)
		{
			for (int i = 0; i < numberOperatorPairs.Count(); i++)
			{
				if (numberOperatorPairs[i].Operator == op)
				{
					orderedNumberOperatorPairs.Add(numberOperatorPairs[i]);
				}
			}
		}
		
		return orderedNumberOperatorPairs;
	}

	private decimal CalculateNumberOperatorPairSum(NumberOperatorPair[] numberOperatorPairs, decimal startValue)
	{
		decimal result = startValue;

		foreach (NumberOperatorPair numberOperatorPair in numberOperatorPairs)
		{
			switch (numberOperatorPair.Operator)
			{
				case '+':
					result += numberOperatorPair.Number;
					break;
				case '-':
					result -= numberOperatorPair.Number;
					break;
				case '*':
					result *= numberOperatorPair.Number;
					break;
				case '/':
					result /= numberOperatorPair.Number;
					break;
			}
		}

		return result;

	}
}
