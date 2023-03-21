namespace CompMath_Lab6.Interpolations;

public class HermiteSplineFinDiffInterpolation : HermiteSplineInterpolation
{
	public HermiteSplineFinDiffInterpolation(FunctionData samples) : base(samples)
	{
	}

	public override string Name => "Hermite Spline (finite differences)";

	protected override double[] GetDerivatives(FunctionData samples)
	{
		int n = samples.Count - 1;
		var finDiff = new double[n];
		for (int i = 0; i < n; i++)
		{
			finDiff[i] = (samples[i + 1].Y - samples[i].Y) / (samples[i + 1].X - samples[i].X);
		}

		var m = new double[n + 1];
		m[0] = finDiff[0] / 2.0;
		m[n] = finDiff[n - 1] / 2.0;
		for (int i = 0; i < n - 1; i++)
		{
			m[i] = (finDiff[i] + finDiff[i + 1]) / 2.0;
		}
		return m;
	}
}
