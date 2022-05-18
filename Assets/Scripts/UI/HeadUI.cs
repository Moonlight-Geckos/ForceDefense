using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class HeadUI : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup endGamePanel;

    [SerializeField]
    private Transform ControlPanel;
    private void Awake()
    {
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
}