using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    /// <summary>
    /// Simple UI to show detection progress
    /// </summary>
    public class DetectionUi : MonoBehaviour
    {
        [SerializeField]
        private Image detectionProgressImage;
    
        [SerializeField]
        private TMP_Text detectedText;

        private Player _player;
    
        private void Start()
        {
            var players = FindObjectsByType<Player>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            if (players.Length != 1)
            {
                throw new Exception($"Expected one and only {nameof(Player)} on scene.");
            }

            _player = players[0];
            _player.OnDetectedChanged += OnPlayerDetectedChanged;
        
            detectedText.enabled = false;
            detectionProgressImage.fillAmount = _player.DetectionProgress;
        }

        private void OnPlayerDetectedChanged(float detectionPercent)
        {
            detectionProgressImage.fillAmount = detectionPercent;

            if (detectionPercent >= 1)
            {
                detectedText.enabled = true;
            }
        }
    }
}