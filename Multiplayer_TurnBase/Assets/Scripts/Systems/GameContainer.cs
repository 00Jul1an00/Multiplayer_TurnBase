using Field;
using UnityEngine;

namespace GameSystems
{
    public class GameContainer : MonoBehaviour
    {
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private PathVisualizer _visualizer;
        [SerializeField] private PlayingField _playingField;

        private InputEventsReciver _inputEventsReciver;

        private void Awake()
        {
            _playingField.Init();
            _inputEventsReciver = new(_inputHandler, _visualizer, Units.Membersip.Player1);
            _inputEventsReciver = new(_inputHandler, _visualizer, Units.Membersip.Player2);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}
