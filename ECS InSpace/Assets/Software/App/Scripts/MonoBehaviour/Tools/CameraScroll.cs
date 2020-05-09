using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScroll : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    [SerializeField] private float ZoomOutDistance;
    [SerializeField] private Slider slider;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        _endPosition = _startPosition - transform.forward * ZoomOutDistance;
    }

    // Update is called once per frame
    void Update()
    {
      transform.position= Vector3.Lerp(_startPosition, _endPosition, slider.value);
    }
}
