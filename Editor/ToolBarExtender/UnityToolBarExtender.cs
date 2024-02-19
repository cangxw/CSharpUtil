using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace UnityToolBarExtension
{
    //InitializeOnLoad用于在编辑器启动时或脚本重新编译后自动执行指定的操作。
    //这种特性非常适合用于在编辑器启动时执行一些初始化操作，以确保项目在启动后能够正常运行
    //1, 注册事件；2，自动加载配置文件；3，自动检查资源更新
    [InitializeOnLoad]
    public static class UnityToolBarExtender
    {
        public static readonly List<Action> LeftToolbarGUI = new List<Action>();
        public static readonly List<Action> RightToolbarGUI = new List<Action>();

        static UnityToolBarExtender()
        {
            UnityToolBarCallback.OnToolBarGUI = OnGUI;
            UnityToolBarCallback.OnToolbarGUILeft = GUILeft;
            UnityToolBarCallback.OnToolbarGUIRight = GUIRight;
        }

        static void OnGUI()
        {
            Debug.Log("UnityToolBarExtender.Ongui");
        }

        static void GUILeft()
        {
            GUILayout.BeginHorizontal();
            foreach (var handler in LeftToolbarGUI)
            {
                handler();
            }
            GUILayout.EndHorizontal();
        }

        static void GUIRight()
        {
            GUILayout.BeginHorizontal();
            foreach (var handler in RightToolbarGUI)
            {
                handler();
            }
            GUILayout.EndHorizontal();
        }

        static void OnToolbarGUI()
        {

        }
    }
}


