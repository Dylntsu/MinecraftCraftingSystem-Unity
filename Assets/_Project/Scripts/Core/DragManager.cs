using UnityEngine;

// Class that holds the information of the item being dragged
public class DragData
{
    public ItemData item;
    public int amount;
    public SlotUI originalSlot; // Reference to the slot where the item was taken from
    
    public bool IsHoldingItem => item != null && amount > 0;

    public void Clear()
    {
        item = null;
        amount = 0;
        originalSlot = null;
    }
}


// Singleton Manager
public class DragManager : MonoBehaviour
{
    // Singleton
    public static DragManager Instance { get; private set; }

    [Header("Dependencies")]
    public DragVisual dragVisualPrefab; 

    // Logical state of the dragged item
    public DragData currentDragData { get; private set; } = new DragData();
    
    // Visual instance in the scene
    private DragVisual visualInstance; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            // Key Step: Instantiate visual object on canvas
            if (dragVisualPrefab != null)
            {
                // Instantiate as child of manager
                visualInstance = Instantiate(dragVisualPrefab, transform); 
                visualInstance.HideVisual();
            }
            else
            {
                Debug.LogError("Drag Visual Prefab is not assigned in the Drag Manager Inspector!");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Visual object follows mouse if holding item
        if (visualInstance != null && currentDragData.IsHoldingItem)
        {
            visualInstance.transform.position = Input.mousePosition;
        }
    }
    public void StartDrag(ItemData item, int amount, SlotUI originalSlot)
    {
        currentDragData.item = item;
        currentDragData.amount = amount;
        currentDragData.originalSlot = originalSlot;

        visualInstance.SetVisualData(item.icon, amount);
    }

    public void EndDrag()
    {
        currentDragData.Clear();
        visualInstance.HideVisual();
    }
}