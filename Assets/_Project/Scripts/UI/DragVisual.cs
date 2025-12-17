using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DragVisual : MonoBehaviour
{
    [Header("UI References")]
    public Image icon;
    public TextMeshProUGUI countText;

    // Updates the visual icon and count text for the dragged item
    public void SetVisualData(Sprite itemIcon, int count)
    {
        icon.sprite = itemIcon;
        countText.text = count > 1 ? count.ToString() : ""; 
        
        icon.enabled = true;
        countText.enabled = count > 1;

        // Activate the object so Update tracking works
        gameObject.SetActive(true); 
    }

    // Hides the drag visual object
    public void HideVisual()
    {
        gameObject.SetActive(false);
    }
}