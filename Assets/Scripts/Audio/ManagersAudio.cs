using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioManager))]
    public class ManagersAudio : MonoBehaviour
    {
        [SerializeField] private bool startInitializeManagers;
        public static AudioManager Audio { get; private set; }

        private List<IGameManagerAudio> _startSequence;

        private void Awake()
        {
            if (startInitializeManagers == false)
                return;

            Audio = GetComponent<AudioManager>();

            _startSequence = new List<IGameManagerAudio> {Audio,};

            StartCoroutine(StartupManagers());
        }

        private IEnumerator StartupManagers()
        {
            var networkService = new NetworkService();
            foreach (var gameManager in _startSequence)
            {
                gameManager.Setup(networkService);
            }
            
            yield return null;
            var numModules = _startSequence.Count;
            var numReady = 0;
                
            while (numReady < numModules)
            {
                var lastReady = numReady;
                numReady = 0;

                foreach (var manager in _startSequence)
                {
                    if (manager.Status == ManageStatus.Started)
                    {
                        numReady++;
                    }
                }

                if (numReady > lastReady)
                {
                    Debug.Log($"Progress: {numReady}/{numModules}");
                }
                yield return null;
            }
            Debug.Log("All managers started up");
        }
    }
}