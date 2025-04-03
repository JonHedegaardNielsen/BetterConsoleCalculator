using BetterConsoleCalculator;

static class Program
{
	public static void Main(string[] args)
	{
		while (true)
		{
			Calculator calculator = new Calculator();
			//try
			//{
				string? calculation = Console.ReadLine();
				var calculationValidation = Calculator.IsValidCalculationString(calculation);
				if (!calculationValidation.isValid)
				{
					Console.WriteLine(calculationValidation.errorMessage);
					continue;
				}

				decimal result = calculator.PeformCalculation(calculation!);
				Console.WriteLine(result);

			//}
			//catch (DivideByZeroException)
			//{
			//	Console.WriteLine("Cannot divide by zero");
			//}
			//catch (OverflowException)
			//{
			//	Console.WriteLine("Result too big");
			//}
			//catch (Exception ex)
			//{
			//	Console.WriteLine(ex.Message);
			//}
			
		}
	}
}