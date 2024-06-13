using UnityEngine;

namespace Gameplay
{
    public class GameplayLevelView : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
        [field: SerializeField] public BoxCollider2D BoxCollider { get; private set; }

        public void SetSprite(Sprite sprite)
        {
            SpriteRenderer.sprite = sprite;
            
            var bounds = sprite.bounds;
            BoxCollider.size = bounds.size;
        }
    }
}