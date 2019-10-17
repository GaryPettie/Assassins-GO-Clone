/************************************************************************************************************************************	
* Developed by Mamadou Cisse                                                                                                        *
* Edited by Gary Pettie                                                                                                             *
* Mail => mciissee@gmail.com                                                                                                        *
* Twitter => http://www.twitter.com/IncMce                                                                                          *
* Unity Asset Store catalog: http://u3d.as/riS	                                                                					*
*************************************************************************************************************************************/

using System.IO;
using UnityEngine;
using UnityEditor;
using System.Text;
using Idoc.Lib;
using System;
using System.Linq;

namespace IDocUnity
{
    /// <summary>
    /// Setting class editor
    /// </summary>
    public class SettingEditor
    {

        #region Fields

        private static SettingEditor instance;
        private Setting settings;
        private Vector2 scroll;

        #endregion Fields

        #region Properties

        private static SettingEditor Instance => instance ?? (instance = new SettingEditor());

        #endregion Properties

        #region Methods

        /// <summary>
        /// Draws thr editor
        /// </summary>
        public static void OnGUI()
        {
            Instance.scroll = GUILayout.BeginScrollView(Instance.scroll);
            GUILayout.Space(10);

            // SAVE
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Save", GUILayout.Width(100)))
            {
                Setting.SaveSettings();
                AssetDatabase.Refresh();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.Label("Options", EditorStyles.boldLabel);
            Setting.ExtractPublic = EditorGUILayout.Toggle("Extract Public", Setting.ExtractPublic);
            Setting.ExtractProtected = EditorGUILayout.Toggle("Extract Protected", Setting.ExtractProtected);
            Setting.ExtractPrivate = EditorGUILayout.Toggle("Extract Private", Setting.ExtractPrivate);
            Setting.ExtractInternal = EditorGUILayout.Toggle("Extract Internal", Setting.ExtractInternal);
            Setting.ExtractUndocumentedMembers = EditorGUILayout.Toggle("Extract Undocumented", Setting.ExtractUndocumentedMembers);

            GUILayout.Space(10);

            GUILayout.Label("Directories", EditorStyles.boldLabel);
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Output : ", Setting.OutputDirectory, EditorStyles.textField);
            if (GUILayout.Button("...", EditorStyles.miniButtonRight, GUILayout.Width(22)))
            {
                Setting.OutputDirectory = EditorUtility.OpenFolderPanel("Select the output folder", Setting.OutputDirectory, string.Empty);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (IDocEditor.Drop(350, 20, "DRAG AND DROP MULTIPLE FOLDERS TO PROCESS", new Color(0, 0, 0, .1f)))
            {
                foreach (var folder in DragAndDrop.paths)
                {
                    if (IDocEditor.IsDirectory(folder) && !Setting.TryAddInputDirectory(folder))
                    {
                        IDocEditor.Toast($"The folder {folder} is already added");
                    }
                }
                GUILayout.EndScrollView();
                return;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Or choose individual folders", GUILayout.Width(350)))
            {
                var folder = EditorUtility.OpenFolderPanel("Select a folder to process", string.Empty, string.Empty);
                if (!string.IsNullOrEmpty(folder) && !Setting.TryAddInputDirectory(folder))
                {
                    IDocEditor.Toast($"The folder {folder} is already added");
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            for (var i = 0; i < Setting.InputDirectories.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(Setting.InputDirectories[i], EditorStyles.boldLabel);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                    Setting.InputDirectories.RemoveAt(i);
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            if (Setting.InputDirectories.Any())
            {
                GUI.color = new Color(1, 1, 1, .2f);
                GUI.Box(GUILayoutUtility.GetLastRect(), GUIContent.none);
                GUI.color = Color.white;
            }
            GUILayout.EndScrollView();
        }

        #endregion Methods

    }
}