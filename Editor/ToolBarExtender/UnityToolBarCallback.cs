using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using UnityEngine.UIElements;

namespace UnityToolBarExtension
{
    public static class UnityToolBarCallback
    {
        static Type _toolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar");

        static ScriptableObject _currentToolBar;
        public static Action OnToolBarGUI;
        public static Action OnToolbarGUILeft;
        public static Action OnToolbarGUIRight;
        static UnityToolBarCallback()
        {
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        static void OnUpdate()
        {
            if(_currentToolBar == null)
            {
                //通过反射类型拿到所有该类型的资源，应该是BuildIn的资源
                UnityEngine.Object[] toolbars = Resources.FindObjectsOfTypeAll(_toolbarType);
                //是一个ScriptableObject类型配置的
                _currentToolBar = toolbars.Length > 0 ? (ScriptableObject)toolbars[0] : null;
                if(_currentToolBar != null)
                {
#if UNITY_2021_1_OR_NEWER
                    //BindingFlags.Instance只搜索实例成员，对应相反的是静态成员
                    //通过反射获取m_Root变量
                    FieldInfo root = _currentToolBar.GetType().GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
                    object raw_root = root.GetValue(_currentToolBar);
                    VisualElement root_element = raw_root as VisualElement;
                    //ToolBar的VisualElement共3个子节点
                    //ToolbarZoneLeftAlign, ToolbarZonePlayMode, ToolbarZoneRightAlign
                    RegisterCallback("ToolbarZoneLeftAlign", OnToolbarGUILeft);
                    RegisterCallback("ToolbarZoneRightAlign", OnToolbarGUIRight);

                    //本地函数（私有）
                    //当需要一个辅助函数。
                    //您仅能在单个函数中使用它，并且它可能使用包含在该函数作用域内的变量和类型参数。
                    //另一方面，与 lambda 不同，您不需要将其作为第一类对象，
                    //因此您不必关心为它提供一个委托类型并分配一个实际的委托对象。
                    //另外，您可能希望它是递归的或泛型的，或者将其作为迭代器实现。
                    void RegisterCallback(string root, Action cb)
                    {
                        //在ToolBar的ve tree里找到对应的区域，然后在区域里插入1个ve节点，
                        //ve节点中是1个IMGUIContainer的容器
                        //容器的onGUIHandler绑定传入的cb
                        //cb是注册的action，所有绘制可以注册在这个上面

                        //Q stand for Query
                        //VisualElement中查找子VisualElement
                        var toolbar_zone = root_element.Q(root);
                        //新建一个VisualElement作为父节点
                        var parent = new VisualElement()
                        {
                            style =
                            {
                                flexGrow = 1,
                                flexDirection = FlexDirection.Row,
                            }
                        };
                        //新建一个Container来绑定我们自己的绘制
                        //cb对应可以是绘制信息
                        var container = new IMGUIContainer();
                        container.style.flexGrow = 1;
                        //绑定CB到container的onGUI回调
                        container.onGUIHandler += () =>
                        {
                            cb?.Invoke();
                        };
                        parent.Add(container);
                        toolbar_zone.Add(parent);
                    }
#else
        //TODO
#endif
                }
            }
        }

        static void OnGUI()
        {
            if (OnToolBarGUI != null)
                OnToolBarGUI();
        }
    }
}


