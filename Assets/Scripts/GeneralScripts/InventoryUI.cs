using UnityEngine;

public class InventoryUI : MonoBehaviour {

    Inventory inventory;

    void Start() {
        // inventory = Inventory.instance;
        // inventory.OnItemChangedCallback += UpdateUI;
    }

    void Update() {
        
    }

    void UpdateUI () {
        Debug.Log("Updating UI");
    }
}
