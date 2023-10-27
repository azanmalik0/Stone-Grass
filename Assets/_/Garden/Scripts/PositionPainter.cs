using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Es.InkPainter;

namespace PT.Garden
{
    public class PositionPainter : MonoBehaviour
    {
        [SerializeField] private Brush _removeBrush, _bendBrush;
        public InkCanvas _bendCanvas, _removeCanvas;
        public bool _isPainting = false;
        public LayerMask lm;

        private void Update(){

            if (_isPainting){

                

                _bendCanvas.Paint(
                    _bendBrush,
                    new Vector3(
                        transform.position.x,
                        _bendCanvas.transform.position.y,
                        transform.position.z
                    ), null, null
                );

                _removeCanvas.Paint(
                    _removeBrush,
                    new Vector3(
                        transform.position.x,
                        _removeCanvas.transform.position.y,
                        transform.position.z
                    )
                );
            }
        }
    }
}