using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OnButtonAnim : MonoBehaviour
{
    Image image;

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClickAnim);
        image = GetComponent<Image>();
    }
    void OnClickAnim()
    {
        transform.DOPunchScale(punch: Vector3.one * 0.1f, duration: 0.2f, vibrato: 1)
            .SetEase(Ease.OutElastic);
    }

    public IEnumerator Shrink(int time)
    {
        yield return new WaitForEndOfFrame();
        
        for (int i = time*2; i > 0; i--)
        {
            image.fillAmount -= (float)1 / (time * 2);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator Fill(int time)
    {
        for (int i = 0; i < time*2; i++)
        {
            image.fillAmount += (float)1 / (time * 2);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
