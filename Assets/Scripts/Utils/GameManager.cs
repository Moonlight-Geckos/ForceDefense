using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Serialized

    #endregion

    #region Private

    private static Transform playerPos;
    private static GameManager _instance;
    private static UnityEvent finishGameEvent = new UnityEvent();
    private static UnityEvent clearPoolsEvent = new UnityEvent();

    #endregion

    static public Vector3 PlayerPos
    {
        get { return playerPos.position; }
    }
    static public GameManager Instance
    {
        get { return _instance; }
    }
    static public UnityEvent FinishGameEvent
    {
        get { return finishGameEvent; }
    }
    static public UnityEvent ClearPoolsEvent
    {
        get { return clearPoolsEvent; }
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
        clearPoolsEvent.Invoke();
        playerPos = GameObject.FindGameObjectWithTag("Character").transform;
    }

    void FixedUpdate()
    {
        TimersPool.UpdateTimers();
    }

    public static void FinishGame()
    {
        finishGameEvent.Invoke();
    }
}
