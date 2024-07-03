namespace MaimaiGame.Charts;

public class Difficulty
{
	public string? ShortMessage { get; private set; }
	public string? Designer { get; private set; }
	public float? Offset { get; private set; }
	public string? Level { get; private set; }

	public readonly string Name;

	public Difficulty(DifficultyCategory category)
	{
		Name = category.ToString();
	}

	public bool Parse(string name, object value)
	{
		switch (name)
		{
			case "smsg":
				ShortMessage = (string) value;
				return true;
			case "des":
				Designer = (string) value;
				return true;
			case "first":
				if (value is float offset)
				{
					Offset = offset;
					return true;
				}

				string offsetStr = (string) value;

				if (!float.TryParse(offsetStr, out offset))
				{
					Logger.Error($"Invalid \"first\" value \"{offsetStr}\"");
					return false;
				}

				Offset = offset;
				return true;
			case "lv":
				Level = (string) value;
				return true;
		}

		Logger.Error($"Unknown token name \"{name}\"");
		return false;
	}

	public bool Load(string contents)
	{
		// TODO: Parse chart note data into note objects
		return true;
	}
}