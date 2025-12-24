using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreativeSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    public Image iconImage; 

    private ItemData _myItemData;
    private int _amountToGive = 64; 

    public void Setup(ItemData itemData)
    {
        _myItemData = itemData;
        if (_myItemData != null)
        {
            iconImage.sprite = _myItemData.icon;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_myItemData == null) return;

        // use findfirstobjectbytype for security
        InventoryManager inventory = Object.FindFirstObjectByType<InventoryManager>();

        if (inventory != null)
        {
            // Use AddItem method that handles space logic
            bool added = inventory.AddItem(_myItemData, _amountToGive);

            if (added)
            {
                // Sound feedback
                if (AudioManager.Instance != null)
                    AudioManager.Instance.PlaySound(AudioManager.Instance.pickUpSound);
                
                Debug.Log($"Creative: Added {_amountToGive} of {_myItemData.name}");
            }
            else
            {
                Debug.Log("Inventory full");
            }
        }
    }
}