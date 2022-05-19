using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using TMPro;

public class HeadUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup endGamePanel;

    [SerializeField]
    private Transform ControlPanel;

    [SerializeField]
    private ProgressBar toMultiplierPB;

    [SerializeField]
    private ProgressBar toFinishPB;

    [SerializeField]
    private TextMeshProUGUI gemsNumber;

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
        GameManager.FinishGameEvent.AddListener(finishGame);
    }

    public void Restart()
    {
        IEnumerator restart()
        {
            yield return new WaitForEndOfFrame();
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        StartCoroutine(restart());

    }

    private void finishGame(bool win)
    {
        IEnumerator activate()
        {
            yield return new WaitForSeconds(2);
            ControlPanel.gameObject.SetActive(false);
            while (endGamePanel.alpha < 1)
            {
                endGamePanel.alpha += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            Time.timeScale = 0f;
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
}