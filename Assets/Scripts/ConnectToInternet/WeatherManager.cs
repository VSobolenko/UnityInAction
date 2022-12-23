using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using MiniJSON;
using UnityEngine;

public class WeatherManager : MonoBehaviour, IGameManagerNetwork
{
    public ManageStatus Status { get; private set; }
    public  float cloudValue { get; private set; }
    
    private NetworkService _network;
    public void Setup(NetworkService service)
    {
        Debug.Log("Weather manager started...");
        _network = service;
        StartCoroutine(_network.GetWeatherData(OnXMLDataLoaded));
        Status = ManageStatus.Initializing;
    }

    private void OnXMLDataLoaded(string data)
    {
        Debug.Log("Result request: " + data);

        cloudValue = GetDataFromJson(data);
        Messenger.Broadcast(GameEvent.WEATHER_UPDATE);
        Status = ManageStatus.Started;
    }

    private float GetDataFromXml(string data)
    {
        // <clouds value="40" name="scattered clouds"/>
        try
        {
            var doc = new XmlDocument();
            doc.LoadXml(data);
            XmlNode root = doc.DocumentElement;
            XmlNode node = root.SelectSingleNode("clouds");
            string value = node.Attributes["value"].Value;
            var result = Convert.ToInt32(value) / 100f;
            Debug.Log($"Result parse from xml: {result}");
            return result;
        }
        catch (Exception)
        {
            Debug.Log($"Error in xml parse. Use default");
            // ignored
        }

        return 0.1f;
    }
    
    private float GetDataFromJson(string data)
    {
        // "clouds":{"all":40}
        try
        {
            var dic = Json.Deserialize(data) as Dictionary<string, object>;
            var cloud = (Dictionary<string, object>) dic["clouds"];
            var result = (long)cloud["all"] / 100f;
            Debug.Log($"Result parse from json: {result}");
            return result;
        }
        catch (Exception)
        {
            Debug.Log($"Error in json parse. Use default");
            // ignored
        }

        return 0.1f;
    }

    public void LogWeather(string nameLog)
    {
        StartCoroutine(_network.LogWeather(nameLog, cloudValue, OnLogged));
    }
    
    public void OnLogged(string response)
    {
        Debug.Log(response);
    }
}
