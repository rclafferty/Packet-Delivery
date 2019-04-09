using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    readonly Vector2[] spawnPointLocations = {
        new Vector2(-14, 11),
        new Vector2(107, -66.5f),
        new Vector2(251, -1.5f),
        new Vector2(-5, -79.5f)
    };

    readonly string[] spawnPointTitles = {
        "Office Spawn Point",
        "CLA Spawn Point",
        "LLA NE Spawn Point",
        "LLA SW Spawn Point"
    };

    static RespawnManager instance = null;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector2[] SpawnPointLocations
    {
        get
        {
            return spawnPointLocations;
        }
    }

    public Vector2 GetSpawnPointByName(string name)
    {
        string lower = name.ToLower();
        string titleLower = "";

        for (int i = 0; i < spawnPointLocations.Length; i++)
        {
            titleLower = spawnPointTitles[i].ToLower();
            if (titleLower.Contains(lower))
            {
                return spawnPointLocations[i];
            }
        }

        return Vector2.zero;
    }
}
