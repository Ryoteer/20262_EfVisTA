using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PortalBehaviour : MonoBehaviour
{
    [Header("<color=orange>Gameplay</color>")]
    [SerializeField] private float _transitionTime = 1.0f;

    [Header("<color=orange>Rendering</color>")]
    [SerializeField] private string _activationFloatName = "_ActivationStatus";
    [SerializeField] private string _destinationTexName = "_Destination";
    [SerializeField] private Texture[] _destinationTextures;
    [SerializeField] private string _baseColorName = "_BaseColor";
    [SerializeField] private Color[] _baseColors;
    [SerializeField] private string _borderColorName = "_BorderColor";
    [SerializeField] private Color[] _borderColors;
    [SerializeField] private string _changeBoolName = "_IsChangeActive";

    private InputSystem_Actions _inputs;

    private Renderer _renderer;
    private Material _material;

    private bool _isChangeActive = false;
    private int _destinationIndex = 0;

    private void Awake()
    {
        _inputs = new InputSystem_Actions();
    }

    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _material = _renderer.material;

        _destinationTextures[0] = _material.GetTexture(_destinationTexName);
        _baseColors[0] = _material.GetColor(_baseColorName);
        _borderColors[0] = _material.GetColor(_borderColorName);
    }

    #region Input Actions
    private void OnEnable()
    {
        _inputs.Enable();

        _inputs.Player.Interact.performed += Interact;
    }

    private void OnDisable()
    {
        _inputs.Disable();

        _inputs.Player.Interact.performed -= Interact;
    }
    #endregion

    private void Interact(InputAction.CallbackContext value)
    {
        if((_destinationTextures.Length == _baseColors.Length && _destinationTextures.Length == _borderColors.Length) && !_isChangeActive)
        {
            StartCoroutine(ChangeDestination());
        }
    }

    private IEnumerator ChangeDestination()
    {
        _isChangeActive = true;

        float t = 0.0f;

        _material.SetInt(_changeBoolName, 1);

        while(t < 1.0f)
        {
            t += Time.deltaTime / _transitionTime;

            _material.SetFloat(_activationFloatName, Mathf.Lerp(1.0f, 0.0f, t));

            yield return null;
        }

        if(_destinationIndex == _destinationTextures.Length - 1)
        {
            _destinationIndex = 0;
        }
        else
        {
            _destinationIndex++;
        }

        _material.SetTexture(_destinationTexName, _destinationTextures[_destinationIndex]);
        _material.SetColor(_baseColorName, _baseColors[_destinationIndex]);
        _material.SetColor(_borderColorName, _borderColors[_destinationIndex]);

        t = 0.0f;

        while (t < 1.0f)
        {
            t += Time.deltaTime / _transitionTime;

            _material.SetFloat(_activationFloatName, Mathf.Lerp(0.0f, 1.0f, t));

            yield return null;
        }

        _material.SetInt(_changeBoolName, 0);

        _isChangeActive = false;
    }
}
