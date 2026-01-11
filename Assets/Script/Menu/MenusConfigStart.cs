using System.Collections.Generic;
using UnityEngine;



public class InitialMenu : MonoBehaviour
{
    public List<UIElementEntry> elementsUI;

    void Start()
    {
        foreach (var entry in elementsUI)
        {
            entry.value.SetActive(entry.key);
        }
    }
}
