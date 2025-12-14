using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System;


public class SlotUI : MonoBehaviour
{
    [Header("UI References")]
    public Image itemIcon;
    public TextMeshProUGUI itemAmountText;
    
    [HideInInspector] 
    public InventorySlot assignedSlot;

    public void UpdateSlotUI()
    {
        // Updates icon and text visibility based on item data
        if (assignedSlot.item == null)
        {
            itemIcon.gameObject.SetActive(false);
            itemAmountText.text = "";
        }
        else
        {
            itemIcon.sprite = assignedSlot.item.icon;
            itemAmountText.text = assignedSlot.stackSize.ToString();
            // Show icon
            itemIcon.gameObject.SetActive(true);
        }
    }
}