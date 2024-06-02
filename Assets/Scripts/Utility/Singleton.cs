using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
abstract public class Singleton<T> : MonoBehaviour where T :Singleton<T>
{
    ///継承先でフラグを立てると消えるようにする
    protected virtual bool DestroyTargetGameObject => false;

    public static T I { get; private set; } = null;
    public static bool IsValid() => I != null;

    private void Awake()
    {
        if (I == null) {
            I = this as T;
            I.AltAwake();
            return;
        }
        ///一つに絞る
        if (DestroyTargetGameObject)
            Destroy(gameObject);
        else
            Destroy(this);
    }
    private void OnDestroy()
    {
        if (I == this) {
            I = null;
            AltOnDestroy();
        }
    }

    protected virtual void AltAwake()
    {

    }
    protected virtual void AltOnDestroy()
    {

    }
}
