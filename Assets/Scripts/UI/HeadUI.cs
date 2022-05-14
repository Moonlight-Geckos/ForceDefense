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
        ControlPanel.gameObject.SetActive(false);
        endGamePanel.alpha = 1.0f;
    }
}