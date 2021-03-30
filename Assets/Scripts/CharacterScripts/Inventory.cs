using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public static Inventory instance;

    public delegate void OnItemChanged();
    public OnItemChanged OnItemChangedCallback;

    public List<Item> items = new List<Item>();

    public int space = 20;

    /// <summary>
    /// 
    /// </summary>
    void Awake () {

        if(instance != null) {

            Debug.LogWarning("More than one instance of Inventory found!");
        }

        instance = this;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Add(Item item) {

        if(!item.isDefaultItem) {

            if(items.Count < space ) {

                items.Add(item);

                if(OnItemChangedCallback != null) {
                    
                    OnItemChangedCallback.Invoke();
                }

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
