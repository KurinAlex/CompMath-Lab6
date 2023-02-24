namespace CompMath_Lab6
{
	public class NewtonForwardInterpolation : NewtonInterpolation
	{
		public override string Name => "Newton forward";

		protected override double Interpolate(double[][] divDiff, double[] samplesX, double x)
		{
			double p = divDiff[0][0];
			double mul = 1.0;
			for (int i = 1; i < samplesX.Length; i++)
			{
				mul *= x - samplesX[i - 1];
				p += mul * divDiff[0][i];
			}
			return p;
		}
	}
}
