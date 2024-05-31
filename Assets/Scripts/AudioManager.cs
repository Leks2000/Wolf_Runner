using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private static bool isSoundTurnedOn = true;
    public Button soundOnOffBtn;
    public Sprite soundOnImg;
    public Sprite soundOffImg;

    void Start()
    {
        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.isLooped;
            s.source.name = s.name;
            s.source.volume = s.volume;
        }

        if (!isSoundTurnedOn)
        {
            PauseAudio();
            this.soundOnOffBtn.GetComponent<Image>().sprite = this.soundOffImg;
        }
        else
        {
            PlaySound("MainTheme");
            this.soundOnOffBtn.GetComponent<Image>().sprite = this.soundOnImg;
        }
    }

    public void PlaySound(string name)
    {
        if (isSoundTurnedOn)
        {
            sounds.ToList()
                  .Find(x => x.name.Equals(name))
                  .source.Play();
        }
    }

    public void ChangeMainThemeVolume(float newVolume)
    {
        gameObject.GetComponent<AudioSource>().volume = newVolume;
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime, float endLevel)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > endLevel)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        if (endLevel == 0.0f)
        {
            audioSource.Stop();
        }
    }

    public void PauseAudio()
    {
        gameObject.GetComponent<AudioSource>().Pause();
    }

    public void ResumeAudio()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }

    public void ToggleSoundPlaying()
    {
        if (isSoundTurnedOn)
        {
            gameObject.GetComponent<AudioSource>().volume = 0;
            this.soundOnOffBtn.GetComponent<Image>().sprite = this.soundOffImg;
        }
        else
        {
            gameObject.GetComponent<AudioSource>().volume = 0.2f;
            this.soundOnOffBtn.GetComponent<Image>().sprite = this.soundOnImg;
        }

        isSoundTurnedOn = !isSoundTurnedOn;
    }
}
