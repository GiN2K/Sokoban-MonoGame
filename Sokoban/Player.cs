using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sokoban;

public class Player
{
    private int r;
    private int c;
    private Grid grid;

    public Player(int r, int c, Grid grid)
    {
        this.r = r;
        this.c = c;
        this.grid = grid;

    }

    // Code pour que on compte que le joueur peur bouger une fois par touche
    private KeyboardState previousKeyboardState;
    private bool canMove = true;

    public void Update(GameTime gameTime, GraphicsDevice GraphicsDevice)
    {
        KeyboardState currentKeyboardState = Keyboard.GetState();

        if (canMove)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))
            {


                if (IsMoveValid(r, c - 1))
                {

                    if (grid.GetCell(r, c - 1) == " " && grid.GetCell(r, c) == ".P")
                    {
                        grid.SetCell(r, c - 1, "P");
                        grid.SetCell(r, c, ".");

                    }
                    else if (grid.GetCell(r, c - 1) == "." && grid.GetCell(r, c) == ".P")
                    {
                        grid.SetCell(r, c - 1, ".P");
                        grid.SetCell(r, c, ".");
                    }
                    else if (grid.GetCell(r, c - 1) == " " && grid.GetCell(r, c) == "P")
                    {
                        grid.SetCell(r, c - 1, "P");
                        grid.SetCell(r, c, " ");

                    }
                    else if (grid.GetCell(r, c - 1) == "." && grid.GetCell(r, c) == "P")
                    {
                        {
                            grid.SetCell(r, c - 1, ".P");
                            grid.SetCell(r, c, " ");
                        }
                    }

                    this.c -= 1;
                    canMove = false;


                }
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
            {

                if (IsMoveValid(r, c + 1))
                {
                    if (grid.GetCell(r, c + 1) == " " && grid.GetCell(r, c) == ".P")
                    {
                        grid.SetCell(r, c + 1, "P");
                        grid.SetCell(r, c, ".");

                    }
                    else if (grid.GetCell(r, c + 1) == "." && grid.GetCell(r, c) == ".P")
                    {
                        grid.SetCell(r, c + 1, ".P");
                        grid.SetCell(r, c, ".");
                    }
                    else if (grid.GetCell(r, c + 1) == " " && grid.GetCell(r, c) == "P")
                    {
                        grid.SetCell(r, c + 1, "P");
                        grid.SetCell(r, c, " ");

                    }
                    else if (grid.GetCell(r, c + 1) == "." && grid.GetCell(r, c) == "P")
                    {
                        {
                            grid.SetCell(r, c + 1, ".P");
                            grid.SetCell(r, c, " ");
                        }
                    }
                        this.c += 1;
                        canMove = false;

                    
                }
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left))
            {

                if (IsMoveValid(r - 1, c))
                {
                    if (grid.GetCell(r - 1, c) == " " && grid.GetCell(r, c) == ".P")
                    {
                        grid.SetCell(r - 1, c, "P");
                        grid.SetCell(r, c, ".");

                    }
                    else if (grid.GetCell(r - 1, c) == "." && grid.GetCell(r, c) == ".P")
                    {
                        grid.SetCell(r - 1, c, ".P");
                        grid.SetCell(r, c, ".");
                    }
                    else if (grid.GetCell(r - 1, c) == " " && grid.GetCell(r, c) == "P")
                    {
                        grid.SetCell(r - 1, c, "P");
                        grid.SetCell(r, c, " ");

                    }
                    else if (grid.GetCell(r - 1, c) == "." && grid.GetCell(r, c) == "P")
                    {
                        
                            grid.SetCell(r - 1, c, ".P");
                            grid.SetCell(r, c, " ");
                        
                    }
                        this.r -= 1;
                        canMove = false;

                    

                }
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right))
            {

                if (IsMoveValid(r + 1, c))
                {
                    if (grid.GetCell(r + 1, c) == " " && grid.GetCell(r, c) == ".P")
                    {
                        grid.SetCell(r + 1, c, "P");
                        grid.SetCell(r, c, ".");

                    }
                    else if (grid.GetCell(r + 1, c) == "." && grid.GetCell(r, c) == ".P")
                    {
                        grid.SetCell(r + 1, c, ".P");
                        grid.SetCell(r, c, ".");
                    }
                    else if (grid.GetCell(r + 1, c) == " " && grid.GetCell(r, c) == "P")
                    {
                        grid.SetCell(r + 1, c, "P");
                        grid.SetCell(r, c, " ");

                    }
                    else if (grid.GetCell(r + 1, c) == "." && grid.GetCell(r, c) == "P")
                    
                        {
                            grid.SetCell(r + 1, c, ".P");
                            grid.SetCell(r, c, " ");
                        }


                        this.r += 1;
                        canMove = false;

                    

                }



            }

        }
        
        if (currentKeyboardState.GetPressedKeys().Length == 0)
        {
            canMove = true;
        }

        previousKeyboardState = currentKeyboardState;
    }

    // Voir si le mouvement est valide , si c'est une boite, on le bouge et on retourne true
    private bool IsMoveValid(int targetr, int targetY)
    {
        string cell = grid.GetCell(targetr, targetY);
        
        // si c'est un mur
        if (cell == "#")
        {
            return false;
        }

        // si c'est une boite ou une boite sur une cible
        if (cell == "B" || cell == ".B")
        {
            int borNewr = targetr + targetr - r;
            int borNewY = targetY + targetY - c;
        
            if (grid.GetCell(borNewr, borNewY) == " " && cell == ".B")
            {
                grid.SetCell(targetr, targetY, ".");
                grid.SetCell(borNewr, borNewY, "B");
                return true;
            }
            
            else if (grid.GetCell(borNewr, borNewY) == " ")
            {
                grid.SetCell(targetr, targetY, " ");
                grid.SetCell(borNewr, borNewY, "B");
                return true;
            }
            if (grid.GetCell(borNewr, borNewY) == ".B" && cell == ".B")
            {
                grid.SetCell(targetr, targetY, ".");
                grid.SetCell(borNewr, borNewY, ".B");
                return true;
            }
            if (grid.GetCell(borNewr, borNewY) == "." && cell == ".B")
            {
                grid.SetCell(targetr, targetY, ".");
                grid.SetCell(borNewr, borNewY, ".B");
                return true;
            }
            if (grid.GetCell(borNewr, borNewY) == "." && cell == "B")
            {
                grid.SetCell(targetr, targetY, " ");
                grid.SetCell(borNewr, borNewY, ".B");
                return true;
            }
        
            return false;
        }
        
        return true;
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D playerTerture)
    {
        Vector2 position = new Vector2(r * 50, c * 50);
        Rectangle tertureRectangle = new Rectangle((int)position.X, (int)position.Y, 50, 50);
        spriteBatch.Draw(playerTerture, tertureRectangle, Color.White);

    }
}
