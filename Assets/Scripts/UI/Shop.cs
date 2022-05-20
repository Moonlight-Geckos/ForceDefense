using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private Transform skinsParent;

    [SerializeField]
    private int[] skinPrices;

    [SerializeField]
    private TextMeshProUGUI gemsCountText;

    private Transform[] skins;
    int ind = 0;
    int gemsCount;
    private void OnEnable()
    {
        skins = new Transform[skinsParent.childCount];
        for(int i = 0; i < skinsParent.childCount; i++)
        {
            skins[i] = skinsParent.GetChild(i);
        }
        ind = PlayerPrefs.GetInt("Skin");
        for (int i = 0; i < skins.Length; i++)
        {
            skins[i].GetComponent<Outline>().enabled = false;
            if((i == 0 || PlayerPrefs.GetString("BoughtSkins").Contains(i.ToString())) && skins[i].childCount > 2)
            {
                GameObject f = skins[i].GetChild(0).gameObject;
                GameObject s = skins[i].GetChild(1).gameObject;
                Destroy(f);
                Destroy(s);
            }
        }
        skins[ind].GetComponent<Outline>().enabled = true;
        gemsCount = PlayerPrefs.GetInt("Gems");
        gemsCountText.text = gemsCount.ToString();
    }
    public void Buy()
    {
        if (!PlayerPrefs.GetString("BoughtSkins").Contains(ind.ToString()) && ind != 0 && ind < 3 &&
            gemsCount >= skinPrices[ind])
        {
            string s = PlayerPrefs.GetString("BoughtSkins");
            s += ind.ToString();
            PlayerPrefs.SetString("BoughtSkins", s);
            gemsCount -= skinPrices[ind];
            gemsCountText.text = gemsCount.ToString();
            PlayerPrefs.SetInt("Gems", gemsCount);
            if (skins[ind].childCount > 1)
            {
                GameObject f = skins[ind].GetChild(0).gameObject;
                GameObject ss = skins[ind].GetChild(1).gameObject;
                Destroy(f);
                Destroy(ss);
            }
            PlayerPrefs.SetInt("Skin", ind);
        }
    }
    public void Select(int ind)
    {
        skins[this.ind].GetComponent<Outline>().enabled = false;
        if (PlayerPrefs.GetString("BoughtSkins").Contains(ind.ToString()) || ind == 0)
        {
            PlayerPrefs.SetInt("Skin", ind);
        }
        this.ind = ind;
        skins[ind].GetComponent<Outline>().enabled = true;
    }
    private void OnGUI()
    {
        int gems = PlayerPrefs.GetInt("Gems");
        if (GUI.Button(new Rect(10, 10, 150, 100), "Cheat"))
            PlayerPrefs.SetInt("Gems", gems + 1000);
        gemsCount = PlayerPrefs.GetInt("Gems");
        gemsCountText.text = gemsCount.ToString();
    }
}
