using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class GameScreenView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI levelNameLabel;
        [SerializeField] private TextMeshProUGUI progressLabel;
        
        [field: SerializeField] public Button ExitToMenuButton { get; private set; }

        public void SetLevelName(string levelName)
        {
            levelNameLabel.text = levelName;
        }

        public void RefreshProgress(int counter)
        {
            progressLabel.text = counter.ToString();
        }
    }
}