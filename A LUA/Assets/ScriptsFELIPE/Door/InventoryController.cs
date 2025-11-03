using UnityEngine;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    public List<int> keys = new List<int>();

    public void AddItem(int idItem)
    {
        keys.Add(idItem);
    }

    public void RemoveItem(int idItem)
    {
        keys.Remove(idItem);
    }
    
}
