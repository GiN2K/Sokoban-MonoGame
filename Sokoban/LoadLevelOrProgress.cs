using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;


namespace Sokoban.Content;

public class LoadLevelOrProgress
{
    private List<List<string>> rawLevelData;
    private int totalLevels = 0;
    

    public LoadLevelOrProgress(List<List<string>> rawLevelData)
    {
        this.rawLevelData = rawLevelData;
    }
    public int GetTotalLevels()
    {
        return totalLevels;
    }
    
    public List<string[,]> XMLtoLevel()
    {
        List<string[,]> levelDataList = new List<string[,]>();

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

        return levelDataList;
    }
}