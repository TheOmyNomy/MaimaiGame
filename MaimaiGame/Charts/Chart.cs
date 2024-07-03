using System;
using System.Collections.Generic;
using System.IO;

namespace MaimaiGame.Charts;

public class Chart
{
	public string? Title { get; private set; }
	public string? Artist { get; private set; }

	private readonly Dictionary<DifficultyCategory, Difficulty> _difficulties =
		new Dictionary<DifficultyCategory, Difficulty>();

	public IReadOnlyDictionary<DifficultyCategory, Difficulty> Difficulties => _difficulties;

	public Difficulty? Easy => _difficulties.GetValueOrDefault(DifficultyCategory.Easy);
	public Difficulty? Basic => _difficulties.GetValueOrDefault(DifficultyCategory.Basic);
	public Difficulty? Advanced => _difficulties.GetValueOrDefault(DifficultyCategory.Advanced);
	public Difficulty? Expert => _difficulties.GetValueOrDefault(DifficultyCategory.Expert);
	public Difficulty? Master => _difficulties.GetValueOrDefault(DifficultyCategory.Master);
	public Difficulty? ReMaster => _difficulties.GetValueOrDefault(DifficultyCategory.ReMaster);
	public Difficulty? Original => _difficulties.GetValueOrDefault(DifficultyCategory.Original);

	public readonly string Path;

	private Chart(string path)
	{
		Path = path;
	}

	public static Chart? Load(string path)
	{
		if (!Directory.Exists(path))
		{
			Logger.Error($"\"{path}\" does not exist or is not a folder");
			return null;
		}

		if (!path.EndsWith('/'))
			path += '/';

		Chart chart = new Chart(path);

		string maimaiPath = chart.Path + "maidata.txt";

		string contents = File.ReadAllText(maimaiPath)
			.Replace("\r", string.Empty)
			.Replace("\n", string.Empty);

		string[] tokens = contents.Split('&');

		string? globalDesigner = null;
		float? globalOffset = null;

		Dictionary<Difficulty, string> difficultyNoteData = new Dictionary<Difficulty, string>();

		foreach (string token in tokens)
		{
			if (string.IsNullOrWhiteSpace(token))
				continue;

			string[] split = token.Split('=');

			if (split.Length < 2)
			{
				Logger.Error($"Invalid token \"{token}\"");
				return null;
			}

			string name = split[0].ToLower();

			if (string.IsNullOrWhiteSpace(name))
			{
				Logger.Error($"Invalid token \"{token}\"");
				return null;
			}

			string value = string.Join('=', split[1..])
				.Replace("%26", "&")
				.Replace("%3D", "=")
				.Replace("%2B", "+")
				.Replace("%5C", "\\")
				.Replace("%25", "%");

			if (string.IsNullOrWhiteSpace(value))
			{
				Logger.Error($"Invalid token \"{token}\"");
				return null;
			}

			switch (name)
			{
				case "title":
					chart.Title = value;
					continue;
				case "artist":
					chart.Artist = value;
					continue;
				case "des":
					globalDesigner = value;
					continue;
				case "first":
					if (!float.TryParse(value, out float offset))
					{
						Logger.Error($"Invalid \"first\" value \"{value}\"");
						return null;
					}

					globalOffset = offset;
					continue;
			}

			split = name.Split('_');

			if (split.Length < 2)
			{
				Logger.Error($"Invalid token name \"{name}\"");
				return null;
			}

			name = split[0];
			string difficultyIdStr = split[1];

			if (!Enum.TryParse(difficultyIdStr, out DifficultyCategory difficultyCategory) ||
			    difficultyCategory < DifficultyCategory.Easy || difficultyCategory > DifficultyCategory.Original)
			{
				Logger.Error($"Invalid difficulty value \"{difficultyIdStr}\"");
				return null;
			}

			if (!chart._difficulties.TryGetValue(difficultyCategory, out Difficulty? difficulty))
			{
				difficulty = new Difficulty(difficultyCategory);
				chart._difficulties.Add(difficultyCategory, difficulty);
			}

			if (name == "inote")
			{
				difficultyNoteData.Add(difficulty, value);
				continue;
			}

			bool result = difficulty.Parse(name, value);

			if (!result)
			{
				Logger.Error($"Failed to parse difficulty token name; Name: \"{name}\", Value: \"{value}\"");
				return null;
			}
		}

		foreach (Difficulty difficulty in chart._difficulties.Values)
		{
			if (!string.IsNullOrWhiteSpace(globalDesigner) && string.IsNullOrWhiteSpace(difficulty.Designer))
				difficulty.Parse("des", globalDesigner);
			else if (globalOffset.HasValue && !difficulty.Offset.HasValue)
				difficulty.Parse("first", globalOffset);
		}

		// We load the chart note data as to ensure all other metadata is set (i.e. first must be set... first...).
		foreach (var item in difficultyNoteData)
		{
			bool result = item.Key.Load(item.Value);

			if (!result)
			{
				Logger.Error($"Failed to load note data for difficulty \"{item.Key.Name}\"");
				return null;
			}
		}

		return chart;
	}
}

public enum DifficultyCategory
{
	Easy = 1,
	Basic = 2,
	Advanced = 3,
	Expert = 4,
	Master = 5,
	ReMaster = 6,
	Original = 7
}