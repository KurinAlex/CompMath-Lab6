using CompMath_Lab6.Utilities;

namespace CompMath_Lab6.Interpolations;

public class NewtonBackwardInterpolation : PolynomialInterpolation
{
	public NewtonBackwardInterpolation(FunctionData samples) : base(samples)
	{
	}

	public override string Name => "Newton backward";

	protected override Polynomial GetPolynomial(FunctionData samples)
	{
		int n = samples.Count;
		var divDiff = MathHelper.GetDividedDifference(samples.Select(s => (s.X, s.Y)).ToArray());
		var p = new Polynomial(divDiff[n - 1][0]);
		var mul = Polynomial.One;
		for (int i = n - 1; i > 0; i--)
		{
			mul *= new Polynomial(-samples[i].X, 1.0);
			p += mul * divDiff[i - 1][n - i];
		}
		return p;
	}
}
