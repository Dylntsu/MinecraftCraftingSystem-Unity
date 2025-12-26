# ⛏️ Minecraft Crafting System: Backend & UI Challenge

![Unity](https://img.shields.io/badge/Unity-6-black?style=for-the-badge&logo=unity)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp)
![Status](https://img.shields.io/badge/Status-Polished_&_Ready-success?style=for-the-badge)
![Build](https://img.shields.io/badge/Build-WebGL-blue?style=for-the-badge)

### 🕹️ [Play the WebGL Demo on Itch.io](https://dylntsu.itch.io/minecraft-crafting-system)

---

## 🎯 Project Goal

The primary objective is to replicate the logic and interface of the inventory, stacking, and crafting mechanics from *Minecraft*. This challenge focuses on **system architecture**, **clean code principles (SOLID)**, and **decoupling data from presentation** using the **Model-View-Controller (MVC)** pattern.

### ⚙️ Technologies & Concepts Applied
* **Engine:** Unity 6 (URP 2D).
* **Language:** C#.
* **Data Structure:** ScriptableObjects for Item & Recipe Databases.
* **Architecture:** Model-View-Controller (MVC) & Singleton Pattern.
* **Design Principles:** Single Responsibility Principle (SRP), Open/Closed Principle (OCP) & Inheritance.

---

## 🧠 System Architecture Overview

The system is strictly decoupled into data, logic, and presentation layers:

### 1. The Model (Data)
* **`ItemData.cs` (ScriptableObject):** Defines static properties like ID, `maxStackSize`, and icons.
* **`InventorySlot.cs`:** A serializable C# class representing a container space.
* **`CraftingRecipe.cs` (ScriptableObject):** Supports both **Shapeless** and **Shaped** (Grid-based) modes.

### 2. The Controller (Logic)
* **`InventoryManager.cs`:** Central brain for storage, stacking logic, and overflow checks.
* **`CraftingManager.cs`:** Features hybrid validation and relative pattern matching using a Bounding Box algorithm.
* **`DragManager.cs` (Singleton):** Manages the global state of the cursor and dragging data.
* **✨ `AudioManager.cs`:** Centralized system for audio feedback without coupling logic to UI.
* **✨ `CreativePanelManager.cs`:** Dynamic logic that populates a scrollable list of all items for testing purposes.

### 3. Interaction & Input (The "Glue")
* **`SlotInteraction.cs`:** Handles Pointer events with **Procedural Animations** and **Drag Painting**.
* **`OutputSlotInteraction.cs`:** Specialized subclass handling "Craft-All" (Shift-Click) and rapid manual crafting.
* **✨ `CreativeSlot.cs`:** Inherits from `SlotInteraction` but overrides behavior to dispense infinite item stacks.

### 4. The View (UI)
* **`InventoryUI.cs` & `SlotUI.cs`:** Dynamically generates and syncs the visual state of the grid.
* **✨ `DragVisual.cs`:** Smoothly renders the item stack following the mouse cursor.
* **✨ `HotbarUI.cs`:** Implements a smart selection frame that travels to the active slot.

---

## 📸 Visual Progress

[![Crafting Example](https://github.com/user-attachments/assets/1b6134e1-113f-4f43-a0db-cff48f8aea82)](https://github.com/user-attachments/assets/1b6134e1-113f-4f43-a0db-cff48f8aea82)

---

## 📈 Feature Progress Summary

### ✅ Inventory Fundamentals
- [x] **Data Structure:** Separated `ItemData` vs `InventorySlot`.
- [x] **Stacking Logic:** Items automatically stack up to their `maxStackSize`.
- [x] **Shift-Click Shortcuts:** Instantly move items between Inventory, Hotbar, and Grid.

### ✅ Drag & Drop System (Advanced)
- [x] **Visual Feedback:** Item icon follows the mouse cursor.
- [x] **Drag Painting:** Distributes items evenly (1 per slot) while dragging.
- [x] **Splitting:** Right-click splits stacks in half.

### ✅ Crafting System
- [x] **3x3 Grid:** Supports relative positioning anywhere in the grid.
- [x] **Hybrid Recipes:** Full support for Shapeless and Shaped recipes.
- [x] **Output Logic:** Rapid manual crafting and Shift-Click "Craft All" functionality.

### ✅ UX & "Game Feel" (Juice)
- [x] **Procedural Animation:** Smooth scale-up (1.1x) on hover.
- [x] **Audio Feedback:** Satisfying sounds for every interaction.
- [x] **Visual Highlighter:** Dynamic selection frame for active interactions.

### ✅ Developer Tools
- [x] **Creative Mode:** Toggleable panel with infinite resources for testing.
- [x] **Infinite Scrolling:** Dynamically loads every ItemData in the database.
- [x] **WebGL Support:** Optimized for browser play with customized Canvas scaling.

---

## 🚀 Roadmap & Next Steps
- [ ] **Save/Load System:** JSON serialization for inventory state.
- [ ] **Tooltips:** Hover information displaying Item Name and Description.
- [ ] **Item Durability:** Extension of `ItemData` for tool mechanics.
