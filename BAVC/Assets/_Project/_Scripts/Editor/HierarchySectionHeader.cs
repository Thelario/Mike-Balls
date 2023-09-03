#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Game
{
    namespace Editor
    {
        [InitializeOnLoad]
        public static class HierarchySectionHeader
        {
            private static Color bkgrdColor = new Color(0.175f, 0.175f, 0.175f);

            static HierarchySectionHeader()
            {
                EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGui;
            }

            static void HierarchyWindowItemOnGui(int instanceID, Rect selectionRect)
            {
                var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

                if (gameObject != null && gameObject.name.StartsWith("//", System.StringComparison.Ordinal))
                {
                    EditorGUI.DrawRect(selectionRect, bkgrdColor);
                    EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("/", "").ToUpperInvariant());
                }
            }
        }
    }
}

#endif