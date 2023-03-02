namespace CompMath_Lab6.Interpolations;

public class SplineInterpolation : IInterpolation
{
	public string Name => "Spline";

	public double Interpolate((double X, double Y)[] samples, double xx)
	{
		int n = samples.Length;

		var x = samples.Select(s => s.X).ToArray();
		var y = samples.Select(s => s.Y).ToArray();

		// polynomials coefficients
		var a = new double[n - 1];
		var b = new double[n - 1];
		var c = new double[n];
		var d = new double[n - 1];

		var h = new double[n - 1];
		for (int i = 0; i < n - 1; i++)
		{
			h[i] = x[i + 1] - x[i];
		}

		// triagonalization matrix elements
		var aa = new double[n - 2];
		var bb = new double[n - 2];
		var cc = new double[n - 2];
		var dd = new double[n - 2];
		for (int i = 0; i < n - 2; i++)
		{
			aa[i] = h[i];
			bb[i] = 2.0 * (h[i] + h[i + 1]);
			cc[i] = h[i + 1];
			dd[i] = 3.0 * ((y[i + 2] - y[i + 1]) / h[i + 1] - (y[i + 1] - y[i]) / h[i]);
		}

		if (n != 2)
		{
			SolveTridiagonal(aa, bb, cc, dd).CopyTo(c, 1);
		}

		for (int i = n - 2; i >= 0; i--)
		{
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
		j = Math.Min(j, n - 2);

		double t = xx - x[j];
		double t2 = t * t;
		double t3 = t2 * t;
		double yy = a[j] + b[j] * t + c[j] * t2 + d[j] * t3;
		return yy;
	}

	private double[] SolveTridiagonal(double[] a, double[] b, double[] c, double[] d)
	{
		int n = a.Length;

		for (int i = 1; i < n; i++)
		{
			double w = a[i - 1] / b[i - 1];
			b[i] = b[i] - w * c[i - 1];
			d[i] = d[i] - w * d[i - 1];
		}

		var x = new double[n];
		x[n - 1] = d[n - 1] / b[n - 1];
		for (int i = n - 2; i >= 0; i--)
		{
			x[i] = (d[i] - c[i] * x[i + 1]) / b[i];
		}
		return x;
	}
}
