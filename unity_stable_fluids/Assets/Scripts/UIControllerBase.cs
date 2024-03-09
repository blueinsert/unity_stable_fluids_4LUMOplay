using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerBase : MonoBehaviour
{
    public void Init(UIIntent intent)
    {
        OnInit(intent);
    }

    protected virtual void OnInit(UIIntent intent)
    {

    }

    protected virtual void OnClose() { }

    public void Close()
    {
        OnClose();
        UIManager.Instance.Close(this);
    }
}
