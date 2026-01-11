using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    
    
    public static Inventory instance;
    
    public int stickAmount = 0;
    public TextMeshProUGUI stickText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        UIManager.instance.SetStickAmount(stickAmount);
    }

    public void AddItem(int itemId, int amount = 1)
    {
        if (itemId == 1) stickAmount += amount;
    }
    
    public void DeleteItem(int itemId, int amount = 1)
    {
        if (itemId == 1)
        {
            if (stickAmount >= amount) stickAmount -= amount;
            else stickAmount = 0;
        }
    }
    
    public int GetItem(int itemId)
    {
        if (itemId == 1) return stickAmount;
        
        return -1;
    }
}

