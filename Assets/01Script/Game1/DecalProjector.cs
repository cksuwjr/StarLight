using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalProjector : MonoBehaviour
{
    [SerializeField] private Material _projectorMaterial;
    [SerializeField] private float _blendSpeed = 25.0f;
    [SerializeField] private int _textureScale = 2;


    private DecalProjector decalProjector;

    private RenderTexture _prevTexture;
    private RenderTexture _currTexture;

    [SerializeField] private RenderTexture _fogTexture;
    private float _blendAmount;

    private void Awake()
    {
        if (TryGetComponent<DecalProjector>(out decalProjector))
            decalProjector.enabled = true;

        _prevTexture = GenerateTexture();
        _currTexture = GenerateTexture();

        decalProjector._projectorMaterial = new Material(_projectorMaterial);

        decalProjector._projectorMaterial.SetTexture("_PrevTexture", _prevTexture);
        decalProjector._projectorMaterial.SetTexture("_CurrTexture", _currTexture);

        Blend();
    }

    private RenderTexture GenerateTexture()
    {
        RenderTexture rt = new RenderTexture(
            _fogTexture.width * _textureScale,
            _fogTexture.height * _textureScale,
            0,
            _fogTexture.format);
        rt.filterMode = FilterMode.Bilinear;

        return rt;
    }

    private void Blend()
    {
        //StopCoroutine("Fog");
        _blendAmount = 0;
        // Swap the textures
        Graphics.Blit(_currTexture, _prevTexture);
        Graphics.Blit(_fogTexture, _currTexture);

        StartCoroutine("Fog");
    }

    IEnumerator Fog()
    {
        Debug.Log("ºí·»µå");
        while (_blendAmount < 1)
        {
            _blendAmount += Time.deltaTime * _blendSpeed;
            decalProjector._projectorMaterial.SetFloat("_Blend", _blendAmount);
            yield return null;
        }

        Blend();
    }
}
