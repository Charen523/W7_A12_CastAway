using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundOnEnable : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip enableSound; 

    void OnEnable()
    {
        // 활성화 시 소리 재생
        if (audioSource != null && enableSound != null)
        {
            audioSource.PlayOneShot(enableSound);
        }
    }
}
