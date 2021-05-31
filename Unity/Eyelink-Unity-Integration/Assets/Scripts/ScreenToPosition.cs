using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenToPosition : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;

    public Vector2 PupilPosition;
    public Vector2 PupilOffset;

    private Vector3 _screenPosition;

    IsGazeable _lastHit;

    bool _toggleGaze;

    private void Start()
    {
        _screenPosition = Vector3.zero;
        PupilPosition = Vector2.zero;
        PupilOffset = Vector2.zero;
        _toggleGaze = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PupilOffset = PupilPosition;
            PupilOffset.x = Screen.width / 2 - PupilOffset.x;
            PupilOffset.y = Screen.height / 2 - PupilOffset.y;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            _toggleGaze = !_toggleGaze;

            if (!_toggleGaze) this.transform.position = Vector3.zero;

        }

        //mousePos.x = Input.mousePosition.x;
        //mousePos.y = Input.mousePosition.y;

        _screenPosition.x = (PupilPosition.x + PupilOffset.x);
        _screenPosition.y = Screen.height - (PupilPosition.y+PupilOffset.y);
        _screenPosition.z = _mainCamera.nearClipPlane;

        RaycastHit hit;
        Ray ray = _mainCamera.ScreenPointToRay(new Vector2(_screenPosition.x,_screenPosition.y));

        if (Physics.Raycast(ray, out hit))
        {

            if (_lastHit != null)
            {
                if (_lastHit.transform != hit.transform)
                {
                    _lastHit.MyMaterial.color = Color.white;
                }
            }

            if (hit.transform.gameObject.GetComponent<IsGazeable>() != null)
            {
                IsGazeable _gazeable = hit.transform.gameObject.GetComponent<IsGazeable>();
                _gazeable.MyMaterial.color = Color.green;
                _lastHit = _gazeable;
            }
        }
        else
        {
            if (_lastHit != null)
            {
                _lastHit.MyMaterial.color = Color.white;
                _lastHit = null;
            }
        }
        if (_toggleGaze)
        {
            this.transform.position = _mainCamera.ScreenToWorldPoint(_screenPosition);
        }
    }
}
