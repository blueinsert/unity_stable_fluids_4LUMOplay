using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class UIRegisterItem
{
    [SerializeField]
    public string m_id;
    [SerializeField]
    public string m_controllerClassName;
    [SerializeField]
    public GameObject m_prefab;
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]
    public List<UIRegisterItem> m_reisterItemList = new List<UIRegisterItem>();

    public GameObject m_inactiveNode;

    private Dictionary<string, UIControllerBase> m_cacheUICtrlDic = new Dictionary<string, UIControllerBase>();
    public void Awake()
    {
        Instance = this;
        m_inactiveNode.SetActive(false);
    }

    private UIRegisterItem GetRegisterItem(string id)
    {
        var item = m_reisterItemList.Find((e) => { return e.m_id == id; });
        return item;
    }

    public static Type GetType(string typeFullName)
    {
        var type = System.Reflection.Assembly.Load("Assembly-CSharp").GetType(typeFullName);
#if UNITY_EDITOR
        if (type == null)
        {
            type = System.Reflection.Assembly.Load("Assembly-CSharp-Editor").GetType(typeFullName);
        }
#endif
        return type;
    }

    public UIControllerBase GetFromCache(string id)
    {
        UIControllerBase uiCtrl = null;
        m_cacheUICtrlDic.TryGetValue(id, out uiCtrl);
        return uiCtrl;
    }

    private void Add2Cache(string id,UIControllerBase uiCtrl)
    {
        m_cacheUICtrlDic.Add(id, uiCtrl);
    }

    public UIControllerBase Show(UIIntent intent)
    {
        var registerItem = GetRegisterItem(intent.ID);
        if(registerItem == null)
        {
            Debug.Log(string.Format("id: {0} registerItem is null", intent.ID));
            return null;
        }
        var uiCtrl = GetFromCache(intent.ID);
        if(uiCtrl == null)
        {
            var go = GameObject.Instantiate(registerItem.m_prefab, this.transform, false);
            var type = GetType(registerItem.m_controllerClassName);
            if (type == null)
            {
                Debug.Log(string.Format("id: {0} GetControllerType is null", intent.ID));
                return null;
            }
            uiCtrl = go.GetComponent(type) as UIControllerBase;
            if (uiCtrl == null)
            {

                uiCtrl = go.AddComponent(type) as UIControllerBase;
            }
            Add2Cache(intent.ID, uiCtrl);
        }

        uiCtrl.transform.SetParent(this.transform);
        uiCtrl.transform.SetAsLastSibling();
        uiCtrl.Init(intent);
        return uiCtrl;
    }

    public void Close(UIControllerBase uiCtrl)
    {
        uiCtrl.gameObject.transform.SetParent(this.m_inactiveNode.transform);
    }
}
