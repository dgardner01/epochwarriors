using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SFXManager))]
public class SFXManagerEditor : Editor
{
    SerializedProperty soundsProperty;

    void OnEnable()
    {
        // Setup the SerializedProperties.
        soundsProperty = serializedObject.FindProperty("sounds");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("SFX Entries", EditorStyles.boldLabel);

        if (soundsProperty != null)
        {
            for (int i = 0; i < soundsProperty.arraySize; i++)
            {
                SerializedProperty soundProperty = soundsProperty.GetArrayElementAtIndex(i);
                SerializedProperty nameProperty = soundProperty.FindPropertyRelative("name");
                SerializedProperty clipProperty = soundProperty.FindPropertyRelative("clip");
                SerializedProperty volumeProperty = soundProperty.FindPropertyRelative("volume");
                SerializedProperty descriptionProperty = soundProperty.FindPropertyRelative("description");

                EditorGUILayout.LabelField($"SFX Entry #{i + 1}", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(nameProperty, new GUIContent("Name"));
                EditorGUILayout.PropertyField(clipProperty, new GUIContent("Clip"));
                EditorGUILayout.PropertyField(volumeProperty, new GUIContent("Volume"));
                EditorGUILayout.PropertyField(descriptionProperty, new GUIContent("Description"));

                // Button for removing an element
                if (GUILayout.Button("-", GUILayout.Width(30)))
                {
                    soundsProperty.DeleteArrayElementAtIndex(i);
                    break; // Break to prevent invalid iteration if an element was removed
                }

                EditorGUILayout.Space(); // Add some space between elements
            }

            if (GUILayout.Button("Add New Sound"))
            {
                soundsProperty.arraySize++;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
