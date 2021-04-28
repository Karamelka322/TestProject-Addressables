using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animation _mainMenuAnimator;
    [SerializeField] private AnimationClip _fadeOutAnimation;
    [SerializeField] private AnimationClip _fadeInAnimation;

    [SerializeField] private Button _levelOne;
    [SerializeField] private Button _levelTwo;

    public Events.EventFadeComplete OnFadeComplete;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);

        _levelOne.onClick.AddListener(LoadLevel1);
        _levelTwo.onClick.AddListener(LoadLevel2);
    }

    private void LoadLevel1()
    {
        GameManager.Instance.LoadLevel("Level 1");
    }

    private void LoadLevel2()
    {
        GameManager.Instance.LoadLevel("Level 2");
    }

    public void OnFadeOutComplete()
    {
        OnFadeComplete.Invoke(true);
    }

    public void OnFadeInComplete()
    {
        OnFadeComplete.Invoke(false);
        UIManager.Instance.SetDummyCameraActive(true);
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (_mainMenuAnimator == null)
        {
            return;
        }

        if (previousState == GameManager.GameState.PREGAME && currentState != GameManager.GameState.PREGAME)
        {
            UIManager.Instance.SetDummyCameraActive(false);
            _mainMenuAnimator.clip = _fadeOutAnimation;
            _mainMenuAnimator.Play();
        }

        if (previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME)
        {
            _mainMenuAnimator.Stop();
            _mainMenuAnimator.clip = _fadeInAnimation;
            _mainMenuAnimator.Play();
        }
    }
}
