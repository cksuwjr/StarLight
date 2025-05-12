using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PosLoader : MonoBehaviour
{
    void Start()
    {
        if(PlayerPrefs.GetString("SaveScene") == SceneManager.GetActiveScene().name)
            GameManager.Instance.LoadPosition();
    }
}
