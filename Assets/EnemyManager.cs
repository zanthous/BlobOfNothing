using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();

    public static EnemyManager Instance = null;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    void Start()
    {
        var e = FindObjectsOfType<Enemy>();
        for(int i = 0; i < e.Length; i++)
        {
            enemies.Add(e[i].gameObject);
        }
    }

    public void AddEnemy(GameObject g)
    {
        Debug.Assert(!enemies.Contains(g));
        enemies.Add(g);
    }

    public void RemoveEnemy(GameObject g)
    {
        Debug.Assert(enemies.Contains(g));
        enemies.Remove(g);
    }
}
