using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonFactory
{
    private static BalloonFactory _instance;
    public static BalloonFactory Instance
    {
        get
        {
            if(null == _instance)
            {
                _instance = new BalloonFactory();
            }

            return _instance;
        }
    }

    private GameObject _balloonPrefab;

    public BalloonFactory()
    {
        _balloonPrefab = Resources.Load("Balloon/Prefabs/Balloon") as GameObject;
    }

    public Balloon GenerateBollon(int id)
    {
        GameObject newBollon = GameObject.Instantiate(_balloonPrefab);
        Balloon target = newBollon.GetComponent<Balloon>();
        target.Init(id);
        target.setScale();

        return target;
    }
}
