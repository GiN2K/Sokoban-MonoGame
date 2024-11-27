using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

public class ArrayToXmlSerializer
{
    public static void SerializeToFile(string[,] array, string filePath)
    {
        // Create the root XML structure
        XDocument doc = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement("XnaContent",
                new XAttribute(XNamespace.Xmlns + "ns", "Microsoft.Xna.Framework"),
                new XElement("Asset",
                    new XAttribute("Type", "System.Collections.Generic.List[System.Collections.Generic.List[string]]"),
                    new XElement("Item", Serialize2DArray(array))
                )
            )
        );

        // Save the XML to the specified file
        doc.Save(filePath);
    }

    private static IEnumerable<XElement> Serialize2DArray(string[,] array)
    {
        List<XElement> rows = new List<XElement>();

        for (int i = 0; i < array.GetLength(0); i++) // Iterate through rows
        {
            StringBuilder rowBuilder = new StringBuilder();

            for (int j = 0; j < array.GetLength(1); j++) // Iterate through columns
            {
                rowBuilder.Append(array[i, j]);
                if (j < array.GetLength(1) - 1)
                    rowBuilder.Append(",");
            }

            // Add a row as an <Item> element
            rows.Add(new XElement("Item", rowBuilder.ToString()));
        }

        return rows;
    }
}