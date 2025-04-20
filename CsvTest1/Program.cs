using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
///   <para>Program to parse a CSV file with headers.</para>
/// </summary>
internal class Program
{
	/// <summary>Defines the entry point of the application.</summary>
	private static void Main()
	{
		string csvData = "Name, Age, Location\n\"John, Doe\", 25, \"New York, NY\"\n\"Jane, Smith\", 30, \"Los Angeles, CA\"";
		using StringReader reader = new StringReader(csvData);
		var parsedCsv = ParseCsv(reader);
		foreach (var row in parsedCsv)
		{
			foreach (var kvp in row)
			{
				Console.Write($"{kvp.Key}: {kvp.Value}  ");
			}
			Console.WriteLine();
		}
	}

	/// <summary>Parses the CSV.</summary>
	/// <param name="reader">This is a StringReader that contains to the CSV content to be parsed.</param>
	/// <returns>This is the parsed content of the CSV file.</returns>
	private static List<Dictionary<string, string>> ParseCsv(StringReader reader)
	{
		var records = new List<Dictionary<string, string>>();
		var headers = new List<string>();
		string? line;
		bool isHeaderRow = true;
		while ((line = reader.ReadLine()) != null)
		{
			var values = ParseCsvLine(line);

			if (isHeaderRow)
			{
				headers.AddRange(values);
				isHeaderRow = false;
			}
			else
			{
				var record = new Dictionary<string, string>();
				for (int i = 0; i < headers.Count && i < values.Count; i++)
				{
					record[headers[i]] = values[i];
				}
				records.Add(record);
			}
		}
		return records;
	}

	/// <summary>Parses the CSV line.</summary>
	/// <param name="line">This is the line to be parsed (single row in a CSV file).</param>
	/// <returns>This is the parse content of a CSV line.</returns>
	private static List<string> ParseCsvLine(string line)
	{
		var values = new List<string>();
		var pattern = @"(?:\""(?<quoted>.*?)\"")|(?<unquoted>[^,]+)";
		var matches = Regex.Matches(line, pattern);
		foreach (Match match in matches)
		{
			values.Add(match.Groups["quoted"].Success ? match.Groups["quoted"].Value : match.Groups["unquoted"].Value);
		}
		return values;
	}
}
