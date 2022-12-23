using System;
using UnityEngine;

namespace ConnectToInternet
{
    public class CheckPointTrigger : MonoBehaviour
    {
        public string identifier = "unique_checkpoint1";

        private bool _triggered;

        private void OnTriggerEnter(Collider other)
        {
            if (_triggered)
            {
                return;
            }
            ManagersNetwork.Weather.LogWeather(identifier);
            _triggered = true;
        }
    }
}