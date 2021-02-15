using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigNode
{
    public int id = 1;
    public float size = 1;
    public int score = 2;
    public float mass = 1;
    public string imagePath = "";
    public int nextID = 2;

    public ConfigNode(int id, float size, string imagePath, int nextID, int score, float mass)
    {
        this.id = id;
        this.size = size;
        this.imagePath = imagePath;
        this.nextID = nextID;
        this.score = score;
        this.mass = mass;
    }
}


public class BalloonConfig
{
    private Dictionary<int, ConfigNode> _configs = new Dictionary<int, ConfigNode>();

    private static BalloonConfig _instance;
    public static BalloonConfig Instance
    {
        get
        {
            if(null == _instance)
            {
                _instance = new BalloonConfig();
            }
            return _instance;
        }
    }

    public BalloonConfig()
    {
        _configs.Add(1, new ConfigNode(1, 0.5f, "Balloon/Images/planet_pluto", 2, 2, 10));
        _configs.Add(2, new ConfigNode(2, 0.7f, "Balloon/Images/planet_moon", 3, 4, 20));
        _configs.Add(3, new ConfigNode(3, 0.9f, "Balloon/Images/planet_mercury", 4, 8, 30)); 
        _configs.Add(4, new ConfigNode(4, 1.1f, "Balloon/Images/planet_mars", 5, 16, 40)); 
        _configs.Add(5, new ConfigNode(5, 1.4f, "Balloon/Images/planet_venus", 6, 32, 50)); 
        _configs.Add(6, new ConfigNode(6, 1.7f, "Balloon/Images/planet_earth", 7, 64, 60)); 
        _configs.Add(7, new ConfigNode(7, 2.0f, "Balloon/Images/planet_neptune", 8, 128, 70)); 
        _configs.Add(8, new ConfigNode(8, 2.3f, "Balloon/Images/planet_uranus", 9, 256, 80)); 
        _configs.Add(9, new ConfigNode(9, 2.6f, "Balloon/Images/planet_saturn", 10, 512, 90));
        _configs.Add(10, new ConfigNode(10, 2.9f, "Balloon/Images/planet_jupiter", 11, 1024, 100));
        _configs.Add(11, new ConfigNode(11, 3.2f, "Balloon/Images/planet_sun", -1, 2048, 110));
    }

    public ConfigNode GetConfig(int id)
    {
        return _configs[id];
    }
}
