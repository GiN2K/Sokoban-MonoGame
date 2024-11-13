using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content;

namespace Sokoban
{
    public class Game1 : Game
    {
        public enum GameState
        {
            MainMenu,
            Playing,
            Restart
        }


        private int totalLevels = 0;
        private int currentLevel = 0;
        public GameState currentGameState = GameState.MainMenu;
        
        
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        
        private Player player;
        private Grid grid;

        private Texture2D wallTexture;
        private Texture2D boxTexture;
        private Texture2D targetTexture;
        private Texture2D playerTexture;
        private Texture2D boxValidTexture;
        
        
        
        private Texture2D playButtonTexture;
        private Rectangle playButtonRect = new Rectangle(400, 150, 200, 200);

        private List<List<string>> rawLevelData;
        private List<string[,]> levelDataList = new List<string[,]>();

        
        public static SpriteFont _font;
        

        private string alertMessage = "Felicitations! Tu as reussi";
        private TimeSpan alertDuration = TimeSpan.FromSeconds(10); // alert duraion, je vais la changer pour quil reste avant que lutilisateur change le niveau
        private TimeSpan alertTime;
        
        private Alert alert;

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
            playButtonTexture = Content.Load<Texture2D>("play");
            
            // levelDataList = Content.Load<List<List<string>>>("File"); 

            rawLevelData = Content.Load<List<List<string>>>("File");
            
            
            foreach (var level in rawLevelData)
            {
                string[,] levelData = new string[10, 20];
                for (int i = 0; i < 10; i++)
                {
                    var rowAsArray = level[i].Split(',');

                    for (int j = 0; j < 20; j++)
                    {
                        levelData[i, j] = rowAsArray[j];
                    }
                }
                levelDataList.Add(levelData);
                totalLevels++;
            }
            
            
            

            
            // levelData par default 10x20
            alert = new Alert();
            grid = new Grid( wallTexture, boxTexture, targetTexture,boxValidTexture,levelDataList[currentLevel],alert.GetShowAlert());
            player = new Player(grid.GetPlayerPositionR(), grid.GetPlayerPositionC(), grid);
            
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            if(currentKeyboardState.IsKeyDown(Keys.Space))
            {
                currentGameState = GameState.Restart;
            }
            if (currentGameState == GameState.MainMenu)
            {
                if (playButtonRect.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                {
                    currentGameState = GameState.Playing; // Start the game
                }
            }
            else if (currentGameState == GameState.Restart)
            {
                alert = new Alert();
                grid = new Grid(wallTexture, boxTexture, targetTexture, boxValidTexture, levelDataList[currentLevel], alert.GetShowAlert());
                player = new Player(grid.GetPlayerPositionR(), grid.GetPlayerPositionC(), grid);
                currentGameState = GameState.Playing;
            }
            else if(currentGameState == GameState.Playing)
            {
                if (grid.IsGameWon()) // Alerte si le jeu est gagné
                {
                    alert.SetShowAlert(true);
                    alert.Update(gameTime, grid);
                }

            player.Update(gameTime, GraphicsDevice);
            
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            if (currentGameState == GameState.MainMenu)
            {
                

                Texture2D rect = new Texture2D(GraphicsDevice, 200, 50);
                Color[] data = new Color[200 * 50];
                for (int i = 0; i < data.Length; ++i) data[i] = Color.Black * 0.8f;
                rect.SetData(data);
                spriteBatch.Draw(rect, new Rectangle(0, 0, 1000, 500), Color.Black);
                spriteBatch.Draw(playButtonTexture, playButtonRect, Color.White);
            }
            
            
            else if (currentGameState == GameState.Playing)
            {
            grid.Draw(spriteBatch);
            player.Draw(spriteBatch, playerTexture);
            
            // draw alert
            if (alert.GetShowAlert())
            {
                alert.Draw(gameTime, spriteBatch, GraphicsDevice);
            }
            
            }
            
            spriteBatch.End();

            
            base.Draw(gameTime);
        }
    }
}
