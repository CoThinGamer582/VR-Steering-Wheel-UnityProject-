using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : MonoBehaviour
{
    public GameObject neroPanel;

    void Start()
    {
        Invoke("CloseNow", 5f);
    }

    private void CloseNow()
    {
        neroPanel.SetActive(false);
    }
}
