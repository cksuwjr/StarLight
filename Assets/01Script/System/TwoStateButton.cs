using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TwoStateButton : UIClicker
{
    public bool onT_offF = false;

    public bool OnT_OffF
    {
        get { return onT_offF; }
        set 
        { 
            onT_offF = value;
            ChangeSprite();
        }
    }

    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private Image image;

    //public UnityEvent onClick;

    private void Awake()
    {
        TryGetComponent<Image>(out image);

        if(offSprite == null) offSprite = image.sprite;
        if(onSprite == null) onSprite = image.sprite;

        ChangeSprite();
    }

    private void ChangeSprite()
    {
        if(image == null)
            TryGetComponent<Image>(out image);

        if (OnT_OffF)
        {
            if(onSprite)
                image.sprite = onSprite;
        }
        else
        {
            if(offSprite)
                image.sprite = offSprite;
        }
    }
}
