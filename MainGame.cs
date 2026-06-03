using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace AtsinCometStrike;

public static class Global
{
    public static Vector2 ScreenSize;
    public static Vector2 KeepOnScreen(Vector2 position, Vector2 origin)
    {
        return new Vector2(MathHelper.Clamp(position.X, origin.X, ScreenSize.X - origin.X),
                           MathHelper.Clamp(position.Y, origin.Y, ScreenSize.Y - origin.Y));
    }
    public static readonly Rectangle SpaceshipAtlas = new Rectangle(518, 493, 82, 84);
}

public class MainGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Texture2D _gameTexture;
	private Vector2 _spaceshipPosition;
	private Vector2 _spaceshipOrigin;
    private float _spaceshipAngle;
    
    public MainGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        Global.ScreenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
    }

    protected override void Initialize()
    {
		_spaceshipOrigin = new Vector2(Global.SpaceshipAtlas.Width / 2f, Global.SpaceshipAtlas.Height / 2f);
        _spaceshipPosition = new Vector2(Global.SpaceshipAtlas.Width, Global.ScreenSize.Y / 2f);
		_spaceshipAngle = MathHelper.ToRadians(90);
		
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
		
		_gameTexture = Content.Load<Texture2D>("AtlasTexture/Game");
    }
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

        KeyboardState kState = Keyboard.GetState();
        
        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        
        _spriteBatch.Draw(_gameTexture, _spaceshipPosition, Global.SpaceshipAtlas, Color.White, _spaceshipAngle, _spaceshipOrigin, 1.0f, SpriteEffects.FlipVertically, 0f);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
