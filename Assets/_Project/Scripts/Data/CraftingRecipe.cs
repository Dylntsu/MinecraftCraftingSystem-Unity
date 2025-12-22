using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Recipe", menuName = "MinecraftSystem/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("Configuration")]
    public bool isShapeless = false; // Defines if order matters or not

    [Header("Ingredients (Only if Shapeless)")]
    public List<RequiredItem> shapelessIngredients = new List<RequiredItem>();

    [Header("3x3 Grid (Only if NOT Shapeless)")]
    public List<ItemData> shapedGrid = new List<ItemData>(9); 

    [Header("Result")]
    public ItemData resultItem;
    public int resultAmount = 1;

}