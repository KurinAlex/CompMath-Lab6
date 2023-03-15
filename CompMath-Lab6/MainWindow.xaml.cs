using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using OxyPlot.Series;

using CompMath_Lab6.Utilities;

namespace CompMath_Lab6;

public partial class MainWindow : Window
{
	private const string DownloadFolder = @"D:\Sources\University\2 course\CompMath\CompMath-Lab6\Data";

	private int _n;
	private double _a;
	private double _b;

	private readonly ViewModel _viewModel = new();

	public MainWindow()
	{
		InitializeComponent();

		yPlot.Model = _viewModel.YModel;
		ePlot.Model = _viewModel.EModel;
	}

	private void UpdateModels(object sender, RoutedEventArgs e)
	{
		if (nInput is null || (_n = (int)nInput.Value) < 2)
		{
			return;
		}

		if (aInput is null || !double.TryParse(aInput.Text, out _a))
		{
			return;
		}

		if (bInput is null || !double.TryParse(bInput.Text, out _b) || _b <= _a)
		{
			return;
		}

		_viewModel.UpdateModels(_a, _b, _n);

		yPlot.InvalidatePlot();

		ePlot.InvalidatePlot();
		ePlot.ResetAllAxes();
	}
	private void DownloadData(object sender, RoutedEventArgs e)
	{
		var (xData, yData, eData) = _viewModel.GetData();

		string path = Path.Combine(DownloadFolder, "data.txt");
		string text = string.Join(
			Environment.NewLine,
			$"a = {_a}",
			$"b = {_b}",
			$"n = {_n}",
			Drawer.GetTableString(xData, yData, "y(x)", 6),
			Drawer.GetTableString(xData, eData, "e(x)", 6));

		File.WriteAllText(path, text);
		Process.Start("notepad.exe", path);
	}
	private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
	{
		if (sender is TextBox textBox && int.TryParse(textBox.Text, out int tmp))
		{
			nInput.Value = tmp;
		}
	}
}
