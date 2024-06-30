using Microsoft.Xna.Framework.Input;

namespace MaimaiGame;

public static class KeyboardStateExtensions
{
	private static KeyboardState _lastKeyboardState = Keyboard.GetState();

	public static bool IsKeyPressed(this KeyboardState keyboardState, Keys key)
	{
		return keyboardState.IsKeyDown(key) && !_lastKeyboardState.IsKeyDown(key);
	}

	public static void Update()
	{
		_lastKeyboardState = Keyboard.GetState();
	}
}