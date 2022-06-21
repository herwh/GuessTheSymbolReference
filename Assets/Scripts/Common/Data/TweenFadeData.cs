using System;
using UnityEngine;

namespace Common.Data
{
    [Serializable]
    public struct TweenFadeData
    {
        [SerializeField] private float _alphaShowValue;
        [SerializeField] private float _alphaHideValue;
        [SerializeField] private float _showDuration;
        [SerializeField] private float _hideDuration;

        public float AlphaShowValue => _alphaShowValue;
        public float AlphaHideValue => _alphaHideValue;
        public float ShowDuration => _showDuration;
        public float HideDuration => _hideDuration;
    }
}