using UnityEngine;

namespace Common
{
    public class HapticsInitializer : MonoBehaviour
    {
        private void Awake()
        {
            Vibration.Init();
        }
    }
}