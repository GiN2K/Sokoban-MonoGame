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
    private string alertMessage = "Felicitations! Tu as reussi";

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
        
            Vector2 alertPosition = new Vector2(100, 100);
            spriteBatch.DrawString(Game1._font, alertMessage, alertPosition, Color.Red);

            // Background Box Pour alert
            Texture2D rect = new Texture2D(graphicsDevice, 200, 50);
            Color[] data = new Color[200 * 50];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Black * 0.8f;
            rect.SetData(data);
            spriteBatch.Draw(rect, new Rectangle((int)alertPosition.X - 10, (int)alertPosition.Y - 10, 220, 70), Color.White);
        
    }

    
}