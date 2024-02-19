
using UnityEditor;
using UnityEngine;

namespace UnityToolBarExtension.Example
{
    static class ToolbarStyles
    {
        public static readonly GUIStyle commandButtonStyle;

        static ToolbarStyles()
        {
            commandButtonStyle = new GUIStyle("Command")
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
                fontStyle = FontStyle.Bold
            };
        }
    }

    [InitializeOnLoad]
    public class SceneSwitcher
    {
        static SceneSwitcher()
        {
            UnityToolBarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("技", "Start Scene 1"), ToolbarStyles.commandButtonStyle))
            {
                Debug.Log("Start Load Scene 1");
            }

            if (GUILayout.Button(new GUIContent("关", "Start Scene 2"), ToolbarStyles.commandButtonStyle))
            {
                Debug.Log("Start Load Scene 2");
            }
        }
    }
}


