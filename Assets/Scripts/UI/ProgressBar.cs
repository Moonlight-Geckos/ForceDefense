using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField]
    private Gradient colorGradient;


    private Slider slider;
    private Image image;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        image = slider.fillRect.GetComponent<Image>();
    }

    public void UpdateValue(float val)
    {
        image.color = colorGradient.Evaluate(val);
        slider.value = val;
    }

}