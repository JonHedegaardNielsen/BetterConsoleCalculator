using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsoleCalculator;

internal class Calculator
{
	private static readonly char[] operators = { '^', '*', '/', '+', '-' };

	public static (bool isValid, string? errorMessage) IsValidCalculationString(string? calculation)
	{
		if (string.IsNullOrWhiteSpace(calculation))
		{
			return (false, "Calculation string is empty");
		}

		if (calculation.Count(c => c == '(') != calculation.Count(c => c == ')'))
		{
			return (false, "Mismatched parentheses");
		}

		bool charCanBeOperator = false;

		foreach (char c in calculation)
		{
			bool isOperator = operators.Contains(c);

			if (!char.IsDigit(c) && (!charCanBeOperator && isOperator) && c != '(' && c != ')' && c != ',' && c != '.' && operators.Contains(c) && c != ' ')
			{
				return (false, $"Invalid character: '{c}'");
			}

			charCanBeOperator = !isOperator;
		}

		return (true, null);
	}

	public decimal PeformCalculation(string calculation)
	{
		calculation = calculation.Replace(" ", "");

		decimal result = 0m;


		foreach (char c in operators)
		{
			CalculateSingleOperator(c, ref calculation);
		}



		//List<NumberOperatorPair> numberOperatorPairs = GetNumberOperatorPairsOrdered(ref calculation);

		//result = FindNumberInString(calculation, 0).value;

		//foreach (NumberOperatorPair numberOperatorPair in numberOperatorPairs)
		//{
		//	switch (numberOperatorPair.Operator)
		//	{
		//		case '+':
		//			result += numberOperatorPair.Number;
		//			break;
		//		case '-':
		//			result -= numberOperatorPair.Number;
		//			break;
		//		case '*':
		//			result *= numberOperatorPair.Number;
		//			break;
		//		case '/':
		//			result /= numberOperatorPair.Number;
		//			break;
		//		case '^':
		//			result = (decimal)Math.Pow((double)result, (double)numberOperatorPair.Number);
		//			break;
		//		default:
		//			throw new InvalidOperationException($"Invalid operator: {numberOperatorPair.Operator}");
		//	}
		//}


		return decimal.Parse(calculation);
	}

	private (decimal value, int index) FindNumberInStringReverse(string calculation, int index)
	{
		string numberText = "";
		while (index >= 0)
		{
			if (int.TryParse(calculation[index].ToString(), out int _) || calculation[index] == ',' || calculation[index] == '.')
			{
				numberText = calculation[index] + numberText;
			}
			else
			{
				index++;
				break;
			}
			index--;
		}

		return numberText != "" ? (decimal.Parse(numberText), index) : (0, 0);
	}

	private (decimal value, int index) FindNumberInString(string calculation, int index)
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
				index--;
				break;
			}
			index++;
		}

		return numberText != "" ? (decimal.Parse(numberText), index) : (0, 0);
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

			decimal startValue = FindNumberInString(calculation, startIndex + 1).value;
			calculation = calculation.Remove(startIndex, subStringLength + 1);
			calculation = calculation.Insert(startIndex, CalculateNumberOperatorPairSum(subNumberOperatorPairs.ToArray(), startValue).ToString());
		}

		for (int i = 1; i < calculation.Length; i++)
		{
			if (operators.Contains(calculation[i]))
			{
				NumberOperatorPair numberOperatorPair = new NumberOperatorPair()
				{
					Number = FindNumberInString(calculation, i + 1).value,
					Operator = calculation[i]
				};

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

		}

		return result;

	}

	private void CalculateSingleOperator(char operation, ref string calculation)
	{
		while (true)
		{
			int operatorIndex = calculation.IndexOf(operation);
			if (operatorIndex == -1)
			{
				return;
			}
			var num1 = FindNumberInString(calculation, operatorIndex + 1);
			var num2 = FindNumberInStringReverse(calculation, operatorIndex - 1);
			num2.index = num2.index + operatorIndex - 1;
			num1.index = num1.index + operatorIndex + 1;

			calculation = calculation.Remove(num2.index, num1.index - num2.index);
			calculation = calculation.Insert(num2.index, CalculateOperator(operation, num1.value, num2.value).ToString());

		}
	}

	private decimal CalculateOperator(char operation, decimal num1, decimal num2)
	{
		switch (operation)
		{
			case '+':
				return num2 + num1;
			case '-':
				return num2 - num1;
			case '*':
				return num2 * num1;
			case '/':
				return num2 / num1;
		}
		return 0;
	}

}
