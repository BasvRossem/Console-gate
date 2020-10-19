using UnityEngine;
using UnityEngine.UI;

namespace Visuals
{
    /// <summary>
    /// A cursor to use in the UI.
    /// </summary>
    public class UICursor : MonoBehaviour
    {
        public float blinkingSpeed;
        private float lastBlinkUpdate;

        public bool isVisible;
        public bool isBlinking;

        public Vector2 characterSize;

        public MonitorCursor linkedCursor;

        private void Start()
        {
            isVisible = true;
            characterSize = new Vector2(8, 18);
            lastBlinkUpdate = Time.time;
            SetSize(characterSize);
        }

        private void Update()
        {
            if (isVisible && isBlinking && Time.time - lastBlinkUpdate > blinkingSpeed)
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
            isBlinking = blink;
        }

        /// <summary>
        /// Returns the size of the cursor.
        /// </summary>
        /// <returns>The size of the cursor.</returns>
        public Vector2 GetSize()
        {
            Vector2 size = GetComponent<RectTransform>().sizeDelta;
            return new Vector2(size.x, size.y);
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
            RectTransform rectTransform = GetComponent<RectTransform>();
            float width = rectTransform.sizeDelta.x;
            float height = rectTransform.sizeDelta.y;
            Vector3 centerOffset = new Vector3(width / 2, -height / 2, 0);

            transform.position = newPosition;
            transform.position += centerOffset;
        }
    }
}