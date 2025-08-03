using JHelpers;
using Signals;
using System.Collections;
using UnityEngine;

public class TurnTimer : MonoBehaviour
{
    [SerializeField] private float _durationOnTurn;

    private Coroutine _timerRoutine;
    private EventBus _eventBus;

    public void Init(EventBus eventBus)
    {
        _eventBus = eventBus;
        Subscribe();
    }

    private void OnDestroy()
    {
        StopTimer();
        Unsubscribe();
    }

    private void Subscribe()
    {
        _eventBus.Subscribe<ChangeTurnSignal>(OnChangeTurn);
    }

    private void Unsubscribe()
    {
        _eventBus.Unsubscribe<ChangeTurnSignal>(OnChangeTurn);
    }

    private void OnChangeTurn(ChangeTurnSignal signal)
    {
        StopTimer();
        StartTimer();
    }

    public void StartTimer()
    {
        if (_timerRoutine != null)
        {
            StopCoroutine(_timerRoutine);
        }

        _timerRoutine = StartCoroutine(TimerRoutine(_durationOnTurn));
    }

    public void StopTimer()
    {
        if (_timerRoutine != null)
        {
            StopCoroutine(_timerRoutine);
            _timerRoutine = null;
        }
    }

    private IEnumerator TimerRoutine(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _eventBus.Invoke(new TimerUpdatedSignal(duration - elapsed));
            yield return null;
        }

        if (elapsed > duration)
        {
            _eventBus.Invoke(new RequestToChangeTurnSignal());
        }

        _timerRoutine = null;
    }
}