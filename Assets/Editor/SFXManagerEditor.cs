using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SFXManager))]
public class SFXManagerEditor : Editor
{
    SerializedProperty soundsProperty;

    void OnEnable()
    {
        soundsProperty = serializedObject.FindProperty("sounds");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        for (int i = 0; i < soundsProperty.arraySize; i++)
        {
            SerializedProperty soundProperty = soundsProperty.GetArrayElementAtIndex(i);
            SerializedProperty nameProperty = soundProperty.FindPropertyRelative("name");
            SerializedProperty clipsProperty = soundProperty.FindPropertyRelative("clips");
            SerializedProperty volumeProperty = soundProperty.FindPropertyRelative("volume");
            SerializedProperty minPitchProperty = soundProperty.FindPropertyRelative("minPitch");
            SerializedProperty maxPitchProperty = soundProperty.FindPropertyRelative("maxPitch");
            SerializedProperty descriptionProperty = soundProperty.FindPropertyRelative("description");
            SerializedProperty loopIndefinitelyProperty = soundProperty.FindPropertyRelative("loopIndefinitely");

            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.PropertyField(nameProperty);
            EditorGUILayout.PropertyField(clipsProperty, new GUIContent("Clips"), true);
            volumeProperty.floatValue = EditorGUILayout.Slider("Volume", volumeProperty.floatValue, 0f, 1f);

            EditorGUILayout.LabelField("Pitch Range");
            float minPitch = minPitchProperty.floatValue;  // Use temporary float variables
            float maxPitch = maxPitchProperty.floatValue;  // Use temporary float variables
            EditorGUILayout.MinMaxSlider(ref minPitch, ref maxPitch, 0.1f, 3.0f);
            minPitchProperty.floatValue = minPitch;  // Assign the temporary variable back to the SerializedProperty
            maxPitchProperty.floatValue = maxPitch;  // Assign the temporary variable back to the SerializedProperty

            EditorGUILayout.PropertyField(minPitchProperty);
            EditorGUILayout.PropertyField(maxPitchProperty);

            EditorGUILayout.PropertyField(descriptionProperty, new GUIContent("Description"), true);
            EditorGUILayout.PropertyField(loopIndefinitelyProperty, new GUIContent("Loop Indefinitely"));
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Remove Sound", GUILayout.MaxWidth(150)))
            {
                soundsProperty.DeleteArrayElementAtIndex(i);
                serializedObject.ApplyModifiedProperties();
                break; // Exit the loop to avoid index errors after removing an item
            }

            EditorGUILayout.Space();
        }


        if (GUILayout.Button("Add New Sound", GUILayout.MaxWidth(150)))
        {
            soundsProperty.arraySize++;
            serializedObject.ApplyModifiedProperties(); // Apply changes after adding a new sound
        }

        serializedObject.ApplyModifiedProperties(); // Apply changes made within the loop
    }
}
