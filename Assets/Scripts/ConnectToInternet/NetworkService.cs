using System;
using System.Collections;
using System.Globalization;
using System.Net;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkService
{
    private const string xmlApi = "http://api.openweathermap.org/data/2.5/weather?q=Chicago, " +
                                  "us&mode=xml&APPID=9a56d64506501a552c749ec1a26639f7";

    private const string jsonApi = "http://api.openweathermap.org/data/2.5/weather?q=Chicago," +
                                   "us&APPID=9a56d64506501a552c749ec1a26639f7";

    private const string webImage = "http://upload.wikimedia.org/wikipedia/commons/c/c5/Moraine_Lake_17092005.jpg";

    private const string localApi = "http://localhost/uia/api.php";

    public IEnumerator GetWeatherData(Action<string> callback)
    {
        return CallApi(jsonApi, null, callback);
    }

    public IEnumerator DownloadImage(Action<Texture2D> callback)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(webImage);
        yield return request.SendWebRequest();
        callback?.Invoke(DownloadHandlerTexture.GetContent(request));
    }

    private IEnumerator CallApi(string url, WWWForm form, Action<string> callback)
    {
        using (var request = (form == null) ? UnityWebRequest.Get(url) : UnityWebRequest.Post(url, form))
        {
            yield return request.SendWebRequest();
            
            if (request.isNetworkError)
            {
                Debug.LogError($"network problem: {request.error}");
            }
            else if (request.responseCode != (long)HttpStatusCode.OK)
            {
                Debug.LogError($"response error: {request.error}");
            }
            else
            {
                callback?.Invoke(request.downloadHandler?.text);
            }
        }
    }

    public IEnumerator LogWeather(string name, float cloudValue, Action<string> callback)
    {
        var form = new WWWForm();
        form.AddField("message", name);
        form.AddField("cloud_value", cloudValue.ToString(CultureInfo.InvariantCulture));
        form.AddField("timestamp", DateTime.UtcNow.Ticks.ToString());

        return CallApi(localApi, form, callback);
    }
}
