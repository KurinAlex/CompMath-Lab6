namespace CompMath_Lab6._3D;

public class Program
{
	static double F(double x, double y) => Math.Log10(3 * (x + y) + 1);

	static (double, double, double)[] GetSamples(double x0, double stepX, double y0, double stepY, int n)
		=> Enumerable.Range(0, n).Select(i => x0 + stepX * i)
				.Zip(Enumerable.Range(0, n).Select(i => y0 + stepY * i))
				.Select(t => (t.First, t.Second, F(t.First, t.Second)))
				.ToArray();

	static double Interpolate((double X, double Y, double Z)[] samples, double x, double y)
	{
		double z = 0.0;
		int n = samples.Length;
		for (int i = 0; i < n; i++)
		{
			var (xi, yi, zi) = samples[i];
			double p = zi;
			for (int j = 0; j < n; j++)
			{
				if (i == j)
				{
					continue;
				}

				var (xj, yj, _) = samples[j];
				double rx1 = x - xj;
				double ry1 = y - yj;
				double rx2 = xi - xj;
				double ry2 = yi - yj;
				p *= (rx1 * rx2 + ry1 * ry2) / (rx2 * rx2 + ry2 * ry2);
			}
			z += p;
		}
		return z;
	}

	static void Main()
	{
		int n = 6;
		double x0 = 0.0;
		double x1 = 5.0;
		double y0 = 0.0;
		double y1 = 5.0;

		double stepX = (x1 - x0) / (n - 1);
		double stepY = (y1 - y0) / (n - 1);
		double testStepX = stepX / 5.0;
		double testStepY = stepY / 5.0;

		var samples = GetSamples(x0, stepX, y0, stepY, n);
		var testSamples = GetSamples(x0, testStepX, y0, testStepY, n * 5);

		Console.WriteLine(Interpolate(samples, 2, 2));
		Console.WriteLine(F(2, 2));
		Console.ReadLine();
	}
}