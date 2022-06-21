using System;
using System.Collections;
using UnityEngine;

namespace Common
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private DifficultyAdjuster _difficultyAdjuster;
        [SerializeField] private float _delay;
        [SerializeField] private GridBuilder _gridBuilder;

        public event Action NewLevelStarts;
        public event Action LastLevelCompleted;

        private int _levelCount;

        public void NextLevelStart()
        {
            _difficultyAdjuster.UpdateDifficulty();
            NewLevelStart();
        }

        private void Start()
        {
            _difficultyAdjuster.LevelCompleted += OnLevelCompleted;
            NewLevelStart();
        }

        private void OnLevelCompleted()
        {
            _levelCount++;
            _gridBuilder.Grid.Clear();

            if (_levelCount % _difficultyAdjuster.DifficultiesCount == 0)
            {
                if (LastLevelCompleted != null)
                {
                    LastLevelCompleted();
                }
            }

            else
            {
                StartCoroutine(UpdateNextLevelCoroutine());
            }
        }

        private void NewLevelStart()
        {
            if (_gridBuilder.Grid != null)
            {
                _gridBuilder.Grid.Clear();
                _gridBuilder.Grid.RemoveCells();
            }

            _difficultyAdjuster.SetDifficulty();
            _difficultyAdjuster.SetGoal();
            _difficultyAdjuster.ChooseRandomObjects();
            _difficultyAdjuster.SetCellsContent();

            if (NewLevelStarts != null)
            {
                NewLevelStarts();
            }
        }

        private IEnumerator UpdateNextLevelCoroutine()
        {
            yield return new WaitForSeconds(_delay);
            NextLevelStart();
        }

    }
}