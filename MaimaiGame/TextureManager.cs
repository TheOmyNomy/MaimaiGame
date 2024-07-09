using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MaimaiGame;

public static class TextureManager
{
	public static Texture2D DefaultBackground { get; private set; } = null!;
	public static Texture2D Vignette { get; private set; } = null!;
	public static Texture2D RingBase { get; private set; } = null!;

	private static bool _initialised;

	public static void Initialise(ContentManager contentManager)
	{
		if (_initialised)
			return;

		_initialised = true;

		DefaultBackground = contentManager.Load<Texture2D>("Images/DefaultBackground");
		Vignette = contentManager.Load<Texture2D>("Images/Vignette");
		RingBase = contentManager.Load<Texture2D>("Images/Skin/RingBase");
	}
}