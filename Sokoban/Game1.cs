using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Content;
using Sokoban.Content;

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


        private int totalLevels;
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

        private SaveProgress sessionSaving;
        
        
        
        
        //dropdown menu
        Texture2D buttonTexture;
        Texture2D itemTexture;
        DropdownMenu dropdownMenu;
        
        
        
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
            rawLevelData = Content.Load<List<List<string>>>("File");
            
            
            
            // dropdwon menu
            buttonTexture = new Texture2D(GraphicsDevice, 1, 1);
            buttonTexture.SetData(new[] { Color.White });

            itemTexture = new Texture2D(GraphicsDevice, 1, 1);
            itemTexture.SetData(new[] { Color.Gray });

            List<string> items = new List<string> { "Level 1", "Level 2", "Level 3", "Level 4", "Level 5", "Level 6", "Level 7", "Level 8", "Level 9", "Level 10","Level 11", "Level 12" };
            dropdownMenu = new DropdownMenu(_font, buttonTexture, itemTexture, new Vector2(100, 100), 200, items);

            
            //Load Level Data from XML or progress
            LoadLevelOrProgress session = new LoadLevelOrProgress(rawLevelData);
            levelDataList = session.XMLtoLevel();
            
            totalLevels = session.GetTotalLevels();
            
            sessionSaving = new SaveProgress(levelDataList);
            

            
            // levelData par default 10x20
            alert = new Alert();
            grid = new Grid( wallTexture, boxTexture, targetTexture,boxValidTexture,levelDataList[currentLevel]);
            player = new Player(grid.GetPlayerPositionR(), grid.GetPlayerPositionC(), grid);
            
        }
        MouseState currentMouseState;
        MouseState previousMouseState;
        KeyboardState previousKeyboardState;
        
        protected override void Update(GameTime gameTime)
        {
            // dropdown menu
            currentMouseState = Mouse.GetState();
            dropdownMenu.Update(currentMouseState, previousMouseState);
            previousMouseState = currentMouseState;
            if (currentGameState == GameState.MainMenu && dropdownMenu.GetSelectedIndex() != -1)
            { 
                currentLevel = dropdownMenu.GetSelectedIndex();
                currentGameState = GameState.Restart;
            }
            
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
                grid = new Grid(wallTexture, boxTexture, targetTexture, boxValidTexture, levelDataList[currentLevel]);
                player = new Player(grid.GetPlayerPositionR(), grid.GetPlayerPositionC(), grid);
                currentGameState = GameState.Playing;
            }
            else if(currentGameState == GameState.Playing)
            {
                if (currentKeyboardState.IsKeyDown(Keys.S) && previousKeyboardState.IsKeyUp(Keys.S))
                {
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "Save1.xml");
                    sessionSaving.ChangeLevel(grid.GetCells(),currentLevel);
                    sessionSaving.SaveToXML(filePath);
                }
                
                
                if (grid.IsGameWon()) // Alerte si le jeu est gagné
                {
                    alert.SetShowAlert(true);
                    alert.Update(gameTime, grid);
                    if (currentKeyboardState.IsKeyDown(Keys.N) && previousKeyboardState.IsKeyUp(Keys.N))
                    {
                        if(currentLevel  == totalLevels-1)
                        {
                            // currentLevel = 0;
                            dropdownMenu.SetSelectedIndex(-1);
                            currentGameState = GameState.MainMenu;
                        }
                        else{
                        alert.SetShowAlert(false);
                        currentLevel += 1;
                        currentGameState = GameState.Restart;
                        }
                    }
                    
                    
                }

            player.Update(gameTime, GraphicsDevice);
            
            }
            previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            
            if (currentGameState == GameState.MainMenu)
            {
                dropdownMenu.Draw(spriteBatch);

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
