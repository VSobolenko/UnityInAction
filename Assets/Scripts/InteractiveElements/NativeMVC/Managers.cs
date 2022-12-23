using System;
using System.Collections;
using System.Collections.Generic;
using UnionToFinalGame;
using UnityEngine;

namespace InteractiveElements.NativeMVC
{
    [RequireComponent(typeof(PlayerManager), typeof(InventoryManager), typeof(MissionManager))]
    public class Managers : MonoBehaviour
    {
        [SerializeField] private bool startInitializeManagers;
        public static PlayerManager Player { get; private set; }
        public static InventoryManager Inventory { get; private set; }
        public static MissionManager Mission { get; private set; }

        private List<IGameManager> _startSequence;

        private void Awake()
        {
            if (startInitializeManagers == false)
                return;
            DontDestroyOnLoad(gameObject);
            Player = GetComponent<PlayerManager>();
            Inventory = GetComponent<InventoryManager>();
            Mission = GetComponent<MissionManager>();

            _startSequence = new List<IGameManager> {Player, Inventory, Mission};

            StartCoroutine(StartupManagers());
        }

        private IEnumerator StartupManagers()
        {
            var service = new NetworkService();
            foreach (var gameManager in _startSequence)
            {
                gameManager.Startup();
                gameManager.Setup(service);
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
                    Messenger<int, int>.Broadcast(StartupEvent.MANAGERS_PROGRESS, numReady, numModules);
                }
                yield return null;
            }
            Debug.Log("All managers started up");
            Messenger.Broadcast(StartupEvent.MANAGERS_STARTED);
        }
    }
}