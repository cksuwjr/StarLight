using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterClearChecker : MonoBehaviour
{
    [SerializeField] private string id;
    [SerializeField] private string beforeClearId;


    [SerializeField] private Sprite notClear;
    [SerializeField] private Sprite clear;

    private Image image;

    private void Awake()
    {
        bool isClear;
        if (PlayerPrefs.GetInt(id) == 1)
            isClear = true;
        else
            isClear = false;
        if (transform.GetChild(1).GetChild(0).TryGetComponent<Image>(out image))
        {
            if (isClear)
                image.sprite = clear;
            else
                image.sprite = notClear;
        }

        if (beforeClearId != "" && beforeClearId != null) {
            if (PlayerPrefs.GetInt(beforeClearId) != 1)
            {
                transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "<??? ??? ???>";
                transform.GetChild(3).GetComponent<Button>().interactable = false;
                transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
                transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
            }
        }
    }
}
