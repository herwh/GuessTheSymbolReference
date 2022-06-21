using System.Collections.Generic;
using UnityEngine;

namespace Common.Data
{
    [CreateAssetMenu(fileName = "New Difficulty", menuName = "DifficultyAdjuster")]
    public class DifficultyDataContainer : ScriptableObject
    {
        [SerializeField] private List<DifficultyData> _difficulties;

        public List<DifficultyData> Difficulties => _difficulties;
    }
}