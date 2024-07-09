using MaimaiGame.Audio;
using MaimaiGame.Charts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MaimaiGame.Scenes;

public class PlayScene : Scene
{
	private Chart _chart = null!;
	private Difficulty _difficulty = null!;

	private Music _music = null!;

	private Texture2D _bgImage = null!;
	private Rectangle _backgroundImageRect;
	private Color _bgImageColour;

	public override void OnCreate(ContentManager contentManager)
	{
		void UpdateBackgroundImageRect()
		{
			int height = (int) (_bgImage.Height / (float) _bgImage.Width * MaimaiGame.Instance.BottomDisplayArea.Width);

			_backgroundImageRect = new Rectangle(MaimaiGame.Instance.BottomDisplayArea.Left,
				MaimaiGame.Instance.BottomDisplayArea.Top, MaimaiGame.Instance.BottomDisplayArea.Width, height);
		}

		Chart? chart = Chart.Load("Resources/Chart");

		if (chart == null || chart.Master == null)
		{
			MaimaiGame.Instance.Exit();
			return;
		}

		_chart = chart;
		_difficulty = _chart.Master;

		_music = Music.Load(_chart.MusicPath);
		_music.Play();

		_bgImage = !string.IsNullOrWhiteSpace(_chart.BgImagePath)
			? Texture2D.FromFile(MaimaiGame.Instance.GraphicsDevice, _chart.BgImagePath)
			: TextureManager.DefaultBackground;

		const float dimValue = 0.5f;
		_bgImageColour = new Color(dimValue, dimValue, dimValue, 1.0f);

		MaimaiGame.Instance.DisplayModeChanged += (_, _) =>
		{
			UpdateBackgroundImageRect();
		};

		UpdateBackgroundImageRect();
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

		// TODO: Need a localised sprite font (see: https://stackoverflow.com/a/35300034)
		/* spriteBatch.DrawString(
			FontManager.Tahoma12,
			$"Chart: {_chart.Artist} - {_chart.Title} ({_difficulty.Designer}) [{_difficulty.Name} ({_difficulty.Level})]",
			new Vector2(2.0f, 17.0f),
			Color.White
		); */


		spriteBatch.Draw(_bgImage, _backgroundImageRect, null, _bgImageColour);
		spriteBatch.Draw(TextureManager.RingBase, MaimaiGame.Instance.BottomDisplayArea, null, Color.White);
		spriteBatch.Draw(TextureManager.Vignette, MaimaiGame.Instance.BottomDisplayArea, null, Color.White);

		spriteBatch.End();
	}
}