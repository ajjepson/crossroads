using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Crossroads.HandpaintedLogic.Editor
{
    public class TreeCreatorBaker : EditorWindow
    {
        private GameObject sourceTree;
        private string exportFolder = "Assets/BakedTrees";
        
        // Custom texture overrides
        private Texture2D barkTexture;
        private Texture2D leafTexture;
        
        // Leaf alpha cutoff setting
        private float alphaCutoff = 0.1f;

        [MenuItem("Tools/Tree Creator Baker (URP Converter)")]
        public static void ShowWindow()
        {
            TreeCreatorBaker window = GetWindow<TreeCreatorBaker>("Tree Baker");
            window.minSize = new Vector2(400, 480);
            window.Show();
        }

        private void OnEnable()
        {
            if (sourceTree == null)
            {
                sourceTree = Selection.activeGameObject;
            }
            
            // Persist settings
            exportFolder = EditorPrefs.GetString("TreeCreatorBaker_ExportFolder", "Assets/BakedTrees");
            alphaCutoff = EditorPrefs.GetFloat("TreeCreatorBaker_AlphaCutoff", 0.1f);
            
            string barkPath = EditorPrefs.GetString("TreeCreatorBaker_BarkPath", "");
            if (!string.IsNullOrEmpty(barkPath))
            {
                barkTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(barkPath);
            }
            
            string leafPath = EditorPrefs.GetString("TreeCreatorBaker_LeafPath", "");
            if (!string.IsNullOrEmpty(leafPath))
            {
                leafTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(leafPath);
            }
        }

        private void OnSelectionChange()
        {
            if (Selection.activeGameObject != null)
            {
                sourceTree = Selection.activeGameObject;
                Repaint();
            }
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Tree Creator Baker", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Converts legacy Tree Creator assets into static URP-compatible Prefabs.", EditorStyles.miniLabel);
            GUILayout.Space(5);

            EditorGUILayout.HelpBox(
                "Legacy Tree Creator shaders are incompatible with modern Scriptable Render Pipelines (URP/HDRP), causing trees to render as hot pink.\n\n" +
                "This tool extracts the tree mesh, generates fully compatible URP Lit materials with alpha cutout and double-sided rendering for leaves, and saves a ready-to-use Prefab.",
                MessageType.Info);

            GUILayout.Space(10);

            // Inputs Group
            EditorGUILayout.LabelField("Inputs", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            
            sourceTree = (GameObject)EditorGUILayout.ObjectField("Source Tree GameObject", sourceTree, typeof(GameObject), true);
            
            GUILayout.Space(5);
            
            EditorGUI.BeginChangeCheck();
            barkTexture = (Texture2D)EditorGUILayout.ObjectField("Bark Texture Override", barkTexture, typeof(Texture2D), false);
            if (EditorGUI.EndChangeCheck())
            {
                string path = barkTexture != null ? AssetDatabase.GetAssetPath(barkTexture) : "";
                EditorPrefs.SetString("TreeCreatorBaker_BarkPath", path);
            }

            EditorGUI.BeginChangeCheck();
            leafTexture = (Texture2D)EditorGUILayout.ObjectField("Leaf Texture Override", leafTexture, typeof(Texture2D), false);
            if (EditorGUI.EndChangeCheck())
            {
                string path = leafTexture != null ? AssetDatabase.GetAssetPath(leafTexture) : "";
                EditorPrefs.SetString("TreeCreatorBaker_LeafPath", path);
            }
            
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            // Settings Group
            EditorGUILayout.LabelField("Export Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            
            string previousFolder = exportFolder;
            exportFolder = EditorGUILayout.TextField("Export Folder", exportFolder);
            if (exportFolder != previousFolder)
            {
                EditorPrefs.SetString("TreeCreatorBaker_ExportFolder", exportFolder);
            }

            EditorGUI.BeginChangeCheck();
            alphaCutoff = EditorGUILayout.Slider(new GUIContent("Leaf Alpha Cutoff", "Controls transparency clipping threshold for leaf edges. Lower values prevent leaves from appearing thin or cut off."), alphaCutoff, 0.01f, 1.0f);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetFloat("TreeCreatorBaker_AlphaCutoff", alphaCutoff);
            }

            EditorGUILayout.EndVertical();

            GUILayout.Space(15);

            EditorGUI.BeginDisabledGroup(sourceTree == null);
            if (GUILayout.Button("Bake Tree to URP Prefab", GUILayout.Height(35)))
            {
                BakeTree();
            }
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(5);

            // Deletion Section
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Maintenance", EditorStyles.boldLabel);
            if (GUILayout.Button("Delete Previous Generations", GUILayout.Height(25)))
            {
                ClearPreviousGenerations();
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);
        }

        private void ClearPreviousGenerations()
        {
            if (string.IsNullOrEmpty(exportFolder) || !exportFolder.StartsWith("Assets"))
            {
                EditorUtility.DisplayDialog("Error", "Invalid export folder path. It must start with 'Assets'.", "OK");
                return;
            }

            if (!AssetDatabase.IsValidFolder(exportFolder))
            {
                EditorUtility.DisplayDialog("Info", $"The export folder '{exportFolder}' does not exist or has already been deleted.", "OK");
                return;
            }

            if (EditorUtility.DisplayDialog("Confirm Delete", 
                $"Are you sure you want to permanently delete all baked assets, meshes, prefabs, and materials inside '{exportFolder}'?", 
                "Yes, Delete Everything", "Cancel"))
            {
                if (AssetDatabase.DeleteAsset(exportFolder))
                {
                    AssetDatabase.Refresh();
                    EditorUtility.DisplayDialog("Success", "All previous generations successfully deleted!", "OK");
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Could not delete export folder. Ensure no assets inside are locked or being edited.", "OK");
                }
            }
        }

        private void BakeTree()
        {
            if (sourceTree == null) return;

            // Find the UnityEngine.Tree component
            UnityEngine.Tree treeComponent = sourceTree.GetComponent<UnityEngine.Tree>();
            MeshFilter sourceMeshFilter = sourceTree.GetComponent<MeshFilter>();
            MeshRenderer sourceMeshRenderer = sourceTree.GetComponent<MeshRenderer>();

            if (sourceMeshFilter == null || sourceMeshRenderer == null)
            {
                // Fallback to checking children
                treeComponent = sourceTree.GetComponentInChildren<UnityEngine.Tree>();
                sourceMeshFilter = sourceTree.GetComponentInChildren<MeshFilter>();
                sourceMeshRenderer = sourceTree.GetComponentInChildren<MeshRenderer>();
            }

            if (sourceMeshFilter == null || sourceMeshRenderer == null)
            {
                EditorUtility.DisplayDialog("Error", "Selected GameObject or its children must have both a MeshFilter and a MeshRenderer.", "OK");
                return;
            }

            // Create target folders
            if (!Directory.Exists(exportFolder))
            {
                Directory.CreateDirectory(exportFolder);
            }
            string meshesFolder = Path.Combine(exportFolder, "Meshes").Replace("\\", "/");
            if (!Directory.Exists(meshesFolder))
            {
                Directory.CreateDirectory(meshesFolder);
            }
            string materialsFolder = Path.Combine(exportFolder, "Materials").Replace("\\", "/");
            if (!Directory.Exists(materialsFolder))
            {
                Directory.CreateDirectory(materialsFolder);
            }

            AssetDatabase.Refresh();

            string cleanName = sourceTree.name.Replace(" ", "_").Replace("(Clone)", "");

            // Get the mesh
            Mesh sourceMesh = sourceMeshFilter.sharedMesh;
            if (sourceMesh == null)
            {
                EditorUtility.DisplayDialog("Error", "No valid Mesh found on the source tree.", "OK");
                return;
            }

            // 1. Bake the Mesh
            Mesh bakedMesh = Instantiate(sourceMesh);
            string meshPath = Path.Combine(meshesFolder, $"{cleanName}_Mesh.asset").Replace("\\", "/");
            AssetDatabase.CreateAsset(bakedMesh, meshPath);

            // 2. Process Materials
            Material[] sourceMaterials = sourceMeshRenderer.sharedMaterials;
            Material[] bakedMaterials = new Material[sourceMaterials.Length];

            // Find our custom wind shader, fallback to standard Lit if not present
            Shader targetShader = Shader.Find("Universal Render Pipeline/Custom/WindLit");
            if (targetShader == null)
            {
                targetShader = Shader.Find("Universal Render Pipeline/Lit");
            }
            if (targetShader == null)
            {
                targetShader = Shader.Find("Standard");
            }

            for (int i = 0; i < sourceMaterials.Length; i++)
            {
                Material srcMat = sourceMaterials[i];
                if (srcMat == null) continue;

                string matPath = Path.Combine(materialsFolder, $"{cleanName}_{srcMat.name}_URP.mat").Replace("\\", "/");
                Material newMat = new Material(targetShader);

                // Identify if it's a leaf material
                bool isLeaf = srcMat.name.ToLowerInvariant().Contains("leaf") || 
                             srcMat.name.ToLowerInvariant().Contains("leaves") || 
                             (srcMat.shader != null && srcMat.shader.name.ToLowerInvariant().Contains("leaves"));

                // Apply textures
                if (isLeaf)
                {
                    if (leafTexture != null)
                    {
                        if (newMat.HasProperty("_BaseMap")) newMat.SetTexture("_BaseMap", leafTexture);
                        else if (newMat.HasProperty("_MainTex")) newMat.SetTexture("_MainTex", leafTexture);
                    }
                    else if (srcMat.HasProperty("_MainTex"))
                    {
                        Texture mainTex = srcMat.GetTexture("_MainTex");
                        if (newMat.HasProperty("_BaseMap")) newMat.SetTexture("_BaseMap", mainTex);
                        else if (newMat.HasProperty("_MainTex")) newMat.SetTexture("_MainTex", mainTex);
                    }

                    // Configure URP Alpha Cutout
                    if (newMat.HasProperty("_AlphaClip"))
                    {
                        newMat.SetFloat("_AlphaClip", 1f);
                        newMat.SetFloat("_Cutoff", alphaCutoff);
                        newMat.EnableKeyword("_ALPHATEST_ON");
                        newMat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
                    }

                    // Double-Sided rendering
                    if (newMat.HasProperty("_Cull"))
                    {
                        newMat.SetFloat("_Cull", (float)UnityEngine.Rendering.CullMode.Off);
                        newMat.EnableKeyword("_DOUBLE_SIDED_ON");
                    }

                    // Expose Leaf Wind Parameters
                    if (newMat.HasProperty("_WindSpeed")) newMat.SetFloat("_WindSpeed", 2.2f);
                    if (newMat.HasProperty("_WindStrength")) newMat.SetFloat("_WindStrength", 0.18f);
                    if (newMat.HasProperty("_WindFrequency")) newMat.SetFloat("_WindFrequency", 0.45f);
                }
                else
                {
                    if (barkTexture != null)
                    {
                        if (newMat.HasProperty("_BaseMap")) newMat.SetTexture("_BaseMap", barkTexture);
                        else if (newMat.HasProperty("_MainTex")) newMat.SetTexture("_MainTex", barkTexture);
                    }
                    else if (srcMat.HasProperty("_MainTex"))
                    {
                        Texture mainTex = srcMat.GetTexture("_MainTex");
                        if (newMat.HasProperty("_BaseMap")) newMat.SetTexture("_BaseMap", mainTex);
                        else if (newMat.HasProperty("_MainTex")) newMat.SetTexture("_MainTex", mainTex);
                    }

                    if (srcMat.HasProperty("_BumpMap"))
                    {
                        Texture bumpMap = srcMat.GetTexture("_BumpMap");
                        if (newMat.HasProperty("_BumpMap")) newMat.SetTexture("_BumpMap", bumpMap);
                    }

                    if (newMat.HasProperty("_Smoothness"))
                    {
                        newMat.SetFloat("_Smoothness", 0.1f);
                    }

                    // Expose Bark Wind Parameters (subtle rooted trunk swaying)
                    if (newMat.HasProperty("_WindSpeed")) newMat.SetFloat("_WindSpeed", 1.2f);
                    if (newMat.HasProperty("_WindStrength")) newMat.SetFloat("_WindStrength", 0.04f);
                    if (newMat.HasProperty("_WindFrequency")) newMat.SetFloat("_WindFrequency", 0.15f);
                }

                AssetDatabase.CreateAsset(newMat, matPath);
                bakedMaterials[i] = newMat;
            }

            // 3. Create baked GameObject
            GameObject bakedGo = new GameObject($"{cleanName}_URP");
            MeshFilter bakedFilter = bakedGo.AddComponent<MeshFilter>();
            MeshRenderer bakedRenderer = bakedGo.AddComponent<MeshRenderer>();

            bakedFilter.sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
            bakedRenderer.sharedMaterials = bakedMaterials;

            // Preserve original transform values
            bakedGo.transform.position = sourceTree.transform.position;
            bakedGo.transform.rotation = sourceTree.transform.rotation;
            bakedGo.transform.localScale = sourceTree.transform.localScale;

            // 4. Save as Prefab
            string prefabPath = Path.Combine(exportFolder, $"{cleanName}_URP.prefab").Replace("\\", "/");
            GameObject prefabAsset = PrefabUtility.SaveAsPrefabAsset(bakedGo, prefabPath);

            DestroyImmediate(bakedGo);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (prefabAsset != null)
            {
                EditorUtility.DisplayDialog("Success", 
                    $"Tree baked successfully!\n\n" +
                    $"Saved in: {exportFolder}", 
                    "Awesome!");
                Selection.activeObject = prefabAsset;
            }
        }
    }
}
