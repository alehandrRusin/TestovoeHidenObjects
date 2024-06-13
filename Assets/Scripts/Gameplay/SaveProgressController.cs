using System;
using Common;
using Common.Database;
using UniRx;
using Zenject;

namespace Gameplay
{
    public class SaveProgressController : IInitializable, IDisposable
    {
        [Inject] private LevelData _levelData;
        [Inject] private DataBase _dataBase;
        
        private IDisposable _disposable;
        
        public void Initialize()
        {
            SaveOnPause();
        }

        private void SaveOnPause()
        {
            _disposable = Observable.EveryApplicationPause().Subscribe(_ =>
            {
                SaveData();
            });
        }

        private void SaveData()
        {
            _dataBase.SaveData(_levelData);
        }

        public void Dispose()
        {
            SaveData();
            _disposable?.Dispose();
        }
    }
}