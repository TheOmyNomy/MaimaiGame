using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MaimaiGame;

public static class TextureManager
{
	public static Texture2D DefaultBackground { get; private set; } = null!;
	public static Texture2D Vignette { get; private set; } = null!;
	
	public static Texture2D RingBase { get; private set; } = null!;
	public static Texture2D Ring { get; private set; } = null!;
	public static Texture2D RingEach { get; private set; } = null!;
	public static Texture2D RingBreak { get; private set; } = null!;

	public static Texture2D Tap { get; private set; } = null!;
	public static Texture2D TapEx { get; private set; } = null!;
	public static Texture2D TapEach { get; private set; } = null!;
	public static Texture2D TapEachEx { get; private set; } = null!;
	public static Texture2D TapBreak { get; private set; } = null!;
	public static Texture2D TapBreakEx { get; private set; } = null!;

	private static bool _initialised;

	public static void Initialise(ContentManager contentManager)
	{
		if (_initialised)
			return;

		_initialised = true;

		DefaultBackground = contentManager.Load<Texture2D>("Images/DefaultBackground");
		Vignette = contentManager.Load<Texture2D>("Images/Vignette");

		RingBase = contentManager.Load<Texture2D>("Images/Skin/RingBase");
		Ring = contentManager.Load<Texture2D>("Images/Skin/Ring");
		RingEach = contentManager.Load<Texture2D>("Images/Skin/RingEach");
		RingBreak = contentManager.Load<Texture2D>("Images/Skin/RingBreak");

		Tap = contentManager.Load<Texture2D>("Images/Skin/Tap");
		TapEx = contentManager.Load<Texture2D>("Images/Skin/TapEx");
		TapEach = contentManager.Load<Texture2D>("Images/Skin/TapEach");
		TapEachEx = contentManager.Load<Texture2D>("Images/Skin/TapEachEx");
		TapBreak = contentManager.Load<Texture2D>("Images/Skin/TapBreak");
		TapBreakEx = contentManager.Load<Texture2D>("Images/Skin/TapBreakEx");
	}
}