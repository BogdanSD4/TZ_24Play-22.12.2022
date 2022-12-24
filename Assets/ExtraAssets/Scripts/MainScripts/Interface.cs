using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interface : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystemWarp;
    [SerializeField] private UnityEvent _startGame;
    [SerializeField] private UnityEvent _endGame;
    void Start()
    {
        SetGameControllerEvents();
    }

    public void SetGameControllerEvents()
    {
        GameController.GameStart += StartGame;
        GameController.GameEnd += EndGame;
    }

    private void StartGame()
    {
        _particleSystemWarp.gameObject.SetActive(true);
        _startGame.Invoke();
    }

    private void EndGame()
    {
        _particleSystemWarp.gameObject.SetActive(false);
        _endGame.Invoke();
    }
}
