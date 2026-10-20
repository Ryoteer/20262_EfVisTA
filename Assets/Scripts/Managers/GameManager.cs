using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Instance

    public static GameManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    #endregion
    
    private ThirdPersonModel _thirdPersonPlayer;

    public ThirdPersonModel ThirdPersonPlayer
    {
        get { return _thirdPersonPlayer; }
        set { _thirdPersonPlayer = value; }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }
}
