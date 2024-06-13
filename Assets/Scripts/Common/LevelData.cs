using UniRx;
using UnityEngine;

namespace Common
{
    public class LevelData
    {
        public int id;
        public Texture2D texture;
        public string imageName;
        public IntReactiveProperty counter;
    }
}