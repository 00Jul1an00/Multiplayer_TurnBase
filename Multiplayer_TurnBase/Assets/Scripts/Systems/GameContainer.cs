using Field;
using GameUI;
using JHelpers;
using UnityEngine;
using Unity.Netcode;
using Units;

namespace GameSystems
{
    public class GameContainer : NetworkBehaviour
    {
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private PathVisualizer _visualizer;
        [SerializeField] private PlayingField _playingField;
        [SerializeField] private UIView _view;
        [SerializeField] private TurnTimer _turnTimer;

        private InputEventsReceiver _inputEventsReciver;
        private EventBus _eventBus;
        private TurnController _turnController;
        private UIController _uiController;

        private void Awake()
        {
            _eventBus = new();
            _playingField.Init();
            _uiController = new(_eventBus, _view);
            _inputEventsReciver = new(_inputHandler, _visualizer, Membersip.Player1, _eventBus);
            _inputEventsReciver = new(_inputHandler, _visualizer, Membersip.Player2, _eventBus);
            _turnController = new(_eventBus);
            _turnTimer.Init(_eventBus);
            
            if (IsServer)
            {
                StartGameServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void StartGameServerRpc()
        {
            StartGame();
        }

        private void StartGame()
        {
            _turnController.StartGame();
            _turnTimer.StartTimer();

            // Создаем контроллеры ввода для игроков
            CreatePlayerControllers();
        }

        private void CreatePlayerControllers()
        {
            if (IsServer)
            {
                // Создаем контроллер для хоста
                var hostController = new InputEventsReceiver(_inputHandler, _visualizer, Membersip.Player1, _eventBus);

                // Создаем контроллер для клиента
                CreatePlayerControllerClientRpc();
            }
        }

        [ClientRpc]
        private void CreatePlayerControllerClientRpc()
        {
            if (IsOwner)
            {
                _inputEventsReciver = new(_inputHandler, _visualizer, Membersip.Player2, _eventBus);
            }
        }
    }
}
