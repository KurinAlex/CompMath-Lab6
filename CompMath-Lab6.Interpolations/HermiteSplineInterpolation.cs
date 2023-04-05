using CompMath_Lab6.Utilities;

namespace CompMath_Lab6.Interpolations;

public class HermiteSplineInterpolation : SplineInterpolation
{
	public HermiteSplineInterpolation(FunctionData samples) : base(samples)
	{
	}

	public override string Name => "Hermite Spline";

	protected override Polynomial[] GetPolynomials(FunctionData samples)
	{
		int n = samples.Count - 1;
		var x = samples.Select(s => s.X).ToArray();
		var y = samples.Select(s => s.Y).ToArray();
		var m = samples.Select(s => s.DY).ToArray();

		var h = new double[n];
		var divDiff = new double[n];
		for (int i = 0; i < n; i++)
		{
			h[i] = x[i + 1] - x[i];
		}

		var polynomials = new Polynomial[n];
		for (int i = 0; i < n; i++)
		{
			double h1 = h[i];
			double h2 = h1 * h1;
			double h3 = h2 * h[i];
			polynomials[i] = new(
				y[i],
				m[i],
				(3.0 * (y[i + 1] - y[i]) - h1 * (2.0 * m[i] + m[i + 1])) / h2,
				(2.0 * (y[i] - y[i + 1]) + h1 * (m[i] + m[i + 1])) / h3);
		}
		return polynomials;
	}
}
