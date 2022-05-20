using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using TMPro;

public class HeadUI : MonoBehaviour
{
    [SerializeField]
    private Transform ControlPanel;

    [SerializeField]
    private CanvasGroup endGamePanel;

    [SerializeField]
    private Transform ShopPanel;

    [SerializeField]
    private CanvasGroup inGamePanel;

    [SerializeField]
    private Transform winText;

    [SerializeField]
    private Transform loseText;

    [SerializeField]
    private ProgressBar toMultiplierPB;

    [SerializeField]
    private ProgressBar toFinishPB;

    [SerializeField]
    private TextMeshProUGUI gemsNumber;

    [SerializeField]
    private TextMeshProUGUI gemsPrizeNumber;

    [SerializeField]
    [Range(1f, 50f)]
    private float gemsCounterAnimationLength = 4;

    private static HeadUI _instance;

    public static HeadUI Instance
    {
        get { return _instance; }
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }
        ControlPanel.gameObject.SetActive(true);
        ShopPanel.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
        endGamePanel.alpha = 0;
        inGamePanel.alpha = 1;
        GameManager.FinishGameEvent.AddListener(finishGame);
    }

    public void Restart()
    {
        IEnumerator restart()
        {
            yield return new WaitForEndOfFrame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        StartCoroutine(restart());
    }
    public void NextLevel()
    {
        IEnumerator restart()
        {
            yield return new WaitForEndOfFrame();
            int ind = SceneManager.GetActiveScene().buildIndex + 1;
            if (ind < SceneManager.sceneCount)
            {
                SceneManager.LoadScene(ind + 1);
            }
        }
        StartCoroutine(restart());
    }

    private void finishGame(bool win)
    {
        if (win)
            winText.gameObject.SetActive(true);
        else
            loseText.gameObject.SetActive(true);
        ControlPanel.gameObject.SetActive(false);
        IEnumerator activate()
        {
            yield return new WaitForSeconds(2);
            while (endGamePanel.alpha < 1)
            {
                endGamePanel.alpha += Time.deltaTime;
                inGamePanel.alpha -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            int allGems = GameManager.Instance.CollectedGems;
            float gemsCount = 0;
            while (gemsCount < allGems)
            {
                gemsCount += (allGems / gemsCounterAnimationLength) * Time.deltaTime;
                gemsCount = Mathf.Min(gemsCount, allGems);
                gemsPrizeNumber.text = "+ " + ((int)gemsCount).ToString();
                yield return new WaitForEndOfFrame();
            }
        }
        StartCoroutine(activate());
    }

    public void UpdateProgress()
    {
        toMultiplierPB.UpdateValue(GameManager.Instance.ToMultiplierProgress);

        toFinishPB.UpdateValue(GameManager.Instance.ToFinishProgress);
    }
    public void UpdateGems()
    {
        gemsNumber.text = GameManager.Instance.CollectedGems.ToString();
    }
    public void ShowShop()
    {
        loseText.gameObject.SetActive(false);
        winText.gameObject.SetActive(false);
        ShopPanel.gameObject.SetActive(true);
        endGamePanel.alpha = 0;
    }
}