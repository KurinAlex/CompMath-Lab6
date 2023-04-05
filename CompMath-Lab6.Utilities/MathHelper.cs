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
	public static double[] Solve(double[][] matrix)
	{
		int n = matrix[0].Length;
		int m = n - 1;

		for (int k = 0; k < m; k++)
		{
			int kMax = matrix
				.Skip(k)
				.Select((row, i) => (row[k], i))
				.MaxBy(t => Math.Abs(t.Item1))
				.Item2 + k;

			if (matrix[kMax][k] == 0.0)
			{
				throw new ArgumentException("Matrix is singular");
			}

			if (k != kMax)
			{
				(matrix[k], matrix[kMax]) = (matrix[kMax], matrix[k]);
			}

			double f = matrix[k][k];
			for (int j = k; j < n; j++)
			{
				matrix[k][j] /= f;
			}

			for (int i = 0; i < m; i++)
			{
				if (i == k)
				{
					continue;
				}

				f = matrix[i][k] / matrix[k][k];
				for (int j = k; j < n; j++)
				{
					matrix[i][j] -= matrix[k][j] * f;
				}
			}
		}
		return matrix.Select(row => row[m]).ToArray();
	}
}
