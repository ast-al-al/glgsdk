using UnityEngine;

public enum TutorialType { PressAndHold, DragToMove, Tap, None }
public class UniversalActionTutor : MonoBehaviour
{
    public TutorialType tutorialType;
    public bool animateOnEnable = true;
    public virtual void StartAnimation()
    {

    }
    public virtual void StopAnimation()
    {

    }
    private void OnEnable()
    {
        if (animateOnEnable) StartAnimation();
    }
}

