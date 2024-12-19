using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sokoban;

public class DropdownMenu
{
    private SpriteFont _font;
    private Texture2D _buttonTexture;
    private Texture2D _itemTexture;

    private Rectangle _buttonBounds;
    private List<Rectangle> _itemBounds;

    private List<string> _items;
    private bool _isOpen;
    private int _selectedIndex;

    public string SelectedItem => _selectedIndex >= 0 ? _items[_selectedIndex] : null;

    public DropdownMenu(SpriteFont font, Texture2D buttonTexture, Texture2D itemTexture, Vector2 position, int buttonWidth, List<string> items)
    {
        _font = font;
        _buttonTexture = buttonTexture;
        _itemTexture = itemTexture;

        _buttonBounds = new Rectangle((int)position.X, (int)position.Y, buttonWidth, (int)(_font.LineSpacing * 1.5f));
        _items = items;
        _itemBounds = new List<Rectangle>();

        for (int i = 0; i < items.Count; i++)
        {
            var itemBounds = new Rectangle(_buttonBounds.X, _buttonBounds.Bottom + i * _buttonBounds.Height, _buttonBounds.Width, _buttonBounds.Height);
            _itemBounds.Add(itemBounds);
        }

        _isOpen = false;
        _selectedIndex = -1;
    }

    public void Update(MouseState mouseState, MouseState previousMouseState)
    {
        Point mousePosition = new Point(mouseState.X, mouseState.Y);

        if (_buttonBounds.Contains(mousePosition) && mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
        {
            _isOpen = !_isOpen;
        }

        if (_isOpen)
        {
            for (int i = 0; i < _itemBounds.Count; i++)
            {
                if (_itemBounds[i].Contains(mousePosition) && mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    _selectedIndex = i;
                    _isOpen = false;
                }
            }
        }
    }

    public int GetSelectedIndex()
    {
        return _selectedIndex;
    }
    public void SetSelectedIndex(int index)
    {
        _selectedIndex = index;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_buttonTexture, _buttonBounds, Color.White);
        string buttonText = _selectedIndex >= 0 ? _items[_selectedIndex] : "Selection Niveau";
        spriteBatch.DrawString(_font, buttonText, new Vector2(_buttonBounds.X + 5, _buttonBounds.Y + 5), Color.Black);

        if (_isOpen)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                spriteBatch.Draw(_itemTexture, _itemBounds[i], Color.LightGray);
                spriteBatch.DrawString(_font, _items[i], new Vector2(_itemBounds[i].X + 5, _itemBounds[i].Y + 5), Color.Black);
            }
        }
    }
}
