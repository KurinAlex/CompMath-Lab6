namespace CompMath_Lab6
{
	public class SplineInterpolation : IInterpolation
	{
		public string Name => "Spline";

		public double Interpolate((double X, double Y)[] samples, double xx)
		{
			var x = samples.Select(s => s.X).ToArray();
			var y = samples.Select(s => s.Y).ToArray();

			int n = x.Length;

			var h = new double[n - 1];
			var alpha = new double[n - 1];
			var l = new double[n];
			var mu = new double[n - 1];
			var z = new double[n];

			var a = new double[n];
			var b = new double[n - 1];
			var c = new double[n];
			var d = new double[n - 1];

			for (int i = 0; i < n - 1; i++)
			{
				h[i] = x[i + 1] - x[i];
			}

			alpha[0] = 0.0;
			for (int i = 1; i < n - 1; i++)
			{
				alpha[i] = 3.0 * (y[i + 1] - y[i]) / h[i] - 3.0 * (y[i] - y[i - 1]) / h[i - 1];
			}

			l[0] = 1.0;
			mu[0] = 0.0;
			z[0] = 0.0;

			for (int i = 1; i < n - 1; i++)
			{
				l[i] = 2.0 * (x[i + 1] - x[i - 1]) - h[i - 1] * mu[i - 1];
				mu[i] = h[i] / l[i];
				z[i] = (alpha[i] - h[i - 1] * z[i - 1]) / l[i];
			}

			l[n - 1] = 1.0;
			z[n - 1] = 0.0;
			c[n - 1] = 0.0;

			for (int i = n - 2; i >= 0; i--)
			{
				c[i] = z[i] - mu[i] * c[i + 1];
				b[i] = (y[i + 1] - y[i]) / h[i] - h[i] * (c[i + 1] + 2.0 * c[i]) / 3.0;
				d[i] = (c[i + 1] - c[i]) / (3.0 * h[i]);
				a[i] = y[i];
			}

			int j = 0;
			for (; j < n - 1; j++)
			{
				if (x[j] <= xx && xx <= x[j + 1])
				{
					break;
				}
			}

			double t = xx - x[j];
			double t2 = t * t;
			double t3 = t2 * t;
			double yy = a[j] + b[j] * t + c[j] * t2 + d[j] * t3;
			return yy;
		}
	}
}
