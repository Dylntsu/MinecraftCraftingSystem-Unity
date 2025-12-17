using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotUI : MonoBehaviour
{
    [Header("UI Components")]
    public Image itemIcon;
    public TextMeshProUGUI itemAmountText;
    
    [HideInInspector] 
    public InventorySlot assignedSlot;

    // Handles only the visual representation of the slot
    public void UpdateSlotUI()
    {
        if (assignedSlot == null) return;


        bool hasItem = assignedSlot.item != null;
        itemIcon.gameObject.SetActive(hasItem);

        if (hasItem)
        {
            itemIcon.sprite = assignedSlot.item.icon;
            itemAmountText.text = assignedSlot.stackSize > 1 ? assignedSlot.stackSize.ToString() : "";
        }
        else 
        { 
            itemAmountText.text = ""; 
        }
    }
}