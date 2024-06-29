using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MaimaiGame.Scenes;

public abstract class Scene
{
	public Scene() { }

	public virtual void OnCreate() { }
	public virtual void OnEnter() { }
	public virtual void OnLeave() { }
	public virtual void OnDestroy() { }

	public abstract void OnUpdate(GameTime gameTime);
	public abstract void OnRender(SpriteBatch spriteBatch, GameTime gameTime);
}