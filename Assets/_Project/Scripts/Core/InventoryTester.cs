using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    [Header("References")]
    public InventoryManager manager; 
    public CraftingGridManager gridManager; 

    [Header("Test Data")]
    public ItemData itemToAdd; 
    public int amount = 1;

    void Update()
    {
        // Debug Input: Press Space to test adding an item
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (manager != null && itemToAdd != null)
            {
                bool success = manager.AddItem(itemToAdd, amount);
                
                // Shows the result in the Console
                if (success)
                {
                    Debug.Log("Success! Added " + amount + " of " + itemToAdd.displayName);
                }
                else
                {
                    Debug.Log("Failed: Inventory full or stacking error.");
                }
            }
            else
            {
                Debug.LogError("Manager or Item not assigned in the Inspector.");
            }
        }

        // Debug Input: Press T to force check recipes
        if (Input.GetKeyDown(KeyCode.T))
        {
            var cm = FindFirstObjectByType<CraftingManager>();
            
            if (cm != null)
            {
                cm.CheckForRecipes();
                Debug.Log("Manual Recipe Check Triggered via 'T'.");
            }
            else
            {
                Debug.LogWarning("No CraftingManager found in the scene.");
            }
        }
    }
}