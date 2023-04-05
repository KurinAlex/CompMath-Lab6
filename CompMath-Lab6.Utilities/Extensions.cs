namespace CompMath_Lab6.Utilities;

public static class Extensions
{
	public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> e)
		=> e.Aggregate(Enumerable.Empty<T>(), (s, c) => s.Concat(c));
	public static IEnumerable<double> Range(double start, double step, int count)
		=> Enumerable.Range(0, count).Select(i => start + step * i);
}
