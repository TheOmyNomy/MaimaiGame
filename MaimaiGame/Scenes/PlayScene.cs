using MaimaiGame.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MaimaiGame.Scenes;

public class PlayScene : Scene
{
	private Music _music = null!;

	public override void OnCreate(ContentManager contentManager)
	{
		_music = Music.Load("Resources/music.mp3");
		_music.Play();
	}

	public override void OnLeave()
	{
		_music.Pause();
	}

	public override void OnDestroy()
	{
		_music.Dispose();
	}

	public override void OnUpdate(GameTime gameTime)
	{
		if (Keyboard.GetState().IsKeyPressed(Keys.P))
		{
			if (_music.IsPlaying)
				_music.Pause();
			else
				_music.Play();
		}

		if (Keyboard.GetState().IsKeyPressed(Keys.R))
			_music.Play(true);
	}

	public override void OnRender(SpriteBatch spriteBatch, GameTime gameTime)
	{
		spriteBatch.GraphicsDevice.Clear(Color.Black);

		spriteBatch.Begin();

		/* const float topScreenScale = 0.235f;
		spriteBatch.DrawRectangle(
			0,
			0,
			MaimaiGame.Instance.DisplayWidth,
			topScreenScale * MaimaiGame.Instance.DisplayHeight,
			Color.White,
			5
		);

		spriteBatch.DrawRectangle(
			0,
			MaimaiGame.Instance.DisplayHeight - MaimaiGame.Instance.DisplayWidth,
			MaimaiGame.Instance.DisplayWidth,
			MaimaiGame.Instance.DisplayWidth,
			Color.White,
			5
		);

		const float radiusOffset = 50.0f;
		spriteBatch.DrawCircle(
			MaimaiGame.Instance.DisplayWidth / 2.0f,
			MaimaiGame.Instance.DisplayHeight - MaimaiGame.Instance.DisplayWidth / 2.0f,
			MaimaiGame.Instance.DisplayWidth / 2.0f - radiusOffset,
			50,
			Color.White,
			5
		);

		float fps = 1.0f / (float) gameTime.ElapsedGameTime.TotalSeconds;
		spriteBatch.DrawString(FontManager.Tahoma12, $"{fps:N0}fps", new Vector2(7.0f, 4.0f), Color.White); */

		spriteBatch.DrawString(
			FontManager.Tahoma12,
			$"CurrentTime: {_music.CurrentTime:N4} ({(int) (_music.CurrentTime / 60.0f):D2}:{(int) (_music.CurrentTime % 60):D2})",
			new Vector2(2.0f, 2.0f),
			Color.White
		);

		spriteBatch.End();
	}
}