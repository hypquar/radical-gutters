using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [Header("Sound settings")]
    public AudioClip clip;
    public bool isLooped = false;
    [Range(0f, 1f)]
    public float spatialBlend = 1f; // 0 = 2D, 1 = 3D

    [Header("Optional")]
    public AudioSource source; 

    void Reset()
    {
        source = GetComponent<AudioSource>();
    }

    void Awake()
    {
        if (source == null)
            source = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        if (clip == null)
        {
            Debug.LogWarning($"AudioPlayer ({name}): clip is null.");
            return;
        }

        if (source == null)
        {
            Debug.LogWarning($"AudioPlayer ({name}): no AudioSource found.");
            return;
        }

        source.spatialBlend = Mathf.Clamp01(spatialBlend);

        if (isLooped)
        {
            source.clip = clip;
            source.loop = true;
            if (!source.isPlaying)
                source.Play();
        }
        else
        {
            source.loop = false;
            source.PlayOneShot(clip);
        }
    }

    public void StopSound()
    {
        if (source == null) return;
        source.loop = false;
        if (source.isPlaying) source.Stop();
    }
}
