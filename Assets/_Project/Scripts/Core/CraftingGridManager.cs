using UnityEngine;
using System.Collections.Generic;

public class CraftingGridManager : MonoBehaviour
{
    [Header("Input Slots")]
    public List<SlotUI> inputSlots = new List<SlotUI>(); 

    [Header("Output Slot")]
    public SlotUI outputSlot;

    [Header("Dependencies")]
    public CraftingManager craftingManager; 

    private void Start()
    {
        InitializeCraftingSlots();
        // Listeners Soon...
    }

    // Initialize slots
    private void InitializeCraftingSlots()
    {
        // make sure each SlotUI has an InventorySlot associated to avoid NREs
        foreach (SlotUI slotUI in inputSlots)
        {
            slotUI.assignedSlot = new InventorySlot(null, 0); 
            slotUI.UpdateSlotUI(); 
        }

        if (outputSlot != null)
        {
            outputSlot.assignedSlot = new InventorySlot(null, 0);
            outputSlot.UpdateSlotUI();
        }
    }

    /// <summary>
    /// Returns the list of UI slots in the crafting grid.
    /// Used by CraftingManager to check recipes and consume ingredients.
    /// </summary>
    public List<SlotUI> GetGridSlots()
    {
        return inputSlots;
    }
}