using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum NOTES { C, Cs, D, Ds, E, F, Fs, G, Gs, A, As, B };
public class Octave : MonoBehaviour {
    public AudioClip A;
    public AudioClip As;
    public AudioClip B;
    public AudioClip C;
    public AudioClip Cs;
    public AudioClip D;
    public AudioClip Ds;
    public AudioClip E;
    public AudioClip F;
    public AudioClip Fs;
    public AudioClip G;
    public AudioClip Gs;

    public AudioClip GetNote(NOTES note) {
        switch (note) {
            case NOTES.A: return A;
            case NOTES.As: return As;
            case NOTES.B: return B;
            case NOTES.C: return C;
            case NOTES.Cs: return Cs;
            case NOTES.D: return D;
            case NOTES.Ds: return Ds;
            case NOTES.E: return E;
            case NOTES.F: return F;
            case NOTES.Fs: return Fs;
            case NOTES.G: return F;
            case NOTES.Gs: return Fs;
            default: return null;
        }
    }
}
