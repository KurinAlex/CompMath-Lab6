using System;
using System.Collections.Generic;
using System.Linq;

using OxyPlot;
using OxyPlot.Legends;
using OxyPlot.Series;

using CompMath_Lab6.Interpolations;

namespace CompMath_Lab6;

using Series = IEnumerable<double>;
using SeriesDictionary = Dictionary<string, IEnumerable<double>>;

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
		YModel = InitModel("y(x)", LegendPosition.TopLeft);
		EModel = InitModel("e(x)", LegendPosition.TopCenter);
	}

	public PlotModel YModel { get; init; }
	public PlotModel EModel { get; init; }

	private PlotModel InitModel(string title, LegendPosition legendPosition)
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
	private void UpdateSeries(PlotModel model, Series xArr, SeriesDictionary yData)
	{
		model.Series.Clear();
		foreach (var yArr in yData)
		{
			var series = new LineSeries
			{
				Title = yArr.Key,
				MarkerType = MarkerType.Diamond
			};
			foreach (var (x, y) in xArr.Zip(yArr.Value))
			{
				series.Points.Add(new(x, y));
			}
			model.Series.Add(series);
		}
	}

	public (Series, SeriesDictionary, SeriesDictionary) GetData(double a, double b, int n)
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

		var yData = new SeriesDictionary(interpolations.Length)
		{
			["True"] = xData.Select(x => f(x))
		};
		var eData = new SeriesDictionary(interpolations.Length);
		foreach (var interpolation in interpolations)
		{
			var yDataArray = new double[testN];
			var eDataArray = new double[testN];
			for (int i = 0; i < testN; i++)
			{
				double x = xData[i];
				double y = interpolation.Interpolate(samples, x);
				yDataArray[i] = y;
				eDataArray[i] = Math.Abs(y - f(x));
			}
			yData.Add(interpolation.Name, yDataArray);
			eData.Add(interpolation.Name, eDataArray);
		}


		return (xData, yData, eData);
	}
	public void UpdateModels(double a, double b, int n)
	{
		var (x, y, e) = GetData(a, b, n);
		UpdateSeries(YModel, x, y);
		UpdateSeries(EModel, x, e);
	}
}
