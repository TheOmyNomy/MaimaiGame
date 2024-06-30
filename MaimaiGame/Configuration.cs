using System.IO;
using System.IO.Ports;
using Tomlyn;

namespace MaimaiGame;

public class Configuration
{
	public ConfigurationAudio Audio { get; private set; } = new ConfigurationAudio
	{
		Offset = 0.1f,
		Volume = 0.25f
	};

	public ConfigurationInput Input { get; private set; } = new ConfigurationInput
	{
		TouchScreen = new ConfigurationInputTouchScreen
		{
			BaudRate = 115200,
			DataBits = 8,
			Parity = Parity.None,
			PortName = "COM3",
			StopBits = StopBits.One
		}
	};

	public static Configuration Instance { get; private set; } = null!;

	private static bool _initialised;

	public static void Initialise()
	{
		if (_initialised)
			return;

		_initialised = true;

		const string path = "settings.toml";

		if (File.Exists(path))
		{
			string contents = File.ReadAllText(path);
			Instance = Toml.ToModel<Configuration>(contents);
		}
		else
		{
			Instance = new Configuration();

			string contents = Toml.FromModel(Instance);
			File.WriteAllText(path, contents);
		}
	}
}

public class ConfigurationAudio
{
	public float Offset { get; init; }
	public float Volume { get; init; }
}

public class ConfigurationInput
{
	public ConfigurationInputTouchScreen TouchScreen { get; init; } = null!;
}

public class ConfigurationInputTouchScreen
{
	public int BaudRate { get; init; }
	public int DataBits { get; init; }
	public Parity Parity { get; init; }
	public string PortName { get; init; } = null!;
	public StopBits StopBits { get; init; }
}