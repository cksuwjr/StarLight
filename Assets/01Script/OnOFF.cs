using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class OnOFF : MonoBehaviour
{
    [SerializeField] private List<GameObject> setting1 = new List<GameObject>();
    [SerializeField] private List<GameObject> setting2 = new List<GameObject>();

    public void Setting1()
    {
        for(int i = 0; i < setting1.Count; i++)
            setting1[i].SetActive(true);

        for (int i = 0; i < setting2.Count; i++)
            setting2[i].SetActive(false);
    }

    public void Setting2()
    {
        for (int i = 0; i < setting1.Count; i++)
            setting1[i].SetActive(false);

        for (int i = 0; i < setting2.Count; i++)
            setting2[i].SetActive(true);
    }
}
