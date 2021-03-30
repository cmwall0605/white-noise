using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {

    public List<Item> items = new List<Item>();

    public int space = 20;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Add(Item item) {

        if(!item.isDefaultItem) {

            if(items.Count < space ) {

                items.Add(item);

                InventoryUI.instance.AddItemInvUI(item);

                return true;

            } else {

                Debug.Log("Not enough room!");
            }
        }
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    public void Remove (Item item) {
        items.Remove(item);
    }
}
