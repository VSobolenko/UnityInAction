using System;
using UnityEngine;

namespace ConnectToInternet
{
    public class ImagesManager : MonoBehaviour, IGameManagerNetwork
    {
        public ManageStatus Status { get; private set; }
        private NetworkService _networkService;
        private Texture2D _image;
        
        public void Setup(NetworkService service)
        {
            Debug.Log("Images manager started...");

            _networkService = service;
            Status = ManageStatus.Started;
        }

        public void GetWebImage(Action<Texture2D> callback)
        {
            if (_image == null)
            {
                StartCoroutine(_networkService.DownloadImage(image =>
                {
                    _image = image;
                    callback?.Invoke(_image);
                }));
            }
            else
            {
                callback?.Invoke(_image);
            }
        }
    }
}