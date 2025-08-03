using UnityEngine;

namespace Units
{
    public class SelectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _selectIndicator;

        public void InitComponent()
        {
            Deselect();
        }

        public void Select()
        {
            _selectIndicator.SetActive(true);
        }

        public void Deselect()
        {
            _selectIndicator.SetActive(false);
        }
    }
}