using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TwoStateButton : UIClicker
{
    public bool onT_offF = false;

    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private Image image;

    //public UnityEvent onClick;

    private void Awake()
    {
        TryGetComponent<Image>(out image);

        if(offSprite == null) offSprite = image.sprite;
        if(onSprite == null) onSprite = image.sprite;

        OnClick += ChangeSprite;

        onT_offF = !onT_offF;
        ChangeSprite();
    }

    private void ChangeSprite()
    {
        onT_offF = !onT_offF;
        if (onT_offF)
            image.sprite = onSprite;
        else
            image.sprite = offSprite;
    }
}
