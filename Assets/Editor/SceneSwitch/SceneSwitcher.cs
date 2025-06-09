using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;
using System.Linq;

namespace UnitySamples.Editor
{
    [InitializeOnLoad]
    public class SceneSwitchLeftButton
    {
        static SceneSwitchLeftButton()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);   
        }

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();
            string activeSceneName = SceneManager.GetActiveScene().name;
            if (EditorGUILayout.DropdownButton(new GUIContent($"Scene: {activeSceneName}", "Select Scene"), FocusType.Passive, ToolbarStyles.ButtonStyle))
            {
                GenericMenu menu = new GenericMenu();
                foreach (DropdownItem<string> sceneNameDropdown in GetBuildSceneNames().OrderBy(x => x.Text))
                {
                    menu.AddItem(new GUIContent(sceneNameDropdown.Text), sceneNameDropdown.Text == activeSceneName, SceneSelectedClick, sceneNameDropdown.Value);
                }
                menu.ShowAsContext();
            }
        }
        
        private static IList<DropdownItem<string>> GetBuildSceneNames()
        {
            var scenes = EditorBuildSettings.scenes;
            var sceneNames = new List<DropdownItem<string>>();
            foreach (var scene in scenes)
            {
                var name = Path.GetFileNameWithoutExtension(scene.path);
                sceneNames.Add(new DropdownItem<string>(name, scene.path));
            }
            return sceneNames;
        }

        private static void SceneSelectedClick(object parameter)
        {
            SceneHelper.SwitchScene((string)parameter);            
        }
    }
}