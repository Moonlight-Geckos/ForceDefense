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
        }
        StartCoroutine(restart());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void finishGame()
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
        }
        StartCoroutine(activate());
    }
}