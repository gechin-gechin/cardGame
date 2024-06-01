using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(ParticleSystem))]

public class TapEffect : Singleton<TapEffect>
{

    [SerializeField] private ParticleSystem effect=null;
    [SerializeField] private Camera cameraEffect=null;
    // Start is called before the first frame update
    private void Start()
    {
        if(effect==null)
            effect = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    private void Update()
    {
        var current = Mouse.current;
        if (current == null)
        {
            return;
        }

        if (current.leftButton.wasPressedThisFrame||Keyboard.current.enterKey.wasPressedThisFrame)
        {
            Vector3 pos = new Vector3(current.position.ReadValue().x, current.position.ReadValue().y,2f);
            effect.Stop();
            effect.transform.position = cameraEffect.ScreenToWorldPoint(pos);
            effect.Play();
        }
    }
}
