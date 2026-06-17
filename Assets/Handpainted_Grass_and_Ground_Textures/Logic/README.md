# Terrain Texture Randomizer

A high-performance Unity Editor tool that eliminates repeating tile patterns on Unity Terrains by randomly distributing directional variations (`_up`, `_down`, `_left`, `_right`) of a texture on a per-pixel or per-patch basis. 

This tool is specifically designed to leverage the four directional textures included in this handpainted grounds pack to create seamless, organic, and non-repetitive landscapes without any runtime shader overhead!

---

## 🚀 Key Features

* **Zero Runtime Overhead**: Modifies the terrain's alphamap data directly in the Editor, meaning it is compatible with **all** rendering pipelines (URP, HDRP, and Built-in) and requires no custom shaders or scripts at runtime.
* **Deterministic Noise (Seeds)**: Uses a high-quality, ultra-fast FNV-1a hash function. You can change the **Random Seed** to try out different variations of the randomization pattern.
* **Organic Patch Sizes**: Offers a **Patch Size** slider so you can choose between per-pixel randomization (size = 1) or larger square clusters/patches (size > 1) to perfectly match the handpainted art style.
* **Non-Destructive & Undo Support**: Fully integrates with Unity's standard Undo system. You can undo/redo (`Ctrl+Z` / `Ctrl+Y`) changes instantly.
* **One-Click Reset**: Includes a **Reset to Default Direction (_up)** button to collapse randomized variations back to their default state whenever you want to re-paint.

---

## 📖 Instructions for Use

Follow these step-by-step instructions to make use of this tool in your Unity project:

### Step 1: Set up Terrain Layers
1. Select your **Terrain** in the Scene or Hierarchy.
2. In the **Terrain Inspector**, switch to the **Paint Terrain** tab (the paintbrush icon) and select **Paint Texture** from the dropdown.
3. Add the directional variations of your desired textures to the Terrain's **Terrain Layers** list. For example, to use the randomized Clay Dirt, make sure you add all four layers:
   - `dirt_clay_up`
   - `dirt_clay_down`
   - `dirt_clay_left`
   - `dirt_clay_right`

### Step 2: Paint Your Terrain
1. Paint your terrain exactly as you normally would, using the **`_up`** variation as your primary painting brush.
2. *Tip: You do not need to paint the different directions manually! Paint everything with the default `_up` layer. The tool will handle all the rotation distribution for you!*

### Step 3: Open the Randomizer Tool
1. In the top menu bar of Unity, navigate to **Tools > Terrain Texture Randomizer**.
2. This will open the **Terrain Randomizer** window.

### Step 4: Randomize the Textures
1. **Target Terrain**: The tool will automatically assign the active or selected Terrain. If it doesn't, select your Terrain object in the Hierarchy or drag-and-drop it into the **Target Terrain** slot.
2. **Select Groups**: The tool will scan your Terrain Layers and group them by base name. Check or uncheck the groups you want to randomize (e.g., `dirt_clay`, `Grass_bluetint`).
3. **Configure Settings**:
   - **Patch Size**: Controls how fine or chunky the rotation is. A value of `1` randomizes on a single-pixel level. Higher values (e.g., 2, 4, 8) group the rotations into organic blocky patches, which can look more cohesive for handpainted styles.
   - **Random Seed**: A numeric value used to generate the pattern. Changing the seed will give you a different randomized layout.
4. Click the **"Randomize Selected Groups"** button.
5. Watch the repetition disappear! The tool will instantly redistribute the painted weights across the four directional layers.

---

## 🔄 Re-Painting or Resetting

If you ever want to re-paint or edit your terrain:
1. Select the texture groups you want to modify in the **Terrain Randomizer** window.
2. Click **"Reset to Default Direction (_up)"**. All directional weights will merge back into the default `_up` variation.
3. Paint your changes with the standard brush tools.
4. Click **"Randomize Selected Groups"** again to apply the randomized rotation to your new layout!
5. *Don't forget: You can also simply press **`Ctrl + Z`** to undo the randomization immediately.*
