using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace MaimaiGame.Scenes;

public class TouchScreenTestScene : Scene
{
	public override void OnUpdate(GameTime gameTime) { }

	public override void OnRender(SpriteBatch spriteBatch, GameTime gameTime)
	{
		spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);

		spriteBatch.Begin();

		const float topScreenScale = 0.235f;
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
		spriteBatch.DrawString(FontManager.Tahoma12, $"{fps:N0}fps", new Vector2(7.0f, 4.0f), Color.White);

		spriteBatch.End();
	}
}