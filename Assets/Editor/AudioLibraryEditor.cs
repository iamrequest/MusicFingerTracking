using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


// TODO: This inspector doesn't save properly. 
//  Any re-compiles in this file will erase data in all AudioLibrary ScriptableObjects!
// Changes persist when testing with primitives (int), but they do not persist with Lists. Possibly because
//  AudioLibrary is a SerializedObject instead of a MonoBehaviour?
[CustomEditor(typeof(AudioLibrary))]
public class AudioLibraryEditor : Editor {
    AudioLibrary audioLibrary;
    private List<bool> expandOctaves;

    public void Reset() {
        if (audioLibrary.notes == null) {
            audioLibrary.notes = new List<AudioClip> ();
        }

        if (expandOctaves == null) {
            expandOctaves = new List<bool> ();
        }

        if (expandOctaves.Count != audioLibrary.notes.Count / 12) {
            expandOctaves.Clear();

            for(int i = 0; i < audioLibrary.notes.Count / 12; i++) 
                expandOctaves.Add(false);
        }
    }

    private void OnEnable() {
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        audioLibrary = target as AudioLibrary;
        Reset();

        // -- Summary diagnostics
        // Number of octaves, and add octave button
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Number of Octaves: ", GUILayout.MaxWidth(150));
        EditorGUILayout.TextField(audioLibrary.notes.Count / 12 + "");
        if (GUILayout.Button("+")) {
            Undo.RecordObject(target, "Add Octave");
            EditorUtility.SetDirty(target);

            audioLibrary.AddOctave();
            expandOctaves.Add(false);
        }

        // Raw number of notes, and clear all button
        EditorGUILayout.LabelField("Number of Notes: ", GUILayout.MaxWidth(150));
        EditorGUILayout.TextField(audioLibrary.notes.Count+ "");
        if (GUILayout.Button("x")) {
            Undo.RecordObject(target, "Clear all");
            EditorUtility.SetDirty(target);

            audioLibrary.notes.Clear();
            expandOctaves.Clear();
        }
        EditorGUILayout.EndHorizontal();


        // -- Looping through octaves
        int numberOfOctaves = audioLibrary.notes.Count / 12;
        for (int currentOctave = 0; currentOctave < numberOfOctaves; currentOctave++) {
            expandOctaves[currentOctave] = EditorGUILayout.Foldout(expandOctaves[currentOctave], "Octave " + (currentOctave+1));
            EditorGUI.indentLevel += 1;

            if (expandOctaves[currentOctave]) {
                int offset = currentOctave * 12;

                // -- Drawing each note in the octave
                audioLibrary.notes[offset + 0] =  DrawNoteGUI(audioLibrary.notes[offset + 0], "C");
                audioLibrary.notes[offset + 1] =  DrawNoteGUI(audioLibrary.notes[offset + 1], "C# / Bb");
                audioLibrary.notes[offset + 2] =  DrawNoteGUI(audioLibrary.notes[offset + 2], "D");
                audioLibrary.notes[offset + 3] =  DrawNoteGUI(audioLibrary.notes[offset + 3], "D# / Eb");
                audioLibrary.notes[offset + 4] =  DrawNoteGUI(audioLibrary.notes[offset + 4], "E");
                audioLibrary.notes[offset + 5] =  DrawNoteGUI(audioLibrary.notes[offset + 5], "F");
                audioLibrary.notes[offset + 6] =  DrawNoteGUI(audioLibrary.notes[offset + 6], "F# / Gb");
                audioLibrary.notes[offset + 7] =  DrawNoteGUI(audioLibrary.notes[offset + 7], "G");
                audioLibrary.notes[offset + 8] =  DrawNoteGUI(audioLibrary.notes[offset + 8], "G# / Ab");
                audioLibrary.notes[offset + 9] =  DrawNoteGUI(audioLibrary.notes[offset + 9], "A");
                audioLibrary.notes[offset + 10] = DrawNoteGUI(audioLibrary.notes[offset + 10], "A# / Bb");
                audioLibrary.notes[offset + 11] = DrawNoteGUI(audioLibrary.notes[offset + 11], "B");

                if (GUILayout.Button("Delete Octave")) {
                    Undo.RecordObject(target, "Clear all");
                    EditorUtility.SetDirty(target);

                    audioLibrary.DeleteOctave(currentOctave);
                    expandOctaves.RemoveAt(currentOctave);
                    return;
                }
            }
            EditorGUI.indentLevel -= 1;
        }
    }

    private AudioClip DrawNoteGUI(AudioClip previousValue, string label) {
        EditorGUI.BeginChangeCheck();

        AudioClip newValue;
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.MaxWidth(100));
        newValue = (AudioClip)EditorGUILayout.ObjectField(previousValue, typeof(AudioClip), true, GUILayout.MinWidth(100));
        GUILayout.EndHorizontal();


        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(target, "Set audioclip");
            EditorUtility.SetDirty(target);
        }

        return newValue;
    }
}
