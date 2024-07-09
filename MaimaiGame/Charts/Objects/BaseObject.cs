using System;
using Microsoft.Xna.Framework.Graphics;

namespace MaimaiGame.Charts.Objects;

public abstract class BaseObject
{
	public readonly float Time;
	public readonly Position Position;
	public virtual Modifiers Modifiers { get; set; }

	public BaseObject(float time, Position position, Modifiers modifiers = Modifiers.None)
	{
		Time = time;
		Position = position;
		Modifiers = modifiers;
	}

	public virtual void OnDisplayModeChanged() { }

	public abstract void OnUpdate(float currentTime);
	public abstract void OnRender(SpriteBatch spriteBatch, float currentTime, float renderDuration);
}

[Flags]
public enum Modifiers
{
	None = 1 << 0,
	Each = 1 << 1,
	Break = 1 << 2,
	Ex = 1 << 3
}