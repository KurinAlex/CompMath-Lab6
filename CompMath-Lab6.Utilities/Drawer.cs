using System.Text;

namespace CompMath_Lab6.Utilities;

public static class Drawer
{
	private static string Center(this string s, int length)
		=> s.PadLeft((length - s.Length) / 2 + s.Length).PadRight(length);

	public static string GetTableString(
		IEnumerable<double> keys,
		IDictionary<string, IEnumerable<double>> values,
		string title,
		int precision)
		=> GetTableString(new Dictionary<string, IEnumerable<double>>() { ["x"] = keys }, values, title, precision);

	public static string GetTableString(
		IDictionary<string, IEnumerable<double>> keys,
		IDictionary<string, IEnumerable<double>> values,
		string title,
		int precision)
	{
		if (precision < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(precision), "Precision must be non-negative");
		}

		int n = keys.First().Value.Count();
		if (keys.Values.Any(d => d.Count() != n || values.Values.Any(d => d.Count() != n)))
		{
			throw new ArgumentException("Wrong entries count in y data array", nameof(values));
		}

		// id column data
		string idLabel = "id";
		var idStrings = Enumerable.Range(1, n).Select(d => d.ToString());
		int idColumnWidth = idStrings.Append(idLabel).Max(x => x.Length);
		var id = idStrings.Select(x => x.Center(idColumnWidth)).ToArray();

		// key columns data
		var keysStrings = keys.ToDictionary(p => p.Key, p => p.Value.Select(y => y.ToString($"E{precision}")));
		var keysColumnsWidth = keysStrings.Select(col => col.Value.Append(col.Key).Max(y => y.Length));
		int keysAreaWidth = keysColumnsWidth.Sum() + keys.Count - 1;
		var k = keysStrings
			.Zip(keysColumnsWidth)
			.ToDictionary(
				t => t.First.Key.Center(t.Second),
				t => t.First.Value.Select(y => y.Center(t.Second)).ToArray());

		// value columns data
		var valuesStrings = values.ToDictionary(p => p.Key, p => p.Value.Select(y => y.ToString($"E{precision}")));
		var valuesColumnsWidth = valuesStrings.Select(col => col.Value.Append(col.Key).Max(y => y.Length));
		int valuesAreaWidth = valuesColumnsWidth.Sum() + values.Count - 1;
		var v = valuesStrings
			.Zip(valuesColumnsWidth)
			.ToDictionary(
				t => t.First.Key.Center(t.Second),
				t => t.First.Value.Select(y => y.Center(t.Second)).ToArray());

		// dividers and spaces
		string idRowDivider = new('─', idColumnWidth);
		string idRowSpace = new(' ', idColumnWidth);
		var keysRowsSpaces = keysColumnsWidth.Select(w => new string(' ', w));
		var keysRowsDividers = keysColumnsWidth.Select(w => new string('─', w));
		string valuesAreaDivider = new('─', valuesAreaWidth);
		var valuesRowsDividers = valuesColumnsWidth.Select(w => new string('─', w));

		// center the labels
		idLabel = idLabel.Center(idColumnWidth);
		title = title.Center(valuesAreaWidth);

		var sb = new StringBuilder();
		sb.AppendFormat("┌{0}┬{1}┬{2}┐", idRowDivider, string.Join('┬', keysRowsDividers), valuesAreaDivider).AppendLine();
		sb.AppendFormat("│{0}│{1}│{2}│", idRowSpace, string.Join('│', keysRowsSpaces), title).AppendLine();
		sb.AppendFormat("│{0}│{1}├{2}┤", idLabel, string.Join('│', k.Keys), string.Join('┬', valuesRowsDividers)).AppendLine();
		sb.AppendFormat("│{0}│{1}│{2}│", idRowSpace, string.Join('│', keysRowsSpaces), string.Join('│', v.Keys)).AppendLine();
		for (int i = 0; i < n; i++)
		{
			sb.AppendFormat("├{0}┼{1}┼{2}┤", idRowDivider, string.Join('┼', keysRowsDividers), string.Join('┼', valuesRowsDividers)).AppendLine();
			sb.AppendFormat("│{0}│{1}│{2}│", id[i], string.Join('│', k.Values.Select(key => key[i])), string.Join('│', v.Values.Select(val => val[i]))).AppendLine();
		}
		sb.AppendFormat("└{0}┴{1}┴{2}┘", idRowDivider, string.Join('┴', keysRowsDividers), string.Join('┴', valuesRowsDividers));

		return sb.ToString();
	}
}
