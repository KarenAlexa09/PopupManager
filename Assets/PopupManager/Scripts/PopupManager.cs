using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    public Action OnPopupsFinished;

    [Header("Popup Prefabs")]
    [SerializeField] private List<PopupViewBase> popupPrefabs;

    [Header("Popup Container")]
    [SerializeField] private Transform popupContainer;

    [Header("Popup Animation")]
    [SerializeField] private float animationTime = 0.5f;
    [SerializeField] private Ease animationType = Ease.OutBack;

    private CanvasGroup popupCanvasGroup;

    private Queue<PopupViewBase> popupQueue = new();
    private Stack<PopupViewBase> popupHistory = new();
    private List<PopupViewBase> shownPopups = new();

    private PopupViewBase currentPopup;

    private bool _hasReplay = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        popupCanvasGroup = popupContainer.GetComponent<CanvasGroup>();

        if (popupCanvasGroup == null)
        {
            popupCanvasGroup = popupContainer.gameObject.AddComponent<CanvasGroup>();
        }

        popupCanvasGroup.alpha = 0;
        popupCanvasGroup.interactable = false;
        popupCanvasGroup.blocksRaycasts = false;
    }

    public void SetPopupsList(IEnumerable<PopupType> popupsToShow, IEnumerable<object[]> popupParametersList = null, bool hasReplay = false)
    {
        _hasReplay = hasReplay;

        ClearPopups();

        var parametersArray = popupParametersList?.ToArray();
        int index = 0;

        foreach (var popupType in popupsToShow)
        {
            var popupPrefab = popupPrefabs.FirstOrDefault(p => p.PopupData.PopupType == popupType);

            if (popupPrefab == null)
            {
                Debug.LogError($"Popup not found: {popupType}");
                continue;
            }

            var popupInstance = Instantiate(popupPrefab, popupContainer);

            var parameters = parametersArray != null && index < parametersArray.Length ? parametersArray[index] : null;

            popupInstance.Initialize(parameters);

            popupQueue.Enqueue(popupInstance);

            index++;
        }
    }

    public void ShowPopup(PopupType popupType, params object[] parameters)
    {
        if (currentPopup != null && currentPopup.PopupData.PopupType == popupType)
        {
            currentPopup.BackButtonClicked += OnClosedPopup;
            currentPopup.NextButtonClicked += OnClosedPopup;

            ShowPopupContainer();

            StartPopup(currentPopup);
            return;
        }

        var popupPrefab = popupPrefabs.FirstOrDefault(p => p.PopupData.PopupType == popupType);

        if (popupPrefab == null)
        {
            Debug.LogError($"Popup not found: {popupType}");
            return;
        }

        var popupInstance = Instantiate(popupPrefab, popupContainer);
        popupInstance.Initialize(parameters);

        currentPopup = popupInstance;

        currentPopup.BackButtonClicked += OnClosedPopup;
        currentPopup.NextButtonClicked += OnClosedPopup;

        ShowPopupContainer();

        StartPopup(currentPopup);
    }

    public void OnShowNextPopup()
    {
        if (popupQueue.Count == 0)
        {
            HidePopupContainer();

            if (_hasReplay)
                ReplayPopups();

            OnPopupsFinished?.Invoke();

            Debug.Log("No more popups to show.");

            return;
        }

        ShowPopupContainer();

        if (currentPopup != null)
        {
            popupHistory.Push(currentPopup);

            currentPopup.NextButtonClicked -= OnShowNextPopup;
            currentPopup.BackButtonClicked -= OnShowPreviousPopup;
        }

        currentPopup = popupQueue.Dequeue();
        currentPopup.NextButtonClicked += OnShowNextPopup;
        currentPopup.BackButtonClicked += OnShowPreviousPopup;

        StartPopup(currentPopup);

        if (_hasReplay)
            shownPopups.Add(currentPopup);
    }

    public void OnShowPreviousPopup()
    {
        if (currentPopup != null)
        {
            currentPopup.NextButtonClicked -= OnShowNextPopup;
            currentPopup.BackButtonClicked -= OnShowPreviousPopup;
        }

        if (popupHistory.Count > 0)
        {
            currentPopup = popupHistory.Pop();
            currentPopup.NextButtonClicked += OnShowNextPopup;
            currentPopup.BackButtonClicked += OnShowPreviousPopup;
            StartPopup(currentPopup);
        }
        else
        {
            HidePopupContainer();

            Debug.Log("No more popups to show.");

            OnPopupsFinished?.Invoke();
        }
    }

    private void ReplayPopups()
    {
        if (shownPopups.Count > 0)
        {
            foreach (var popup in shownPopups)
            {
                popupQueue.Enqueue(popup);
            }

            shownPopups.Clear();
        }
    }

    private void OnClosedPopup()
    {
        HidePopupContainer();

        currentPopup.BackButtonClicked -= OnClosedPopup;
        currentPopup.NextButtonClicked -= OnClosedPopup;
    }

    private void ShowPopupContainer()
    {
        if (popupCanvasGroup.alpha == 0)
        {
            popupCanvasGroup.DOFade(1, animationTime).SetEase(animationType)
           .OnStart(() =>
           {
               popupCanvasGroup.interactable = true;
               popupCanvasGroup.blocksRaycasts = true;
           }).SetAutoKill(true);
        }
    }

    private void HidePopupContainer()
    {
        popupCanvasGroup.DOFade(0, animationTime).SetEase(animationType)
            .OnComplete(() =>
            {
                popupCanvasGroup.interactable = false;
                popupCanvasGroup.blocksRaycasts = false;
            }).SetAutoKill(true);
    }

    private void StartPopup(PopupViewBase popup)
    {
        popup.gameObject.SetActive(true);
        popup.Open();
    }

    private void ClearPopups()
    {
        while (popupQueue.Count > 0)
        {
            var popup = popupQueue.Dequeue();
            if (popup != null)
            {
                popup.BackButtonClicked -= HidePopupContainer;
                popup.NextButtonClicked -= HidePopupContainer;
                Destroy(popup.gameObject);
            }
        }

        popupHistory.Clear();
        shownPopups.Clear();
        HidePopupContainer();
    }
}
