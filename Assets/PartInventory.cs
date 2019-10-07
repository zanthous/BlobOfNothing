using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartInventory : MonoBehaviour
{
    //false is empty
    private const int inventorySize = 8;
    private bool[] parts = new bool[inventorySize];
    private int partsFree;


    [SerializeField] private AudioSource attach;
    [SerializeField] private AudioSource detach;

    // Start is called before the first frame update
    void Start()
    {
        var p = GetComponentsInChildren<Part>();
        partsFree = inventorySize - p.Length;
        for(int i = 0; i < p.Length; i++)
        {
            AddPart(p[i].Slot);
        }
       
    }

    public void AddPart(int index)
    {
        Debug.Assert(index >= 0 && index < inventorySize);
        Debug.Assert(parts[index] == false);
        parts[index] = true;
        partsFree--;
        if(attach!=null)
            attach.Play();
    }
    public int AddPart()
    {
        for(int i = 0; i < parts.Length; i++)
        {
            if(parts[i] == false)
            {
                parts[i] = true;
                partsFree--;
                attach?.Play();
                return i;
            }
        }
        Debug.Log("Error - addpart partinventory");
        return -1;
    }

    public int RemovePart()
    {
        for(int i = 0; i < parts.Length; i++)
        {
            if(parts[i] == true)
            {
                parts[i] = false;
                partsFree++;
                return i;
            }
        }
        if(detach!=null)
            detach.Play();
        Debug.Log("Error - removepart partinventory");
        return -1;

    }

    public void RemovePart(int index)
    {
        Debug.Assert(index >= 0 && index < inventorySize);
        Debug.Assert(parts[index] == true);
        parts[index] = false;
        partsFree++;
        if(detach != null)
            detach.Play();
    }

    public bool SpaceFree()
    {
        for(int i = 0; i < parts.Length; i++)
        {
            if(parts[i] == false)
            {
                return true;
            }
        }
        return false;
    }
    public int FirstFree()
    {
        for(int i = 0; i < parts.Length; i++)
        {
            if(parts[i] == false)
            {
                return i;
            }
        }
        return -1;
    }

    public int[] GetFreeSlots()
    {
        int[] slots = new int[partsFree];
        int index = 0;
        for(int i = 0; i < parts.Length; i++)
        {
            if(parts[i] == false)
            {
                slots[index] = i;
                index++;
            }
        }
        return slots;
    }
}
