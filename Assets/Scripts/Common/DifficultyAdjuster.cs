using System;
using System.Collections.Generic;
using Common.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common
{
    public class DifficultyAdjuster : MonoBehaviour
    {
        [SerializeField] private DifficultyDataContainer _difficulties;
        [SerializeField] private GridBuilder _gridBuilder;
        [SerializeField] private List<ObjectsContainer> _objectsContainer;

        public ObjectData GoalObject => _goalObject;
        public event Action LevelCompleted;
        public int DifficultiesCount => _difficulties.Difficulties.Count;

        private ObjectData _goalObject;
        private DifficultyData _difficult;
        private int _currentDifficulty;
        private List<ObjectData> _selectedObjects = new List<ObjectData>();
        private List<ObjectData> _allObjects = new List<ObjectData>();

        public void SetDifficulty()
        {
            _difficult = _difficulties.Difficulties[_currentDifficulty];
            var rows = _difficult.Rows;
            var columns = _difficult.Columns;

            _gridBuilder.Build(rows, columns);
        }

        public void UpdateDifficulty()
        {
            _currentDifficulty = ++_currentDifficulty % _difficulties.Difficulties.Count;
        }

        public void SetCellsContent()
        {
            var index = 0;
            for (var row = 0; row < _gridBuilder.Grid.Cells.GetLength(0); row++)
            {
                for (var column = 0; column < _gridBuilder.Grid.Cells.GetLength(1); column++)
                {
                    ObjectData selectedObject = _selectedObjects[index];
                    SetCellContent(selectedObject, row, column);
                    index++;
                }
            }
        }


        public void ChooseRandomObjects()
        {
            _selectedObjects.Clear();
            _selectedObjects.Add(_goalObject);

            int goalIndex = _selectedObjects.Count - 1;

            var selectedContainer = ChooseGoalObjectContainer();

            if (selectedContainer == null)
            {
                Debug.LogError($"Can't select container for goal: {_goalObject.SpriteValue}");
                return;
            }

            List<ObjectData> TempObjectsContainer = new List<ObjectData>(selectedContainer.ObjectData);
            TempObjectsContainer.Remove(_goalObject);

            for (int i = 0; i < _difficult.Rows * _difficult.Columns - 1; i++)
            {
                _selectedObjects.Add(ChooseRandomObject(TempObjectsContainer));
            }

            var randomIndexSelectedObjects = Random.Range(0, _selectedObjects.Count);
            (_selectedObjects[goalIndex], _selectedObjects[randomIndexSelectedObjects]) =
                (_selectedObjects[randomIndexSelectedObjects], _selectedObjects[goalIndex]); //swap goal/random objects
        }

        public void SetGoal()
        {
            if (_allObjects.Count == 0)
            {
                CopyAllObjects();
            }

            var goal = ChooseRandomGoal();

            _goalObject = goal;
            _allObjects.Remove(goal);
        }

        private ObjectData ChooseRandomObject(List<ObjectData> tempObjectsContainer)
        {
            var randomIndexObject = Random.Range(0, tempObjectsContainer.Count);
            ObjectData randomObjectData = tempObjectsContainer[randomIndexObject];
            tempObjectsContainer.Remove(randomObjectData);
            return randomObjectData;
        }

        private ObjectsContainer ChooseGoalObjectContainer() // 
        {
            foreach (var container in _objectsContainer)
            {
                if (container.ObjectData.Contains(_goalObject))
                {
                    return container;
                }
            }

            return null;
        }

        private void SetCellContent(ObjectData objectData, int row, int column)
        {
            var cell = _gridBuilder.Grid.Cells[row, column];
            cell.CellClick += OnCellClick;
            cell.SetObject(objectData);
        }

        private void OnCellClick(Cell cell, ObjectData objectData)
        {
            if (_goalObject.SpriteValue != null && _goalObject.SpriteValue == objectData.SpriteValue)
            {
                if (LevelCompleted != null)
                {
                    LevelCompleted();
                }

                Vibration.Vibrate();
                cell.PlayWinBounce();
                cell.PlayStarParticle();
            }
            else
            {
                Vibration.VibratePop();
                cell.ShakePosition();
            }
        }

        private ObjectData ChooseRandomGoal()
        {
            var goalRandomIndex = Random.Range(0, _allObjects.Count);
            ObjectData goalObject = _allObjects[goalRandomIndex];
            return goalObject;
        }

        private void CopyAllObjects()
        {
            foreach (var container in _objectsContainer)
            {
                foreach (var objectData in container.ObjectData)
                {
                    _allObjects.Add(objectData);
                }
            }
        }
    }
}