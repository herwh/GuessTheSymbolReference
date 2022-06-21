using System;
using UnityEngine;

namespace Common.Data
{
    [Serializable]

    public class DifficultyData
    {
        [SerializeField] private int _rows;
        [SerializeField] private int _columns;

        public int Rows => _rows;
        public int Columns => _columns;
    }
}