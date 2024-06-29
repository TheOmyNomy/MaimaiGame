using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace MaimaiGame;

public class MaimaiGame : Game
{
	// TODO: Using keyboard shortcuts to move the window to another display does not update the current adapter.

	private readonly GraphicsDeviceManager _graphicsDeviceManager;
	private SpriteBatch _spriteBatch;

	private KeyboardState _lastKeyboardState;

	public const int DefaultDisplayWidth = 720;
	public const int DefaultDisplayHeight = 1280;

	public int DisplayWidth => Window.ClientBounds.Width;
	public int DisplayHeight => Window.ClientBounds.Height;
	public bool IsFullscreen => _graphicsDeviceManager.IsFullScreen;

	public EventHandler<DisplayModeChangedEventArgs> DisplayModeChanged;

	public static MaimaiGame Instance { get; private set; }

	public MaimaiGame()
	{
		Instance = this;

		_graphicsDeviceManager = new GraphicsDeviceManager(this);
		_graphicsDeviceManager.SynchronizeWithVerticalRetrace = false;

		Window.ClientSizeChanged += (_, _) => SetDisplayMode(
			Window.ClientBounds.Width,
			Window.ClientBounds.Height,
			IsFullscreen
		);

		Content.RootDirectory = "Content";
		IsMouseVisible = true;
	}

	protected override void Initialize()
	{
		SetDisplayMode(DefaultDisplayWidth, DefaultDisplayHeight, false);
		_lastKeyboardState = Keyboard.GetState();

		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);
	}

	protected override void Update(GameTime gameTime)
	{
		KeyboardState keyboardState = Keyboard.GetState();

		if (keyboardState.IsKeyDown(Keys.F) && !_lastKeyboardState.IsKeyDown(Keys.F))
			SetDisplayMode(DefaultDisplayWidth, DefaultDisplayHeight, !IsFullscreen);

		_lastKeyboardState = keyboardState;

		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		_spriteBatch.Begin();

		const float topScreenScale = 0.235f;
		_spriteBatch.DrawRectangle(0, 0, DisplayWidth, topScreenScale * DisplayHeight, Color.White, 5);

		_spriteBatch.DrawRectangle(0, DisplayHeight - DisplayWidth, DisplayWidth, DisplayWidth, Color.White, 5);

		const float radiusOffset = 50.0f;
		_spriteBatch.DrawCircle(
			DisplayWidth / 2.0f,
			DisplayHeight - DisplayWidth / 2.0f,
			DisplayWidth / 2.0f - radiusOffset,
			50,
			Color.White,
			5
		);

		_spriteBatch.End();

		base.Draw(gameTime);
	}

	public void SetDisplayMode(int width, int height, bool fullscreen)
	{
		if (fullscreen)
		{
			width = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
			height = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
		}

		_graphicsDeviceManager.IsFullScreen = fullscreen;

		_graphicsDeviceManager.PreferredBackBufferWidth = width;
		_graphicsDeviceManager.PreferredBackBufferHeight = height;

		// TODO: Enforce the given aspect ratio, regardless of the display size.

		GraphicsDevice.Viewport = new Viewport(0, 0, width, height);
		GraphicsDevice.ScissorRectangle = new Rectangle(0, 0, width, height);

		try
		{
			_graphicsDeviceManager.ApplyChanges();
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
		}

		DisplayModeChanged?.Invoke(this, new DisplayModeChangedEventArgs());
	}
}

public class DisplayModeChangedEventArgs : EventArgs { }