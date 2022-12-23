using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnionToFinalGame
{
    public class MissionManager : MonoBehaviour, IGameManager
    {
        private NetworkService _network;
        public int curLevel { get; private set; }
        public int maxLevel { get; private set; }
        public ManageStatus Status { get; private set; }

        public void Startup()
        {
            
        }

        public void Setup(NetworkService service)
        {
            Debug.Log("Mission manager starting...");
            _network = service;
            curLevel = 0;
            maxLevel = 1;
            Status = ManageStatus.Started;
        }

        public void GoToNext()
        {
            if (curLevel < maxLevel)
            {
                curLevel++;
                var name = "Level" + curLevel;
                Debug.Log("Loading " + name);
                SceneManager.LoadScene(name);
            }
            else
            {
                Debug.Log("Last level");
            }
        }
    }
}