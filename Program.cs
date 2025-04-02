using BetterConsoleCalculator;

class Program
{
	public static void Main(string[] args)
	{
		while (true)
		{
			Calculator calculator = new Calculator();
			decimal result = calculator.PeformCalculation(Console.ReadLine());
			Console.WriteLine(result);
		}
	}
}