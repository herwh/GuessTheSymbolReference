using System;
using Common.Data;
using DG.Tweening;
using UnityEngine;

namespace Common
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite; //prefab sprite
        [SerializeField] private SpriteRenderer _objectRenderer; //object sprite

        [SerializeField] private TweenScaleData _bigInitialTweenScale;
        [SerializeField] private TweenScaleData _originalInitialTweenScale;
        [SerializeField] private TweenScaleData _smallInitialTweenScale;

        [SerializeField] private TweenScaleData _bigWinTweenScale;
        [SerializeField] private TweenScaleData _originalWinTweenScale;
        [SerializeField] private TweenScaleData _smallWinTweenScale;

        [SerializeField] private Vector3 _shakeTween;
        [SerializeField] private float _shakeTweenDuration;
        [SerializeField] private int _shakeTweenVibrato;
        [SerializeField] private float _shakeTweenRandomness;

        [SerializeField] private ParticleSystem _starParticle;
        
        public event Action<Cell, ObjectData> CellClick;

        private ObjectData _objectData;
        private Tween _shakePositionTween;
        private Vector3 _initialPosition;
        
        public Vector3 GetSize()
        {
            return _sprite.bounds.size;
        }

        public void SetObject(ObjectData objectData)
        {
            _objectData = objectData;
            _objectRenderer.transform.Rotate(0, 0, objectData.RotationAngle);
            _objectRenderer.sprite = _objectData.Sprite;
        }
        
        public void OnMouseDown()
        {
            if (CellClick != null)
            {
                CellClick(this, _objectData);
            }
        }

        public void Clear()
        {
            CellClick = null;
        }
        
         public void ShakePosition()
                {
                    _shakePositionTween.Kill();
                    transform.position = _initialPosition;
                    _shakePositionTween = transform.DOShakePosition(_shakeTweenDuration, _shakeTween, _shakeTweenVibrato,
                        _shakeTweenRandomness);
                }
        
                public void PlayWinBounce()
                {
                    var sequence = DOTween.Sequence();
                    sequence.Append(GetScaleTween(_objectRenderer.transform,_smallWinTweenScale));
                    sequence.Append(GetScaleTween(_objectRenderer.transform,_bigWinTweenScale));
                    sequence.Append(GetScaleTween(_objectRenderer.transform,_originalWinTweenScale));
                }
        
                public void PlayStarParticle()
                {
                    _starParticle.gameObject.SetActive(true);
                }
        private void Start()
        {
            _initialPosition = transform.position;
            PlayInitialBounce();
        }

        private void PlayInitialBounce()
        {
            transform.localScale = Vector3.zero;
            var sequence = DOTween.Sequence();
            sequence.Append(GetScaleTween(transform,_bigInitialTweenScale));
            sequence.Append(GetScaleTween(transform,_smallInitialTweenScale));
            sequence.Append(GetScaleTween(transform,_originalInitialTweenScale));
        }

        private Tween GetScaleTween(Transform _transform, TweenScaleData tweenScaleData)
        {
            return _transform.DOScale(tweenScaleData.Scale, tweenScaleData.Duration);
        }
    }
}