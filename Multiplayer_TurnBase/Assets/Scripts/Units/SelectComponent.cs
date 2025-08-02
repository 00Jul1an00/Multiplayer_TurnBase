using UnityEngine;

public class SelectComponent : MonoBehaviour
{
    [SerializeField] private GameObject _selectIndicator;
    private bool _selected;

    public void ToggleSelection()
    {
        _selected = !_selected;
        _selectIndicator.SetActive(_selected);
    }

    public void Deselect()
    {
        _selected = false;
        _selectIndicator.SetActive(false);
    }
}
