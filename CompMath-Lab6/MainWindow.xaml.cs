using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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

		plot.Model = _viewModel.PlotModel;
	}

	private void UpdateModel(object sender, RoutedEventArgs e)
	{
		if (nInput == null || (_n = (int)nInput.Value) < 2)
		{
			return;
		}

		if (aInput == null || !double.TryParse(aInput.Text, out _a))
		{
			return;
		}

		if (bInput == null || !double.TryParse(bInput.Text, out _b) || _b <= _a)
		{
			return;
		}

		_viewModel.UpdateModel(_a, _b, _n);
		plot.InvalidatePlot();
	}

	private void DownloadData(object sender, RoutedEventArgs e)
	{
		var (xData, eData) = _viewModel.GetData(_a, _b, _n);
		string path = Path.Combine(DownloadFolder, "data.txt");
		string text = string.Join(
			Environment.NewLine,
			$"a = {_a}",
			$"b = {_b}",
			$"n = {_n}",
			Drawer.GetTableString(xData, eData, "e(x)", 6));
		File.WriteAllText(path, text);
	}

	private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (sender != null && sender is TextBox textBox && int.TryParse(textBox.Text, out int tmp))
		{
			nInput.Value = tmp;
		}
	}
}
