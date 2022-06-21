using System.Collections.Generic;
using UnityEngine;

namespace Common.Data
{
    [CreateAssetMenu(fileName = "New ObjectsContainer", menuName = "ObjectsContainer")]

    public class ObjectsContainer : ScriptableObject
    {
        [SerializeField] private List<ObjectData> _objectData;

        public List<ObjectData> ObjectData => _objectData;

    }
}