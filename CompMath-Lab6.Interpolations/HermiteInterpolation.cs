using CompMath_Lab6.Utilities;

namespace CompMath_Lab6.Interpolations;

public class HermiteInterpolation : PolynomialInterpolation
{
	private const int DerivativeCount = 2;

	public HermiteInterpolation(FunctionData samples) : base(samples)
	{
	}

	public override string Name => "Hermite";

	protected override Polynomial GetPolynomial(FunctionData samples)
	{
		int n = samples.Count * (DerivativeCount + 1);
		var matrix = samples
			.Select(s =>
				new[]
				{
					Enumerable.Range(0, n).Select(i => Math.Pow(s.X, i)).Append(s.Y).ToArray(),
					Enumerable.Range(-1, n).Select(i => (i + 1) * Math.Pow(s.X, i)).Append(s.DY).ToArray(),
					Enumerable.Range(-2, n).Select(i => (i + 2) * (i + 1) * Math.Pow(s.X, i)).Append(s.DDY).ToArray()
				})
			.Concat()
			.ToArray();
		var coef = MathHelper.Solve(matrix);
		return new(coef);
	}
}
