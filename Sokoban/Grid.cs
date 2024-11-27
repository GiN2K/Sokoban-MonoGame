using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Sokoban;

public class Grid
{
    private string[,] cells;
    private int rows = 10;
    private int columns = 20;
    private Texture2D wallTexture;
    private Texture2D boxTexture;
    private Texture2D targetTexture;
    private Texture2D boxValidTexture;

    // fonction pour aider a copier le tableau ( a la place de l'utiliser par reference)
    public static string[,] DeepCopy(string[,] original)
    {
        int rows = original.GetLength(0);
        int cols = original.GetLength(1);
        string[,] copy = new string[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                copy[i, j] = String.Copy(original[i, j]);
            }
        }

        return copy;
    }

    public Grid(Texture2D wall, Texture2D box, Texture2D target,Texture2D boxValid,string[,] levelData)
    {
        wallTexture = wall;
        boxTexture = box;
        targetTexture = target;
        boxValidTexture = boxValid;

        cells = DeepCopy(levelData);
    }

    public bool IsGameWon()
    {
        for(int i=0;i<rows;i++)
        {
            for(int j=0;j<columns;j++)
            {
                if(cells[i,j]=="." || cells[i,j]==".P")
                {
                    return false;
                }
            }
        }
        return true;
    }
    
    public string[,] GetCells()
    {
        return cells;
    }
    public int GetPlayerPositionC()
    {
        for(int i=0;i<rows;i++)
        {
            for(int j=0;j<columns;j++)
            {
                if(cells[i,j]=="P" || cells[i,j]==".P")
                {
                    return i;
                }
            }
        }

        return -1;
    }
    public int GetPlayerPositionR()
    {
        for(int i=0;i<rows;i++)
        {
            for(int j=0;j<columns;j++)
            {
                if(cells[i,j]=="P" || cells[i,j]==".P")
                {
                    return j;
                }
            }
        }

        return -1;
    }
    
    public string GetCell(int x, int y)
    {
        return cells[y, x];
    }
    public void SetCell(int x, int y, string cell)
    {
        cells[y, x] = cell;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Texture2D texture = null;
                Vector2 position = new Vector2(j * 50, i * 50);
                Rectangle textureRectangle = new Rectangle((int)position.X, (int)position.Y, 50, 50);

                switch (cells[i, j])
                {
                    case "#": texture = wallTexture; break;
                    case "B": texture = boxTexture; break;
                    case ".": texture = targetTexture; break;
                    case ".P": texture = targetTexture; break;
                    case ".B": texture = boxValidTexture; break;
                }

                if (texture != null)
                {
                    spriteBatch.Draw(texture, textureRectangle, Color.White);
                }
            }
        }
    }
}