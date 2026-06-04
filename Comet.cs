using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AtsinCometStrike;

public class Comet : Entity
{
	public float MovementSpeed = 250f;
	public float Strength = 1.0f;
	public int Health = 100;
	
	public override void Initialize(Rectangle atlas, Vector2 Position, float Angle = 0)
    {
        base.Initialize(atlas,Position,Angle);
    }
	public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        Position.X -= MovementSpeed * deltaTime;
    }
    public override void OnCollision(Entity other)
    {
    		if(other is Projectile projectile)
    		{
    			projectile.Destroy();
    			int damageAmount = (int)(projectile.DemageAmount / Strength);
    			Health -= damageAmount;
    			Global.Score += (int)(damageAmount * 5);
			if(Health <= 0)
			{
				Global.Score += (int)(150 * Strength);
				Destroy();
			}
    		}
    		if(other is Spaceship spaceship)
    		{
    			spaceship.Health -= 1;
    			Destroy();
    		}
    }
	public override void Draw(Texture2D texture, SpriteBatch spriteBatch)
    {
         base.Draw(texture, spriteBatch);
    }
}
