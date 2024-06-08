using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class TempCommunicater : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]string newname = "pikachu";
    async void Start()
    {
        Communication.Manager CM= Communication.Manager.I;

        UserProfile u=new UserProfile("qwertyuiop",newname);
        var po = await CM.PostReq("/change-name",u);
        Debug.Log(po);
    }
}
