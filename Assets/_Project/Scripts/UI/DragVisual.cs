using UnityEngine;
using UnityEngine.UI;
using TMPro;

// This script handles the visual representation of the item being dragged
public class DragVisual : MonoBehaviour
{
    [Header("UI References")]
    public Image icon;
    public TextMeshProUGUI countText;

    // Update the visual representation
    public void SetVisualData(Sprite itemIcon, int count)
    {
        icon.sprite = itemIcon;
        countText.text = count > 1 ? count.ToString() : ""; // Only shows the number if it's > 1
        
        // Make the object visible
        icon.enabled = true;
        countText.enabled = count > 1;
    }

    // Hide the visual representation
    public void HideVisual()
    {
        icon.enabled = false;
        countText.enabled = false;
    }
}