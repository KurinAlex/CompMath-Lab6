namespace CompMath_Lab6.Interpolations;

public class NewtonBackwardInterpolation : NewtonInterpolation
{
	public override string Name => "Newton backward";

	protected override double Interpolate(double[][] divDiff, double[] samplesX, double x)
	{
		int n = samplesX.Length;
		double p = divDiff[n - 1][0];
		var mul = 1.0;
		for (int i = n - 1; i > 0; i--)
		{
			mul *= x - samplesX[i];
			p += mul * divDiff[i - 1][n - i];
		}
		return p;
	}
}
