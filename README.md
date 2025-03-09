# Popup System for Unity

## Introduction
This popup system allows displaying messages on the screen with various configurations, navigation buttons, and custom animations. It is based on `ScriptableObjects` for flexible and reusable configuration.

## Installation and Usage
1. Import the scripts into your Unity project.
2. Create a `PopupData` from the Editor (`Assets > Create > Popup System > Popup Data`).
3. Configure the `PopupData` values such as message, colors, buttons, and animations.
4. Use `PopupManager` to manage the sequence of popups in your scene.

## Project Structure
### 1. `PopupViewBase`
Base class for popups. Handles initialization, button setup, and activation/deactivation with animations.

### 2. `PopupData`
A `ScriptableObject` that defines each popup's properties, such as type, message, colors, and navigation settings.

### 3. `PopupManager`
Centralized system to manage the popup queue and its behavior in the scene.

### 4. `PopupAnimation`
Serializable class that handles popup opening and closing animations using `DOTween`.

### 5. `PopupExample`
Example usage of the system, demonstrating different ways to trigger popups.

## Usage Example
To show a specific popup at the start of the scene:
```csharp
[SerializeField] private PopupType popupToShow;

private void Start()
{
    PopupManager.Instance.ShowPopup(popupToShow);
}
```

To set up a sequence of popups:
```csharp
[SerializeField] private PopupType[] popupsToShow;

private void Start()
{
    PopupManager.Instance.SetPopupsList(popupsToShow, hasReplay: true);
}
```

## Editor Configuration
1. **`PopupData`**: Defines the message, colors, and navigation settings.
2. **`PopupAnimation`**: Adjusts animation duration, easing type, and initial/final scales.

## Animations
Animations use `DOTween`. They can be customized by editing values in `PopupAnimation`:
- **Animation duration** (`animationTime`)
- **Easing type** (`animationType`)
- **Initial and final scale** (`startScale`, `endScale`)

Example of animation customization:
```csharp
popupAnimation.animationTime = 1.0f;
popupAnimation.animationType = Ease.InOutBounce;
```

## Contributions & Contact
If you want to improve this system or report issues, feel free to contribute or get in touch.

---
This document may be updated with more details as the system evolves.
