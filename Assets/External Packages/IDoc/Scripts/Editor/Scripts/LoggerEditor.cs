/************************************************************************************************************************************	
* Developed by Mamadou Cisse                                                                                                        *
* Edited by Gary Pettie                                                                                                             *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                					*
*************************************************************************************************************************************/

using UnityEngine;
using UnityEditor;
using Idoc.Lib;
using Logger = Idoc.Lib.Logger;
using LogType = Idoc.Lib.LogType;
using System.Collections.Generic;

namespace IDocUnity
{

    /// <summary>
    /// Editor claas for <see cref="Logger"/> class for Unity Editor
    /// </summary>
    internal static class LoggerEditor
    {
        #region Fields
        private const string ShowInfoPref = "LOGGER_EDITOR_SHOW_INFO";
        private const string ShowWarningPref = "LOGGER_EDITOR_SHOW_WARNING";
        private const string ShowErrorPref = "LOGGER_EDITOR_SHOW_ERROR";

        private const float resizerHeight = 5f;
        private const float menuBarHeight = 20f;
        private const int maxVisibleItems = 25;
        private const int rowHeight = 20;
        private static Rect upperPanel;
        private static Rect lowerPanel;
        private static Rect resizer;
        private static Rect menuBar;

        private static float sizeRatio = 0.5f;
        private static bool isResizing;

        private static float height;
        private static float width;

        private static Texture2D errorIcon;
        private static Texture2D errorIconSmall;
        private static Texture2D warningIcon;
        private static Texture2D warningIconSmall;
        private static Texture2D infoIcon;
        private static Texture2D infoIconSmall;


        private static GUIStyle boxStyle;
        private static GUIStyle resizerStyle;
        private static GUIStyle textAreaStyle;

        private static Texture2D boxBgOdd;
        private static Texture2D boxBgEven;
        private static Texture2D boxBgSelected;
        private static Texture2D icon;

        private static Vector2 upperScroll;
        private static Vector2 lowerScroll;
        private static Log selectedLog;
        private static List<Log> logs;
        private static bool isInitialized;
        #endregion Fields

        #region Properties

        private static bool ShowInfo { get; set; }
        private static bool ShowWarning { get; set; }
        private static bool ShowError { get; set; }

        #endregion Properties

        private static void OnEnable()
        {
            if (isInitialized)
                return;

            errorIcon = EditorGUIUtility.Load("icons/console.erroricon.png") as Texture2D;
            warningIcon = EditorGUIUtility.Load("icons/console.warnicon.png") as Texture2D;
            infoIcon = EditorGUIUtility.Load("icons/console.infoicon.png") as Texture2D;

            errorIconSmall = EditorGUIUtility.Load("icons/console.erroricon.sml.png") as Texture2D;
            warningIconSmall = EditorGUIUtility.Load("icons/console.warnicon.sml.png") as Texture2D;
            infoIconSmall = EditorGUIUtility.Load("icons/console.infoicon.sml.png") as Texture2D;

            resizerStyle = new GUIStyle("box");
            resizerStyle.border.left = resizerStyle.border.right = 1;
            resizerStyle.margin.left = resizerStyle.margin.right = 1;
            resizerStyle.padding.left = resizerStyle.padding.right = 1;

            boxStyle = new GUIStyle();
            boxStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f);

            boxBgOdd = EditorGUIUtility.Load("builtin skins/darkskin/images/cn entrybackodd.png") as Texture2D;
            boxBgEven = EditorGUIUtility.Load("builtin skins/darkskin/images/cnentrybackeven.png") as Texture2D;
            boxBgSelected = EditorGUIUtility.Load("builtin skins/darkskin/images/menuitemhover.png") as Texture2D;

            textAreaStyle = new GUIStyle();
            textAreaStyle.normal.textColor = new Color(0.9f, 0.9f, 0.9f);
            textAreaStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/projectbrowsericonareabg.png") as Texture2D;
            selectedLog = null;

            ShowInfo = PlayerPrefs.GetInt(ShowInfoPref, 0) == 1;
            ShowWarning = PlayerPrefs.GetInt(ShowWarningPref, 0) == 1;
            ShowError = PlayerPrefs.GetInt(ShowErrorPref, 0) == 1;


            isInitialized = true;
            logs = new List<Log>();
        }

        public static void OnGUI()
        {
            OnEnable();

            logs.Clear();
            var len = Logger.Logs.Count;
            for (var i = 0; i < len; i++)
            {
                if (ShouldDisplay(Logger.Logs[i].type))
                    logs.Add(Logger.Logs[i]);
            }

            width = IDocEditor.Width - IDocEditor.ToolbarWidth;
            height = IDocEditor.Height - IDocEditor.ToolbarHeight;

            DrawToolbar();
            DrawUpperPanel();
            DrawResizer();
            DrawLowerPanel();

            ProcessEvents(Event.current);
        }

        private static void DrawResizer()
        {
            resizer = new Rect(0, (height * sizeRatio) - resizerHeight, width, resizerHeight * 2);
            GUILayout.BeginArea(new Rect(resizer.position + (Vector2.up * resizerHeight), new Vector2(width, 2)), resizerStyle);
            GUILayout.EndArea();
            EditorGUIUtility.AddCursorRect(resizer, MouseCursor.ResizeVertical);
        }

        private static void DrawToolbar()
        {
            var rect = new Rect(0, 0, IDocEditor.Width - IDocEditor.ToolbarWidth, 20);
            GUILayout.BeginArea(rect, EditorStyles.toolbar);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(40)))
            {
                Logger.Clear();
                selectedLog = null;
            }

            GUILayout.Space(5);

            GUILayout.FlexibleSpace();

            var _showInfo = ShowInfo;
            var _showWarning = ShowWarning;
            var _showError = ShowError;

            ShowInfo = GUILayout.Toggle(ShowInfo, new GUIContent("I", infoIconSmall), EditorStyles.toolbarButton, GUILayout.Width(30));
            ShowWarning = GUILayout.Toggle(ShowWarning, new GUIContent("W", warningIconSmall), EditorStyles.toolbarButton, GUILayout.Width(30));
            ShowError = GUILayout.Toggle(ShowError, new GUIContent("E", errorIconSmall), EditorStyles.toolbarButton, GUILayout.Width(30));

            if (_showInfo != ShowInfo || _showWarning != ShowWarning || _showError != ShowError)
            {
                PlayerPrefs.SetInt(ShowInfoPref, ShowInfo ? 1 : 0);
                PlayerPrefs.SetInt(ShowWarningPref, ShowWarning ? 1 : 0);
                PlayerPrefs.SetInt(ShowErrorPref, ShowError ? 1 : 0);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private static void DrawUpperPanel()
        {
            upperPanel = new Rect(0, menuBarHeight, width, (height * sizeRatio) - menuBarHeight);


            GUILayout.BeginArea(upperPanel);
            upperScroll = GUILayout.BeginScrollView(upperScroll);


            var firstVisibleIndex = (int)(upperScroll.y / rowHeight);
            firstVisibleIndex = Mathf.Clamp(firstVisibleIndex, 0, Mathf.Max(0, logs.Count - maxVisibleItems));
            GUILayout.Space(firstVisibleIndex * rowHeight);
            for (int i = firstVisibleIndex; i < Mathf.Min(logs.Count, firstVisibleIndex + maxVisibleItems); i++)
            {
                if (ShouldDisplay(logs[i].type) && Box(logs[i].message, logs[i].type, i % 2 == 0, logs[i].isSelected))
                {
                    if (selectedLog != null)
                    {
                        selectedLog.isSelected = false;
                    }
                    logs[i].isSelected = true;
                    selectedLog = logs[i];
                }
            }
            GUILayout.Space(Mathf.Max(0, (logs.Count - firstVisibleIndex - maxVisibleItems) * rowHeight));
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private static void DrawLowerPanel()
        {
            lowerPanel = new Rect(0, ((height) * sizeRatio) + resizerHeight, width, ((height) * (1 - sizeRatio)) - resizerHeight);
            GUILayout.BeginArea(lowerPanel);
            lowerScroll = GUILayout.BeginScrollView(lowerScroll);

            if (selectedLog != null && Logger.Logs.Count > 0)
            {
                GUILayout.TextArea(selectedLog.message, textAreaStyle);
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private static void ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    isResizing |= (e.button == 0 && resizer.Contains(e.mousePosition));
                    break;
                case EventType.MouseUp:
                    isResizing = false;
                    break;
            }

            Resize(e);
        }

        private static void Resize(Event e)
        {
            if (isResizing)
            {
                sizeRatio = e.mousePosition.y / height;
                IDocEditor.Refresh();
            }
        }

        private static bool Box(string content, LogType logType, bool isOdd, bool isSelected)
        {
            if (isSelected)
            {
                boxStyle.normal.background = boxBgSelected;
            }
            else
            {
                boxStyle.normal.background = isOdd ? boxBgOdd : boxBgEven;
            }

            switch (logType)
            {
                case LogType.Error: icon = errorIcon; break;
                case LogType.Warning: icon = warningIcon; break;
                case LogType.Log: icon = infoIcon; break;
            }

            return GUILayout.Button(new GUIContent(content, icon), boxStyle, GUILayout.ExpandWidth(true), GUILayout.Height(rowHeight));
        }

        private static bool ShouldDisplay(LogType type)
        {
            switch (type)
            {
                case LogType.Log:
                    return ShowInfo;

                case LogType.Warning:
                    return ShowWarning;

                case LogType.Error:
                    return ShowError;

                default:
                    return false;
            }

        }

    }
}