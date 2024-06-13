using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Menu
{
    public class LevelPreviewView : MonoBehaviour
    {
        [SerializeField] private Image loadingImage;
        [SerializeField] private TextMeshProUGUI unknownErrorLabel;
        [SerializeField] private RawImage previewImage;
        [SerializeField] private TextMeshProUGUI progressLabel;
        [SerializeField] private TextMeshProUGUI levelNameLabel;
        [SerializeField] private Image completedMarkImage;

        [field: SerializeField] public Button Button { get; private set; }
        
        public void Initialize(Texture2D texture, string levelName)
        {
            previewImage.texture = texture;
            levelNameLabel.text = levelName;
        }

        public void RefreshProgress(int counter)
        {
            progressLabel.text = counter.ToString();
        }

        public void SetCompleted()
        {
            completedMarkImage.gameObject.SetActive(true);
        }

        public void ShowLoadingIndicator()
        {
            var loadingImageTransform = loadingImage.transform;
            
            loadingImageTransform.gameObject.SetActive(true);
            loadingImageTransform.DORotate(Vector3.back * 90f, 0.3f).SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental).SetId(loadingImageTransform);
        }

        public void HideLoadingIndicator()
        {
            var loadingImageTransform = loadingImage.transform;
            DOTween.Kill(loadingImageTransform);
            
            loadingImageTransform.gameObject.SetActive(false);
        }

        public void ShowUnknownErrorLabel()
        {
            unknownErrorLabel.gameObject.SetActive(true);
        }

        public class Factory : PlaceholderFactory<LevelPreviewView>
        {
            
        }
    }
}