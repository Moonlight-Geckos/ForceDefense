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
    private Transform nextShopButton;

    [SerializeField]
    private Transform retryShopButton;

    [SerializeField]
    private CanvasGroup inGamePanel;

    [SerializeField]
    private Transform winText;

    [SerializeField]
    private Transform loseText;

    [SerializeField]
    private CanvasGroup permanentUpgradePanel;

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

    public bool choseUpgrade = true;

    public bool ChoseUpgrade
    {
        get { return choseUpgrade; }
        set { choseUpgrade = value; }
    }

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
        choseUpgrade = true;
        permanentUpgradePanel.alpha = 0;
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
        IEnumerator next()
        {
            yield return new WaitForEndOfFrame();
            int ind = SceneManager.GetActiveScene().buildIndex;
            if(ind < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(ind + 1);
        }
        StartCoroutine(next());
    }

    private void finishGame(bool win)
    {
        
        ControlPanel.gameObject.SetActive(false);

        IEnumerator FinishGame()
        {
            if (win)
            {
                permanentUpgradePanel.gameObject.SetActive(true);
                permanentUpgradePanel.alpha = 0;
                nextShopButton.gameObject.SetActive(true);
                retryShopButton.gameObject.SetActive(false);
                winText.gameObject.SetActive(true);
                choseUpgrade = false;
                for (int i = 0; i < permanentUpgradePanel.transform.GetChild(1).childCount; i++)
                    permanentUpgradePanel.transform.GetChild(1).GetChild(i).gameObject.SetActive(true);
                permanentUpgradePanel.transform.GetChild(1).GetChild(Random.Range(1, 3)).gameObject.SetActive(false);
                while (permanentUpgradePanel.alpha < 1)
                {
                    permanentUpgradePanel.alpha += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                nextShopButton.gameObject.SetActive(false);
                retryShopButton.gameObject.SetActive(true);
                loseText.gameObject.SetActive(true);
            }
            while (!choseUpgrade)
            {
                yield return null;
            }
            endGamePanel.gameObject.SetActive(true);
            while (endGamePanel.alpha < 1)
            {
                permanentUpgradePanel.alpha -= Time.deltaTime;
                endGamePanel.alpha += Time.deltaTime;
                inGamePanel.alpha -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            permanentUpgradePanel.gameObject.SetActive(false);
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
        StartCoroutine(FinishGame());
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