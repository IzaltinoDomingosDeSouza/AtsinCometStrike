using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AtsinCometStrike;

public abstract class Entity
{
	public Vector2 Position;
	public Vector2 Origin;
    public float Angle;
    public bool IsActive = true;
    
    private Rectangle _atlas;
    
	public virtual void Initialize(Rectangle atlas, Vector2 Position, float Angle = 0)
    {
    		_atlas = atlas;
    		this.Position = Position;
    		this.Angle = Angle;
        this.Origin = new Vector2(atlas.Width / 2f, atlas.Height / 2f);
    }
    public virtual void Reset(Rectangle atlas, Vector2 Position, float Angle = 0)
    {
		Initialize(atlas,Position,Angle);
		IsActive = true;
    }
    
	public virtual void Update(float deltaTime) { }
	
	public virtual void Draw(Texture2D texture, SpriteBatch spriteBatch)
    {
    		if(IsActive)
    			spriteBatch.Draw(texture, Position, _atlas, Color.White, Angle, Origin, 1.0f, SpriteEffects.None, 0f);
    }
    public virtual Rectangle GetBounds()
	{
        return new Rectangle(
            (int)(Position.X - Origin.X), 
            (int)(Position.Y - Origin.Y), 
            _atlas.Width, 
            _atlas.Height
        );
	}
    public virtual void Destroy() => IsActive = false;
}
