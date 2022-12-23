using ConnectToInternet;
using UnityEngine;

public class WetherController : MonoBehaviour
{
    [SerializeField] private Material sky;
    [SerializeField] private Light sun;

    private float _fullIntensity;
    private static readonly int Blend = Shader.PropertyToID("_Blend");

    private void Awake()
    {
        Messenger.AddListener(GameEvent.WEATHER_UPDATE, OnWeatherUpdate);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.WEATHER_UPDATE, OnWeatherUpdate);
    }

    private void OnWeatherUpdate()
    {
        SetOvercast(ManagersNetwork.Weather.cloudValue);
    }

    private void Start()
    {
        _fullIntensity = sun.intensity;
    }

    private void SetOvercast(float value)
    {
        sky.SetFloat(Blend, value);
        sun.intensity = _fullIntensity - (_fullIntensity * value);
    }
}
