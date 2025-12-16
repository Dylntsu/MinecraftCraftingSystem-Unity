using System;

[Serializable] 
/// <summary>
/// Spaces for items in the inventory
/// </summary>
public class InventorySlot
{
    public ItemData item; // Object inside the Slot
    public int stackSize;

    public InventorySlot(ItemData item, int amount) // Slot constructor 
    {
        this.item = item;
        this.stackSize = amount;
    }

    public void AddStack(int amount) // Add to stack
    {
        stackSize += amount;
    }
    
    public void UpdateSlot(ItemData data, int amount)
    {
        item = data;
        stackSize = amount;
    }

    public void RemoveStack(int amount) 
    {
        // Reduces amount; clears slot if empty
        stackSize -= amount;
        if (stackSize <= 0) ClearSlot();
    }

    public void ClearSlot() 
    {
        item = null;
        stackSize = 0;
    }
}