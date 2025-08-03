using System;
using Units;
using UnityEngine;

namespace GameSystems
{
    public class InputHandler : MonoBehaviour
    {
        public event Action OnLeftBtnClick;
        public event Action OnRightBtnClick;
        public event Action OnDoubleRightBtnClick;

        private float _doubleClickTime = .2f;
        private float _lastClickTime;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnLeftBtnClick?.Invoke();
            }

            if (Input.GetMouseButtonDown(1))
            {
                float timeSinceLastClick = Time.time - _lastClickTime;

                if (timeSinceLastClick <= _doubleClickTime)
                {
                    OnDoubleRightBtnClick?.Invoke();
                }
                else
                {
                    OnRightBtnClick?.Invoke();
                }

                _lastClickTime = Time.time;
            }
        }
    }
}

