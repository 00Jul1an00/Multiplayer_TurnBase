using System;
using Units;
using UnityEngine;

namespace GameSystems
{
    public class TurnController : MonoBehaviour
    {
        [SerializeField] private float _timeOnTurnInSec;

        private Action _signalToChangeTurn;

        public Membersip CurrentTurnMembetship { get; private set; }

        public void Init(Action signalToChangeTurn)
        {
            CurrentTurnMembetship = Membersip.Player1;
            _signalToChangeTurn = signalToChangeTurn;
            _signalToChangeTurn += NextTurn;
        }

        public void DeInit()
        {
            _signalToChangeTurn -= NextTurn;
        }

        private void NextTurn()
        {
            if (CurrentTurnMembetship == Membersip.Player1)
            {
                CurrentTurnMembetship = Membersip.Player2;
            }
            else
            {
                CurrentTurnMembetship = Membersip.Player1;
            }
        }
    }
}