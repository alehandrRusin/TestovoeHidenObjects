using UnityEngine;
using UnityEngine.UI;

namespace Menu.UI
{
    public class ConnectionProblemPopupView : MonoBehaviour
    {
        [field: SerializeField] public Button RetryConnectButton { get; private set; }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}