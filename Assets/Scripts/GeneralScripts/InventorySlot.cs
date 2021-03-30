using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {

    [SerializeField]
    public Image icon;

    [SerializeField]
    public Text itemName;

    Item item;

    public void SetItem(Item newItem) {
        item = newItem;
        itemName.text = item.name;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void DeleteItem() {
        MasterGameScript.instance.character.inv.Remove(item);
        Destroy(transform.gameObject);
    }

    public void UseItem() {
        if(item != null) {
            item.Use();
        }
    }
}
