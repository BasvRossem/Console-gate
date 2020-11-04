using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Visuals
{
    /// <summary>
    /// Monitor object which renders its layers to the text mesh.
    /// </summary>
    public class Monitor : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMesh = null;
        public static readonly GridSize Size = new GridSize(24, 80);

        public UICursor uiCursor;

        private List<Layer> _layers;
        private Layer _mainLayer;

        private TextGrid _textGrid;
        private string _text = "";

        private void Awake()
        {
            if (Tools.CheckError(textMesh == null, "No TextMeshPro object has been added")) return;
            if (Tools.CheckError(uiCursor == null, "No UICursor object has been added")) return;

            _mainLayer = new Layer(Size);
            _layers = new List<Layer>();

            _textGrid = new TextGrid(Size);

            CalibrateTextMesh();
        }

        private void Update()
        {
            Render();
        }

        /// <summary>
        /// Calibrate the positions of the characters of the text mesh to be used by the UICursor.
        /// </summary>
        public void CalibrateTextMesh()
        {
            // Fill text grid with data
            _textGrid.Fill('*');

            // Render data to text mesh
            AssembleGridIntoText();
            RenderTextToMesh();
            
            textMesh.ForceMeshUpdate();
            
            // Calculate character center positions
            var textMeshCharacterPositions = new List<List<Vector2>>();

            for (var row = 0; row < textMesh.textInfo.lineCount; row++)
            {
                var rowPositions = new List<Vector2>();

                int firstCharacterIndex = textMesh.textInfo.lineInfo[row].firstCharacterIndex;
                for (var column = 0; column < textMesh.textInfo.lineInfo[row].characterCount; column++)
                {
                    TMP_CharacterInfo characterInfo = textMesh.textInfo.characterInfo[firstCharacterIndex + column];
                    Vector2 offset = textMesh.transform.position;
                    Vector2 characterCenter = (characterInfo.topLeft + characterInfo.bottomRight) / 2;
                    characterCenter += offset;

                    rowPositions.Add(characterCenter);
                }

                textMeshCharacterPositions.Add(rowPositions);
            }

            uiCursor.textMeshCharacterPositions = textMeshCharacterPositions;

            // Empty monitor
            _textGrid.Reset();
        }

        // Layer functions
        /// <summary>
        /// Create a new layer to be rendered.
        /// </summary>
        /// <param name="render">If the layer should be rendered from creation.</param>
        /// <returns>The newly created layer.</returns>
        public Layer NewLayer(bool render = true)
        {
            var newLayer = new Layer(Size);
            if(render) _layers.Add(newLayer);
            return newLayer;
        }

        /// <summary>
        /// Delete the layer from the list of layers to be rendered.
        /// </summary>
        /// <remarks>The user of the layer is responsible for deleting the layer from the memory by not using it.</remarks>
        /// <param name="layer">The layer to remove.</param>
        public void DeleteLayer(Layer layer)
        {
            _layers.Remove(layer);
        }
        
        /// <summary>
        /// Add an existing layer to the monitor to render.
        /// </summary>
        /// <param name="layer">The layer to render.</param>
        public void AddLayer(Layer layer)
        {
            _layers.Add(layer);
        }
        
        // Render functions
        /// <summary>
        /// Render the layers if they have changed.
        /// </summary>
        private void Render()
        {
            if (_layers.Count <= 0) return;
            if (!LayersHaveChanged()) return;
            
            CombineLayers();
            AssembleGridIntoText();
            RenderTextToMesh();
        }

        /// <summary>
        /// Check if any of the layers have changed.
        /// </summary>
        /// <returns>A boolean value indicating if any of the layers have changed.</returns>
        private bool LayersHaveChanged()
        {
            var changed = _layers.Select(layer => layer.HasChanged()).ToList();
            return changed.Contains(true);
        }

        /// <summary>
        /// Combines the layers using their views onto its own TextGrid.
        /// </summary>
        private void CombineLayers()
        {
            var sortedLayers = _layers.OrderBy(layer => layer.zIndex).ToList();
            _textGrid.Reset();
            foreach (Layer layer in sortedLayers)
            {
                View view = layer.RenderView();

                for (var row = 0; row < view.size.rows; row++)
                {
                    for (var column = 0; column < view.size.columns; column++)
                    {
                        int monitorPositionRow = view.externalPosition.row + row;
                        int monitorPositionColumn = view.externalPosition.column + column;
                        
                        if(monitorPositionRow >= _textGrid.GetSize().rows) continue;
                        if(monitorPositionColumn >= _textGrid.GetSize().columns) continue;
                        
                        _textGrid[monitorPositionRow, monitorPositionColumn] = view.textGrid[row, column];
                    }
                }

                layer.Change(false);
                layer.view.Change(false);
            }
        }

        /// <summary>
        /// Assemble the text grid into a string.
        /// </summary>
        private void AssembleGridIntoText()
        {
            _text = "";

            for (var y = 0; y < Size.rows; y++)
            {
                _text += new string(_textGrid[y]);
                _text += "\n";
            }
        }

        /// <summary>
        /// Render the assembled text to the text mesh.
        /// </summary>
        private void RenderTextToMesh()
        {
            textMesh.SetText(_text);
        }
    }
}