using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Visuals
{
    /// <summary>
    /// A cursor to use in the UI.
    /// </summary>
    public class UICursor : MonoBehaviour
    {
        public float blinkingSpeed = 0.5f;
        private float _lastBlinkUpdate;

        public bool isVisible;
        public bool isBlinking;

        public Vector2 characterSize;

        public Layer linkedLayer;

        public List<List<Vector2>> textMeshCharacterPositions;
        private Image _image;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _rectTransform = GetComponent<RectTransform>();
            
            textMeshCharacterPositions = new List<List<Vector2>>();
            
            isVisible = true;
            characterSize = new Vector2(8, 18);
            _lastBlinkUpdate = Time.time;
            SetSize(characterSize);
        }

        private void Update()
        {
            if (!isVisible) return;

            RenderBlink();
            UpdateUICursorPosition();
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
        /// Turn the blinking effect on or off.
        /// </summary>
        /// <param name="blink">If the cursor should blink.</param>
        public void Blink(bool blink)
        {
            isBlinking = blink;
            if(blink == false) GetComponent<Image>().enabled = isVisible;
        }

        /// <summary>
        /// Render the blinking effect.
        /// </summary>
        private void RenderBlink()
        {
            if (!isBlinking) return;
            if (!(Time.time - _lastBlinkUpdate > blinkingSpeed)) return;
            _image.enabled = !_image.enabled;
            _lastBlinkUpdate = Time.time;
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
            _rectTransform.sizeDelta = newSize;
        }

        /// <summary>
        /// Reset the size of the cursor to its initial size.
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
            Vector2 size = _rectTransform.sizeDelta / 2;
            Vector2 centerOffset = size;

            transform.position = newPosition + centerOffset;
        }

        /// <summary>
        /// Update the UI cursor position to its linked cursor.
        /// </summary>
        private void UpdateUICursorPosition()
        {
            if (linkedLayer == null) return;

            ResetSize();
            GridPosition viewPosition = linkedLayer.view.externalPosition;

            int characterRow = linkedLayer.cursor.position.row + viewPosition.row;
            int characterColumn = linkedLayer.cursor.position.column + viewPosition.column;

            if (characterRow >= textMeshCharacterPositions.Count) return;
            if (characterColumn >= textMeshCharacterPositions[characterRow].Count) return;

            Vector2 newPosition = textMeshCharacterPositions[characterRow][characterColumn];
            SetPositionCenter(newPosition);
        }

        /// <summary>
        /// Select a row on the Layer using the UI cursor.
        /// </summary>
        /// <param name="row">The index of the row to be selected</param>
        public void SelectRow(int row)
        {
            List<Vector2> rowPositions = textMeshCharacterPositions[row];

            Vector2 newSize = new Vector2(characterSize.x * rowPositions.Count, characterSize.y);
            Vector2 characterCenter = (rowPositions[0] + rowPositions[rowPositions.Count - 1]) / 2 + new Vector2(0,2);

            SetSize(newSize);
            SetPositionCenter(characterCenter);
        }

        public void SetGridPosition(GridPosition gridPosition)
        {
            Vector2 newPosition = textMeshCharacterPositions[gridPosition.row][gridPosition.column];
            SetPositionCenter(newPosition);
        }
    }
}