using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItemHandler : MonoBehaviour, IPointerDownHandler, IEndDragHandler
{
    [Header("Dependencies")]
    private SlotUI slotUI;

    private void Awake()
    {
        slotUI = GetComponent<SlotUI>();
    }

    // Called when the user clicks the slot
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // Handle full stack drag
            HandleLeftClick(eventData);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Handle split drag (Take 1 item)
            HandleRightClick();
        }
    }

    // Called when the user releases the mouse
    public void OnEndDrag(PointerEventData eventData)
    {

        // If the item wasn't dropped on a valid slot, return it to the original slot
        if (DragManager.Instance.currentDragData.IsHoldingItem)
        {
            SlotUI originalSlot = DragManager.Instance.currentDragData.originalSlot;
            ItemData dataToReturn = DragManager.Instance.currentDragData.item;
            int amountToReturn = DragManager.Instance.currentDragData.amount;
            
            originalSlot.assignedSlot.UpdateSlot(dataToReturn, originalSlot.assignedSlot.stackSize + amountToReturn);
            originalSlot.UpdateSlotUI();
            
            // Clear global drag state
            DragManager.Instance.EndDrag();
        }
    }

    // ----------------------------------------------------------------------
    // STACK MANIPULATION LOGIC
    // ----------------------------------------------------------------------

    private void HandleLeftClick(PointerEventData eventData)
    {
        // Only if current slot has an item AND we are not dragging anything
        if (slotUI.assignedSlot.item != null && !DragManager.Instance.currentDragData.IsHoldingItem)
        {
            // get data before clearing
            ItemData dataToDrag = slotUI.assignedSlot.item;
            int amountToDrag = slotUI.assignedSlot.stackSize;
            
            DragManager.Instance.StartDrag(dataToDrag, amountToDrag, slotUI);
            
            slotUI.assignedSlot.ClearSlot();
            slotUI.UpdateSlotUI();
        }
    }

    private void HandleRightClick()
    {
        if (slotUI.assignedSlot.stackSize > 1 && !DragManager.Instance.currentDragData.IsHoldingItem)
        {
            // Reduce original stack
            slotUI.assignedSlot.stackSize -= 1;
            
            // Get Data (single item)
            ItemData dataToDrag = slotUI.assignedSlot.item;
            int amountToDrag = 1;

            DragManager.Instance.StartDrag(dataToDrag, amountToDrag, slotUI);
            
            slotUI.UpdateSlotUI();
        }
    }
}