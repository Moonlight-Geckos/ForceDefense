using System;
using TMPro;
using UnityEngine;

[Serializable]
public class MultiplierCustomization
{
    public Color planeColor;
    public Color textColor;
    public Color outlineColor;
}

public class MultiplierPlane : MonoBehaviour
{
    TextMeshPro text;
    private Renderer plane;
    private void Initialize()
    {
        plane = GetComponent<Renderer>();
        text = GetComponentInChildren<TextMeshPro>();
    }

    public void ChangeCustomization(MultiplierCustomization customization,float alphaValue)
    {
        if(text == null)
        {
            Initialize();
        }
        customization.planeColor.a = alphaValue;
        plane.material.color = customization.planeColor;
        text.text = "x " + GameManager.Instance.CurrentMultiplier.ToString();
        text.faceColor = new Color32(
            (byte)(customization.textColor.r * 255),
            (byte)(customization.textColor.g * 255),
            (byte)(customization.textColor.b * 255),
            255);
        text.outlineColor = new Color32(
            (byte)(customization.outlineColor.r * 255),
            (byte)(customization.outlineColor.g * 255),
            (byte)(customization.outlineColor.b * 255),
            255);
    }
}
