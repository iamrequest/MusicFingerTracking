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

    public Hand primaryHand;
    private Hand secondaryHand;
    public SteamVR_Action_Skeleton skeletonAction;
    public SteamVR_Action_Vector2 playSoundAction;

    [Tooltip("The minimum amount of finger curl to count as pressing a valve down")]
    [Range(0f, 1f)]
    public float fingerPressThreshold;

    [Tooltip("The absoulute minimum amount of vertical axis to count as playing a note")]
    [Range(0f, 1f)]
    public float notePlayThreshold;

    // Start is called before the first frame update
    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;

        // Default the primary hand to the right hand
        if (primaryHand == Player.instance.leftHand) {
            secondaryHand = Player.instance.rightHand;
        } else {
            primaryHand = Player.instance.rightHand;
            secondaryHand = Player.instance.leftHand;
        }

        playSoundAction.AddOnChangeListener(PlayAudio, primaryHand.handType);
        playSoundAction.AddOnUpdateListener(UpdateOvertone, secondaryHand.handType);
    }

    // Update is called once per frame
    void Update() {
        AudioClip previousClip = audioSource.clip;
        AudioClip newClip = GetPlayedNote();

        // If the audio clip changed, we need to restart the audio source
        if (previousClip != newClip) {
            if (audioSource.isPlaying) {
                audioSource.Stop();
                audioSource.clip = newClip;
                audioSource.Play();
            } else {
                audioSource.clip = newClip;
            }
        }
    }

    private void PlayAudio(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta) {
        if (Mathf.Abs(axis.y) > notePlayThreshold) {
            if(!audioSource.isPlaying) audioSource.Play();

            audioSource.volume = RemapFloat(Mathf.Abs(axis.y), notePlayThreshold, 1f, 0f, 1f);
        } else {
            audioSource.Stop();
        }
    }


    public int overtone;
    private AudioClip GetPlayedNote() {
        if (overtone == 2) {
            switch (GetValveCombination()) {
                //case 0: return soundLibrary.GetNote(2, NOTES.G);
                case 1: return soundLibrary.GetNote(2, NOTES.As);
                case 2: return soundLibrary.GetNote(2, NOTES.B);
                case 3: return soundLibrary.GetNote(3, NOTES.A);
                //case 4: return soundLibrary.GetNote(2, NOTES.?);
                case 5: return soundLibrary.GetNote(2, NOTES.G);
                case 6: return soundLibrary.GetNote(2, NOTES.Gs);
                case 7: return soundLibrary.GetNote(2, NOTES.Fs);
                default:
                    Debug.LogError("No note returned. Overtone: " + overtone + ", Valves: " + GetValveCombination());
                    return null;
            }
        } else if (overtone == 3) {
            switch (GetValveCombination()) {
                case 0: return soundLibrary.GetNote(3, NOTES.C);
                case 1: return soundLibrary.GetNote(3, NOTES.F);
                case 2: return soundLibrary.GetNote(3, NOTES.Fs);
                case 3: return soundLibrary.GetNote(3, NOTES.E);
                //case 4: return soundLibrary.GetNote(3, NOTES.?);
                case 5: return soundLibrary.GetNote(3, NOTES.D);
                case 6: return soundLibrary.GetNote(3, NOTES.Ds);
                case 7: return soundLibrary.GetNote(3, NOTES.Cs);
                default:
                    Debug.LogError("No note returned. Overtone: " + overtone + ", Valves: " + GetValveCombination());
                    return null;
            }
        } else if (overtone == 4) {
            switch (GetValveCombination()) {
                case 0: return soundLibrary.GetNote(3, NOTES.G);
                case 1: return soundLibrary.GetNote(3, NOTES.As);
                case 2: return soundLibrary.GetNote(3, NOTES.B);
                case 3: return soundLibrary.GetNote(3, NOTES.A);
                //case 4: return soundLibrary.GetNote(3, NOTES.?);
                //case 5: return soundLibrary.GetNote(3, NOTES.?);
                case 6: return soundLibrary.GetNote(3, NOTES.Gs);
                //case 7: return soundLibrary.GetNote(3, NOTES.?);
                default:
                    Debug.LogError("No note returned. Overtone: " + overtone + ", Valves: " + GetValveCombination());
                    return null;
            }
        }

        Debug.LogError("No note returned. Overtone: " + overtone + ", Valves: " + GetValveCombination());
        return null;
    }

    private void UpdateOvertone(SteamVR_Action_Vector2 fromAction, SteamVR_Input_Sources fromSource, Vector2 axis, Vector2 delta) {
        if (axis.y > 0.5) {
            overtone = 4;
        } else if (axis.y < -0.5) {
            overtone = 2;
        } else {
            overtone = 3;
        }
    }


    /// <summary>
    /// Given the finger curl amount on each of the player's finger, return a boolean representation
    /// of what valve combination is pressed.
    /// 
    /// While facing the trumpet's right side (mouthpiece on the left), the valves align as such:
    /// Valve 1: +1
    /// Valve 2: +2
    /// Valve 3: +4
    ///
    /// So, figure out your fingering with your right hand, and turn your palm to face you.
    /// The result of your finger placement will be in binary format
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

    private float RemapFloat(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
