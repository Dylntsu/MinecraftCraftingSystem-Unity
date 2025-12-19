using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Recipe", menuName = "MinecraftSystem/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("Configuración")]
    public bool isShapeless = false; // Define si importa el orden o no

    [Header("Ingredientes (Solo si es Shapeless)")]
    public List<RequiredItem> shapelessIngredients = new List<RequiredItem>();

    [Header("Grid 3x3 (Solo si NO es Shapeless)")]
    public List<ItemData> shapedGrid = new List<ItemData>(9); 

    [Header("Resultado")]
    public ItemData resultItem;
    public int resultAmount = 1;

}