namespace CompMath_Lab6.Utilities;

public static class Input
{
	public static T GetInput<T>(string name, params Predicate<T>[] condition) where T : IParsable<T>
	{
		T? res;
		string? input;
		do
		{
			Console.Write($"Enter {name}: ");
			input = Console.ReadLine();
		} while (!T.TryParse(input, null, out res) || !condition.All(p => p(res)));
		return res;
	}
}
