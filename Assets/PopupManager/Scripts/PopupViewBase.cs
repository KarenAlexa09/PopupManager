using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupViewBase : MonoBehaviour
{
    public Action NextButtonClicked;
    public Action BackButtonClicked;

    [SerializeField] protected TMP_Text messageText;
    [SerializeField] protected Image background;
    [SerializeField] protected Button nextButton;
    [SerializeField] protected Button backButton;
    [SerializeField] protected PopupData popupData;

    public PopupData PopupData { get { return popupData; } }

    protected PopupAnimation popupAnimation;

    private Coroutine autoCloseCoroutine;

    public virtual void Initialize(params object[] parameters)
    {
        if (popupData == null) return;

        SetupButton(nextButton, OnNextClicked, popupData.HasNextButton);
        SetupButton(backButton, OnBackClicked, popupData.HasBackButton);

        messageText.text = popupData.Message;
        background.color = popupData.BackgroundColor;
        popupAnimation = popupData.AnimationConfig;

        gameObject.SetActive(false);
    }

    private void SetupButton(Button button, Action action, bool isActive)
    {
        if (button == null) return;

        button.gameObject.SetActive(isActive);

        if (isActive)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => action?.Invoke());
        }
    }

    private IEnumerator AutoClosePopup(float time)
    {
        yield return new WaitForSeconds(time);

        Deactivate(()=> NextButtonClicked?.Invoke());
    }

    private void OnNextClicked()
    {
        Deactivate(() => NextButtonClicked?.Invoke());
    }

    private void OnBackClicked()
    {
        Deactivate(() => BackButtonClicked?.Invoke());
    }

    public virtual void Open()
    {
        popupAnimation.PlayAnimation(transform);

        if (popupData.AutoClose)
        {
            autoCloseCoroutine = StartCoroutine(AutoClosePopup(popupData.AutoCloseTime));
        }
    }

    public virtual void Deactivate(Action closed)
    {
        if (autoCloseCoroutine != null)
        {
            StopCoroutine(autoCloseCoroutine);
            autoCloseCoroutine = null;
        }

        if (popupData != null && !popupData.AutoClose)
        {
            popupAnimation.ReverseAnimation(transform, () =>
            {
                gameObject.SetActive(false);
                closed?.Invoke();
            });
        }
        else
        {
            gameObject.SetActive(false);
            closed?.Invoke();
        }
    }
}
