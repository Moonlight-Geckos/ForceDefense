using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitglowEffect : MonoBehaviour
{
    [SerializeField]
    Renderer[] m_Renderer;

    [SerializeField]
    Material referenceMaterial;

    [Range(0.0f, 1f)]
    [SerializeField]
    private float maxAlpha = 0.5f;

    [Range(0.0f, 4f)]
    [SerializeField]
    private float glowScale = 0.25f;

    [SerializeField]
    private Color defaultGlowColor = Color.red;

    private List<Material> effectedMats = new List<Material>();
    private Timer timer;

    private void Awake()
    {
        foreach (var renderer in m_Renderer)
        {
            Material mat = new Material(referenceMaterial);
            List<Material> mats = new List<Material>(renderer.materials);
            mats.Add(mat);
            renderer.materials = mats.ToArray();
            effectedMats.Add(mat);
        }
        foreach (Material material in effectedMats)
        {
            material.SetFloat("_Alpha", 0);

            material.SetFloat("_Scale", glowScale);

            material.SetColor("_Color", defaultGlowColor);
        }
    }

    public void Reset()
    {
        StopAllCoroutines();
        timer?.Stop();
        foreach (Material material in effectedMats)
        {
            material.SetFloat("_Alpha", 0);
        }
    }

    public void HitActivate(float animationLength, float duration, Color? color)
    {

        void ChangeColor()
        {
            foreach (Material material in effectedMats)
            {
                IEnumerator changeColor()
                {
                    float alpha = material.GetFloat("_Alpha");
                    while (alpha < maxAlpha)
                    {
                        alpha += Time.deltaTime / animationLength;
                        yield return new WaitForEndOfFrame();
                        material.SetFloat("_Alpha", alpha);
                    }
                }
                if (color != null)
                    material.SetColor("_Color", (Color)color);
                else
                    material.SetColor("_Color", defaultGlowColor);
                StartCoroutine(changeColor());
            }
        }
        void ResetColors()
        {
            foreach (Material material in effectedMats)
            {
                IEnumerator changeColor()
                {
                    float alpha = material.GetFloat("_Alpha");
                    while (alpha > 0)
                    {
                        alpha -= Time.deltaTime / animationLength;
                        yield return new WaitForEndOfFrame();
                        material.SetFloat("_Alpha", alpha);
                    }
                }
                StartCoroutine(changeColor());
            }
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
            timer.Refresh();
        }
        ChangeColor();
        timer.Run();
    }
}
