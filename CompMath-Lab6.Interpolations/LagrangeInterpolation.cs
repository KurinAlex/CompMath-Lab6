using CompMath_Lab6.Utilities;

namespace CompMath_Lab6.Interpolations;

public class LagrangeInterpolation : PolynomialInterpolation
{
	public LagrangeInterpolation(FunctionData samples) : base(samples)
	{
	}

	public override string Name => "Lagrange";

	protected override Polynomial GetPolynomial(FunctionData samples)
	{
		var p = Polynomial.Zero;
		int n = samples.Count;

		for (int i = 0; i < n; i++)
		{
			var l = Polynomial.One;
			double xi = samples[i].X;

			for (int j = 0; j < n; j++)
			{
				if (i == j)
				{
					continue;
				}

				double xj = samples[j].X;
				l *= new Polynomial(-xj, 1.0) / (xi - xj);
			}

			p += l * samples[i].Y;
		}
		return p;
	}
}
