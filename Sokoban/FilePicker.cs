using System.Windows.Forms;

namespace Sokoban
{
    public static class FilePicker
    {
        public static string PickXMLFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
                openFileDialog.Title = "Select an XML File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    return openFileDialog.FileName; // Return the selected file path
                }
            }

            return null; // Return null if no file is selected
        }
    }
}