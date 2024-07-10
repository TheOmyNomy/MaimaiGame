using System;
using MaimaiGame.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MaimaiGame.Charts.Objects;

public sealed class Tap : BaseObject
{
	private Vector2 _centrePosition;
	private Vector2 _destinationPosition;
	private Vector2 _startPosition;

	private Texture2D _image = null!;
	private Vector2 _origin;
	private float _scale = 1.0f;

	private Texture2D _ringImage = null!;
	private Vector2 _ringOrigin;
	private float _minimumRingScale, _maximumRingScale;

	private float _rotation;

	public override Modifiers Modifiers
	{
		get => base.Modifiers;
		set
		{
			base.Modifiers = value;
			ApplyModifiers();
		}
	}

	public Tap(float time, Position position, Modifiers modifiers = Modifiers.None) :
		base(time, position, modifiers)
	{
		ApplyModifiers();
		OnDisplayModeChanged();
	}

	public override void OnDisplayModeChanged()
	{
		_centrePosition = new Vector2(
			MaimaiGame.Instance.BottomDisplayPlayArea.Left + MaimaiGame.Instance.BottomDisplayPlayArea.Width / 2.0f,
			MaimaiGame.Instance.BottomDisplayPlayArea.Top + MaimaiGame.Instance.BottomDisplayPlayArea.Height / 2.0f
		);

		_destinationPosition = new Vector2(
			MaimaiGame.Instance.BottomDisplayPlayArea.Left +
			Position.X * MaimaiGame.Instance.BottomDisplayPlayArea.Width,
			MaimaiGame.Instance.BottomDisplayPlayArea.Top +
			Position.Y * MaimaiGame.Instance.BottomDisplayPlayArea.Height
		);

		// Lots of magic numbers here ... /shrug
		// The best solution would be to resize the image resources to where they're correct at a scale of 1
		// for the highest resolution we intend to support (2160x3840).
		// ... but we'll deal with that later :)

		const float startOffset = 0.275f;
		_startPosition = _centrePosition + (_destinationPosition - _centrePosition) * startOffset;

		_origin = new Vector2(_image.Width / 2.0f, _image.Height / 2.0f);
		_scale = 0.075f * MaimaiGame.Instance.DisplayScale;

		_ringOrigin = new Vector2(_ringImage.Width / 2.0f, 120.0f * MaimaiGame.Instance.DisplayScale);
		_minimumRingScale = 0.09f * MaimaiGame.Instance.DisplayScale;
		_maximumRingScale = 0.335f * MaimaiGame.Instance.DisplayScale;

		_rotation = (float) Math.Atan2(_destinationPosition.Y - _startPosition.Y,
			_destinationPosition.X - _startPosition.X) + MathHelper.ToRadians(90);
	}

	public override void OnUpdate(float currentTime) { }

	public override void OnRender(SpriteBatch spriteBatch, float currentTime, float renderDuration)
	{
		float timeDifference = Time - currentTime + AudioManager.Offset;
		float t = timeDifference.MapBetween(renderDuration, 0.0f, 0.0f, 1.0f);

		if (t < 0.0f || t > 1.25f)
			return;

		Vector2 position;
		float scale;

		float ringScale, ringOpacity;

		if (t >= 0.5f)
		{
			position = _startPosition + (_destinationPosition - _startPosition) * t.MapBetween(0.5f, 1.0f, 0.0f, 1.0f);
			scale = _scale;

			ringScale = t.MapBetween(0.5f, 1.0f, _minimumRingScale, _maximumRingScale);
			ringOpacity = 1.0f;
		}
		else
		{
			position = _startPosition;
			scale = t.MapBetween(0.0f, 0.5f, 0.0f, _scale);

			ringScale = _minimumRingScale;
			ringOpacity = t * 2.0f;
		}

		_ringOrigin = new Vector2(_ringImage.Width / 2.0f, 120.0f);

		spriteBatch.Draw(_ringImage, position, null, Color.White * ringOpacity, _rotation, _ringOrigin, ringScale,
			SpriteEffects.None, 0.0f);

		spriteBatch.Draw(_image, position, null, Color.White, _rotation, _origin, scale, SpriteEffects.None, 0.0f);
	}

	private void ApplyModifiers()
	{
		bool isEx = (Modifiers & Modifiers.Ex) > 0;

		if ((Modifiers & Modifiers.Break) > 0)
		{
			_image = isEx ? TextureManager.TapBreakEx : TextureManager.TapBreak;
			_ringImage = TextureManager.RingBreak;
		}
		else if ((Modifiers & Modifiers.Each) > 0)
		{
			_image = isEx ? TextureManager.TapEachEx : TextureManager.TapEach;
			_ringImage = TextureManager.RingEach;
		}
		else
		{
			_image = isEx ? TextureManager.TapEx : TextureManager.Tap;
			_ringImage = TextureManager.Ring;
		}
	}
}