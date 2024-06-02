using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
[RequireComponent(typeof(Slider))]

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string paramaterName;

    private Slider slider;

    private void Reset()
    {
        Slider _slider = GetComponent<Slider>();
        _slider.minValue = 0f;
        _slider.maxValue = 1f;
    }

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    private void OnEnable()
    {
        audioMixer.GetFloat(paramaterName, out float mixerVolume);
        slider.value = DbToPa(mixerVolume);
        slider.onValueChanged.AddListener((sliderValue) => audioMixer.SetFloat(paramaterName, PaToDb(sliderValue)));
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveAllListeners();
    }

    private float PaToDb(float pa)
    {
        pa = Mathf.Clamp(pa, 0.0001f, 10f);
        return 20f * Mathf.Log10(pa);
    }
    private float DbToPa(float db)
    {
        db = Mathf.Clamp(db, -80f, 20f);
        return Mathf.Pow(10, db / 20f);
    }
}
