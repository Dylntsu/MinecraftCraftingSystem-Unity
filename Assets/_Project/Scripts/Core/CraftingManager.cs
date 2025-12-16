using UnityEngine;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
    [Header("Dependencies")]
    public InventoryManager inventoryManager;

    [Header("Recipe Database")]
    public List<CraftingRecipe> availableRecipes;

    public bool CanCraft(CraftingRecipe recipe)
    {
        // Verify the required ingredients in the recipe
        foreach (RequiredItem required in recipe.requiredItems)
        {
            // Search for the total amount of the required item in the entire inventory
            int totalOwned = CountItem(required.itemData); 

            // If the amount we have is less than required, canot craft
            if (totalOwned < required.amount)
            {
                Debug.Log($"Failed to craft {recipe.resultItem.displayName}: Missing {required.itemData.displayName}. Needed {required.amount}, have {totalOwned}.");
                return false; // Fails on the first requirement not met
            }
        }

        // If the loop finishes without returning 'false', it means all requirements are met.
        return true; 
    }

    private int CountItem(ItemData itemToCount)
    {
        int count = 0;
        
        // Iterate over all logical inventory slots
        foreach (InventorySlot slot in inventoryManager.inventory)
        {
            // If the slot contains the item we are looking for
            if (slot.item == itemToCount) 
            {
                // Add the stack amount
                count += slot.stackSize;
            }
        }
        return count;
    }

    public CraftingRecipe FindMatchingRecipe(List<RequiredItem> currentInput)
    {
        // Search for a matching recipe in the database
        foreach (CraftingRecipe recipe in availableRecipes)
        {
            // Verify the number of ingredients: they must be equal
            if (recipe.requiredItems.Count != currentInput.Count)
            {
                continue; // They can't match if they have different number of items
            }

            // Verify that each ingredient and quantity match (Shapeless Crafting)
            bool recipeMatches = true;

            foreach (RequiredItem required in recipe.requiredItems)
            {
                // Search for the required item in the player's input
                RequiredItem playerInputItem = currentInput.Find(i => i.itemData == required.itemData);

                if (playerInputItem == null || playerInputItem.amount < required.amount)
                {
                    // If the item is missing OR the quantity is less
                    recipeMatches = false;
                    break;
                }
            }

            if (recipeMatches)
            {
                return recipe; // recipe found
            }
        }

        return null; // If the loop ends, no recipe was found.
    }
}