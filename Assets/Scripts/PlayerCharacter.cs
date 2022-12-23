using InteractiveElements.NativeMVC;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    // private int _health;
    //
    // void Start()
    // {
    //     _health = 5;
    // }

    public void Hurt(int damage)
    {
        if (Managers.Player == null)
            return;
        Managers.Player.ChangeHealth(damage);
        // _health -= damage;
        // Debug.Log("Helth: " + _health);
    }
}
