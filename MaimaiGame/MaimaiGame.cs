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
	// TODO: MonoGame doesn't update display resolution after launch (can change in Windows but MonoGame won't know).

	private readonly GraphicsDeviceManager _graphicsDeviceManager;
	private SpriteBatch _spriteBatch = null!;

	public const int DefaultDisplayWidth = 720;
	public const int DefaultDisplayHeight = 1280;

	public float DisplayScale { get; private set; }

	public int DisplayWidth => Window.ClientBounds.Width;
	public int DisplayHeight => Window.ClientBounds.Height;
	public bool IsFullscreen => _graphicsDeviceManager.IsFullScreen;

	public Rectangle BottomDisplayArea { get; private set; }
	public Rectangle BottomDisplayPlayArea { get; private set; }

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
		Logger.Initialise();
		AudioManager.Initialise();
		InputManager.Initialise();

		base.Initialize();
	}

	protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);
		FontManager.Initialise(Content);
		TextureManager.Initialise(Content);

		SceneManager.Push(Content, new PlayScene());
	}

	protected override void Update(GameTime gameTime)
	{
		if (Keyboard.GetState().IsKeyPressed(Keys.F))
		{
			if (IsFullscreen)
				SetDisplayMode(DefaultDisplayWidth, DefaultDisplayHeight, false);
			else
				SetDisplayMode(GraphicsDevice.Adapter.CurrentDisplayMode.Width,
					GraphicsDevice.Adapter.CurrentDisplayMode.Height, true);
		}

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

	protected override void OnExiting(object sender, EventArgs args)
	{
		SceneManager.Clear();
		base.OnExiting(sender, args);
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

		DisplayScale = Instance.DisplayWidth / (float) DefaultDisplayWidth;

		BottomDisplayArea = new Rectangle(0, height - width, width, width);

		// The play area is slightly smaller than the width of the screen, so we need account for it.
		const int spacing = 40, spacing2x = spacing * 2;

		BottomDisplayPlayArea = new Rectangle(BottomDisplayArea.Left + spacing, BottomDisplayArea.Top + spacing,
			BottomDisplayArea.Width - spacing2x, BottomDisplayArea.Width - spacing2x);

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