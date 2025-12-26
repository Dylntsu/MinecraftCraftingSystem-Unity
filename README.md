# ⛏️ Minecraft Crafting System: Backend & UI Challenge

![Unity](https://img.shields.io/badge/Unity-2022%2B-black?style=for-the-badge&logo=unity)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp)
![Status](https://img.shields.io/badge/Status-Polished_&_Ready-success?style=for-the-badge)
![Build](https://img.shields.io/badge/Build-WebGL-blue?style=for-the-badge)

---

## 🎯 Project Goal

The primary objective is to replicate the logic and interface of the inventory, stacking, and crafting mechanics from *Minecraft*. This challenge focuses on **system architecture**, **clean code principles (SOLID)**, and **decoupling data from presentation** using the **Model-View-Controller (MVC)** pattern.

### ⚙️ Technologies & Concepts Applied
* **Engine:** Unity (URP 2D)
* **Language:** C#
* **Data Structure:** ScriptableObjects (Item & Recipe Databases)
* **Architecture:** Model-View-Controller (MVC) & Singleton Pattern
* **Design Principles:** Single Responsibility Principle (SRP), Open/Closed Principle (OCP) & Inheritance
* **Version Control:** GitFlow (Atomic Commits)

---

## 🧠 System Architecture Overview

The system is strictly decoupled into data, logic, and presentation layers:

### 1. The Model (Data)
* **`ItemData.cs` (ScriptableObject):** Defines static properties (ID, `maxStackSize`, icon). Acts as the item database.
* **`InventorySlot.cs`:** A serializable C# class representing a container space (Item reference + Count).
* **`CraftingRecipe.cs` (ScriptableObject):** Supports two crafting modes (Shapeless & Shaped/Grid-based).

### 2. The Controller (Logic)
* **`InventoryManager.cs`:** The central brain for storage. Handles `AddItem()`, stacking logic, and overflow checks.
* **`CraftingManager.cs`:** Features hybrid validation, relative pattern matching (Bounding Box), and safe ingredient consumption.
* **`DragManager.cs` (Singleton):** Manages the global state of the "hand" (cursor), holding item data while dragging between slots.
* **✨ `AudioManager.cs` (Singleton):** Centralized audio system handling feedback for clicks, drops, and crafting success without coupling logic to UI.
* **✨ `CreativePanelManager.cs`:** Dynamic logic that populates a scrollable list of all `ItemData` assets found in the project for debugging/testing.

### 3. Interaction & Input (The "Glue")
* **`SlotInteraction.cs`:** Handles Pointer events. Now includes **Procedural Animations** (Scale Hover) and **Drag Painting** logic.
* **`OutputSlotInteraction.cs`:** Overrides interaction logic to handle "Craft-All" (Shift-Click) and continuous manual crafting (Spam-Click protection).
* **✨ `CreativeSlot.cs`:** Inherits from `SlotInteraction` but overrides behavior to dispense infinite item stacks instead of swapping.

### 4. The View (UI)
* **`InventoryUI.cs` & `SlotUI.cs`:** Dynamically generates the grid and syncs visual state.
* **✨ `DragVisual.cs`:** Follows the mouse cursor, rendering the item stack with smooth positioning.
* **✨ `HotbarUI.cs`:** Implements a smart **Highlighter Frame** that visually travels to the active slot.

---

## 📸 Visual Progress

https://github.com/user-attachments/assets/1b6134e1-113f-4f43-a0db-cff48f8aea82

---

## 📈 Feature Progress Summary

### ✅ Inventory Fundamentals
- [x] **Data Structure:** Separated `ItemData` vs `InventorySlot`.
- [x] **Stacking Logic:** Items automatically stack up to their `maxStackSize`.
- [x] **Shift-Click Shortcuts:** Instantly move items between Inventory, Hotbar, and Crafting Grid (Smart stacking).

### ✅ Drag & Drop System (Advanced)
- [x] **Visual Feedback:** Item icon follows the mouse cursor.
- [x] **Drag Painting:** Holding left click and dragging across slots distributes items evenly (1 per slot).
- [x] **Splitting:** Right-click splits stacks in half.
- [x] **Double-Click Collect:** (Planned) Gather all items of the same type.

### ✅ Crafting System
- [x] **3x3 Grid:** Relative positioning (2x1 stick recipe works in any column).
- [x] **Shapeless & Shaped:** Full support for both recipe types.
- [x] **Output Logic:** - **Spam-Clicking:** Rapidly craft items one by one without drag glitches.
    - **Shift-Click Craft All:** Instantly crafts the maximum possible amount based on available ingredients.
    - **Ghost Item Prevention:** Output clears correctly when ingredients are removed.

### ✅ UX & "Game Feel" (Juice)
- [x] **Procedural Animation:** Slots scale up (1.1x) smoothly on Hover.
- [x] **Audio Feedback:** Satisfying clicks, pops, and success sounds for every interaction.
- [x] **Visual Highlighter:** A selection frame that snaps to the slot being interacted with.
- [x] **Feedback Loops:** Visual shake/red tint (Planned) for invalid actions.

### ✅ Developer Tools
- [x] **Creative Mode:** A toggleable "Supply Chest" panel.
- [x] **Infinite Scrolling:** Dynamically loads every ItemData in the database for easy testing.
- [x] **WebGL Support:** optimized build settings for browser play.

---

## 🚀 Roadmap & Next Steps
- [ ] **Save/Load System:** JSON serialization for inventory state.
- [ ] **Tooltips:** Hover information displaying Item Name and Description.
- [ ] **Item Durability:** Extension of `ItemData` for tools.
