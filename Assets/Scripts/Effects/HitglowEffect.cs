using System;
using System.Collections.Generic;
using UnityEngine;


struct ColorPair
{
    public Color color;
    public string reference;
}
public class HitglowEffect : MonoBehaviour
{
    [SerializeField]
    Renderer[] m_Renderer;

    [SerializeField]
    private Color defaultGlowColor = Color.red;

    private List<Material> effectedMats = new List<Material>();
    private List<ColorPair> originalColors = new List<ColorPair>();
    private Timer timer;

    private void Awake()
    {
        foreach (var renderer in m_Renderer)
        {
            List<Material> materials = new List<Material>(renderer.materials);
            effectedMats.AddRange(materials);
            foreach (var material in materials)
            {
                Color color;
                string reference;
                if (material.HasProperty("_Color"))
                {
                    reference = "_Color";
                    color = material.GetColor("_Color");
                }
                else
                {
                    color = material.GetColor("_BaseColor");
                    reference = "_BaseColor";
                }
                ColorPair pair;
                pair.color = color;
                pair.reference = reference;
                originalColors.Add(pair);
            }
        }
    }
    public void Reset()
    {
        StopAllCoroutines();
        timer?.Stop();
        for(int i = 0; i < effectedMats.Count; i++)
        {
            effectedMats[i].SetColor(originalColors[i].reference, originalColors[i].color);
        }
    }

    public void HitActivate(float duration, Color? color)
    {
        if (color == null)
            color = defaultGlowColor;

        void ChangeColor()
        {
            for(int i=0; i<effectedMats.Count; i++)
            {
                effectedMats[i].SetColor(originalColors[i].reference, (Color)color);
            }
        }
        void ResetColors()
        {
            for (int i = 0; i < effectedMats.Count; i++)
            {
                effectedMats[i].SetColor(originalColors[i].reference, originalColors[i].color);
            }
            timer = null;
        }

        if (timer == null)
        {
            timer = TimersPool.Pool.Get();
            timer.Duration = duration;
            timer.AddTimerFinishedEventListener(ResetColors);
        }

        if (timer.Running)
        {
            StopAllCoroutines();
            timer.Duration = duration;
            timer.Refresh();
        }
        ChangeColor();
        timer.Run();
    }
}
