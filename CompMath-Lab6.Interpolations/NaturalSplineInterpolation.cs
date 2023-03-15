using CompMath_Lab6.Utilities;

namespace CompMath_Lab6.Interpolations;

public class NaturalSplineInterpolation : SplineInterpolation
{
	public NaturalSplineInterpolation((double X, double Y)[] samples) : base(samples)
	{
	}

	public override string Name => "Natural Spline";

	protected override Polynomial[] GetPolynomials((double X, double Y)[] samples)
	{
		int n = samples.Length - 1;
		var x = samples.Select(s => s.X).ToArray();
		var y = samples.Select(s => s.Y).ToArray();

		// polynomials coefficients
		var a = new double[n];
		var b = new double[n];
		var c = new double[n + 1];
		var d = new double[n];

		var h = new double[n];
		for (int i = 0; i < n; i++)
		{
			h[i] = x[i + 1] - x[i];
		}

		// triagonalization matrix elements
		var aa = new double[n - 1];
		var bb = new double[n - 1];
		var cc = new double[n - 1];
		var dd = new double[n - 1];
		for (int i = 0; i < n - 1; i++)
		{
			aa[i] = h[i];
			bb[i] = 2.0 * (h[i] + h[i + 1]);
			cc[i] = h[i + 1];
			dd[i] = 3.0 * ((y[i + 2] - y[i + 1]) / h[i + 1] - (y[i + 1] - y[i]) / h[i]);
		}

		if (n != 1)
		{
			MathHelper.SolveTridiagonal(aa, bb, cc, dd).CopyTo(c, 1);
		}

		for (int i = n - 1; i >= 0; i--)
		{
			b[i] = (y[i + 1] - y[i]) / h[i] - h[i] * (c[i + 1] + 2.0 * c[i]) / 3.0;
			d[i] = (c[i + 1] - c[i]) / (3.0 * h[i]);
			a[i] = y[i];
		}

		return Enumerable.Range(0, n)
			.Select(i => new Polynomial(a[i], b[i], c[i], d[i]))
			.ToArray();
	}
}
