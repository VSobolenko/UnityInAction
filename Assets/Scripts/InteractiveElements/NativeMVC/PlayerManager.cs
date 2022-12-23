using UnityEngine;

namespace InteractiveElements.NativeMVC
{
    public class PlayerManager : MonoBehaviour, IGameManager
    {
        public ManageStatus Status { get; private set; }
        public int Health { get; private set; }
        public int MaxHealth { get; private set; }
        
        public void Startup()
        {
            Debug.Log("Player manager starting...");

            Health = 50;
            MaxHealth = 100;
            
            Status = ManageStatus.Started;
        }

        public void Setup(NetworkService service)
        {
            
        }

        public void ChangeHealth(int value)
        {
            Health += value;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
            else if (Health < 0)
            {
                Health = 0;
            }
            
            Messenger.Broadcast(GameEvent.HEALT_UPDATE);
        }
    }
}