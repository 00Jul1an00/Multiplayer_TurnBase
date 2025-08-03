using JHelpers;
using Signals;
using UnityEngine;

namespace GameUI
{
    public class UIController
    {
        private UIView _view;
        private EventBus _eventBus;

        public UIController(EventBus eventBus, UIView view)
        {
            _eventBus = eventBus;
            _view = view;
            Subscribe();
        }

        ~UIController()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<MoveSuccesSignal>(OnMoveSucces);
            _eventBus.Subscribe<AttackSuccesSignal>(OnAttackSucces);
            _eventBus.Subscribe<ChangeTurnSignal>(OnChangeTurn);
            _eventBus.Subscribe<TimerUpdatedSignal>(OnTimerUpdated);
        }

        private void Unsubscribe()
        {
            _eventBus.Unsubscribe<MoveSuccesSignal>(OnMoveSucces);
            _eventBus.Unsubscribe<AttackSuccesSignal>(OnAttackSucces);
            _eventBus.Unsubscribe<ChangeTurnSignal>(OnChangeTurn);
            _eventBus.Unsubscribe<TimerUpdatedSignal>(OnTimerUpdated);
        }

        private void OnAttackSucces(AttackSuccesSignal signal)
        {
            _view.ChangeAttackText($"Attack: 0");
        }

        private void OnMoveSucces(MoveSuccesSignal signal)
        {
            _view.ChangeMoveText($"Move: 0");
        }

        private void OnChangeTurn(ChangeTurnSignal signal)
        {
            _view.ChangeTurnCountText(signal.TurnCount.ToString());
            _view.ChangeTurnText(signal.ChangeToMembersip.ToString());
            _view.ChangeMoveText($"Move: 1");
            _view.ChangeAttackText($"Attack: 1");
        }

        private void OnTimerUpdated(TimerUpdatedSignal signal)
        {
            _view.ChangeTurnTimerText(Mathf.CeilToInt(signal.RemainigTime).ToString());
        }
    }
}
