using UnityEngine;

public class PopupExample : MonoBehaviour
{
    [SerializeField] private PopupType popupToShow;
    [SerializeField] private PopupType[] popupsToShow;

    private void Start()
    {
        PopupManager.Instance.SetPopupsList(popupsToShow, hasReplay: true);
    }

    public void ShowPopups()
    {
        PopupManager.Instance.OnShowNextPopup();
    }

    public void ShowInstantPopup()
    {
        PopupManager.Instance.ShowPopup(popupToShow);
    }
}
