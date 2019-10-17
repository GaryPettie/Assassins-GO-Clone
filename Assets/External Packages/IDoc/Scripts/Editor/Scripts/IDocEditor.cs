/************************************************************************************************************************************	
* Developed by Mamadou Cisse                                                                                                        *
* Edited by Gary Pettie                                                                                                             *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                					*
*************************************************************************************************************************************/

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Idoc.Lib;
using UnityEditor;
using UnityEngine;
namespace IDocUnity
{

    /// <summary>
    /// Editor class of <see cref="IDoc"/> class for Unity Editor
    /// </summary>
    [InitializeOnLoad]
    internal class IDocEditor : EditorWindow
    {
        #region Fields

        private static IDocEditor Instance;
        private static Texture2D Logo;

        private const string Version = "3.0.0";
        private static string[] tabs = { "Build", "Setting", "About" };

        private int selectedTabIndex;
        private bool isInitialized;
        private GUIStyle separatorStyle;

        public const int ToolbarHeight = 20;
        public const int ToolbarWidth = 250;
        private Thread thread;
        #endregion Fields

        #region Properties

        /// <summary>
        /// The current width of the editor window
        /// </summary>
        public static float Width => Instance?.position.width ?? 0;

        /// <summary>
        /// The current height of the editor window
        /// </summary>
        public static float Height => Instance?.position.height ?? 0;

        #endregion Properties

        #region Methods

        #region Unity

        private void OnEnable()
        {
            if (!isInitialized)
            {
                separatorStyle = new GUIStyle("box");
                separatorStyle.border.top = separatorStyle.border.bottom = 1;
                separatorStyle.margin.top = separatorStyle.margin.bottom = 1;
                separatorStyle.padding.top = separatorStyle.padding.bottom = 1;
                separatorStyle.stretchWidth = true;
                Logo = Resources.Load<Texture2D>("logo");

                isInitialized = true;
            }
        }

        private void OnGUI()
        {

            Instance = this;
            selectedTabIndex = GUILayout.Toolbar(selectedTabIndex, tabs, EditorStyles.toolbarButton, GUILayout.MaxWidth(ToolbarWidth), GUILayout.Height(ToolbarHeight));
            switch (selectedTabIndex)
            {
                case 0:
                    BuildGUI();
                    break;
                case 1:
                    SettingGUI();
                    break;
                case 2:
                    AboutGUI();
                    break;
            }

            if (IDoc.Instance.IsRunning)
            {
                if (IDoc.Instance.Progression >= 1.0f)
                {
                    IDoc.Instance.Terminate();
                    EditorApplication.update -= Repaint;
                    ShowNotification(new GUIContent("Build Finished"));
                }
            }
        }

        private void OnDestroy()
        {
            IDoc.Instance.Terminate();
        }

        [MenuItem("Tools/IDoc Editor")]
        private static void OpenEditor()
        {
            var window = GetWindow<IDocEditor>();
            Instance = window;
            window.titleContent = new GUIContent("IDoc Editor", Logo);
            window.minSize = new Vector2(924, 500);
            window.maxSize = new Vector2(924, 500);
            window.Show();
        }

        #endregion Unity

        #region GUI

        private void BuildGUI()
        {
            // LEFT AREA
            GUI.Label(new Rect(ToolbarWidth, ToolbarHeight, 2, position.width), GUIContent.none, separatorStyle);
            GUILayout.BeginArea(new Rect(0, ToolbarHeight, ToolbarWidth, position.height));

            Setting.Language = (Language)EditorGUILayout.EnumPopup("Language :", Setting.Language);
            if (IDoc.Instance.IsRunning)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Stop", GUILayout.Width(100)))
                {
                    IDoc.Instance.Cancel();
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                var progression = IDoc.Instance.Progression;
                EditorGUI.ProgressBar(GUILayoutUtility.GetRect(0, 30, GUILayout.ExpandWidth(true)), progression, $"{progression * 100}%");
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Build", GUILayout.Width(100)))
                {
                    Build();
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUILayout.EndArea();

            // RIGHT AREA
            GUILayout.BeginArea(new Rect(ToolbarWidth, ToolbarHeight, position.width - ToolbarWidth, position.height));
            LoggerEditor.OnGUI();
            GUILayout.EndArea();
        }

        private void SettingGUI()
        {
            SettingEditor.OnGUI();
        }

        private void AboutGUI()
        {
            var center = new GUIStyle(EditorStyles.largeLabel);
            center.alignment = TextAnchor.MiddleCenter;
            GUILayout.Space(20);
            GUILayout.Label("IDOC is an automatic C# documentation generator for Unity", center);
            GUILayout.Label($"Version: {Version}", center);
            GUILayout.Label("Developed by Mamadou Cisse", center);

            GUILayout.Space(20);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Follow me for more informations about my projects", EditorStyles.miniLabel);
            GUILayout.Space(15);
            if (GUILayout.Button("TWITTER"))
            {
                Application.OpenURL("http://www.twitter.com/IncMce");
            }
            GUILayout.Space(20);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.Label("Asset Store Catalog", EditorStyles.miniLabel);
            if (GUILayout.Button("ASSET STORE"))
            {
                Application.OpenURL("http://u3d.as/riS");
            }
            GUILayout.Space(20);
            EditorGUILayout.EndHorizontal();
        }

        public static void Refresh()
        {
            if (Instance != null && !IDoc.Instance.IsRunning)
            {
                Instance.Repaint();
            }
        }

        #endregion GUI

        private void Build()
        {
            Setting.SaveSettings();
            if (!Setting.InputDirectories.Any())
            {
                Toast("There is no input folder");
                return;
            }

            foreach (var dir in Setting.InputDirectories)
            {
                if (!Directory.Exists(dir))
                {
                    Toast($"The folder {dir} does not exists");
                    Debug.LogError($"The folder {dir} does not exists");
                    return;
                }
            }
            EditorApplication.update += Repaint;
            thread = new Thread(() =>
            {
                try
                {
                    Task.WaitAll(IDoc.Instance.Run());
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            });

            thread.Start();
        }

        /// <summary>
        /// Draws a drag and drop area and return <c>true</c> if there is an dragged object.
        /// </summary>
        /// <param name="width">The max witdh of the drag and drop area</param>
        /// <param name="height">The height of the drag and drop area</param>
        /// <param name="message">The message to display in the drag and drop area</param>
        /// <param name="onDragColor">Color of the drag and drop area when there is a dragged object</param>
        /// <returns><c>true</c> if there is an dragged object <c>false</c> otherwise.</returns>
        public static bool Drop(float width, float height, string message, Color onDragColor)
        {
            var dragArea = GUILayoutUtility.GetRect(0, 0, GUILayout.MaxWidth(width), GUILayout.Height(height));
            var evt = Event.current;

            GUI.color = dragArea.Contains(evt.mousePosition) ? onDragColor : Color.white;
            GUI.Box(dragArea, message);
            GUI.color = Color.white;

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dragArea.Contains(evt.mousePosition))
                        break;
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        return true;
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// Show a message
        /// </summary>
        /// <param name="content">The message</param>
        public static void Toast(string content)
        {
            Instance.ShowNotification(new GUIContent(content));
        }

        /// <summary>
        /// Checks if there is a folder at the given path
        /// </summary>
        /// <param name="path">The path</param>
        /// <returns><c>true</c> if there is a folder at the path <c>false</c> otherwise</returns>
        public static bool IsDirectory(string path)
        {
            var attributes = File.GetAttributes(path);
            return ((attributes & FileAttributes.Directory) == FileAttributes.Directory);
        }

        #endregion Methods

    }
}