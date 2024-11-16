using UnityEngine;
using System.Collections;

public class Grib : MonoBehaviour 
{
    public float _yLift = 2.1f;
    public string _gribTag = "grib1"; 
    private bool _isBouncing;
    private bool _hasBounced;
    private Transform _grib1;

    private void Start() 
    {
        _hasBounced = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Player") && !_isBouncing && !_hasBounced) 
        {
            foreach (ContactPoint2D contact in collision.contacts) 
            {
                if (contact.normal.y > 0.5f) 
                {
                    _isBouncing = true;
                    _hasBounced = true;
                    _grib1 = GameObject.FindGameObjectWithTag(_gribTag).transform;
                    if (_grib1 != null) 
                    {
                        StartCoroutine(MoveAndEnable(_grib1));
                    }
                }
            }
        }
    }

    private IEnumerator MoveAndEnable(Transform _grib1) 
    {
        Vector3 _targetPositionY = _grib1.position + new Vector3(0, _yLift, 0);
        float _elapsedTime = 0f;
        float _duration = 1f;

        while (_elapsedTime < _duration) 
        {
            _grib1.position = Vector3.Lerp(_grib1.position, _targetPositionY, (_elapsedTime / _duration));
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        _grib1.position = _targetPositionY;

        GribMovement _gribMovement = _grib1.GetComponent<GribMovement>();
        if (_gribMovement != null) 
        {
            _gribMovement.EnableMovement();
        }
    }
}
