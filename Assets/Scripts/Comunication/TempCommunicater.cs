using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DB;
using BULLET;

public class TempCommunicater : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]string newId="hanakuso";
    [SerializeField]string newname = "pikachu";
    
    async void Start()
    {
        Communication.Manager CM= Communication.Manager.I;

        UserProfile u=new UserProfile(newId,newname);
        var po = await CM.PostReq("/profile/create",u);
        Debug.Log(po);
    }
}
