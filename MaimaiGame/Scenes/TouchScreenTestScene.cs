using System;
using System.Collections.Generic;
using MaimaiGame.Charts;
using MaimaiGame.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace MaimaiGame.Scenes;

public class TouchScreenTestScene : Scene
{
	private Texture2D _baseImage = null!;

	private readonly Dictionary<TouchScreenSensor, Texture2D> _sensorImages =
		new Dictionary<TouchScreenSensor, Texture2D>();

	public override void OnCreate(ContentManager contentManager)
	{
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

		spriteBatch.Draw(_baseImage, MaimaiGame.Instance.BottomDisplayPlayArea, Color.White);

		foreach (var item in _sensorImages)
		{
			if (!InputManager.TouchScreen.Sensors[item.Key])
				continue;

			spriteBatch.Draw(item.Value, MaimaiGame.Instance.BottomDisplayPlayArea, Color.White);
		}

		/* float startingAngle = MathHelper.ToRadians(360.0f / 16.0f);
		float endingAngle = MathHelper.ToRadians(360.0f);
		float incAngle = MathHelper.ToRadians(360.0f / 8.0f);
		float radius = MaimaiGame.Instance.BottomDisplayPlayArea.Width / 2.0f - 67.0f;

		Vector2 centre = new Vector2(MaimaiGame.Instance.DisplayWidth / 2.0f,
			MaimaiGame.Instance.BottomDisplayPlayArea.Y + MaimaiGame.Instance.BottomDisplayPlayArea.Height / 2.0f);

		int i = 1;

		for (float angle = startingAngle; angle <= endingAngle; angle += incAngle)
		{
			Vector2 destination = new Vector2(centre.X + radius * (float) Math.Cos(angle),
				centre.Y + radius * (float) Math.Sin(angle));

			float roundX = (float) Math.Round(destination.X, 4);
			float roundY = (float) Math.Round(destination.Y, 4);

			spriteBatch.DrawCircle(roundX, roundY, 5.0f, 100, Color.Red, 5.0f);

			float normaliseX = roundX.MapBetween(MaimaiGame.Instance.BottomDisplayPlayArea.Left,
				MaimaiGame.Instance.BottomDisplayPlayArea.Right, 0.0f, 1.0f);

			float normaliseY = roundY.MapBetween(MaimaiGame.Instance.BottomDisplayPlayArea.Top,
				MaimaiGame.Instance.BottomDisplayPlayArea.Bottom, 0.0f, 1.0f);

			Console.WriteLine(
				$"public static readonly Position A{i} = new Position({normaliseX:N4}f, {normaliseY:N4}f);");

			i++;
		} */

		foreach (Position position in Position.Values)
		{
			float x = MaimaiGame.Instance.BottomDisplayPlayArea.X +
			          position.X * MaimaiGame.Instance.BottomDisplayPlayArea.Width;

			float y = MaimaiGame.Instance.BottomDisplayPlayArea.Y +
			          position.Y * MaimaiGame.Instance.BottomDisplayPlayArea.Height;

			spriteBatch.DrawCircle(x, y, 5.0f, 100, Color.LimeGreen, 5.0f);
		}

		spriteBatch.End();
	}
}