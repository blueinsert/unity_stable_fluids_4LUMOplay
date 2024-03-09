using bluebean.UGFramework.Log;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class MainPageUIController : MonoBehaviour
{
    public DragableUIControllerBase m_settingButton = null;

    public void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        m_settingButton.EventOnClick += OnSettingButtonClick;
        m_settingButton.gameObject.SetActive(LocalAccountConfig.Instance.Data.IsShowSettingButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnSettingButtonClick()
    {
        Debug.Log("OnSettingButtonClick");
        UIManager.Instance.Show(new UIIntent("ConfigUI"));
    }
}
