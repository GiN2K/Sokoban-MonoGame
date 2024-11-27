using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;


namespace Sokoban.Content;

public class LoadProgress
{
    private List<List<string>> rawLevelData;
    private List<string[,]> levelDataList = new List<string[,]>();
    private int totalLevels = 0;
    
    protected override void LoadContent()
    {
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
    }
}