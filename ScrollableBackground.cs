using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AtsinCometStrike;

public class ScrollableBackground
{
	private Texture2D _texture;
	private Rectangle _atlas;
	private float _scrollOffset = 0f;
	public float ScrollSpeed;
	
	public void Initialize(float scrollSpeed = 128f)
	{
		ScrollSpeed = scrollSpeed;
	}
	public void LoadContent(Texture2D texture, Rectangle atlas)
	{
		_texture = texture;
		_atlas = atlas;
	}
	public void Update(float deltaTime)
	{
		_scrollOffset -= ScrollSpeed * deltaTime;
		
		if(_scrollOffset <= -_atlas.Width)
        {
            _scrollOffset += _atlas.Width;
        }
	}
	public void Draw(SpriteBatch spriteBatch, Vector2 screenSize)
	{
		for(float x = _scrollOffset - _atlas.Width; x < screenSize.X; x += _atlas.Width)
		{
			for(float y = 0; y < screenSize.Y; y += _atlas.Height)
			{
				spriteBatch.Draw(_texture, new Vector2((int)x, y), _atlas, Color.White);
			}
		}
	}
}
