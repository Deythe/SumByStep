using System.IO;
using UnityEditor;
using UnityEngine;

public class ToolGenerateActionsEnum 
{
    [MenuItem("Tools/ReGenerate Actions Enum")]
    public static void GenerateEnumFromCSFiles()
    {
        string sourcePath = "Assets/Scripts/Actions";
        string enumName = "ActionsEnum";
        string outputPath = "Assets/Scripts/Enums/" + enumName + ".cs";

        if (!Directory.Exists(sourcePath))
        {
            Debug.LogError("Le dossier source n'existe pas : " + sourcePath);
            return;
        }

        string[] csFiles = Directory.GetFiles(sourcePath, "*.cs");

        if (csFiles.Length == 0)
        {
            Debug.LogWarning("Aucun fichier .cs trouvé dans le dossier : " + sourcePath);
            return;
        }

        string enumContent = "public enum " + enumName + "\n{\n";

        foreach (string filePath in csFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string cleanName = SanitizeEnumName(fileName);
            enumContent += $"    {cleanName},\n";
        }

        enumContent += "}";

        File.WriteAllText(outputPath, enumContent);
        AssetDatabase.Refresh();

        Debug.Log("Enum générée avec succès à : " + outputPath);
    }

    private static string SanitizeEnumName(string name)
    {
        // Remplace les caractères non valides pour un identifiant C#
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c.ToString(), "");
        }

        name = name.Replace(" ", "_");

        if (char.IsDigit(name[0]))
        {
            name = "_" + name;
        }

        return name;
    }
}
