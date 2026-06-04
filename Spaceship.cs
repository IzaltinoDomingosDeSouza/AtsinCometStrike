using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AtsinCometStrike;

public class Spaceship : Entity
{
	private float _movementSpeed = 100f;
	private float _shootCooldown = 0.3f;
	private float _timer = 0f;
	public int DemageAmount = 25;
	public int Health = 3;
	
	public override void Initialize(Rectangle atlas, Vector2 Position, float Angle = 0)
    {
        base.Initialize(atlas,Position,Angle);
    }
    public override void Reset(Rectangle atlas, Vector2 Position, float Angle = 0)
    {
        base.Reset(atlas,Position,Angle);
    }
	public override void Update(float deltaTime)
    {
    		if(!IsActive) return;
    		
    		base.Update(deltaTime);
        Position = Global.KeepOnScreen(Position, Origin);
        
		if(_timer > 0)
			_timer -= deltaTime;
		
		if(Health <= 0)
			Destroy();
    }
    public override void OnCollision(Entity other)
    {
    
    }
    public void MoveUp(float deltaTime)
	{
	    Position.Y -= _movementSpeed * deltaTime;
	}
	public void MoveDown(float deltaTime)
	{
        Position.Y += _movementSpeed * deltaTime;
    }
    public bool CanShoot()
    {
    		if(_timer > 0 || !IsActive) return false;
    		
    		_timer = _shootCooldown;
    		return true;
    }
	public override void Draw(Texture2D texture, SpriteBatch spriteBatch)
    {
         base.Draw(texture, spriteBatch);
    }
}
