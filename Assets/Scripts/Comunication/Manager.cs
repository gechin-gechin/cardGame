using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;

namespace Communication
{
    public class Manager : Singleton<Manager>
    {
        const string host = "http://127.0.0.1:8000";
        // Start is called before the first frame update
        public async UniTask<string> GetReq(string endPoint,string param=""){
            var req = UnityWebRequest.Get(host+endPoint+param);
            await req.SendWebRequest();
            if(!string.IsNullOrEmpty(req.error)){
                Debug.LogError(req.error);
            }
            Debug.Log("Get"+endPoint+" was success!");
            return req.downloadHandler.text;
        }

        //objは[System.Serializable]が必須
        public async UniTask<string> PostReq(string endPoint,object obj){
            string myjson = JsonUtility.ToJson(obj);//objをjsonに
            byte[] postData = System.Text.Encoding.UTF8.GetBytes (myjson);//jsonを送信用にエンコード
            /*
            var jsonRes = await GetReq("/csrf-token");
            var tokenObj=JsonUtility.FromJson<TokenResponse>(jsonRes);
            var csrfToken = tokenObj.token;
            Debug.Log("token is "+csrfToken);
            */
            using(var req = new UnityWebRequest(host+endPoint,"POST")){
                //req.SetRequestHeader("Authorization", "Bearer !your_token_here!");
                //req.SetRequestHeader("X-CSRFToken",csrfToken);
                req.SetRequestHeader("Content-Type", "application/json");
                req.uploadHandler = (UploadHandler)new UploadHandlerRaw(postData);
                req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

                await req.SendWebRequest();

                if(!string.IsNullOrEmpty(req.error)){
                    Debug.LogError(req.error);
                }
                Debug.Log("POST"+endPoint+" was success!");
                return req.downloadHandler.text;
            }
        }
        /*
        [System.Serializable]
        private class TokenResponse{
            public string token;
        }
        */
    }
}