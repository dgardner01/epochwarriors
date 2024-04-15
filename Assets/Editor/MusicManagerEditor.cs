using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(MusicManager))]
public class MusicManagerEditor : Editor
{
    private SerializedProperty musicTracksProperty;

    private void OnEnable()
    {
        musicTracksProperty = serializedObject.FindProperty("musicTracks");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (musicTracksProperty != null)
        {
            for (int i = 0; i < musicTracksProperty.arraySize; i++)
            {
                SerializedProperty musicProperty = musicTracksProperty.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(musicProperty);

                if (GUILayout.Button("Remove Music Track"))
                {
                    musicTracksProperty.DeleteArrayElementAtIndex(i);
                }
            }

            if (GUILayout.Button("Add New Music Track"))
            {
                musicTracksProperty.arraySize++;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
