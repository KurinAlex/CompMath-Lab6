namespace CompMath_Lab6;

public class LagrangeInterpolation : IInterpolation
{
	public string Name => "Lagrange";

	public double Interpolate((double X, double Y)[] samples, double x)
	{
		double p = 0.0;
		int n = samples.Length;

		for (int i = 0; i < n; i++)
		{
			double l = 1.0;
			double xi = samples[i].X;

			for (int j = 0; j < n; j++)
			{
				if (i == j)
				{
					continue;
				}

				double xj = samples[j].X;
				l *= (x - xj) / (xi - xj);
			}

			p += l * samples[i].Y;
		}
		return p;
	}
}
