using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioClip[] FootstepAudioClips;
    public float FootstepAudioVolume = 1.0f;

    [SerializeField] private AudioSource audioSourcePrefab; // Referensi ke prefab AudioSource

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                // Posisi di mana klip akan dimainkan (misalnya, posisi karakter)
                Vector3 playPosition = transform.position;

                // Instansiasi prefab AudioSource di posisi yang diinginkan
                AudioSource tempAudioSource = Instantiate(audioSourcePrefab, playPosition, Quaternion.identity);

                // Atur klip audio dan volume
                tempAudioSource.clip = FootstepAudioClips[index];
                tempAudioSource.volume = FootstepAudioVolume;

                // Mainkan klip audio
                tempAudioSource.Play();

                // Hancurkan game object setelah klip audio selesai dimainkan
                Destroy(tempAudioSource.gameObject, FootstepAudioClips[index].length);
            }
        }
    }
}
