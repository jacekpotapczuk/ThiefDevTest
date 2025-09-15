using System;
using TMPro;
using UnityEngine;

namespace Ui
{
    /// <summary>
    /// Ui to show theft progress. 
    /// </summary>
    public class TheftProgressUi : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text progressText;

        private TheftArea _theftArea;
        private void Start()
        {
            var theftAreas = FindObjectsByType<TheftArea>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            if (theftAreas.Length != 1)
            {
                throw new Exception($"Expected one and only {nameof(TheftArea)} on scene.");
            }

            _theftArea = theftAreas[0];
        }

        private void Update()
        {
            progressText.text = $"Objects to steal: {_theftArea.CurrentObjectCount}";
        }
    }
}