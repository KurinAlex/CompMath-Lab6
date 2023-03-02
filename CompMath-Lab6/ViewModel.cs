using System;
using System.Collections.Generic;
using System.Linq;

using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;

using CompMath_Lab6.Interpolations;

namespace CompMath_Lab6;

public class ViewModel
{
	private const int TestScale = 5;

	private static readonly Func<double, double> f = (double x) => Math.Log(3 * x + 1);

	private static readonly IInterpolation[] interpolations = {
		new LagrangeInterpolation(),
		new NewtonForwardInterpolation(),
		new NewtonBackwardInterpolation(),
		new SplineInterpolation(),
	};

	public ViewModel()
	{
		PlotModel = new PlotModel
		{
			Title = "e(x)"
		};

		var legend = new Legend
		{
			LegendBorder = OxyColors.Black,
			LegendPosition = LegendPosition.TopCenter
		};
		PlotModel.Legends.Add(legend);
	}

	public PlotModel PlotModel { get; init; }

	public (IEnumerable<double>, Dictionary<string, IEnumerable<double>>) GetData(double a, double b, int n)
	{
		double width = b - a;
		double step = width / (n - 1);
		double testStep = step / TestScale;
		int testN = (int)(width / testStep) + 1;

		var samples = new (double, double)[n];
		for (int i = 0; i < n; i++)
		{
			double x = a + i * step;
			samples[i] = (x, f(x));
		}

		var xData = new double[testN];
		for (int i = 0; i < testN; i++)
		{
			xData[i] = a + i * testStep;
		}

		var eData = new Dictionary<string, IEnumerable<double>>(interpolations.Length);
		foreach (var interpolation in interpolations)
		{
			var eDataArray = new double[testN];
			for (int i = 0; i < testN; i++)
			{
				double x = xData[i];
				double y = interpolation.Interpolate(samples, x);
				eDataArray[i] = Math.Abs(f(x) - y);
			}
			eData.Add(interpolation.Name, eDataArray);
		}

		return (xData, eData);
	}

	public void UpdateModel(double a, double b, int n)
	{
		PlotModel.Series.Clear();

		var (xData, eData) = GetData(a, b, n);
		foreach (var eArr in eData)
		{
			var series = new LineSeries
			{
				Title = eArr.Key,
				MarkerType = MarkerType.Diamond
			};
			foreach (var (x, e) in xData.Zip(eArr.Value))
			{
				series.Points.Add(new(x, e));
			}
			PlotModel.Series.Add(series);
		}
	}
}
