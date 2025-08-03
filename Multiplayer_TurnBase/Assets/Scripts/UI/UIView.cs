using TMPro;
using UnityEngine;

namespace GameUI
{
    public class UIView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _turnCountText;
        [SerializeField] private TMP_Text _turnTimerText;
        [SerializeField] private TMP_Text _turnText;
        [SerializeField] private TMP_Text _attackText;
        [SerializeField] private TMP_Text _moveText;

        public void ChangeTurnCountText(string text)
        {
            _turnCountText.text = text;
        }

        public void ChangeTurnTimerText(string text)
        {
            _turnTimerText.text = text;
        }

        public void ChangeAttackText(string text)
        {
            _attackText.text = text;
        }

        public void ChangeMoveText(string text)
        {
            _moveText.text = text;
        }

        public void ChangeTurnText(string text)
        {
            _turnText.text = text;
        }
    }
}
