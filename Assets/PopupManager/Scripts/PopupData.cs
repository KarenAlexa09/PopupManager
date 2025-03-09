using UnityEngine;

[CreateAssetMenu(fileName = "PopupData", menuName = "Popup System/Popup Data")]
public class PopupData : ScriptableObject
{
    [Header("General Settings")]
    [SerializeField] private PopupType popupType;
    [TextArea(3, 5)]
    [SerializeField] private string message;
    [SerializeField] private Color backgroundColor = Color.white;
    [SerializeField] private PopupAnimation animationConfig;

    [Header("Navigation Settings")]
    [SerializeField] private bool hasNextButton = true;
    [SerializeField] private bool hasBackButton = true;
    [SerializeField] private bool autoClose = false;
    [SerializeField] private float autoCloseTime = 3f;

    public PopupType PopupType => popupType;

    public string Message => message;

    public Color BackgroundColor => backgroundColor;

    public PopupAnimation AnimationConfig => animationConfig;

    public bool HasNextButton => hasNextButton;

    public bool HasBackButton => hasBackButton;

    public bool AutoClose => autoClose;

    public float AutoCloseTime => Mathf.Max(autoCloseTime, 0); 
}

public enum PopupType
{
    Warning,
    Error,
    Welcome
}
