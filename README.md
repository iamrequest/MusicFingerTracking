# MusicFingerTracking
An musical experiment in finger tracking, using the Valve Knuckle controllers.

This is a quick whitebox experiment, where the player can play a synth trumpet (Bb), using Valve Knuckles. By curling your index/middle/ring finger, the 3 valves of the trumpet are closed.

I've only had a brief run-in with a trumpet many years ago, so it's not unlikely that things are not wired up properly. Some fingerings are not filled out, since the fingering charts I referenced were not complete - it's good enough for a whitebox tech demo :) 

## Controls

Right hand:
* Curl index/middle/ring fingers: Open/close valves
* Joystick up/down: Play note

Left Hand:
* Joystick up/down: Shift overtone (?). Essentially, this replicates shifting how high/low your lip buzzing is. Not familiar with the terminology, so this may be the wrong word for it.

## Some things of interest

* Finger tracking used to control the trumpet's notes
* Custom inspector to make loading audioclips manageable. 

## Future Works

* Replace the trumpet with an actual trumpet model
* Animate the trumpet's valves opening and closing based on how closed your fingers are (either lerp between 2 positions, or pass the raw/smoothed value into an Animator)
* Animate right hand to match valve presses
* Add sheet music, tutorial
* Add functionality to use voice to trigger a note (eg: hum in different pitches to simulate lip buzzing)
* Make the trumpet a generic interactable object that can be picked up, rather than something that's glued to the hand. Maybe pressing the face buttons would drop it, since the player's fingers are in use
* Generate URP materials (not sure why I made this project with URP - I had no need for shadergraph here)
* Re-record sample audio. 
  * The synth audio I recorded in famitracker came with a few miliseconds of intro/outro silence, which I wasn't able to cut cleanly with a simple Audacity macro. 
  * Since the audio waves don't line up, there's a skip in the sound roughly every second when the audio track loops.
* Automate the population of AudioClips in an AudioLibrary ScriptableObject via a custom inspector. Dragging and dropping multiple octaves worth of sound files is tedious (12 notes per octave, ~3 octaves for the trumpet in this implementation)
  * Something to look into if this is something anyone's interested in: https://forum.unity.com/threads/how-to-add-a-folder-to-a-selection.354280/
  
Future works could be to introduce other instruments. Instruments that have simpler interfaces are more do-able with this method (trumpet, flute, drums), as compared to others (piano, guitar). Essentially, the less you have to shift your hands, the better.

If I were to continue with this project, I'd probably look into something like the saxophone or a flute. Never played those instruments, but they're roughly similar. Shifting hand positions could just be a change in hand transform. 

For something in a different route, I'd take a look into a drum set. If the player had hands were made via rigidbodies and joints (See: [Wirewhiz Half Life Alyx hands](https://wirewhiz.com/making-half-life-alyx-physics-hands-in-unity/)), you could simulate a drum roll by adding force to the drumstick relative to the collision normal. This would be more satisfying, compared to having your hand clip through the drum 

## Credits

* Initial piano audio (used in earlier commits, still in project files, not used anymore): [by SingleInfinity, from /r/piano](https://www.reddit.com/r/piano/comments/3u6ke7/heres_some_midi_and_mp3_files_for_individual/)
* Synth audio: Created in famitracker, processed in audacity)
* Unity Version: 2019.4.5f1
* VR Plugin: SteamVR v2.6.1 (sdk 1.13.10)
