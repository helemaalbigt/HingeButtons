using UnityEngine;

public class AudioEffect : Effect
{
    public AudioSource _audioSource;

    protected override void OnStateChange(InteractableState state) {
        switch (state) {
            case InteractableState.clicked:
                _audioSource.Play();
                break;
        }
    }
}
