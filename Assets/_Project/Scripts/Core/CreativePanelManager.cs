using UnityEngine;
using System.Collections.Generic;

public class CreativePanelManager : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject slotPrefab;     
    public Transform contentParent;   
    
    [Header("Base de Datos")]
    public List<ItemData> allItems;   

    private void Start()
    {
        GenerateCreativeSlots();
    }

    private void GenerateCreativeSlots()
    {
        // Clear any existing slots
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Create a slot for each item in the list
        foreach (ItemData item in allItems)
        {
            if (item == null) continue;

            GameObject newSlotObj = Instantiate(slotPrefab, contentParent);
            
            // Configure the slot script
            CreativeSlot slotScript = newSlotObj.GetComponent<CreativeSlot>();
            if (slotScript != null)
            {
                slotScript.Setup(item);
            }
        }
    }

    // Function for the "X" button and the Chest button
    public void ToggleVisibility()
    {
        bool isActive = !gameObject.activeSelf;
        gameObject.SetActive(isActive);

        if (isActive && AudioManager.Instance != null)
        {
            // Sound on open
            AudioManager.Instance.PlaySound(AudioManager.Instance.dropSound); 
        }
    }
}