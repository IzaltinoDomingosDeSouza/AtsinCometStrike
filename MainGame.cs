using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

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
	public static bool IsInsideOfScreen(Vector2 position, Vector2 origin)
	{
		return position.X >= origin.X && 
			   position.X <= ScreenSize.X - origin.X &&
			   position.Y >= origin.Y && 
			   position.Y <= ScreenSize.Y - origin.Y;
	}
	public static readonly Rectangle SpaceshipAtlas = new Rectangle(518, 493, 82, 84);
	public static readonly Rectangle ProjectileAtlas = new Rectangle(856, 421, 9, 54);
	public static readonly Rectangle tinyCometAtlas = new Rectangle(346, 814, 18, 18);
	public static readonly Rectangle smallCometAtlas = new Rectangle(406, 234, 28, 28);
	public static readonly Rectangle medCometAtlas = new Rectangle(651, 447, 43, 43);
	public static readonly Rectangle bigCometAtlas = new Rectangle(224, 664, 101, 84);
}

public class MainGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Texture2D _gameTexture;
    
    private Spaceship _spaceship;
    
    private List<Entity> _entitiesPool = new(); 
    
    private SoundEffect _shootSound;
    
    public MainGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        Global.ScreenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
    }

    protected override void Initialize()
    {
		_spaceship = new Spaceship();
		_spaceship.Initialize(Global.SpaceshipAtlas,new Vector2(Global.SpaceshipAtlas.Width, Global.ScreenSize.Y / 2f),MathHelper.ToRadians(-90));

		_entitiesPool.Add(_spaceship);

		var tinyComet = new Comet();
		tinyComet.Initialize(Global.tinyCometAtlas,new Vector2(Global.ScreenSize.X - Global.tinyCometAtlas.Width, Global.ScreenSize.Y / 2f),MathHelper.ToRadians(-90));

		_entitiesPool.Add(tinyComet);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
		
		_gameTexture = Content.Load<Texture2D>("AtlasTexture/Game");
		
		_shootSound = Content.Load<SoundEffect>("SoundEffects/Projectile");
    }
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

        KeyboardState kState = Keyboard.GetState();
        
		if(kState.IsKeyDown(Keys.W))
			_spaceship.MoveUp(elapsed);
		if(kState.IsKeyDown(Keys.S))
			_spaceship.MoveDown(elapsed);
			
		if(kState.IsKeyDown(Keys.Space) && _spaceship.CanShoot())
		{
			var projectile = new Projectile();
			projectile.Initialize(Global.ProjectileAtlas,
								  new Vector2(_spaceship.Position.X + _spaceship.Origin.X,_spaceship.Position.Y),
								  MathHelper.ToRadians(90));
			_entitiesPool.Add(projectile);
			_shootSound.Play();
		}
        
		foreach(var entity in _entitiesPool)
		{
			entity.Update(elapsed);
		}
        
        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        
		foreach(var entity in _entitiesPool)
		{
			entity.Draw(_gameTexture,_spriteBatch);
		}
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
