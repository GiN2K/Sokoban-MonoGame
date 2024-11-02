﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sokoban
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        
        private Player player;
        private Grid grid;

        private Texture2D wallTexture;
        private Texture2D boxTexture;
        private Texture2D targetTexture;
        private Texture2D playerTexture;
        private Texture2D boxValidTexture;
        
        
        
        // Code pour alert
        private SpriteFont _font;
        private bool showAlert = false;

        public void SetShowAlert(bool showAlert)
        {
            this.showAlert = showAlert;
        }

        private string alertMessage = "Felicitations! Tu as reussi"; // Alert message
        private TimeSpan alertDuration = TimeSpan.FromSeconds(3); // Alert duration
        private TimeSpan alertTime;
        
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            // Window 1000x500 pour 10x20 avec 50pxiel par case
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 500; 
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            wallTexture = Content.Load<Texture2D>("wall");
            boxTexture = Content.Load<Texture2D>("box");
            targetTexture = Content.Load<Texture2D>("target");
            playerTexture = Content.Load<Texture2D>("player");
            boxValidTexture = Content.Load<Texture2D>("boxValid");
            _font = Content.Load<SpriteFont>("SpriteFont");

            
            // levelData par default 10x20
            string[,] levelData = {
                { "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#" },
                { "#", " ", "P", " ", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#" },
                { "#", " ", " ", " ", " ", " ", " ", " ", "#", "#", "#", " ", " ", ".", ".", " ", " ", "#", "#", "#" },
                { "#", "#", "#", "#", " ", "#", " ", "#", "#", " ", "#", " ", "B", " ", " ", "#", " ", "#", "#", "#" },
                { "#", "#", " ", "#", " ", "#", " ", " ", " ", " ", " ", "#", " ", "#", " ", "#", " ", "#", "#", "#" },
                { "#", "#", " ", " ", " ", " ", " ", "#", "#", "#", " ", " ", " ", "#", "B", " ", " ", " ", "#", "#",},
                { "#", "#", "#", "#", "#", "#", "#", " ", " ", " ", " ", " ", " ", "#", " ", " ", " ", "#", "#", "#" },
                { "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#" },
                { "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#" },
                { "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#", "#" }
            };

            grid = new Grid( wallTexture, boxTexture, targetTexture,boxValidTexture,levelData,showAlert);
            player = new Player(grid.GetPlayerPositionR(), grid.GetPlayerPositionC(), grid);
        }

        protected override void Update(GameTime gameTime)
        {

            
            if (grid.IsGameWon()) // Alerte si le jeu est gagné
            {
                showAlert = true;
                alertTime = gameTime.TotalGameTime + alertDuration;
            }

            // Check if the alert time has expired
            if (showAlert && gameTime.TotalGameTime >= alertTime)
            {
                showAlert = false;
            }

            base.Update(gameTime);
            player.Update(gameTime, GraphicsDevice);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            grid.Draw(spriteBatch);
            player.Draw(spriteBatch, playerTexture);
            
            // draw alert
            if (showAlert)
            {
                Vector2 alertPosition = new Vector2(100, 100);
                spriteBatch.DrawString(_font, alertMessage, alertPosition, Color.Red);

                // Background Box Pour alert
                Texture2D rect = new Texture2D(GraphicsDevice, 200, 50);
                Color[] data = new Color[200 * 50];
                for (int i = 0; i < data.Length; ++i) data[i] = Color.Black * 0.8f;
                rect.SetData(data);
                spriteBatch.Draw(rect, new Rectangle((int)alertPosition.X - 10, (int)alertPosition.Y - 10, 220, 70), Color.White);
            }
            
            spriteBatch.End();

            
            base.Draw(gameTime);
        }
    }
}
