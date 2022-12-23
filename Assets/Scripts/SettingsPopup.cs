using Audio;
using InteractiveElements.NativeMVC;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private Slider speedSlider;
    [SerializeField] private AudioClip sound;
    
    private void Start()
    {
        speedSlider.value = PlayerPrefs.GetFloat("speed", 1);
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void OnSubmitName(string name)
    {
        Debug.Log(name);
    }

    public void OnSpeedValue(float speed)
    {
        Messenger<float>.Broadcast(GameEvent.SPEED_CHANGED, speed);
        //Debug.Log("Speed: " + speed);
        PlayerPrefs.SetFloat("speed", speed);
    }

    public void OnSoundToggle()
    {
        ManagersAudio.Audio.SoundMute = !ManagersAudio.Audio.SoundMute;
        ManagersAudio.Audio.PlaySound(sound);
    }

    public void OnSoundValue(float volume)
    {
        ManagersAudio.Audio.SoundVolume = volume;
    }
    
    public void OnPlayMusic(int selector)
    {
        ManagersAudio.Audio.PlaySound(sound);

        switch (selector)
        {
            case 1:
                ManagersAudio.Audio.PlayIntroMusic();
                break;
            case 2:
                ManagersAudio.Audio.PlayLevelMusic();
                break;
            default:
                ManagersAudio.Audio.StopMusic();
                break;
        }
    }
    
    public void OnMusicToggle() {
        ManagersAudio.Audio.MusicMute = !ManagersAudio.Audio.MusicMute;
        ManagersAudio.Audio.PlaySound(sound);
    }
    public void OnMusicValue(float volume) {
        ManagersAudio.Audio.MusicVolume = volume;
    }
}
