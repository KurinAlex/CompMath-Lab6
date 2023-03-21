using CompMath_Lab6.Utilities;

namespace CompMath_Lab6.Interpolations;

public class NewtonForwardInterpolation : PolynomialInterpolation
{
	public NewtonForwardInterpolation(FunctionData samples) : base(samples)
	{
	}

	public override string Name => "Newton forward";

	protected override Polynomial GetPolynomial(FunctionData samples)
	{
		var divDiff = MathHelper.GetDividedDifference(samples.Select(s => (s.X, s.Y)).ToArray())[0];
		var p = new Polynomial(divDiff[0]);
		var mul = Polynomial.One;
		for (int i = 1; i < samples.Count; i++)
		{
			mul *= new Polynomial(-samples[i - 1].X, 1.0);
			p += mul * divDiff[i];
		}
		return p;
	}
}
