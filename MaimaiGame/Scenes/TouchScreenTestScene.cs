using System;
using System.Collections.Generic;
using MaimaiGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

	public override void OnUpdate(GameTime gameTime)
	{
		if (Keyboard.GetState().IsKeyPressed(Keys.Escape))
			SceneManager.Pop();
	}

	public override void OnRender(SpriteBatch spriteBatch, GameTime gameTime)
	{
		spriteBatch.GraphicsDevice.Clear(Color.Black);

		spriteBatch.Begin();

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