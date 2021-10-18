using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public List<GameObject> inventory = new List<GameObject>();
    public GameObject inventoryWindow;
    public Image[] itemImages; 
    public GameObject description;
    public Image descriptionImage;
    public Text descriptionName; 
    public bool showInventory = false;

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowInventory();
        }
        
    }

    void ShowInventory()
    {
        showInventory = !showInventory;
        inventoryWindow.SetActive(showInventory);
        UIUpdate();
    }
    public void PickUpForInventory(GameObject item)
    {
        inventory.Add(item);
        UIUpdate();
    }

    void UIUpdate()
    {
        //first ensure all item image slots are hidden in Inventory UI in order not to show empty ones
        foreach (var i in itemImages) i.gameObject.SetActive(false);
        DescriptionOff();
        //show each picked up item in item image slots in Inventory UI
        for (int i = 0; i < inventory.Count; i++)
        {
            itemImages[i].sprite = inventory[i].GetComponent<SpriteRenderer>().sprite;
            //enable that item window (since all are disabled above by default)
            itemImages[i].gameObject.SetActive(true);
        }
    }

    public void DescriptionOn(int id)
    {
        descriptionImage.sprite = itemImages[id].sprite;
        descriptionName.text = inventory[id].name;
        descriptionImage.gameObject.SetActive(true);
        descriptionName.gameObject.SetActive(true);
    }

    public void DescriptionOff()
    {
        descriptionImage.gameObject.SetActive(false);
        descriptionName.gameObject.SetActive(false);
    }
}
