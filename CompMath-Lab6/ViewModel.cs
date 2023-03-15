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

	private static readonly Func<double, double> _f = (double x) => Math.Log(3 * x + 1);

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
			.Select(x => (x, _f(x)))
			.ToArray();

		int newN = (n - 1) * _testScale + 1;
		IInterpolation[] interpolations = {
			new LagrangeInterpolation(samples),
			new NewtonForwardInterpolation(samples),
			new NewtonBackwardInterpolation(samples),
			new NaturalSplineInterpolation(samples),
			new HermiteSplineInterpolation(samples)
		};

		UpdateSeries(YModel, interpolations, (i, x) => i.Interpolate(x), a, b, newN);
		YModel.Series.Add(new FunctionSeries(_f, a, b, newN, "True"));

		UpdateSeries(EModel, interpolations, (i, x) => Math.Abs(_f(x) - i.Interpolate(x)), a, b, newN);
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
		return(xData, yData, eData);
	}
}
