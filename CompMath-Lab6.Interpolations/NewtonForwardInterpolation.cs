using CompMath_Lab6.Utilities;

namespace CompMath_Lab6.Interpolations;

public class NewtonForwardInterpolation : PolynomialInterpolation
{
	public NewtonForwardInterpolation((double X, double Y)[] samples) : base(samples)
	{
	}

	public override string Name => "Newton forward";

	protected override Polynomial GetPolynomial((double X, double Y)[] samples)
	{
		var divDiff = MathHelper.GetDividedDifference(samples)[0];
		var p = new Polynomial(divDiff[0]);
		var mul = Polynomial.One;
		for (int i = 1; i < samples.Length; i++)
		{
			mul *= new Polynomial(-samples[i - 1].X, 1.0);
			p += mul * divDiff[i];
		}
		return p;
	}
}
