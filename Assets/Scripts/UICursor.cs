using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A cursor to use in the UI.
/// </summary>
public class UICursor : MonoBehaviour
{
    public float blinkingSpeed;
    private float lastBlinkUpdate;

    public bool isVisible;
    public bool blinking;

    public Vector2 characterSize;

    private void Start()
    {
        characterSize = new Vector2(8, 18);
        isVisible = true;

        lastBlinkUpdate = Time.time;
    }

    private void Update()
    {
        if (isVisible && blinking && Time.time - lastBlinkUpdate > blinkingSpeed)
        {
            GetComponent<Image>().enabled = !GetComponent<Image>().enabled;
            lastBlinkUpdate = Time.time;
        }
    }

    /// <summary>
    /// Turn the visibility of the cursor on or off.
    /// </summary>
    /// <param name="visibility">If it should be visible</param>
    public void Show(bool visibility)
    {
        GetComponent<Image>().enabled = visibility;
        isVisible = visibility;
    }

    /// <summary>
    /// Turn the blinkin geffect on or off.
    /// </summary>
    /// <param name="blink">If the cursor should blink.</param>
    public void Blink(bool blink)
    {
        blinking = blink;
    }

    /// <summary>
    /// Returns the size of the cursor.
    /// </summary>
    /// <returns>The size of the cursor.</returns>
    public Vector2 GetSize()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        return new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
    }

    /// <summary>
    /// Set a new cursor size.
    /// </summary>
    /// <param name="newSize">New size of the cursor.</param>
    public void SetSize(Vector2 newSize)
    {
        GetComponent<RectTransform>().sizeDelta = newSize;
    }

    /// <summary>
    /// Reset the size of the cursor to its inital size.
    /// </summary>
    public void ResetSize()
    {
        SetSize(characterSize);
    }

    /// <summary>
    /// Set the position of the center of the cursor.
    /// </summary>
    /// <param name="newPosition">The new position of the center.</param>
    public void SetPositionCenter(Vector2 newPosition)
    {
        transform.position = newPosition;
    }

    /// <summary>
    /// Set the position top left position of the cursor.
    /// </summary>
    /// <param name="newPosition">The new position of the top left.</param>
    public void SetPositionTopLeft(Vector2 newPosition)
    {
        Debug.Log(newPosition);
        var rectTransform = GetComponent<RectTransform>();
        float width = rectTransform.sizeDelta.x;
        float height = rectTransform.sizeDelta.y;
        Vector3 centerOffset = new Vector3(width / 2, -height / 2, 0);

        transform.position = newPosition;
        transform.position += centerOffset;
        Debug.Log(transform.position);
    }
}