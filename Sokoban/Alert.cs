using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content;

namespace Sokoban;

public class Alert
{
    private bool showAlert;
    private string alertMessage = "Felicitations! Tu as reussi, Click Sur N pour passer au niveau suivant";

    private TimeSpan
        alertDuration =
            TimeSpan.FromSeconds(
                10); // alert duraion, je vais la changer pour quil reste avant que lutilisateur change le niveau

    private TimeSpan alertTime;

    public Alert()
    {
        this.showAlert = false;
    }
    public bool GetShowAlert()
        {
            return showAlert;
        }
    public void SetShowAlert(bool showAlert)
    {
        this.showAlert = showAlert;
    }

    public void Update(GameTime gameTime, Grid grid)
    {
        alertTime = gameTime.TotalGameTime + alertDuration;

        if(showAlert){
            // Check if the alert time has expired
        if (showAlert && gameTime.TotalGameTime >= alertTime)
        {
            showAlert = false;
        }
        }
        

    }
    
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
    {
        
            Vector2 alertPosition = new Vector2((graphicsDevice.Viewport.Width - Game1._font.MeasureString(alertMessage).X) / 2, (graphicsDevice.Viewport.Height - Game1._font.MeasureString(alertMessage).Y) / 2);
                    spriteBatch.DrawString(Game1._font, alertMessage, alertPosition, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
                        spriteBatch.DrawString(Game1._font, alertMessage, alertPosition, Color.White);
            // Background Box Pour alert
            Texture2D rect = new Texture2D(graphicsDevice, 500, 50);
            Color[] data = new Color[500 * 50];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.White * 0.2f;
            rect.SetData(data);
            spriteBatch.Draw(rect, new Rectangle((int)alertPosition.X - 10, (int)alertPosition.Y - 10, (int)Game1._font.MeasureString(alertMessage).X + 20, (int)Game1._font.MeasureString(alertMessage).Y + 20), Color.Black);        
    }

    
}