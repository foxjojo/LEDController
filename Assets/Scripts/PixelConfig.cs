using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PixelConfig : MonoBehaviour,IPointerDownHandler
{
    private RGB rgbData = new RGB();

    private void Start()
    {
        GetComponent<Toggle>().onValueChanged.AddListener(delegate(bool arg0)
        {
            if (arg0)
            {
                rgbData = new RGB(123, 12, 123);
            }
            else
            {
                rgbData = new RGB();
            }
        });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            
        }
    }

    public RGB GetData()
    {
        return rgbData;
    }
}
