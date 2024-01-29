using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Translator : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private float _animationDuration = 1;
    [SerializeField] private float _currentTime = 0;

    // public enum AnimationMode { Lineal, curve}
    //[SerializeField] private AnimationMode _animationMode = AnimationMode.Lineal;
    [SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0,0,1,1);

    [Header("Translations")]
    [SerializeField] private Vector3 _desplacement = Vector3.zero;
    private Vector3 _originPosition;

    [SerializeField] private Vector3 _Rotation = Vector3.zero;
    private Quaternion _originRotation;

    private IEnumerator _currentAnimator;

    [Header("Animation Triggers")]
    public UnityEvent OnOrigintReach;
    public UnityEvent OnTargetReach;
    public UnityEvent<float> OnChange;
    private void Awake()
    {
        _originPosition = transform.localPosition;
        _originRotation = transform.localRotation;
    }

    public void ToOrigin()
    {
        ChangeAnumation(ToOriginAnimator());

    }


    public void ToTarget()
    {
        ChangeAnumation(ToTargetAnimator());
    }

    private void ChangeAnumation(IEnumerator newAnimation)
    {

        if (_currentAnimator != null)
        {
            StopCoroutine(_currentAnimator);
        }
        _currentAnimator = newAnimation;
        StartCoroutine(_currentAnimator);
    }

    private IEnumerator ToOriginAnimator()
    {
        while (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;

            SetPositionForCurrentTime();

            yield return new WaitForEndOfFrame();
        }
        _currentTime = 0;
        SetPositionForCurrentTime();


        _currentAnimator = null;

        OnOrigintReach.Invoke();
    }

    private IEnumerator ToTargetAnimator() 
    {
        while (_currentTime < _animationDuration)
        {
            _currentTime += Time.deltaTime;

            SetPositionForCurrentTime();

            yield return new WaitForEndOfFrame();
        }
        _currentTime = _animationDuration;
        SetPositionForCurrentTime();


        _currentAnimator = null;
        OnTargetReach.Invoke();
    }



    private void SetPositionForCurrentTime()
    {
        float interpolateValue = _currentTime / _animationDuration;
/*
        switch (_animationMode)
        {
            case AnimationMode.Lineal:
                break;
            case AnimationMode.curve:
                interpolateValue = Mathf.Sin(interpolateValue);
                break;
        }

*/

        interpolateValue = _curve.Evaluate(interpolateValue);
        transform.localPosition = _originPosition + (_desplacement * interpolateValue);
        Vector3 newRotation = _Rotation * interpolateValue;
        transform.localRotation = _originRotation * Quaternion.Euler(newRotation.x , newRotation.y , newRotation.z );

        OnChange.Invoke(interpolateValue);
    }
}