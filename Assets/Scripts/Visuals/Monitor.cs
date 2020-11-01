using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Visuals
{
    public class Monitor : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMesh = null;
        public static readonly GridSize Size = new GridSize(24, 80);

        public UICursor uiCursor;

        private List<Layer> _layers;
        private Layer _mainLayer;

        private TextGrid _textGrid;
        private string _text = "";

        private void Awake()
        {
            if (Tools.CheckError(_textMesh == null, "No TextMeshPro object has been added")) return;

            _mainLayer = new Layer(Size);
            _layers = new List<Layer>();

            _textGrid = new TextGrid(Size);

            CalibrateTextMesh();
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
            var textMeshCharacterPositions = new List<List<Vector2>>();

            for (var row = 0; row < _textMesh.textInfo.lineCount; row++)
            {
                var rowPositions = new List<Vector2>();

                int firstCharacterIndex = _textMesh.textInfo.lineInfo[row].firstCharacterIndex;
                for (var column = 0; column < _textMesh.textInfo.lineInfo[row].characterCount; column++)
                {
                    TMP_CharacterInfo characterInfo = _textMesh.textInfo.characterInfo[firstCharacterIndex + column];
                    Vector2 offset = _textMesh.transform.position;
                    Vector2 characterCenter = (characterInfo.topLeft + characterInfo.bottomRight) / 2;
                    characterCenter += offset;

                    rowPositions.Add(characterCenter);
                }

                textMeshCharacterPositions.Add(rowPositions);
            }

            uiCursor.textMeshCharacterPositions = textMeshCharacterPositions;

            // Empty monitor
            _mainLayer.textGrid.Reset();
        }

        // Layer functions
        public Layer NewLayer()
        {
            var newLayer = new Layer(Size);
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
            if (!LayersHaveChanged()) return;

            CombineLayers();
            AssembleGridIntoText();
            RenderTextToMesh();
        }

        private bool LayersHaveChanged()
        {
            var changed = _layers.Select(layer => layer.HasChanged()).ToList();
            return changed.Contains(true);
        }

        private void CombineLayers()
        {
            var sortedLayers = _layers.OrderBy(layer => layer.zIndex).ToList();
            foreach (Layer layer in sortedLayers)
            {
                View view = layer.RenderView();

                for (var row = 0; row < view.size.rows; row++)
                {
                    for (var column = 0; column < view.size.columns; column++)
                    {
                        int monitorPositionRow = view.monitorPosition.row + row;
                        int monitorPositionColumn = view.monitorPosition.column + column;

                        _textGrid[monitorPositionRow, monitorPositionColumn] = view.textGrid[row, column];
                    }
                }

                layer.Change(false);
                layer.view.Change(false);
            }
        }

        private void AssembleGridIntoText()
        {
            _text = "";

            for (var y = 0; y < Size.rows; y++)
            {
                _text += new string(_textGrid[y]);
                _text += "\n";
            }
        }

        private void RenderTextToMesh()
        {
            _textMesh.SetText(_text);
        }
    }
}