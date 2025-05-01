using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutLineEffecter : MonoBehaviour
{
    private MeshRenderer mesh;
    private Material[] meshsOrigin;
    private Material[] meshs;

    public Material effectMaterial;

    private void Awake()
    {
        if (!effectMaterial) return;

        if (TryGetComponent<MeshRenderer>(out mesh))
        {
            meshsOrigin = mesh.sharedMaterials;
            meshs = new Material[mesh.sharedMaterials.Length + 1];
            for (int i = 0; i < mesh.sharedMaterials.Length; i++)
                meshs[i] = mesh.sharedMaterials[i];
            meshs[meshs.Length - 1] = effectMaterial;
        }

        if(TryGetComponent<InteractionObject>(out var interact))
            interact.OnInteractable += (tf) => { SetEffect(tf); };
    }

    public void SetEffect(bool tf)
    {
        if(mesh)
            mesh.sharedMaterials = tf ? meshs : meshsOrigin;
    }
}
