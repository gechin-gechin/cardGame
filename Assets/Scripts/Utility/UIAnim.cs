using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]

public class UIAnim : MonoBehaviour
{
    [SerializeField] private List<Sprite> frames;
    [SerializeField] private float frameRate;

    private Image image;

    private int currentFrame;
    private float timer;

    private void Start()
    {
        image = GetComponent<Image>();
        currentFrame = 0;
        timer = 0;
        image.sprite = frames[currentFrame];
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > frameRate)
        {
            currentFrame = (currentFrame + 1) % frames.Count;
            image.sprite = frames[currentFrame];
            timer -= frameRate;
        }
    }
}
