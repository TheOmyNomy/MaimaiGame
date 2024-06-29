using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MaimaiGame;

public class FontManager
{
	public static SpriteFont Tahoma12 { get; private set; } = null!;

	private static bool _initialised;

	public static void Initialise(ContentManager contentManager)
	{
		if (_initialised)
			return;

		_initialised = true;

		Tahoma12 = contentManager.Load<SpriteFont>("Fonts/tahoma_12");
	}
}