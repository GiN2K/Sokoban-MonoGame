﻿using System;
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
        
        private Texture2D loadButtonTexture;
        private Rectangle loadButtonRect = new Rectangle(370, 250, 250, 80);

        
        
        private Texture2D playButtonTexture;
        private Rectangle playButtonRect = new Rectangle(370, 150, 250, 80);

        private List<List<string>> rawLevelData;
        private List<string[,]> levelDataList = new List<string[,]>();


        private List<List<string>> rawLoadedLevelData;
        private List<string[,]> loadedLevelDataList = new List<string[,]>();

        
        public static SpriteFont _font;
        
        private Alert alert;

        private SaveProgress sessionSaving;
        
        private HashSet<int> completedLevels = new HashSet<int>();

        private void MarkLevelAsCompleted(int levelNumber)
        {
            if (!completedLevels.Contains(levelNumber))
            {
                completedLevels.Add(levelNumber);
            }
        }

        private string GetCompletedLevelsAsString()
        {
            return string.Join(",", completedLevels);
        }
        
        
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
            loadButtonTexture = Content.Load<Texture2D>("load");

            rawLevelData = Content.Load<List<List<string>>>("File");
            rawLoadedLevelData = Content.Load<List<List<string>>>("LoadedLevel");

            
            
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
            
            
            LoadLevelOrProgress sessionLoaded = new LoadLevelOrProgress(rawLoadedLevelData);
            loadedLevelDataList = sessionLoaded.XMLtoLevel();

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
            
            KeyboardState currentKeyboardState = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();
            if (currentGameState == GameState.MainMenu && dropdownMenu.GetSelectedIndex() != -1)
            { 
                currentLevel = dropdownMenu.GetSelectedIndex();
                currentGameState = GameState.Restart;
            }
            if (currentGameState == GameState.MainMenu)
            {
                if (playButtonRect.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                {
                    currentLevel = 0;
                    LoadLevelOrProgress session = new LoadLevelOrProgress(rawLevelData);
                    levelDataList = session.XMLtoLevel();
                    grid = new Grid(wallTexture, boxTexture, targetTexture, boxValidTexture, levelDataList[currentLevel]);
                    player = new Player(grid.GetPlayerPositionR(), grid.GetPlayerPositionC(), grid);
                    currentGameState = GameState.Playing; // Start the game
                }
                if (loadButtonRect.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                {
                    levelDataList = loadedLevelDataList;
                    grid = new Grid(wallTexture, boxTexture, targetTexture, boxValidTexture, levelDataList[currentLevel]);
                    player = new Player(grid.GetPlayerPositionR(), grid.GetPlayerPositionC(), grid);                    
                    currentGameState = GameState.Playing;
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
                    // string filePath = @"F:\Users\Amine\RiderProjects\Sokoban-MonoGame\Sokoban\Content\LoadedLevel.xml";
                  
                    string baseDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    baseDirectory = baseDirectory.Substring(0, baseDirectory.Length - 4);
                    string saveDirectory = Path.Combine(baseDirectory, "Content");
                    string filePath = Path.Combine(saveDirectory, "LoadedLevel.xml");
                    sessionSaving.ChangeLevel(grid.GetCells(),currentLevel);
                    sessionSaving.SaveToXML(filePath);

                }
                
                // avec echap on revien au menu
                if (currentKeyboardState.IsKeyDown(Keys.Escape)&& previousKeyboardState.IsKeyUp(Keys.Escape))
                {
                    dropdownMenu.SetSelectedIndex(-1);
                    currentGameState = GameState.MainMenu;
                    // on recharge la partie
                    // LoadLevelOrProgress session = new LoadLevelOrProgress(rawLevelData);
                    // levelDataList = session.XMLtoLevel();
                    // grid = new Grid(wallTexture, boxTexture, targetTexture, boxValidTexture, levelDataList[currentLevel]);
                    // player = new Player(grid.GetPlayerPositionR(), grid.GetPlayerPositionC(), grid);
                    
                }
             
                
                
                if (grid.IsGameWon() && !alert.GetShowAlert())
                {

                    MarkLevelAsCompleted(currentLevel + 1);

                    string completedLevels = GetCompletedLevelsAsString();

                    string baseDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
                    baseDirectory = baseDirectory.Substring(0, baseDirectory.Length - 4);
                    string contentDirectory = Path.Combine(baseDirectory, "Content");

                    string fileXmlPath = Path.Combine(contentDirectory, "File.xml");
                    string xsltPath = Path.Combine(contentDirectory, "GenerateLevelsStatus.xslt");
                    string outputXmlPath = Path.Combine(contentDirectory, "LevelsStatus.xml");

                    

                    if (File.Exists(fileXmlPath) && File.Exists(xsltPath))
                    {
                        
                            XSLTProcessor.GenerateLevelsStatus(fileXmlPath, xsltPath, outputXmlPath, completedLevels);
                        
                    }
                    



                    alert.SetShowAlert(true);
                    alert.Update(gameTime, grid);
                }

                if (alert.GetShowAlert() && currentKeyboardState.IsKeyDown(Keys.N) && previousKeyboardState.IsKeyUp(Keys.N))
                {
                    if (currentLevel >= totalLevels - 1)
                    {
                        dropdownMenu.SetSelectedIndex(-1);
                        currentGameState = GameState.MainMenu;
                    }
                    else
                    {
                        alert.SetShowAlert(false); 
                        currentLevel++; 
                        grid = new Grid(wallTexture, boxTexture, targetTexture, boxValidTexture, levelDataList[currentLevel]);
                        player = new Player(grid.GetPlayerPositionR(), grid.GetPlayerPositionC(), grid);
                        currentGameState = GameState.Restart;
                    }
                }

            player.Update(gameTime, GraphicsDevice);
            
            }
            
            if(currentKeyboardState.IsKeyDown(Keys.Space))
            {
                currentGameState = GameState.Restart;
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

                //
                // Texture2D rect = new Texture2D(GraphicsDevice, 200, 50);
                // Color[] data = new Color[200 * 50];
                // for (int i = 0; i < data.Length; ++i) data[i] = Color.Black * 0.8f;
                // rect.SetData(data);
                // spriteBatch.Draw(rect, new Rectangle(0, 0, 1000, 500), Color.Black);
                //
                
                
                spriteBatch.Draw(playButtonTexture, playButtonRect, Color.White);
                
                spriteBatch.Draw(loadButtonTexture, loadButtonRect, Color.White);
                
                // tutorial box
                Rectangle boxRect = new Rectangle(350, 0, 300, 100); // X, Y, Width, Height
                string message = "Espace pour redemarrer le niveau\nN pour passer au niveau prochain\nS pour sauvgarder la partie\nEchap pour aller au menu principale";

                Texture2D boxTexture = new Texture2D(GraphicsDevice, 1, 1);
                boxTexture.SetData(new[] { Color.Gray });
                spriteBatch.Draw(boxTexture, boxRect, Color.Gray * 0.8f);

                Vector2 textSize = _font.MeasureString(message);
                Vector2 textPosition = new Vector2(
                    boxRect.X + (boxRect.Width - textSize.X) / 2,
                    boxRect.Y + (boxRect.Height - textSize.Y) / 2
                );

                spriteBatch.DrawString(_font, message, textPosition, Color.White);
                
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
