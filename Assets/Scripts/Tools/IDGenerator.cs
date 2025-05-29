using UnityEngine;

public static class IDGenerator
{
    private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int LENGTH = 6;
    
    public static string GenerateID(string name)
    {
        string id = "";
        
        for (int i = 0; i < LENGTH; i++)
        {
            id+= CHARS[Random.Range(0, CHARS.Length)];
        }
        
        name+=id;
        return name;
    }
}