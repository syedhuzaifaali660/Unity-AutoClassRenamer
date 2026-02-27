using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace syedhuzaifaali660.AutoRenamer
{
    public class AutoClassRenamer : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            string[] moved = (string[])movedAssets.Clone();
            string[] movedFrom = (string[])movedFromAssetPaths.Clone();

            EditorApplication.delayCall += () =>
            {
                for (int i = 0; i < moved.Length; i++)
                {
                    if (!moved[i].EndsWith(".cs")) continue;

                    string oldClassName = Path.GetFileNameWithoutExtension(movedFrom[i]);
                    string newClassName = Path.GetFileNameWithoutExtension(moved[i]);

                    RenameClassToMatchFileName(moved[i]);

                    if (oldClassName != newClassName)
                        UpdateReferencesInAllScripts(oldClassName, newClassName, moved[i]);
                }
            };
        }

        static string ExtractClassName(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            string content = File.ReadAllText(filePath);
            Match match = Regex.Match(content, @"\bclass\s+(\w+)");
            return match.Success ? match.Groups[1].Value : null;
        }

        static void RenameClassToMatchFileName(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string currentClassName = ExtractClassName(filePath);

            if (currentClassName == null || currentClassName == fileName) return;

            Debug.Log($"[AutoRenamer] Updating class '{currentClassName}' → '{fileName}'");

            string content = File.ReadAllText(filePath);
            content = Regex.Replace(content, $@"\bclass\s+{currentClassName}\b", $"class {fileName}");
            File.WriteAllText(filePath, content);

            AssetDatabase.ImportAsset(filePath);
            Debug.Log($"<color=green><b>[AutoRenamer]</b> Class renamed to '{fileName}'.</color>");
        }

        static void UpdateReferencesInAllScripts(string oldClassName, string newClassName, string skipFilePath)
        {
            if (string.IsNullOrEmpty(oldClassName) || oldClassName == newClassName) return;

            string[] allScripts = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
            string pattern = $@"\b{Regex.Escape(oldClassName)}\b";
            List<string> updatedFiles = new List<string>();

            foreach (string fullPath in allScripts)
            {
                string assetPath = "Assets" + fullPath.Replace(Application.dataPath, "").Replace("\\", "/");
                if (assetPath == skipFilePath) continue;

                string content = File.ReadAllText(fullPath);
                if (!Regex.IsMatch(content, pattern)) continue;

                string updated = Regex.Replace(content, pattern, newClassName);
                File.WriteAllText(fullPath, updated);
                updatedFiles.Add(assetPath);
                AssetDatabase.ImportAsset(assetPath);
            }

            if (updatedFiles.Count > 0)
                Debug.Log($"<color=cyan><b>[AutoRenamer]</b> Updated references in {updatedFiles.Count} file(s):\n{string.Join("\n", updatedFiles)}</color>");
        }
    }
}