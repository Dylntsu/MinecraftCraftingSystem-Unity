using UnityEngine;
using UnityEngine.EventSystems;

public class OutputSlotInteraction : SlotInteraction
{
    private CraftingManager craftingManager;

    protected override void Start()
    {
        base.Start();
        craftingManager = FindFirstObjectByType<CraftingManager>();
    }

    // We override the normal click to prevent the player from placing items here
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (ui.assignedSlot.item == null) return;

        // 1. SHIFT + CLICK LOGIC (CRAFT ALL)
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            HandleShiftClick(); // We use our override version below
        }
        // 2. NORMAL CLICK LOGIC (Single craft)
        else
        {
            // We only allow taking items, not putting them.
            // And only if the hand is empty or has the same stackable item.
            if (!DragManager.Instance.currentDragData.IsHoldingItem || 
               (DragManager.Instance.currentDragData.item == ui.assignedSlot.item))
            {
                // We execute the base pickup logic (HandlePickUp)
                base.OnPointerDown(eventData);
                
                // And we consume 1 set of ingredients
                craftingManager.ConsumeIngredients(1);
            }
        }
    }

    // We override the Shift-Click logic specifically for the OUTPUT
    protected override void HandleShiftClick()
    {
        if (ui.assignedSlot.item == null) return;

        InventoryManager invManager = FindFirstObjectByType<InventoryManager>();
        
        // A. Calculate the theoretical maximum based on ingredients (e.g. I have wood for 20 sticks)
        int maxCrafts = craftingManager.GetMaxCraftableAmount();
        
        // B. Result Data
        ItemData resultItem = ui.assignedSlot.item;
        int amountPerCraft = ui.assignedSlot.stackSize; // Ex: Torches give 4 per craft

        int totalCraftedCount = 0;

        // C. Safe crafting loop
        // We try to craft 1 by 1 (logically) until reaching the maximum or filling the inventory.
        for (int i = 0; i < maxCrafts; i++)
        {
            // We try to add THE RESULT of 1 craft to the inventory
            if (invManager.AddItem(resultItem, amountPerCraft))
            {
                // If it fit in the inventory, we increase the success counter
                totalCraftedCount++;
            }
            else
            {
                // If AddItem returns false, the inventory is full.
                // STOP IMMEDIATELY to avoid wasting materials.
                break;
            }
        }

        // D. Massive consumption of ingredients
        if (totalCraftedCount > 0)
        {
            craftingManager.ConsumeIngredients(totalCraftedCount);
            
            // Visual and sound effect (optional for the future)
            Debug.Log($"Craft-All! {totalCraftedCount} recipe times were crafted.");
        }

        // E. We visually clear the output slot (it will refill only if materials remain)
        ui.UpdateSlotUI();
        craftingManager.CheckForRecipes();
    }

    // Lock Drop and Drag Painting in the output
    public override void OnDrop(PointerEventData eventData) { }
    public override void OnPointerEnter(PointerEventData eventData) { }
}