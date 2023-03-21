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
	private const int _testScale = 5;

	private static readonly Func<double, double> s_f = (double x) => Math.Log10(3.0 * x + 1.0);
	private static readonly Func<double, double> s_df = (double x) => 3.0 / (3.0 * x + 1.0) / Math.Log(10.0);

	public ViewModel()
	{
		YModel = InitModel("y(x)", LegendPosition.TopLeft);
		EModel = InitModel("e(x)", LegendPosition.TopCenter);
	}

	public PlotModel YModel { get; init; }
	public PlotModel EModel { get; init; }

	private static PlotModel InitModel(string title, LegendPosition legendPosition)
	{
		var model = new PlotModel
		{
			Title = title
		};

		var legend = new Legend
		{
			LegendBorder = OxyColors.Black,
			LegendPosition = legendPosition
		};
		model.Legends.Add(legend);

		return model;
	}
	private static void UpdateSeries(
		PlotModel model,
		IEnumerable<IInterpolation> interpolations,
		Func<IInterpolation, double, double> f,
		double a,
		double b,
		int n)
	{
		model.Series.Clear();
		foreach (var interpolation in interpolations)
		{
			var series = new FunctionSeries(x => f(interpolation, x), a, b, n, interpolation.Name)
			{
				MarkerType = MarkerType.Diamond
			};
			model.Series.Add(series);
		}
	}

	public void UpdateModels(double a, double b, int n)
	{
		double step = (b - a) / (n - 1);
		var samples = Enumerable.Range(0, n)
			.Select(i => a + i * step)
			.Select(x => new FunctionDataSample(x, s_f(x), s_df(x)))
			.ToArray();

		IInterpolation[] interpolations = {
			new LagrangeInterpolation(samples),
			new NewtonForwardInterpolation(samples),
			new NewtonBackwardInterpolation(samples),
			new NaturalSplineInterpolation(samples),
			new HermiteSplineInterpolation(samples),
			new HermiteSplineFinDiffInterpolation(samples)
		};

		int newN = (n - 1) * _testScale + 1;

		UpdateSeries(YModel, interpolations, (i, x) => i.Interpolate(x), a, b, newN);
		YModel.Series.Add(new FunctionSeries(s_f, a, b, newN, "Exact"));

		UpdateSeries(EModel, interpolations, (i, x) => Math.Abs(s_f(x) - i.Interpolate(x)), a, b, newN);
	}
	public (IEnumerable<double>, Dictionary<string, IEnumerable<double>>, Dictionary<string, IEnumerable<double>>)
		GetData()
	{
		var xData = ((DataPointSeries)YModel.Series[0]).Points.Select(p => p.X);
		var yData = YModel.Series
			.ToDictionary(
				s => s.Title,
				s => ((DataPointSeries)s).Points.Select(p => p.Y));
		var eData = EModel.Series
			.ToDictionary(
				s => s.Title,
				s => ((DataPointSeries)s).Points.Select(p => p.Y));
		return (xData, yData, eData);
	}
}
