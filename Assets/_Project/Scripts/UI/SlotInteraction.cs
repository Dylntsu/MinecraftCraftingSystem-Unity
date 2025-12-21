using UnityEngine;
using UnityEngine.EventSystems;

// Añadimos IPointerEnterHandler para detectar cuando el mouse entra mientras arrastramos
public class SlotInteraction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IDropHandler, IPointerEnterHandler
{
    protected SlotUI ui;
    protected static float lastPickUpTime; 

    private void Awake() => ui = GetComponent<SlotUI>();

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // --- NUEVO: SHIFT + CLICK ---
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            HandleShiftClick();
            return;
        }

        if (ui.assignedSlot == null || ui.assignedSlot.item == null) return;

        if (!DragManager.Instance.currentDragData.IsHoldingItem)
        {
            HandlePickUp(eventData);
            lastPickUpTime = Time.time; 
        }
    }

    // --- NUEVO: DRAG PAINTING (IPointerEnterHandler) ---
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        // Si el botón izquierdo está presionado y tenemos algo en la mano
        if (Input.GetMouseButton(0) && DragManager.Instance.currentDragData.IsHoldingItem)
        {
            HandleDragPainting();
        }
    }

    protected virtual void HandleDragPainting()
    {
        var drag = DragManager.Instance.currentDragData;
        var slot = ui.assignedSlot;

        // Solo "pintamos" si el slot está vacío o tiene el mismo item y hay espacio
        if (slot.item == null || (slot.item == drag.item && slot.stackSize < slot.item.maxStackSize))
        {
            if (drag.amount > 0)
            {
                if (slot.item == null) slot.UpdateSlot(drag.item, 1);
                else slot.stackSize += 1;

                drag.amount -= 1;

                // Si se acaba el stack en la mano, terminamos el drag
                if (drag.amount <= 0) DragManager.Instance.EndDrag();
                else DragManager.Instance.UpdateDragVisual(); // Actualiza el número en el cursor

                ui.UpdateSlotUI();
                NotifyCrafting();
            }
        }
    }

    protected virtual void HandleShiftClick()
    {
        var slot = ui.assignedSlot;
        if (slot == null || slot.item == null) return;

        var invManager = Object.FindFirstObjectByType<InventoryManager>();
        if (invManager != null)
        {
            // try to add the stack to the inventory
            int initialAmount = slot.stackSize;
            if (invManager.AddItem(slot.item, initialAmount))
            {
                slot.ClearSlot();
                ui.UpdateSlotUI();
                NotifyCrafting();
                Debug.Log("Shift-Click: Item movido rápidamente.");
            }
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (DragManager.Instance.currentDragData.IsHoldingItem && Time.time > lastPickUpTime + 0.1f)
        {
            if (!eventData.dragging) HandleDrop();
        }
    }

    public void OnBeginDrag(PointerEventData eventData) { }
    public void OnDrag(PointerEventData eventData) { }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (DragManager.Instance.currentDragData.IsHoldingItem)
        {
            HandleDrop();
        }
    }
    
    protected virtual void HandlePickUp(PointerEventData eventData)
    {
        var slot = ui.assignedSlot;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            DragManager.Instance.StartDrag(slot.item, slot.stackSize, ui);
            slot.ClearSlot();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            int amount = Mathf.CeilToInt(slot.stackSize / 2f);
            DragManager.Instance.StartDrag(slot.item, amount, ui);
            slot.stackSize -= amount;
            if (slot.stackSize <= 0) slot.ClearSlot();
        }

        ui.UpdateSlotUI();
        NotifyCrafting();
    }

    protected virtual void HandleDrop()
    {
        var drag = DragManager.Instance.currentDragData;
        var slot = ui.assignedSlot;

        if (slot.item == null || (slot.item == drag.item && slot.item.isStackable))
        {
            if (slot.item == null) slot.UpdateSlot(drag.item, drag.amount);
            else slot.stackSize += drag.amount;
            DragManager.Instance.EndDrag();
        }
        else
        {
            ItemData tempItem = slot.item;
            int tempAmount = slot.stackSize;
            slot.UpdateSlot(drag.item, drag.amount);
            DragManager.Instance.StartDrag(tempItem, tempAmount, drag.originalSlot);
        }
        ui.UpdateSlotUI();
        NotifyCrafting();
    }

    protected void NotifyCrafting()
    {
        var cm = Object.FindFirstObjectByType<CraftingManager>();
        if (cm != null) cm.CheckForRecipes();
    }
}