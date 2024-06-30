using System;
using MaimaiGame.Audio;
using MaimaiGame.Input;
using MaimaiGame.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MaimaiGame;

public class MaimaiGame : Game
{
	// TODO: Using keyboard shortcuts to move the window to another display does not update the current adapter.

	private readonly GraphicsDeviceManager _graphicsDeviceManager;
	private SpriteBatch _spriteBatch = null!;

	private const int DefaultDisplayWidth = 720;
	private const int DefaultDisplayHeight = 1280;

	public int DisplayWidth => Window.ClientBounds.Width;
	public int DisplayHeight => Window.ClientBounds.Height;
	public bool IsFullscreen => _graphicsDeviceManager.IsFullScreen;

	public Rectangle PlayArea { get; private set; }

	public EventHandler<DisplayModeChangedEventArgs>? DisplayModeChanged;

	public static MaimaiGame Instance { get; private set; } = null!;

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

		Configuration.Initialise();
		AudioManager.Initialise();
		InputManager.Initialise();

		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);
		FontManager.Initialise(Content);

		SceneManager.Push(Content, new PlayScene());
	}

	protected override void Update(GameTime gameTime)
	{
		if (Keyboard.GetState().IsKeyPressed(Keys.F))
			SetDisplayMode(DefaultDisplayWidth, DefaultDisplayHeight, !IsFullscreen);

		if (Keyboard.GetState().IsKeyPressed(Keys.T) && SceneManager.Current is not TouchScreenTestScene)
			SceneManager.Push(Content, new TouchScreenTestScene());

		SceneManager.Current?.OnUpdate(gameTime);

		KeyboardStateExtensions.Update();
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		SceneManager.Current?.OnRender(_spriteBatch, gameTime);
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

		// The play area is slightly smaller than the width of the screen, so we need account for it.
		const int spacing = 25, spacing2x = spacing * 2;
		PlayArea = new Rectangle(spacing, height - width + spacing, width - spacing2x, width - spacing2x);

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