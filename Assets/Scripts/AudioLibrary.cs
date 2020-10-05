using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NOTES { C, Cs, D, Ds, E, F, Fs, G, Gs, A, As, B};
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnAudioLibrary", order = 1)]
public class AudioLibrary : ScriptableObject {
    const int NOTES_IN_OCTAVE = 12;

    // This represents all notes available in this library.
    // To find some note in some octave, eg: C3, then multiply octave*12 + noteOffset
    public List<AudioClip> notes;

    public void AddOctave() {
        for(int i = 0; i < NOTES_IN_OCTAVE; i++) 
            notes.Add(null);
    }
    public void DeleteOctave(int octave) {
        notes.RemoveRange(octave * NOTES_IN_OCTAVE, NOTES_IN_OCTAVE);
    }

    /// <summary>
    /// Fetches the AudioClip associated with this note/octave pair.
    /// </summary>
    /// <param name="octave">The index of the octave. This parameter is one-indexed, to align with music notation</param>
    /// <param name="note">The note in the octave.</param>
    /// <returns></returns>
    public AudioClip GetNote(int octave, NOTES note) {
        int index = (octave-1) * NOTES_IN_OCTAVE + (int)note;

        if (index < 0 || index > notes.Count - 1) {
            Debug.LogError("Index is out of bounds! Octave: " + octave + 
                ", note: " + note + "(" + (int)note+ ")");
            return null;
        }

        // TODO: Check for out of bounds on octaves list
        return notes[index];
    }
}
