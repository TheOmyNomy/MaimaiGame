using MaimaiGame.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MaimaiGame.Scenes;

public class PlayScene : Scene
{
	private KeyboardState _lastKeyboardState;

	private Music _music = null!;

	public override void OnCreate(ContentManager contentManager)
	{
		_music = Music.Load("Resources/music.mp3");
		_music.Play();

		_lastKeyboardState = Keyboard.GetState();
	}

	public override void OnDestroy()
	{
		_music.Dispose();
	}

	public override void OnUpdate(GameTime gameTime)
	{
		KeyboardState keyboardState = Keyboard.GetState();

		if (keyboardState.IsKeyDown(Keys.P) && !_lastKeyboardState.IsKeyDown(Keys.P))
		{
			if (_music.IsPlaying)
				_music.Pause();
			else
				_music.Play();
		}

		if (keyboardState.IsKeyDown(Keys.R) && !_lastKeyboardState.IsKeyDown(Keys.R))
			_music.Play(true);

		_lastKeyboardState = keyboardState;
	}

	public override void OnRender(SpriteBatch spriteBatch, GameTime gameTime)
	{
		spriteBatch.GraphicsDevice.Clear(Color.Black);

		spriteBatch.Begin();

		spriteBatch.DrawString(
			FontManager.Tahoma12,
			$"CurrentTime: {_music.CurrentTime:N4} ({(int) (_music.CurrentTime / 60.0f):D2}:{(int) (_music.CurrentTime % 60):D2})",
			new Vector2(2.0f, 2.0f),
			Color.White
		);

		spriteBatch.End();
	}
}