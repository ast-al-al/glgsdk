using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BaseInteractible : MonoBehaviour, IInteractible
{
    public event Action<IInteractor> OnInteractionStarted;
    public event Action<IInteractor, float> OnInteractionPerformed;
    public event Action<IInteractor> OnInteractionComplete;
    public event Action<IInteractor> OnInteractionCanceled;

    [SerializeField] private EntitiesWhitelist _interactorsWhitelist;
    [SerializeField] private EntitiesBlacklist _interactorsBlacklist;
    [SerializeField] private EntityData _entityData;
    [Tooltip("Время необходимое для завершения взаимодействия")]
    [SerializeField] private float _interactionDuration = 0f;
    [Tooltip("Может взаимодействовать с несколькими интеракторами")]
    [SerializeField] private bool _canServeManyInteractors;
    [Tooltip("Дополнительные интеракторы ускоряют работу первого")]
    [SerializeField] private bool _additiveInteractors;

    private Dictionary<IInteractor, float> _currentInteractors = new Dictionary<IInteractor, float>();
    private bool _isInteracting;
    private float _selfTime;

    public bool IsInteracting => _currentInteractors.Count > 0;
    public float InteractionDuration => _interactionDuration;
    public EntityData EntityData => _entityData;
    public float InteractionSpeedMultiplier { get; set; } = 1f;
    public bool CanInteract { get; set; } = true;

    public void CancelInteraction(IInteractor interactor)
    {
        if (_currentInteractors.TryGetValue(interactor, out float time)) // у нас действительно есть такой интерактор
        {
            _currentInteractors.Remove(interactor);
            OnInteractionCanceled?.Invoke(interactor);
        }
    }
    public bool CanInteractWith(IInteractor interactor)
    {
        return
            _interactorsWhitelist.IsEntityAllowed(interactor.EntityData)
            && _interactorsBlacklist.IsEntityAllowed(interactor.EntityData);
    }
    public float RecieveInteraction(IInteractor interactor, float interactionTime)
    {
        if (!CanInteract) return 0f;
        // при мгновенном взаимодействии - просто завершаем взаимодейсмтвие
        if (_interactionDuration <= 0f)
        {
            OnInteractionPerformed?.Invoke(interactor, 1f);
            OnInteractionComplete?.Invoke(interactor);
            return 1f;
        }

        // взаимодействие занимает время

        if (_currentInteractors.Count == 0)
        {
            _currentInteractors[interactor] = 0f;
            OnInteractionStarted?.Invoke(interactor);
            float progress;
            if (_additiveInteractors)
            {
                _selfTime += interactionTime;
                progress = _selfTime / _interactionDuration;
                if (progress >= 1f)
                {
                    AllInteractorsCompletedHandler();
                    return 1f;
                }
                OnInteractionPerformed?.Invoke(interactor, progress);
                return progress;
            }
            else
            {
                float time = _currentInteractors[interactor];
                time += interactionTime;
                progress = time / _interactionDuration;
                if (progress >= 1f)
                {
                    OnInteractionPerformed?.Invoke(interactor, 1f);
                    InteractionCompleteHanlder(interactor);
                    return 1f;
                }
                _currentInteractors[interactor] = time;
                OnInteractionPerformed?.Invoke(interactor, progress);
                return progress;
            }
        }
        else
        {
            if (_currentInteractors.ContainsKey(interactor))
            {
                // запишем в общий прогресс
                if (_additiveInteractors)
                {
                    _selfTime += interactionTime;
                    float progress = _selfTime / _interactionDuration;
                    if (progress >= 1f)
                    {
                        AllInteractorsCompletedHandler();
                        return 1f;
                    }
                    OnInteractionPerformed?.Invoke(interactor, progress);
                    return progress;
                }
                // запишем в собственный прогресс
                else
                {
                    float time = _currentInteractors[interactor];
                    time += interactionTime;
                    float progress = time / _interactionDuration;
                    if (progress >= 1f)
                    {
                        OnInteractionPerformed?.Invoke(interactor, 1f);
                        InteractionCompleteHanlder(interactor);
                        return 1f;
                    }
                    _currentInteractors[interactor] = time;
                    OnInteractionPerformed?.Invoke(interactor, progress);
                    return progress;
                }
            }
            else
            {
                if (_canServeManyInteractors)
                {
                    OnInteractionStarted?.Invoke(interactor);
                    _currentInteractors[interactor] = 0f;
                    return 0f;
                }
                else
                {
                    return 0f;
                }
            }

        }
    }

    protected void InteractionCompleteHanlder(IInteractor interactor)
    {
        _currentInteractors.Remove(interactor);
        OnInteractionComplete?.Invoke(interactor);
    }
    protected void AllInteractorsCompletedHandler()
    {
        foreach (var interactor in _currentInteractors)
        {
            interactor.Key.ForceInteractionComplete(this);
            OnInteractionPerformed?.Invoke(interactor.Key, 1f);
            OnInteractionComplete?.Invoke(interactor.Key);
        }
        _currentInteractors.Clear();
        _selfTime = 0f;
    }
}