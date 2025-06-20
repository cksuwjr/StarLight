using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GamePlayer : MonoBehaviour
{
    private string sourceImg = "";

    public void FloatAddressableImage(string source)
    {
        sourceImg = source;
        StartCoroutine("FloatImage");
    }

    private IEnumerator FloatImage()
    {
        var handle = Addressables.LoadAssetAsync<Sprite>(sourceImg);
        yield return handle;
        UIManager.Instance.FloatImage(handle.Result);
    }
}
