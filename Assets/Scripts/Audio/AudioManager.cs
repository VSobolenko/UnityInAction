using System.Collections;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour, IGameManagerAudio
    {
        [SerializeField] private AudioSource soundSource;
        
        [SerializeField] private AudioSource music1Source;
        [SerializeField] private AudioSource music2Source;
        [SerializeField] private string introBGMusic;
        [SerializeField] private string levelBGMusic;
        
        public ManageStatus Status { get; private set; }

        private AudioSource _activeMusic;
        private AudioSource _inactiveMusic;
        public float crossFadeRate = 1.5f;
        private bool _crossFading;
        
        private float _musicVolume;

        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                _musicVolume = value;
                if (music1Source != null && music2Source != null)
                {
                    music1Source.volume = _musicVolume;
                    music2Source.volume = _musicVolume;
                }
            }
        }

        public bool MusicMute
        {
            get
            {
                if (music1Source != null)
                {
                    return music1Source.mute;
                }

                return false;
            }
            set
            {
                if (music1Source != null && music2Source != null)
                {
                    music1Source.mute = value;
                    music2Source.mute = value;
                }
            }
        }
        
        public float SoundVolume
        {
            get => AudioListener.volume;
            set => AudioListener.volume = value;
        }

        public bool SoundMute
        {
            get => AudioListener.pause;
            set => AudioListener.pause = value;
        }
        
        private NetworkService _network;
        public void Setup(NetworkService service)
        {
            Debug.Log("Audio manager starting...");
            _network = service;

            music1Source.ignoreListenerVolume = true;
            music2Source.ignoreListenerVolume = true;
            music1Source.ignoreListenerPause = true;
            music2Source.ignoreListenerPause = true;
            
            SoundVolume = 1f;
            MusicVolume = 1f;

            _activeMusic = music1Source;
            _inactiveMusic = music2Source;
            
            Status = ManageStatus.Started;
        }

        public void PlaySound(AudioClip audioClip)
        {
            soundSource.PlayOneShot(audioClip);
        }

        public void PlayIntroMusic() 
        { 
            PlayMusic(Resources.Load("Music/"+introBGMusic) as AudioClip);
        }
        public void PlayLevelMusic() 
        { 
            PlayMusic(Resources.Load<AudioClip>("Music/"+levelBGMusic));
        }
        private void PlayMusic(AudioClip clip) 
        {
            if (_crossFading)
                return;

            // music1Source.clip = clip;
            // music1Source.Play();
            StartCoroutine(CrossFadeMusic(clip));
        }

        private IEnumerator CrossFadeMusic(AudioClip clip)
        {
            _crossFading = true;

            _inactiveMusic.clip = clip;
            _inactiveMusic.volume = 0;
            _inactiveMusic.Play();

            var scaledRate = crossFadeRate * _musicVolume;

            while (_activeMusic.volume > 0)
            {
                _activeMusic.volume -= scaledRate * Time.deltaTime;
                _inactiveMusic.volume += scaledRate * Time.deltaTime;

                yield return null;
            }

            var temp = _activeMusic;
            _activeMusic = _inactiveMusic;
            _activeMusic.volume = _musicVolume;

            _inactiveMusic = temp;
            _inactiveMusic.Stop();

            _crossFading = false;
        }

        public void StopMusic() {
            // music1Source.Stop();
            _activeMusic.Stop();
            _inactiveMusic.Stop();
        }
    }
}