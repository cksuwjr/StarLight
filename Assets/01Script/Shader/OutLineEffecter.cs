using UnityEngine;

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
        else
        {
            if (transform.parent.TryGetComponent<InteractionObject>(out var interact2))
                interact2.OnInteractable += (tf) => { SetEffect(tf); };
        }

    }

    public void SetEffect(bool tf)
    {
        if(mesh)
            mesh.sharedMaterials = tf ? meshs : meshsOrigin;
    }
}
