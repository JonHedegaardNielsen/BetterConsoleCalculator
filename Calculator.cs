using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsoleCalculator;

internal class Calculator
{
	private static readonly char[] operators = {'/', '*', '+', '-'};

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
			bool isOperator = operators.Contains(c) && c != '-';

			if ((!char.IsDigit(c)) && (!charCanBeOperator && isOperator) && c != '(' && c != ')' && c != ',' && c != '.' && c != ' ')
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

		GetNumberOperatorPairsOrdered(ref calculation);

		foreach (char c in operators)
		{
			CalculateSingleOperator(c, ref calculation);
		}

		return decimal.Parse(calculation);
	}

	private (decimal value, int index) FindNumberInStringReverse(string calculation, int index)
	{
		string numberText = "";
		while (index >= 0)
		{
			if (int.TryParse(calculation[index].ToString(), out int _) || calculation[index] == ',' || calculation[index] == '.' || calculation[index] == '-')
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
		if (index < 0)
		{
			index = 0;
		}

		return numberText != "" ? (decimal.Parse(numberText), index) : (0, 0);
	}

	private (decimal value, int index) FindNumberInString(string calculation, int index)
	{
		string numberText = "";
		while (index < calculation.Length)
		{
			if (int.TryParse(calculation[index].ToString(), out int _) || calculation[index] == ',' || calculation[index] == '.' || calculation[index] == '-')
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
		

		return numberText != "" ? (decimal.Parse(numberText), index - numberText.Length + 1) : (0, 0);
	}

	private void GetNumberOperatorPairsOrdered(ref string calculation)
	{
		while (calculation.Contains('('))
		{
			int startIndex = calculation.IndexOf('(');
			int subStringLength = calculation.IndexOf(')') - startIndex;
			string substring = calculation.Substring(startIndex + 1, subStringLength - 1);

			GetNumberOperatorPairsOrdered(ref substring);

			foreach (char c in operators)
			{
				CalculateSingleOperator(c, ref substring);
			}

			calculation = calculation.Remove(startIndex, subStringLength + 1);

			calculation = calculation.Insert(startIndex, substring);
		}
	}

	private void CalculateSingleOperator(char operation, ref string calculation)
	{
		while (true)
		{
			int operatorIndex = calculation.IndexOf(operation);
			if (operatorIndex == -1 || operatorIndex == 0)
			{
				return;
			}

			var rightNumber = FindNumberInString(calculation, operatorIndex + 1);
			var leftNumber = FindNumberInStringReverse(calculation, operatorIndex - 1);
			int length = rightNumber.index + rightNumber.value.ToString().Length - leftNumber.index;

			if (length > calculation.Length)
			{
				length = calculation.Length;
			}
			while (true)
			{
				try
				{
					calculation = calculation.Remove(leftNumber.index, length);
				}
				catch (ArgumentOutOfRangeException)
				{
					length--;
					continue;
				}
				break;
			}

			calculation = calculation.Insert(leftNumber.index, CalculateOperator(operation, rightNumber.value, leftNumber.value).ToString());

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
