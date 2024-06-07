using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Communication
{
    public class Manager : MonoBehaviour
    {
        const string host = "http://127.0.0.1:8000";
        // Start is called before the first frame update
        async void Start()
        {
            var s = await GetReq("/registration");
            Debug.Log(s);
            var l = await PostReq("/login","user_id=3h432ioijo");
            Debug.Log(l);
        }

        private async UniTask<string> GetReq(string endPoint,string param=""){
            var req = UnityWebRequest.Get(host+endPoint+param);
            await req.SendWebRequest();
            if(!string.IsNullOrEmpty(req.error)){
                Debug.LogError(req.error);
            }
            return req.downloadHandler.text;
        }

        private async UniTask<string> PostReq(string endPoint,string param){
            var formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormDataSection(param));
            var req = UnityWebRequest.Post(host+endPoint,formData);
            await req.SendWebRequest();
            if(!string.IsNullOrEmpty(req.error)){
                Debug.LogError(req.error);
            }
            return req.downloadHandler.text;
        }
    }
}