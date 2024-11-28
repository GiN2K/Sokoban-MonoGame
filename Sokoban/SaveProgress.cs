using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Sokoban.Content;

public class SaveProgress
{
    private List<string[,]> levels;
    private int lastLevelSaved;
    
    public SaveProgress(List<string[,]> levels)
    {
        this.levels = DeepCopy(levels);
    }
    private List<string[,]> DeepCopy(List<string[,]> levels)
    {
        List<string[,]> copy = new List<string[,]>();
        foreach (var level in levels)
        {
            string[,] newLevel = new string[10, 20];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    newLevel[i, j] = level[i, j];
                }
            }
            copy.Add(newLevel);
        }
        return copy;
    }

    public void ChangeLevel(string[,] levelChanging,int levelIndex)
    {
        levels[levelIndex] = levelChanging;
        lastLevelSaved = levelIndex;
    }

    public void SaveToXML(string filePath)
    {
        // Start the root XML document
        var xnaContent = new XElement("XnaContent",
            new XAttribute(XNamespace.Xmlns + "ns", "Microsoft.Xna.Framework")
        );

        // Add the Asset element with type attribute
        var assetElement = new XElement("Asset",
            new XAttribute("Type", "System.Collections.Generic.List[System.Collections.Generic.List[string]]")
        );

        // Loop through levels and serialize them into XML
        for (int i = 0; i < levels.Count; i++)
        {
            // Create a comment for the level number
            var levelComment = new XComment($"Level {i + 1}");
            if(i == lastLevelSaved)
            {
                levelComment = new XComment($"Level {i + 1} (Level Saved)");
                assetElement.Add(levelComment);
            }
            
            
            // Serialize each level into an Item element
            var levelElement = new XElement("Item");
            string[,] levelData = levels[i];

            for (int row = 0; row < levelData.GetLength(0); row++) // Loop through rows
            {
                StringBuilder rowBuilder = new StringBuilder();

                for (int col = 0; col < levelData.GetLength(1); col++) // Loop through columns
                {
                    rowBuilder.Append(levelData[row, col]);
                    if (col < levelData.GetLength(1) - 1)
                        rowBuilder.Append(",");
                }

                // Add the row as an Item element
                levelElement.Add(new XElement("Item", rowBuilder.ToString()));
            }

            // Add the comment and level to the asset
            assetElement.Add(levelComment);
            assetElement.Add(levelElement);
        }

        // Add the Asset element to the root XnaContent element
        xnaContent.Add(assetElement);

        // Save the XML to the specified file
        var document = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            xnaContent
        );
        document.Save(filePath);
    }
}
