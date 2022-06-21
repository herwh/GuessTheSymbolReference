using System;
using Common.Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private DifficultyAdjuster _difficultyAdjuster;
        [SerializeField] private LevelController _levelController;

        [SerializeField] private TextMeshProUGUI _goalText;
        [SerializeField] private Color _goalColor;
        [SerializeField] private float _goalTweenDuration;

        [SerializeField] private CanvasGroup _restartWindow;
        [SerializeField] private Button _restartButton;
        [SerializeField] private TweenFadeData _restartWindowTween;

        [SerializeField] private CanvasGroup _loadWindow;
        [SerializeField] private TweenFadeData _loadWindowTween;

        private string _goalValue;
        private Tween _goalColorTween;

        private void Awake()
        {
            _levelController.NewLevelStarts += SetGoalText; //скорее всего придется здесь обращаться к LevelController
            _levelController.LastLevelCompleted += OnLastLevelCompleted;
        }

        private void RestartButtonClicked()
        {
            _restartButton.onClick.RemoveListener(RestartButtonClicked);
            GetCanvasGroupFadeTween(_restartWindow, _restartWindowTween.AlphaHideValue,
                _restartWindowTween.HideDuration, () =>
                {
                    _restartWindow.gameObject
                        .SetActive(false);

                    _loadWindow.gameObject
                        .SetActive(true);
                    ShowLoadWindowTween();
                });
        }

        private void OnLastLevelCompleted()
        {
            _restartWindow.gameObject.SetActive(true);
            GetCanvasGroupFadeTween(_restartWindow, _restartWindowTween.AlphaShowValue,
                _restartWindowTween.ShowDuration,
                () => { _restartButton.onClick.AddListener(RestartButtonClicked); });
        }

        private void SetGoalText()
        {
            _goalColorTween.Kill();
            _goalValue = _difficultyAdjuster.GoalObject.SpriteValue;
            _goalText.text = $"Find {_goalValue}";
            _goalText.gameObject.SetActive(true);

            PlayGoalTween();
        }

        private void PlayGoalTween()
        {
            _goalText.color = Color.clear;
            _goalColorTween = _goalText.DOColor(_goalColor, _goalTweenDuration);
        }

        private Tween GetCanvasGroupFadeTween(CanvasGroup @group, float alpha, float duration, Action onComplete = null)
        {
            return group
                .DOFade(alpha, duration)
                .OnComplete(() =>
                {
                    if (onComplete != null)
                    {
                        onComplete();
                    }
                });
        }

        private void ShowLoadWindowTween()
        {
            var sequence = DOTween.Sequence();

            sequence.Append(GetCanvasGroupFadeTween(_loadWindow, _loadWindowTween.AlphaShowValue,
                _loadWindowTween.ShowDuration));
            sequence.Append(GetCanvasGroupFadeTween(_loadWindow, _loadWindowTween.AlphaShowValue,
                _loadWindowTween.ShowDuration,
                () =>
                {
                    _loadWindow.gameObject
                        .SetActive(false);
                    _levelController.NextLevelStart();
                }));
            sequence.Append(GetCanvasGroupFadeTween(_loadWindow, _loadWindowTween.AlphaHideValue,
                _loadWindowTween.HideDuration));
        }

        private void OnDisable()
        {
            _levelController.NewLevelStarts -= SetGoalText;
            _levelController.LastLevelCompleted -= OnLastLevelCompleted;
        }
    }
}