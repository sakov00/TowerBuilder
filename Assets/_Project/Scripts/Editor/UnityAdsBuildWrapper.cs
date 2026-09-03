using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Luna.Build
{
    public static class UnityAdsBuildWrapper
    {
        private static string ProjectRoot => Path.GetDirectoryName(Application.dataPath);
        private static string LunaJsonPath => Path.Combine(ProjectRoot, "luna.json");
        private static string BuildsDir => Path.Combine(ProjectRoot, "Builds");

        [MenuItem("Luna/UnityAds/Prepare Config")]
        public static void PrepareConfig()
        {
            if (!File.Exists(LunaJsonPath))
            {
                Debug.LogError("[LunaBuild] luna.json not found");
                return;
            }

            // Backup
            File.Copy(LunaJsonPath, LunaJsonPath + ".backup", true);

            var json = File.ReadAllText(LunaJsonPath);

            // Add unityads package before ironsource
            if (!json.Contains("\"unityads\""))
            {
                var match = Regex.Match(json, @"""ironsource""\s*:\s*\{");
                if (match.Success)
                {
                    json = json.Insert(match.Index, "            \"unityads\": { \"packageType\": 0 },\n");
                    File.WriteAllText(LunaJsonPath, json);
                    Debug.Log("[LunaBuild] Config prepared for UnityAds");
                }
            }
            else
            {
                Debug.Log("[LunaBuild] Config already has unityads");
            }

            // Open Luna window
            EditorApplication.ExecuteMenuItem("Tools/Unity Playworks Plugin");
            Debug.Log("[LunaBuild] Click 'Build' in Luna window");
        }

        [MenuItem("Luna/UnityAds/Restore Config")]
        public static void RestoreConfig()
        {
            var backup = LunaJsonPath + ".backup";
            if (File.Exists(backup))
            {
                File.Copy(backup, LunaJsonPath, true);
                File.Delete(backup);
                Debug.Log("[LunaBuild] Config restored");
            }
        }

        [MenuItem("Luna/UnityAds/Copy Output")]
        public static void CopyOutput()
        {
            var stage4 = Path.Combine(ProjectRoot, "LunaTemp/stage4");
            if (!Directory.Exists(stage4))
            {
                Debug.LogWarning("[LunaBuild] No build output found");
                return;
            }

            Directory.CreateDirectory(BuildsDir);

            // Search all subdirectories for HTML files
            foreach (var html in Directory.GetFiles(stage4, "*.html", SearchOption.AllDirectories))
            {
                var dest = Path.Combine(BuildsDir, Path.GetFileName(html));
                File.Copy(html, dest, true);
                Debug.Log($"[LunaBuild] Copied: {Path.GetFileName(html)}");
            }

            // Search for ZIP files
            foreach (var zip in Directory.GetFiles(stage4, "*.zip", SearchOption.AllDirectories))
            {
                var dest = Path.Combine(BuildsDir, Path.GetFileName(zip));
                File.Copy(zip, dest, true);
                Debug.Log($"[LunaBuild] Copied: {Path.GetFileName(zip)}");
            }

            EditorUtility.RevealInFinder(BuildsDir);
        }
    }
}
