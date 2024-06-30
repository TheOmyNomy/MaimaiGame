using System;
using System.Collections.Generic;
using MaimaiGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MaimaiGame.Scenes;

public class TouchScreenTestScene : Scene
{
	private Rectangle _sensorArea;
	private Texture2D _baseImage = null!;

	private readonly Dictionary<TouchScreenSensor, Texture2D> _sensorImages =
		new Dictionary<TouchScreenSensor, Texture2D>();

	public override void OnCreate(ContentManager contentManager)
	{
		void UpdateSensorArea()
		{
			_sensorArea = new Rectangle(
				0,
				MaimaiGame.Instance.DisplayHeight - MaimaiGame.Instance.DisplayWidth,
				MaimaiGame.Instance.DisplayWidth,
				MaimaiGame.Instance.DisplayWidth
			);
		}

		MaimaiGame.Instance.DisplayModeChanged += (_, _) => UpdateSensorArea();
		UpdateSensorArea();

		_baseImage = contentManager.Load<Texture2D>("Images/TouchScreen/SensorTest/Base");

		foreach (TouchScreenSensor sensor in Enum.GetValues<TouchScreenSensor>())
			_sensorImages.Add(sensor, contentManager.Load<Texture2D>($"Images/TouchScreen/SensorTest/{sensor}"));
	}

	public override void OnUpdate(GameTime gameTime) { }

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

		spriteBatch.Draw(_baseImage, _sensorArea, Color.White);

		foreach (var item in _sensorImages)
		{
			if (!InputManager.TouchScreen.Sensors[item.Key])
				continue;

			spriteBatch.Draw(item.Value, _sensorArea, Color.White);
		}

		spriteBatch.End();
	}
}