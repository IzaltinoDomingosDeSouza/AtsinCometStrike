using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AtsinCometStrike;

public class Projectile : Entity
{
	private float movementSpeed = 250f;
	
	public override void Initialize(Rectangle atlas, Vector2 Position, float Angle = 0)
    {
        base.Initialize(atlas,Position,Angle);
    }
	public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        Position.X += movementSpeed * deltaTime;
        
	    if(!Global.IsInsideOfScreen(Position,Origin))
	    {
	    		Destroy();
	    }
    }
	public override void Draw(Texture2D texture, SpriteBatch spriteBatch)
    {
         base.Draw(texture, spriteBatch);
    }
}
