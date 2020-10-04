using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(AudioSource))]
public class Trumpet : MonoBehaviour {
    private AudioSource audioSource;
    public AudioLibrary soundLibrary;

    private Hand hand;
    public SteamVR_Action_Skeleton skeletonAction;
    public SteamVR_Action_Boolean playSoundAction;

    [Tooltip("The minimum amount of finger curl to count as pressing a valve down")]
    [Range(0f, 1f)]
    public float fingerPressThreshold;

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();

        hand = GetComponentInParent<Hand>();
        playSoundAction.AddOnStateDownListener(PlayAudio, hand.handType);
        playSoundAction.AddOnStateUpListener(StopAudio, hand.handType);
    }

    // Update is called once per frame
    void Update() {
        //Debug.Log(skeletonAction.indexCurl);
    }

    private void StopAudio(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        audioSource.Stop();
    }

    // TODO: This should be an update function, and should change the audio clip when the played note changes
    private void PlayAudio(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource) {
        audioSource.clip = GetPlayedNote();
        audioSource.loop = true;
        audioSource.Play();
    }

    private AudioClip GetPlayedNote() {
        int octave = GetOctave();

        switch (GetValveCombination()) {
            case 0: return soundLibrary.GetNote(octave, NOTES.C);
            case 1: return soundLibrary.GetNote(octave, NOTES.F);
            case 2: return soundLibrary.GetNote(octave, NOTES.Fs);
            case 3: return soundLibrary.GetNote(octave, NOTES.E);
            case 4: return soundLibrary.GetNote(octave, NOTES.D);
            case 5: return soundLibrary.GetNote(octave, NOTES.D);
            case 6: return soundLibrary.GetNote(octave, NOTES.Ds);
            case 7: return soundLibrary.GetNote(octave, NOTES.Cs);
            default: 
                Debug.LogError("No note returned. Octave: " + octave + ", Valves: " + GetValveCombination());
                return null;
        }
    }

    private int GetOctave() {
        // TODO: Update this to reflect breath pitch
        return 3;
    }

    /// <summary>
    /// Given the finger curl amount on each of the player's finger, return a boolean representation
    /// of what valve combination is pressed.
    /// 
    /// While facing the trumpet's right side (mouthpiece on the left), the valves align as such:
    /// Valve 1: +1
    /// Valve 2: +2
    /// Valve 3: +4
    /// </summary>
    /// <returns></returns>
    private int GetValveCombination() {
        int valveCombo = 0;
        if (skeletonAction.indexCurl > fingerPressThreshold) {
            valveCombo += 1;
        }
        if (skeletonAction.middleCurl > fingerPressThreshold) {
            valveCombo += 2;
        }
        if (skeletonAction.ringCurl > fingerPressThreshold) {
            valveCombo += 4;
        }

        return valveCombo;
    }
}
