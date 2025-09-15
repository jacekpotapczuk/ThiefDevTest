using System;
using System.Collections;
using Interaction;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Main Class to aggregate Player related functionality.
 * Expected to be only one at a level.
 */
[RequireComponent(typeof(SimpleCharacterController), typeof(PlayerObjectHolder), typeof(InteractionController))]
public class Player : MonoBehaviour
{
    public event Action<float> OnDetectedChanged;
    
    public PlayerObjectHolder ObjectHolder { get; private set; }

    public InteractionController InteractionController { get; private set; }

    [SerializeField] 
    private float maxDetectionTime = 2f;

    private float _detectedTime = 0;

    private bool _isDetected = false;

    public float DetectionProgress => _detectedTime / maxDetectionTime;
    
    private void Start()
    {
        
        ObjectHolder = GetComponent<PlayerObjectHolder>();
        InteractionController = GetComponent<InteractionController>();
    }

    private void Update()
    {
        if (!_isDetected)
        {
            return;
        }
        
        _detectedTime += Time.deltaTime;
        OnDetectedChanged?.Invoke(_detectedTime / maxDetectionTime);

        // very simple game over
        if (_detectedTime > maxDetectionTime)
        {
            StartCoroutine(RestartAfterDelay(2f));
        }
    }

    IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MarkDetected()
    {
        _isDetected = true;
    }

    public void MarkUnDetected()
    {
        _isDetected = false;
    }
}