using UnityEngine;
using UnityEngine.UI;

public class QuestBlock : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private AspectRatioFitter _aspectRatioFitter;
    [SerializeField] private Text _rewardText;
    [SerializeField] private Text _text;
    [SerializeField] private Text _claimButtonText;
    [SerializeField] private RectTransform _progressBarContainer;
    [SerializeField] private RectTransform _progressBarFiller;
    [SerializeField] private Text _progressText;
    [SerializeField] private Button _claimButton;
    [SerializeField] private GameObject _progressBlock;
    public event System.Action<QuestBlock> onClaimButtonPressed;
    public int id;

    public Vector3 IconPosition => _icon.transform.position;

    public QuestBlock SetIcon(Sprite sprite)
    {
        _icon.sprite = sprite;
        float width = sprite.rect.width;
        float height = sprite.rect.height;
        _aspectRatioFitter.aspectRatio = width / height;
        return this;
    }
    public QuestBlock SetRewardText(string text)
    {
        _rewardText.text = text;
        return this;
    }
    public QuestBlock SetText(string text)
    {
        _text.text = text;
        return this;
    }
    public QuestBlock SetProgress(float progress)
    {
        progress = Mathf.Clamp(progress, 0.001f, 1f);
        Vector2 fillerSize = _progressBarFiller.sizeDelta;
        float containerSize = _progressBarContainer.rect.width;
        fillerSize.x = Mathf.Max(24f, containerSize * progress);
        _progressBarFiller.sizeDelta = fillerSize;
        return this;
    }
    public QuestBlock SetProgressText(string progressText)
    {
        _progressText.text = progressText;
        return this;
    }
    public QuestBlock SetClaimButtonVisible(bool visible)
    {
        _claimButton.gameObject.SetActive(visible);
        return this;
    }
    public QuestBlock SetProgressBlockVisible(bool visible)
    {
        _progressBlock.SetActive(visible);
        return this;
    }
    public QuestBlock SetClaimedStatus(bool isClaimed)
    {
        _claimButtonText.text = isClaimed ? "CLAIMED" : "CLAIM";
        _claimButton.interactable = !isClaimed;
        return this;
    }
    public void Dispose()
    {
        onClaimButtonPressed = null;
    }

    public void ClaimButtonHandler()
    {
        onClaimButtonPressed?.Invoke(this);
    }
}
