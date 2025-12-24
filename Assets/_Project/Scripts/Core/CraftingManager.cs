using UnityEngine;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
    [Header("Dependencies")]
    public InventoryManager inventoryManager;

    [Header("Recipe Database")]
    public List<CraftingRecipe> availableRecipes;

    private CraftingRecipe currentActiveRecipe;

    public void CheckForRecipes()
    {
        var gridSlots = FindFirstObjectByType<CraftingGridManager>().GetGridSlots();
        
        // If the table is empty, clear and exit
        if (IsGridEmpty(gridSlots))
        {
            currentActiveRecipe = null;
            UpdateOutputSlot();
            return;
        }

        currentActiveRecipe = null;

        foreach (var recipe in availableRecipes)
        {
            if (recipe.isShapeless)
            {
                if (CheckShapelessRecipe(recipe, gridSlots))
                {
                    currentActiveRecipe = recipe;
                    break; 
                }
            }
            else
            {
                // Use the new relative logic
                if (CheckShapedRecipeRelative(recipe, gridSlots))
                {
                    currentActiveRecipe = recipe;
                    break;
                }
            }
        }

        UpdateOutputSlot();
    }

    private bool IsGridEmpty(List<SlotUI> slots)
    {
        foreach(var slot in slots) if (slot.assignedSlot.item != null) return false;
        return true;
    }
    
    private bool CheckShapedRecipeRelative(CraftingRecipe recipe, List<SlotUI> gridSlots)
    {
        ItemData[,] tableGrid = new ItemData[3, 3];
        ItemData[,] recipeGrid = new ItemData[3, 3];

        for (int i = 0; i < 9; i++)
        {
            int x = i % 3; // Column (0, 1, 2)
            int y = i / 3; // Row (0, 1, 2)
            
            // Fill data from the table
            if (gridSlots[i].assignedSlot.item != null)
                tableGrid[x, y] = gridSlots[i].assignedSlot.item;

            // Fill data from the recipe (if it exists)
            if (i < recipe.shapedGrid.Count && recipe.shapedGrid[i] != null)
                recipeGrid[x, y] = recipe.shapedGrid[i];
        }

        // Calculate the limits (Bounding Box) of both
        // cuts the empty space around the items
        RectInt tableBounds = GetGridBounds(tableGrid);
        RectInt recipeBounds = GetGridBounds(recipeGrid);

                // Verification of the same size
        if (tableBounds.width != recipeBounds.width || tableBounds.height != recipeBounds.height)
        {
            return false;
        }
        // Comparison item by item
        for (int x = 0; x < tableBounds.width; x++)
        {
            for (int y = 0; y < tableBounds.height; y++)
            {
                // We get the item from the table at its relative position (offset)
                ItemData tableItem = tableGrid[tableBounds.x + x, tableBounds.y + y];
                
                // We get the item from the recipe at its relative position
                ItemData recipeItem = recipeGrid[recipeBounds.x + x, recipeBounds.y + y];

                if (tableItem != recipeItem) return false;
            }
        }

        return true;
    }

    // Mathematical helper function to find the occupied rectangle
    private RectInt GetGridBounds(ItemData[,] grid)
    {
        int minX = 3, maxX = -1;
        int minY = 3, maxY = -1;

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (grid[x, y] != null)
                {
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;
                }
            }
        }

        // If the grid is empty, we return 0
        if (maxX == -1) return new RectInt(0, 0, 0, 0);

        // We return the rectangle (x, y, width, height)
        // The +1 in width/height is because if it goes from 0 to 0, the size is 1.
        return new RectInt(minX, minY, (maxX - minX) + 1, (maxY - minY) + 1);
    }

    // --- SHAPELESS LOGIC 
    private bool CheckShapelessRecipe(CraftingRecipe recipe, List<SlotUI> gridSlots)
    {
        Dictionary<ItemData, int> tableContents = new Dictionary<ItemData, int>();

        foreach (var slot in gridSlots)
        {
            if (slot.assignedSlot.item != null)
            {
                ItemData item = slot.assignedSlot.item;
                int amount = slot.assignedSlot.stackSize;
                if (tableContents.ContainsKey(item)) tableContents[item] += amount;
                else tableContents.Add(item, amount);
            }
        }

        foreach (var required in recipe.shapelessIngredients)
        {
            if (!tableContents.ContainsKey(required.itemData)) return false;
            if (tableContents[required.itemData] < required.amount) return false;
        }

        foreach (var kvp in tableContents)
        {
            bool isRequired = recipe.shapelessIngredients.Exists(req => req.itemData == kvp.Key);
            if (!isRequired) return false;
        }
        
        return true;
    }

    public void ConsumeIngredients(int times = 1)
    {
        if (currentActiveRecipe == null) return;

        var gridSlots = FindFirstObjectByType<CraftingGridManager>().GetGridSlots();

        if (currentActiveRecipe.isShapeless)
        {
            // SHAPELESS CONSUMPTION
            foreach (var required in currentActiveRecipe.shapelessIngredients)
            {
                int remainingToRemove = required.amount * times;
                foreach (var slotUI in gridSlots)
                {
                    if (remainingToRemove <= 0) break;
                    if (slotUI.assignedSlot.item == required.itemData)
                    {
                        int take = Mathf.Min(slotUI.assignedSlot.stackSize, remainingToRemove);
                        slotUI.assignedSlot.RemoveStack(take);
                        remainingToRemove -= take;
                        slotUI.UpdateSlotUI();
                    }
                }
            }
        }
        else
        {
            // shaped recipe any item present on the table MUST be consumed.
            foreach (var slot in gridSlots)
            {
                if (slot.assignedSlot.item != null)
                {
                    slot.assignedSlot.RemoveStack(1 * times);
                    slot.UpdateSlotUI();
                }
            }
        }

        currentActiveRecipe = null;
        CheckForRecipes();
        AudioManager.Instance.PlaySound(AudioManager.Instance.craftSuccessSound);
    }

    private void UpdateOutputSlot()
    {
        GameObject outputObj = GameObject.Find("OutputSlot");
        if (outputObj != null)
        {
            SlotUI outputSlotUI = outputObj.GetComponent<SlotUI>();
            
            if (currentActiveRecipe != null)
                outputSlotUI.assignedSlot.UpdateSlot(currentActiveRecipe.resultItem, currentActiveRecipe.resultAmount);
            else
                outputSlotUI.assignedSlot.ClearSlot();
            
            outputSlotUI.UpdateSlotUI();
        }
    }

    public int GetMaxCraftableAmount()
    {
        if (currentActiveRecipe == null) return 0;

        var gridSlots = FindFirstObjectByType<CraftingGridManager>().GetGridSlots();
        int minStack = int.MaxValue;
        bool foundAnyMaterial = false;

        // Iterate through all grid slots
        foreach (var slotUI in gridSlots)
        {
            var slot = slotUI.assignedSlot;
            
            // If the slot has an item, that item is a potential "limiter"
            if (slot.item != null)
            {
                foundAnyMaterial = true;
                if (slot.stackSize < minStack)
                {
                    minStack = slot.stackSize;
                }
            }
        }

        // If we don't find materials return 0
        if (!foundAnyMaterial) return 0;

        return minStack;
    }
}