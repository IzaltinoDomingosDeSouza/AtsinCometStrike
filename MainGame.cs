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
	
	public static int Score = 0;
	public static float ScrollInitialSpeed = 128f;
}

public class MainGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    ScrollableBackground _scrollableBackground = new();
    
    private Texture2D _gameTexture;
    
    private Spaceship _spaceship;
    
    private List<Entity> _entitiesPool = new(); 
    
    private SoundEffect _shootSound;

    private float _cometSpawnTimer = 0f;
    private float _cometSpawn = 1f;
    private Random _random = new Random();
    
    private float _levelCountdownTimer;
    private float _levelTimerAmount = 10 * 60.0f; //10 minutes

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
		
		_scrollableBackground.Initialize(Global.ScrollInitialSpeed);
		
		_levelCountdownTimer = _levelTimerAmount;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
		
		_gameTexture = Content.Load<Texture2D>("AtlasTexture/Game");
		
		_shootSound = Content.Load<SoundEffect>("SoundEffects/Projectile");
		
		_scrollableBackground.LoadContent(Content.Load<Texture2D>("AtlasTexture/Background"),new Rectangle(256, 256, 256, 256));
    }
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        _levelCountdownTimer -= elapsed;
        
        _scrollableBackground.Update(elapsed);

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
		
		CometWaveUpdate(elapsed);
        
		foreach(var entity in _entitiesPool)
		{
			entity.Update(elapsed);
		}
        
        CollisionHandler(elapsed);
        
        GameLevelUpdate(elapsed);
        
		Console.WriteLine($"Score : {Global.Score} | Time : {_levelCountdownTimer} ");
        
        base.Update(gameTime);
    }
    
    void CometWaveUpdate(float deltaTime)
    {
    		if(_cometSpawnTimer <= 0f)
    		{
    			_cometSpawnTimer = _cometSpawn;
    			
    			var comet = new Comet();
    			
    			float cometType = _random.Next(1, 5);
    			Rectangle cometAtlas = new Rectangle();
    			float movementSpeedBase = 250f;
    			float cometAngle = _random.Next(1, 360);	//this help to create varition
    			switch(cometType)
    			{
    				case 1:
    					cometAtlas = Global.tinyCometAtlas;
    					comet.MovementSpeed = (movementSpeedBase * 1.0f) + _scrollableBackground.ScrollSpeed;
    					comet.Strength = 0.1f;
    				break;
    				case 2:
    					cometAtlas = Global.smallCometAtlas;
    					comet.MovementSpeed = (movementSpeedBase * 0.75f) + _scrollableBackground.ScrollSpeed;
    					comet.Strength = 0.25f;
    				break;
    				case 3:
    					cometAtlas = Global.medCometAtlas;
    					comet.MovementSpeed = (movementSpeedBase * 0.50f) + _scrollableBackground.ScrollSpeed;
    					comet.Strength = 0.50f;
    				break;
    				case 4:
    					cometAtlas = Global.bigCometAtlas;
    					comet.MovementSpeed = (movementSpeedBase * 0.25f) + _scrollableBackground.ScrollSpeed;
    					comet.Strength = 0.75f;
    				break;
    			}
    			
    			var _spawnArea = new Rectangle((int)Global.ScreenSize.X, 100,(int)Global.ScreenSize.X + 200,(int)Global.ScreenSize.Y-100);
    			var x = _random.Next(_spawnArea.X,_spawnArea.Width);
            var y = _random.Next(_spawnArea.Y,_spawnArea.Height);
			comet.Initialize(cometAtlas, new Vector2(x,y),MathHelper.ToRadians(cometAngle));
			_entitiesPool.Add(comet);
    		}
    		
    		_cometSpawnTimer -= deltaTime;
    }
    void CollisionHandler(float deltaTime)
    {
    		foreach(var entity1 in _entitiesPool)
    		{
    			if(!entity1.IsActive) continue;
    			foreach(var entity2 in _entitiesPool)
    			{
    				if(!entity2.IsActive || ReferenceEquals(entity1, entity2)) continue;
    				if(entity1.GetBounds().Intersects(entity2.GetBounds()))
    				{
    					entity1.OnCollision(entity2);
    				}
    			}
    		}
    }
    void GameLevelUpdate(float deltaTime)
    {
        if(_levelCountdownTimer <= 0)
        {
        		//TODO Show score amount and game over screen
        }
        if(!_spaceship.IsActive)
        {
        		//TODO Show game over screen
        }
    
    		if(_levelCountdownTimer < 2f*60f)
    		{
    			_scrollableBackground.ScrollSpeed = Global.ScrollInitialSpeed * 2f;
    		}
    		if(_levelCountdownTimer < 5f*60f)
    		{
    			_scrollableBackground.ScrollSpeed = Global.ScrollInitialSpeed * 1.5f;
    		}else
    		{
    			_scrollableBackground.ScrollSpeed = Global.ScrollInitialSpeed;
    		}
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        
        _scrollableBackground.Draw(_spriteBatch,Global.ScreenSize);
        
		foreach(var entity in _entitiesPool)
		{
			entity.Draw(_gameTexture,_spriteBatch);
		}
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
