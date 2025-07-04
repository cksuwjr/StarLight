using UnityEngine;
using UnityEngine.UI;

public class PictureClearChecker : MonoBehaviour
{
    [SerializeField] private string id;

    private Image image;

    private void Awake()
    {
        bool isClear;
        if (PlayerPrefs.GetInt(id) == 1)
            isClear = true;
        else
            isClear = false;
        if (TryGetComponent<Image>(out image))
        {
            if (!isClear)
                image.enabled = false;
        }
    }
}
