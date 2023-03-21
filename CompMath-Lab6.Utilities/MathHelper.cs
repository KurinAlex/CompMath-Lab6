namespace CompMath_Lab6.Utilities;

public static class MathHelper
{
	public static double[][] GetDividedDifference(IList<(double X, double Y)> samples)
	{
		int n = samples.Count;
		var divDiff = new double[n][];
		for (int i = 0; i < n; i++)
		{
			divDiff[i] = new double[n - i];
			divDiff[i][0] = samples[i].Y;
		}
		for (int j = 1; j < n; j++)
		{
			for (int i = 0; i < n - j; i++)
			{
				divDiff[i][j] = (divDiff[i + 1][j - 1] - divDiff[i][j - 1]) / (samples[i + j].X - samples[i].X);
			}
		}
		return divDiff;
	}
	public static double[] SolveTridiagonal(double[] a, double[] b, double[] c, double[] d)
	{
		int n = a.Length;

		for (int i = 1; i < n; i++)
		{
			double w = a[i - 1] / b[i - 1];
			b[i] = b[i] - w * c[i - 1];
			d[i] = d[i] - w * d[i - 1];
		}

		var x = new double[n];
		x[n - 1] = d[n - 1] / b[n - 1];
		for (int i = n - 2; i >= 0; i--)
		{
			x[i] = (d[i] - c[i] * x[i + 1]) / b[i];
		}
		return x;
	}
}
