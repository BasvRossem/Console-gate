using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Visuals
{
    public class Monitor : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMesh;

        public static int RowAmount = 24;
        public static int ColumnAmount = 80;

        private Layer _mainLayer;
        private List<Layer> _layers;
        private List<List<Vector2>> _textMeshCharacterPositions;

        private TextGrid _textGrid;

        private string _text = "";

        public UICursor uiCursor;

        private void Awake()
        {
            _mainLayer = new Layer(RowAmount, ColumnAmount);
            _layers = new List<Layer>();
            _textMeshCharacterPositions = new List<List<Vector2>>();

            _textGrid = new TextGrid(RowAmount, ColumnAmount);

            _textMesh = _textMesh ?? new TextMeshProUGUI();
        }

        private void Start()
        {
        }

        private void Update()
        {
            Render();
        }

        private void CalibrateTextMesh()
        {
            // Fill text grid with data
            _mainLayer.textGrid.Fill('*');

            // Render data to text mesh
            Render();
            _textMesh.ForceMeshUpdate();

            // Calculate character center positions
            _textMeshCharacterPositions = new List<List<Vector2>>();
            for (int row = 0; row < _textMesh.textInfo.lineCount; row++)
            {
                List<Vector2> rowPositions = new List<Vector2>();

                int firstCharacterIndex = _textMesh.textInfo.lineInfo[row].firstCharacterIndex;
                for (int column = 0; column < _textMesh.textInfo.lineInfo[row].characterCount; column++)
                {
                    TMP_CharacterInfo characterInfo = _textMesh.textInfo.characterInfo[firstCharacterIndex + column];
                    Vector2 characterCenter = ((characterInfo.topLeft + characterInfo.bottomRight) / 2) + _textMesh.transform.position;

                    rowPositions.Add(characterCenter);
                }

                _textMeshCharacterPositions.Add(rowPositions);
            }

            // Empty monitor
            _mainLayer.textGrid.Reset();
        }

        // Layer functions
        public Layer NewLayer()
        {
            Layer newLayer = new Layer(RowAmount, ColumnAmount);
            _layers.Add(newLayer);
            return newLayer;
        }

        public void DeleteLayer(Layer layer)
        {
            _layers.Remove(layer);
        }

        // Render functions
        private void Render()
        {
            CombineLayers();
            AssembleGridIntoText();
            RenderTextToMesh();
        }

        private void CombineLayers()
        {
            List<Layer> sortedLayers = _layers.OrderBy(layer => layer.zIndex).ToList();
            foreach (Layer layer in sortedLayers)
            {
                View view = layer.RenderView();

                for (int row = 0; row < view.size.y; row++)
                {
                    for (int column = 0; column < view.size.x; column++)
                    {
                        _textGrid[view.monitorPosition.y + row, view.monitorPosition.x + column] = view.textGrid[row, column];
                    }
                }
            }
        }

        private void AssembleGridIntoText()
        {
            _text = "";

            for (int y = 0; y < RowAmount; y++)
            {
                _text += new string(_textGrid[y]);
                _text += "\n";
            }
        }

        private void RenderTextToMesh()
        {
            _textMesh.SetText(_text);
        }

        // UI Cursor
        /// <summary>
        /// Select a row on the Layer using the UI cursor.
        /// </summary>
        /// <param name="row">The index of the row to be selected</param>
        public void SelectRow(int row)
        {
            // Mesh info is probably only loaded at the text end of the frame.
            // So now we force an update so we can use the mesh data.
            Render();
            _textMesh.ForceMeshUpdate();

            if (Tools.CheckError((row >= _textMesh.textInfo.lineCount), string.Format("Cannot acces row with index {0}, there are {1} lines.", row, _textMesh.textInfo.lineCount))) return;

            // Retrieve character data
            TMP_LineInfo lineInfo = _textMesh.textInfo.lineInfo[row];
            TMP_CharacterInfo characterInfoBegin = _textMesh.textInfo.characterInfo[lineInfo.firstCharacterIndex];
            TMP_CharacterInfo characterInfoFinal = _textMesh.textInfo.characterInfo[lineInfo.lastCharacterIndex];

            Vector2 newPositionCenter = ((characterInfoBegin.topLeft + characterInfoFinal.bottomRight) / 2) + _textMesh.transform.position;
            Vector2 newPositionOffset = new Vector2(-1, -1);
            Vector2 newSize = new Vector2(uiCursor.characterSize.x * ColumnAmount, uiCursor.characterSize.y);

            uiCursor.SetSize(newSize);
            uiCursor.SetPositionCenter(newPositionCenter + newPositionOffset);
            Debug.LogWarning("Here");
        }

        /// <summary>
        /// Update the UI cursor position to its linked cursor.
        /// </summary>
        private void UpdateUICursorPosition()
        {
            uiCursor.ResetSize();
            Cursor linkedCursor = uiCursor.linkedLayer.cursor;
            Vector2 newPosition = _textMeshCharacterPositions[linkedCursor.y][linkedCursor.x];
            uiCursor.SetPositionCenter(newPosition);
        }
    }
}