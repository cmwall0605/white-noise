using UnityEngine;

public class InventoryUI : MonoBehaviour {

    public static InventoryUI instance;

    Inventory inventory;

    [SerializeField]
    private GameObject itemUITemplate;

    public GameObject itemListContent;

    public void AddItemInvUI(Item item) {

        GameObject button = Instantiate(itemUITemplate) as GameObject;

        button.GetComponent<InventorySlot>().SetItem(item);

        button.transform.SetParent(itemListContent.transform, false);
    }

    void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    void Update() {
        if(Input.GetButtonDown("Inventory")) {
            instance.gameObject.SetActive(!instance.gameObject.activeSelf);
        }
    }
}
