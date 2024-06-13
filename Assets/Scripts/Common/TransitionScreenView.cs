using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    public class TransitionScreenView : MonoBehaviour
    {
        [SerializeField] private Image image;
        
        public void Show(Action callback = null)
        {
            gameObject.SetActive(true);
            image.DOFade(1f, 0.3f).SetId(image).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }

        public void Hide(Action callback = null)
        {
            image.DOFade(0f, 0.3f).SetId(image).OnComplete(() =>
            {
                gameObject.SetActive(false);
                callback?.Invoke();
            });
        }
    }
}