using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameParameters
{
    public static Dictionary<ProjectileType, float> ProjectilesDamage = new Dictionary<ProjectileType, float>();

    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Texts/DamageParameters.csv");
        Dictionary<int, ProjectileType> projectileTypes = new Dictionary<int, ProjectileType>();
        try
        {
            string[] text = File.ReadAllLines(path);
            string[] enumsNames = text[0].Split(';');
            string[] values = text[1].Split(';');

            for (int i = 0;i < enumsNames.Length; i++)
            {
                ProjectileType projectileType = (ProjectileType)System.Enum.Parse(typeof(ProjectileType), enumsNames[i], true);
                projectileTypes.Add(i, projectileType);
            }
            for(int i = 0;i < values.Length; i++)
            {
                ProjectilesDamage.Add(projectileTypes[i], float.Parse(values[i]));
            }
        }
        catch(System.Exception e)
        {
            Debug.LogException(e);
        }
    }
}
