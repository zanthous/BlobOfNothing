using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartManager : MonoBehaviour
{
    public List<GameObject> freeParts = new List<GameObject>();

    public static PartManager Instance = null;

    private void Awake()
    {
        if(Instance ==null)
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
        var parts = FindObjectsOfType<Part>();
        for(int i = 0; i < parts.Length; i++)
        {
            if(parts[i].Free)
                freeParts.Add(parts[i].gameObject);
        }
    }

    public void AddFreePart(GameObject g)
    {
        Debug.Assert(!freeParts.Contains(g));
        if(freeParts.Contains(g))
        {
            int x = 0;
        }
        freeParts.Add(g);
    }

    public void RemoveFreePart(GameObject g)
    {
        Debug.Assert(freeParts.Contains(g));
        freeParts.Remove(g);
    }
}
