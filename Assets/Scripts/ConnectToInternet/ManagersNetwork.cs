using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectToInternet
{
    [RequireComponent(typeof(WeatherManager), typeof(ImagesManager))]
    public class ManagersNetwork : MonoBehaviour
    {
        [SerializeField] private bool startInitializeManagers;
        public static WeatherManager Weather { get; private set; }
        public static ImagesManager Images { get; private set; }

        private List<IGameManagerNetwork> _startSequence;

        private void Awake()
        {
            if (startInitializeManagers == false)
                return;

            Weather = GetComponent<WeatherManager>();
            Images = GetComponent<ImagesManager>();

            _startSequence = new List<IGameManagerNetwork> {Weather, Images};

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