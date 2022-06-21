using System;
using UnityEngine;

namespace Common.Data

{
    [Serializable]
    public class ObjectData
    { 
        [SerializeField] private string _spriteValue;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private float _rotationAngle;

        public Sprite Sprite => _sprite;
        public string SpriteValue => _spriteValue;
        public float RotationAngle => _rotationAngle;

    }
}
