using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class CheckClearMinigame : MonoBehaviour
{
    [SerializeField] private string minigameName;
    
    public UnityEvent OnClear;

    private void Start()
    {
        if (PlayerPrefs.GetInt(minigameName, 0) != 0)
        {
            OnClear?.Invoke();
            OnClear = null;

            Debug.Log("Å¬¸®¾î");
        }
    }

    public void DestroySomeThing(GameObject obj)
    {
        obj.SetActive(false);
        if (UIManager.Instance)
            UIManager.Instance.CloseInformText();
    }
}
