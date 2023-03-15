using CompMath_Lab6.Utilities;

namespace CompMath_Lab6.Interpolations;

public class HermiteSplineInterpolation : SplineInterpolation
{
	public HermiteSplineInterpolation((double X, double Y)[] samples) : base(samples)
	{
	}

	public override string Name => "Hermite Spline";

	protected override Polynomial[] GetPolynomials((double X, double Y)[] samples)
	{
		int n = samples.Length - 1;
		var x = samples.Select(s => s.X).ToArray();
		var y = samples.Select(s => s.Y).ToArray();

		var h = new double[n];
		var divDiff = new double[n];
		for (int i = 0; i < n; i++)
		{
			h[i] = x[i + 1] - x[i];
			divDiff[i] = (y[i + 1] - y[i]) / h[i];
		}

		var m = new double[n + 1];
		m[0] = divDiff[0] / 2.0;
		m[n] = divDiff[n - 1] / 2.0;
		for (int i = 0; i < n - 1; i++)
		{
			m[i] = (divDiff[i] + divDiff[i + 1]) / 2.0;
		}

		var polynomials = new Polynomial[n];
		for (int i = 0; i < n; i++)
		{
			double h2 = h[i] * h[i];
			double h3 = h2 * h[i];
			polynomials[i] = new Polynomial(
				y[i],
				m[i] / h[i],
				(-3.0 * y[i] + 3.0 * y[i + 1] - 2.0 * m[i] - m[i + 1]) / h2,
				(2.0 * y[i] - 2.0 * y[i + 1] + m[i] + m[i + 1]) / h3);
		}
		return polynomials;
	}
}
