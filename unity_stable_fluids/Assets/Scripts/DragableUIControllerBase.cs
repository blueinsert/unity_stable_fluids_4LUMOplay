using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragableUIControllerBase : UIControllerBase, IDragHandler, IBeginDragHandler, IEndDragHandler,IPointerClickHandler
{
    public bool m_isInDraging = false;
    public Vector3 m_dragBeginPos;
    public Vector3 m_dragBeginPointerPos;

    private Canvas m_canvas;
    private Camera m_camera;
    private RectTransform m_rectTransform;

    public event Action EventOnClick;

    public void Awake()
    {
        m_canvas = GetComponentInParent<Canvas>();
        m_camera = m_canvas.worldCamera;
        m_rectTransform = this.transform as RectTransform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log(string.Format("{0} OnBeginDrag", this.gameObject.name));
        m_isInDraging = true;
        m_dragBeginPos = this.transform.position;
        Vector3 worldPos = Vector3.zero;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(m_rectTransform, eventData.position, m_camera, out worldPos);
        m_dragBeginPointerPos = worldPos;

    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(string.Format("{0} OnDrag {1}", this.gameObject.name, eventData.position));
        Debug.Log(string.Format("w:{0} h:{1}", Screen.width, Screen.height));
        if (m_isInDraging)
        {
            Vector3 worldPos = Vector3.zero;
            Vector2 screenPos = eventData.position;
            screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width);
            screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height);
            RectTransformUtility.ScreenPointToWorldPointInRectangle(m_rectTransform, screenPos, m_camera, out worldPos);
            var delta = worldPos - m_dragBeginPointerPos;
            var pos = m_dragBeginPos + delta;
            this.transform.position = pos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(string.Format("{0} OnEndDrag", this.gameObject.name));
        m_isInDraging = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_isInDraging)
            return;
        Debug.Log(string.Format("{0} OnPointerClick", this.gameObject.name));
        if (EventOnClick != null)
        {
            EventOnClick();
        }
    }
}
