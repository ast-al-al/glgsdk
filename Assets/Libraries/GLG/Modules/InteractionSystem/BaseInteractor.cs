using System;
using System.Collections.Generic;
using GLG;
using UnityEngine;

public class InteractorTask
{
    public IInteractible interactible;
    public float nextInteractionTime;
    public bool started;

    public InteractorTask(IInteractible interactible, float nextInteractionTime)
    {
        this.interactible = interactible;
        this.nextInteractionTime = nextInteractionTime;
    }
}

public class BaseInteractor : MonoBehaviour, IInteractor, IManaged
{
    public event Action<IInteractible> OnInteractionStarted;
    public event Action<IInteractible, float> OnInteractionPerformed;
    public event Action<IInteractible> OnInteractionComplete;
    public event Action<IInteractible> OnInteractionCanceled;

    [SerializeField] protected EntitiesWhitelist _interactiblesWhitelist;
    [SerializeField] protected EntitiesBlacklist _interactiblesBlacklist;
    [SerializeField] protected EntityData _entityData;
    [SerializeField] protected float _delayBeforeInteractions;
    [SerializeField] protected bool _canDoMultipleTasks = true;

    protected List<InteractorTask> _interactibles = new List<InteractorTask>();
    protected bool _isInteracting;

    public float InteractionSpeedMultiplier { get; set; } = 1f;
    public EntityData EntityData => _entityData;

    protected virtual void Awake()
    {
        Kernel.RegisterManaged(this);
    }
    protected virtual void OnDestroy()
    {
        Kernel.UnregisterManaged(this);
    }
    public virtual void ManagedUpdate()
    {
        for (int i = _interactibles.Count - 1; i >= 0; i--)
        {
            InteractorTask task = _interactibles[i];
            if (task.nextInteractionTime < Time.time)
            {
                if(!task.started)
                {
                    task.started = true;
                    OnInteractionStarted?.Invoke(task.interactible);
                }
                float progress = InteractWith(task.interactible);
                if(progress >= 1f)
                {
                    OnInteractionComplete?.Invoke(task.interactible);
                    task.nextInteractionTime = Time.time + _delayBeforeInteractions;
                    task.started = false;
                }
            }
        }
    }

    public virtual float InteractWith(IInteractible interactible)
    {
        float progress = interactible.RecieveInteraction(this, Time.deltaTime * InteractionSpeedMultiplier);
        OnInteractionPerformed?.Invoke(interactible, progress);
        return progress;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (_interactiblesWhitelist.IsTagAllowed(other) && _interactiblesBlacklist.IsTagAllowed(other))
        {
            if (other.TryGetComponent(out IInteractible interactible))
            {
                if (interactible.CanInteractWith(this))
                {
                    InteractorTask task = new InteractorTask(interactible, Time.time);
                    task.started = false;
                    _interactibles.Add(task);
                }
            }
        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        if (_interactiblesWhitelist.IsTagAllowed(other) && _interactiblesBlacklist.IsTagAllowed(other))
        {
            if (other.TryGetComponent(out IInteractible interactible))
            {
                for (int i = 0; i < _interactibles.Count; i++)
                {
                    if (_interactibles[i].interactible == interactible)
                    {
                        _interactibles.RemoveAt(i);
                        interactible.CancelInteraction(this);
                        OnInteractionCanceled?.Invoke(interactible);
                        return;
                    }
                }
            }
        }
    }

    public void ForceInteractionComplete(IInteractible interactible)
    {
        foreach (var item in _interactibles)
        {
            if(item.interactible == interactible)
            {
                item.nextInteractionTime = Time.time + _delayBeforeInteractions;
                item.started = false;
                OnInteractionComplete?.Invoke(interactible);
            }
        }
    }
}