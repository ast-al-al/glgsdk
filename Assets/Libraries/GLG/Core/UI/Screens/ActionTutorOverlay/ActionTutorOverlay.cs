using GLG.UI;
using UnityEngine;

public class ActionTutorOverlay : UIController
{
    [SerializeField] private UniversalActionTutor[] tutors;

    protected override void OnStartHide()
    {
        HideTutors();
    }
    private void Awake()
    {
        foreach (var tutor in tutors)
        {
            tutor.gameObject.SetActive(false);
        }
    }
    public void ShowTutor(TutorialType tutorialType)
    {
        bool showed = false;
        foreach (var item in tutors)
        {
            if(item.tutorialType == tutorialType)
            {
                item.gameObject.SetActive(true);
                showed = true;
            }
        }
        if (!showed) Hide();
    }
    public void HideTutors()
    {
        foreach (var tutor in tutors)
        {
            tutor.gameObject.SetActive(false);
        }
    }
}
