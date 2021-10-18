using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{
    public enum InteractionType { None, Pickup, Examine };
    //public enum ItemType { Static, Consumable };
    public InteractionType interactionType;
    //public ItemType itemType;

    //only for Examine item interactions
    public string itemDescription; 
    public Sprite itemPicture;

    public UnityEvent customEvent;

    private void Reset() 
    {
        gameObject.layer = 19; // Item layer       
    }

    public void Interact()
    {
        switch(interactionType)
        {
            case InteractionType.Pickup:
                //better use singleton instead of findobjectoftype to access the method
                FindObjectOfType<InventorySystem>().PickUpForInventory(gameObject);
                gameObject.SetActive(false);
                Debug.Log("Pickup");
                break;
            case InteractionType.Examine:
                FindObjectOfType<InteractionSystem>().ExamineItem(this);
                Debug.Log("Examine");
                break;
            default:
                Debug.Log("Null item");
                break;
        }

        SFXManager.Instance.PlaySound(SFXManager.Instance.dash);
        customEvent.Invoke();
    }
}
