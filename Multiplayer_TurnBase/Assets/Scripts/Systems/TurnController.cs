using JHelpers;
using Signals;
using Units;

namespace GameSystems
{
    public class TurnController
    {
        private readonly EventBus _eventBus;

        private Membersip _currentTurnMembetship;
        private int _currentTurn = 1;

        public TurnController(EventBus eventBus)
        {
            _eventBus = eventBus;
            Subsribe();
        }

        ~TurnController()
        {
            Unsubscribe();
        }

        public void StartGame()
        {
            _currentTurnMembetship = Membersip.Player1;
            _eventBus.Invoke(new ChangeTurnSignal(_currentTurnMembetship, _currentTurn));
        }

        private void Subsribe()
        {
            _eventBus.Subscribe<RequestToChangeTurnSignal>(NextTurn);
        }

        private void Unsubscribe()
        {
            _eventBus.Unsubscribe<RequestToChangeTurnSignal>(NextTurn);
        }

        private void NextTurn(RequestToChangeTurnSignal signal)
        {
            if (_currentTurnMembetship == Membersip.Player1)
            {
                _currentTurnMembetship = Membersip.Player2;
            }
            else
            {
                _currentTurnMembetship = Membersip.Player1;
            }

            _currentTurn++;
            _eventBus.Invoke(new ChangeTurnSignal(_currentTurnMembetship, _currentTurn));
        }
    }
}