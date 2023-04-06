using System.Diagnostics;

using CompMath_Lab6.Utilities;

namespace CompMath_Lab6._3D;

public class Program
{
	const int TestScale = 5;
	const string DownloadFile = @"D:\Sources\University\2 course\CompMath\CompMath-Lab6\Data\data3D.txt";

	static double F(double x, double y) => Math.Log10(3 * (x + y) + 1);
	static double Interpolate(double[] x, double[] y, double[][] z, double xx, double yy)
	{
		double zz = 0.0;
		int m = x.Length;
		int n = y.Length;

		for (int i = 0; i < m; i++)
		{
			for (int j = 0; j < n; j++)
			{
				double pr = z[i][j];

				for (int p = 0; p < m; p++)
				{
					if (p == i)
					{
						continue;
					}
					pr *= (xx - x[p]) / (x[i] - x[p]);
				}

				for (int q = 0; q < n; q++)
				{
					if (q == j)
					{
						continue;
					}
					pr *= (yy - y[q]) / (y[j] - y[q]);
				}

				zz += pr;
			}
		}
		return zz;
	}

	static void Main()
	{
		int n = Input.GetInput<int>("n", n => n > 0);
		double x0 = Input.GetInput<double>("x0");
		double x1 = Input.GetInput<double>("x1", x1 => x1 > x0);
		double y0 = Input.GetInput<double>("y0");
		double y1 = Input.GetInput<double>("y1", y1 => y1 > x0);

		double stepX = (x1 - x0) / (n - 1);
		double stepY = (y1 - y0) / (n - 1);

		int testN = (n - 1) * TestScale + 1;
		double testStepX = stepX / TestScale;
		double testStepY = stepY / TestScale;

		var samplesX = Extensions.Range(x0, stepX, n).ToArray();
		var samplesY = Extensions.Range(x0, stepX, n).ToArray();
		var samplesZ = samplesX.Select(x => samplesY.Select(y => F(x, y)).ToArray()).ToArray();

		var testX = Extensions.Range(x0, testStepX, testN);
		var testY = Extensions.Range(x0, testStepY, testN);
		var exactZ = testX.Select(x => testY.Select(y => F(x, y))).Concat();

		var res = testX.Select(x => testY.Select(y => Interpolate(samplesX, samplesY, samplesZ, x, y))).Concat();

		var points = new Dictionary<string, IEnumerable<double>>()
		{
			["x"] = testX.Select(x => Enumerable.Repeat(x, testN)).Concat(),
			["y"] = Enumerable.Repeat(testY, testN).Concat()
		};
		var results = new Dictionary<string, IEnumerable<double>>()
		{
			["Results"] = res,
			["Exact"] = exactZ
		};

		var errors = new Dictionary<string, IEnumerable<double>>()
		{
			["Errors"] = exactZ.Zip(res).Select(t => Math.Abs(t.First - t.Second))
		};

		var text = string.Join(
			Environment.NewLine,
			Drawer.GetTableString(points, results, "z(x,y)", 6),
			Drawer.GetTableString(points, errors, "e(x,y)", 6));

		Console.Write(text);
		File.WriteAllText(DownloadFile, text);
		Process.Start("notepad.exe", DownloadFile);

		Console.ReadLine();
	}
}