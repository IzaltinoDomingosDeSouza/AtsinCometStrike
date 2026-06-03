using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AtsinCometStrike;

public class Spaceship : Entity
{
	public override void Initialize(Rectangle atlas, Vector2 Position, float Angle = 0)
    {
        base.Initialize(atlas,Position,Angle);
    }
	public override void Update(float deltaTime)
    {
        
    }
	public override void Draw(Texture2D texture, SpriteBatch spriteBatch)
    {
         base.Draw(texture, spriteBatch);
    }
}
