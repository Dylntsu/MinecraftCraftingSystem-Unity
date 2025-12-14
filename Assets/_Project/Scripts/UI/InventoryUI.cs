using UnityEngine;
using System.Collections.Generic; 

public class InventoryUI : MonoBehaviour
{
    // Core References
    [Header("UI References")]
    public GameObject slotPrefab; 
    public InventoryManager manager;
    
    [Header("Generated Slots")]
    private List<SlotUI> uiSlots = new List<SlotUI>(); 

    void Start()
    {
        GenerateSlots();
        UpdateUI();
    }

    private void GenerateSlots()
    {
        // Clears and regenerates UI slots based on inventory size
        uiSlots.Clear();
        
        int slotsToGenerate = manager.slotNumber; 

        for (int i = 0; i < slotsToGenerate; i++)
        {
            GameObject newSlotObject = Instantiate(slotPrefab, transform);

            SlotUI newSlotUI = newSlotObject.GetComponent<SlotUI>();

            newSlotUI.assignedSlot = manager.inventory[i]; 
            
            uiSlots.Add(newSlotUI);
        }
    }
    public void UpdateUI()
    {
        // Refreshes the visual state of all slots
        foreach (var slot in uiSlots)
        {
            slot.UpdateSlotUI(); 
        }
    }
}