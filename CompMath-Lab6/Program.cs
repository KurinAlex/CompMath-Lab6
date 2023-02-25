namespace CompMath_Lab6;

class Program
{
	const int N = 5;
	const double A = 1.0;
	const double B = 6.0;
	const int TestScale = 5;
	const int Precision = 8;

	const double Width = B - A;
	const double Step = Width / N;
	const double TestStep = Step / TestScale;
	const int Count = N + 1;
	const int TestCount = (int)(Width / TestStep) + 1;

	static readonly Func<double, double> f = (double x) => Math.Log(3 * x + 1);

	static readonly IInterpolation[] interpolations = {
		new LagrangeInterpolation(),
		new NewtonForwardInterpolation(),
		new NewtonBackwardInterpolation(),
		new SplineInterpolation(),
	};

	static void Main()
	{
		var samples = new (double, double)[Count];
		for (int i = 0; i < Count; i++)
		{
			double x = A + i * Step;
			samples[i] = (x, f(x));
		}

		var xData = new double[TestCount];
		for (int i = 0; i < TestCount; i++)
		{
			xData[i] = A + i * TestStep;
		}

		var eData = new Dictionary<string, double[]>(interpolations.Length);
		foreach (var interpolation in interpolations)
		{
			var eDataArray = new double[TestCount];
			for (int i = 0; i < TestCount; i++)
			{
				double x = xData[i];
				double y = interpolation.Interpolate(samples, x);
				eDataArray[i] = Math.Abs(f(x) - y);
			}
			eData.Add(interpolation.Name, eDataArray);
		}

		Drawer.DrawTable(xData, eData, "e(x)", Precision);
		Console.ReadLine();
	}
}