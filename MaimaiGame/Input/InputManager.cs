namespace MaimaiGame.Input;

public static class InputManager
{
	public static TouchScreen TouchScreen { get; private set; } = null!;

	private static bool _initialised;

	public static void Initialise()
	{
		if (_initialised)
			return;

		_initialised = true;

		ConfigurationInputTouchScreen touchScreen = Configuration.Instance.Input.TouchScreen;

		TouchScreen = new TouchScreen(
			touchScreen.PortName,
			touchScreen.BaudRate,
			touchScreen.Parity,
			touchScreen.DataBits,
			touchScreen.StopBits
		);
	}
}