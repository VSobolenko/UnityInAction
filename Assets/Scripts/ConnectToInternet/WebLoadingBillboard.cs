using InteractiveElements.NativeMVC;
using UnityEngine;

namespace ConnectToInternet
{
    public class WebLoadingBillboard : MonoBehaviour
    {
        public void Operate()
        {
            if (ManagersNetwork.Images == null)
                return;
            ManagersNetwork.Images.GetWebImage(OnWebImage);
        }

        private void OnWebImage(Texture2D obj)
        {
            GetComponent<Renderer>().material.mainTexture = obj;
        }
    }
}