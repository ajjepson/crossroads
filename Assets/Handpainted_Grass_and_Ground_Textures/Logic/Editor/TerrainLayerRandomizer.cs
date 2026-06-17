using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Crossroads.HandpaintedLogic.Editor
{
    public class TerrainLayerRandomizer : EditorWindow
    {
        [Serializable]
        public class LayerInfo
        {
            public int Index;
            public string Suffix;
            public string LayerName;
        }

        [Serializable]
        public class DirectionalGroup
        {
            public string BaseName;
            public bool Enabled = true;
            public List<LayerInfo> Members = new List<LayerInfo>();
        }

        private Terrain targetTerrain;
        private List<DirectionalGroup> detectedGroups = new List<DirectionalGroup>();
        private Vector2 scrollPosition;

        // Settings
        private int patchSize = 1;
        private int seed = 1337;
        private bool showGroupMembers = false;

        private static readonly string[] Suffixes = { "_up", "_down", "_left", "_right" };

        [MenuItem("Tools/Terrain Texture Randomizer")]
        public static void ShowWindow()
        {
            TerrainLayerRandomizer window = GetWindow<TerrainLayerRandomizer>("Terrain Randomizer");
            window.minSize = new Vector2(400, 500);
            window.Show();
        }

        private void OnEnable()
        {
            if (targetTerrain == null)
            {
                targetTerrain = Selection.activeGameObject?.GetComponent<Terrain>();
                if (targetTerrain == null)
                {
                    targetTerrain = Terrain.activeTerrain;
                }
            }
            ScanGroups();
        }

        private void OnSelectionChange()
        {
            if (Selection.activeGameObject != null)
            {
                Terrain terrain = Selection.activeGameObject.GetComponent<Terrain>();
                if (terrain != null && terrain != targetTerrain)
                {
                    targetTerrain = terrain;
                    ScanGroups();
                    Repaint();
                }
            }
        }

        private void OnGUI()
        {
            // Title
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Terrain Texture Randomizer", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Breaks up terrain tiling repetition using directional variations (_up, _down, _left, _right).", EditorStyles.miniLabel);
            GUILayout.Space(5);

            // Help Box
            EditorGUILayout.HelpBox(
                "This tool scans your terrain layers for groups that share a base name and have directional suffixes " +
                "(e.g., '_up', '_down', '_left', '_right'). It then randomizes the texture weights per pixel or patch to eliminate repeating patterns.",
                MessageType.Info);

            GUILayout.Space(10);

            // Terrain Selection
            EditorGUI.BeginChangeCheck();
            targetTerrain = (Terrain)EditorGUILayout.ObjectField("Target Terrain", targetTerrain, typeof(Terrain), true);
            if (EditorGUI.EndChangeCheck())
            {
                ScanGroups();
            }

            if (targetTerrain == null)
            {
                EditorGUILayout.HelpBox("Please select a Terrain object in the Hierarchy or assign it above.", MessageType.Warning);
                return;
            }

            if (targetTerrain.terrainData == null)
            {
                EditorGUILayout.HelpBox("The selected Terrain does not have valid TerrainData.", MessageType.Error);
                return;
            }

            GUILayout.Space(10);

            // Groups Header
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Detected Groups ({detectedGroups.Count})", EditorStyles.boldLabel);
            if (GUILayout.Button("Scan / Refresh", GUILayout.Width(120)))
            {
                ScanGroups();
            }
            EditorGUILayout.EndHorizontal();

            if (detectedGroups.Count == 0)
            {
                EditorGUILayout.HelpBox("No directional groups found on this Terrain.\nEnsure your Terrain has layers ending with _up, _down, _left, or _right added.", MessageType.Warning);
            }
            else
            {
                // Toggle All
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Select All", GUILayout.Width(90)))
                {
                    SetAllGroups(true);
                }
                if (GUILayout.Button("Deselect All", GUILayout.Width(90)))
                {
                    SetAllGroups(false);
                }
                GUILayout.FlexibleSpace();
                showGroupMembers = EditorGUILayout.ToggleLeft("Show Layer Details", showGroupMembers, GUILayout.Width(130));
                EditorGUILayout.EndHorizontal();

                // Scroll view for groups
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, "box", GUILayout.Height(180));
                foreach (var group in detectedGroups)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUILayout.BeginHorizontal();
                    group.Enabled = EditorGUILayout.ToggleLeft($"<b>{group.BaseName}</b> ({group.Members.Count} directions)", group.Enabled, new GUIStyle(EditorStyles.label) { richText = true });
                    EditorGUILayout.EndHorizontal();

                    if (showGroupMembers)
                    {
                        EditorGUI.indentLevel++;
                        foreach (var member in group.Members)
                        {
                            EditorGUILayout.LabelField($"- Index {member.Index}: {member.LayerName}", EditorStyles.miniLabel);
                        }
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndScrollView();
            }

            GUILayout.Space(15);

            // Settings
            EditorGUILayout.LabelField("Randomization Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");

            patchSize = EditorGUILayout.IntSlider(new GUIContent("Patch Size", "1 = Per-pixel random distribution. Higher values group the rotation into larger square patches."), patchSize, 1, 16);
            seed = EditorGUILayout.IntField(new GUIContent("Random Seed", "The seed used to generate the deterministic random distribution pattern."), seed);

            EditorGUILayout.EndVertical();

            GUILayout.Space(15);

            // Action Buttons
            EditorGUI.BeginDisabledGroup(detectedGroups.Count == 0 || !HasEnabledGroup());

            if (GUILayout.Button("Randomize Selected Groups", GUILayout.Height(35)))
            {
                if (EditorUtility.DisplayDialog("Confirm Randomization", "This will modify the Terrain's alphamaps. You can Undo this action. Do you want to proceed?", "Yes", "No"))
                {
                    ApplyRandomization(false);
                }
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Reset to Default Direction (_up)", GUILayout.Height(25)))
            {
                if (EditorUtility.DisplayDialog("Confirm Reset", "This will collapse all active directional layer weights into their default (_up) variation. Do you want to proceed?", "Yes", "No"))
                {
                    ApplyRandomization(true);
                }
            }

            EditorGUI.EndDisabledGroup();

            GUILayout.Space(10);
        }

        private void ScanGroups()
        {
            detectedGroups.Clear();
            if (targetTerrain == null || targetTerrain.terrainData == null) return;

            TerrainLayer[] layers = targetTerrain.terrainData.terrainLayers;
            Dictionary<string, DirectionalGroup> groupsMap = new Dictionary<string, DirectionalGroup>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < layers.Length; i++)
            {
                TerrainLayer layer = layers[i];
                if (layer == null) continue;

                string name = layer.name;
                string matchedSuffix = null;

                foreach (var suffix in Suffixes)
                {
                    if (name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                    {
                        matchedSuffix = suffix;
                        break;
                    }
                }

                if (matchedSuffix != null)
                {
                    string baseName = name.Substring(0, name.Length - matchedSuffix.Length);
                    if (!groupsMap.TryGetValue(baseName, out DirectionalGroup group))
                    {
                        group = new DirectionalGroup { BaseName = baseName };
                        groupsMap[baseName] = group;
                    }

                    group.Members.Add(new LayerInfo
                    {
                        Index = i,
                        Suffix = matchedSuffix.ToLowerInvariant(),
                        LayerName = name
                    });
                }
            }

            // Only add groups that have at least 2 members
            foreach (var kp in groupsMap)
            {
                if (kp.Value.Members.Count >= 2)
                {
                    // Sort members so up, down, left, right have a predictable order
                    kp.Value.Members.Sort((a, b) => string.Compare(a.Suffix, b.Suffix, StringComparison.Ordinal));
                    detectedGroups.Add(kp.Value);
                }
            }
        }

        private void SetAllGroups(bool enabled)
        {
            foreach (var group in detectedGroups)
            {
                group.Enabled = enabled;
            }
        }

        private bool HasEnabledGroup()
        {
            foreach (var group in detectedGroups)
            {
                if (group.Enabled) return true;
            }
            return false;
        }

        private void ApplyRandomization(bool resetToDefault)
        {
            if (targetTerrain == null || targetTerrain.terrainData == null) return;

            TerrainData terrainData = targetTerrain.terrainData;
            int width = terrainData.alphamapWidth;
            int height = terrainData.alphamapHeight;

            float[,,] alphamaps = terrainData.GetAlphamaps(0, 0, width, height);

            // Record Undo before modification
            Undo.RegisterCompleteObjectUndo(terrainData, resetToDefault ? "Reset Terrain Textures" : "Randomize Terrain Textures");

            int activeGroupsCount = 0;
            List<DirectionalGroup> activeGroups = new List<DirectionalGroup>();
            foreach (var group in detectedGroups)
            {
                if (group.Enabled)
                {
                    activeGroups.Add(group);
                    activeGroupsCount++;
                }
            }

            try
            {
                for (int y = 0; y < height; y++)
                {
                    if (y % 16 == 0)
                    {
                        float progress = (float)y / height;
                        if (EditorUtility.DisplayCancelableProgressBar(
                            resetToDefault ? "Resetting Terrain Textures" : "Randomizing Terrain Textures",
                            $"Processing row {y} of {height}...",
                            progress))
                        {
                            break;
                        }
                    }

                    int py = y / patchSize;

                    for (int x = 0; x < width; x++)
                    {
                        int px = x / patchSize;

                        foreach (var group in activeGroups)
                        {
                            // Calculate total weight of this directional group at the pixel
                            float sum = 0f;
                            foreach (var member in group.Members)
                            {
                                sum += alphamaps[y, x, member.Index];
                            }

                            if (sum > 0.0001f)
                            {
                                // Zero out all members
                                foreach (var member in group.Members)
                                {
                                    alphamaps[y, x, member.Index] = 0f;
                                }

                                if (resetToDefault)
                                {
                                    // Assign everything to default (_up) member, or first member if _up isn't in group
                                    int defaultIdx = 0;
                                    for (int m = 0; m < group.Members.Count; m++)
                                    {
                                        if (group.Members[m].Suffix == "_up")
                                        {
                                            defaultIdx = m;
                                            break;
                                        }
                                    }
                                    alphamaps[y, x, group.Members[defaultIdx].Index] = sum;
                                }
                                else
                                {
                                    // Choose a random member using hash
                                    int hash = HashCoords(px, py, seed ^ group.BaseName.GetHashCode());
                                    int chosenMemberIndex = (hash & 0x7FFFFFFF) % group.Members.Count;
                                    alphamaps[y, x, group.Members[chosenMemberIndex].Index] = sum;
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            terrainData.SetAlphamaps(0, 0, alphamaps);
            EditorUtility.SetDirty(terrainData);
            AssetDatabase.SaveAssets();

            string actionName = resetToDefault ? "Reset to Default Direction" : "Randomized";
            EditorUtility.DisplayDialog("Success", $"{actionName} {activeGroupsCount} group(s) successfully across {width}x{height} alphamap pixels!", "OK");
        }

        // FNV-1a hash function for high quality deterministic noise grid
        private static int HashCoords(int x, int y, int seed)
        {
            uint h = (uint)seed ^ 2166136261U;
            h = (h ^ (uint)x) * 16777619U;
            h = (h ^ (uint)y) * 16777619U;
            return (int)h;
        }
    }
}
