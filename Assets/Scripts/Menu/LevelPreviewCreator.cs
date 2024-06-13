using Common.Request;
using Menu.UI;
using Zenject;

namespace Menu
{
    public class LevelPreviewCreator
    {
        [Inject] private LevelPreviewView.Factory _previewViewFactory;
        [Inject] private LevelPreviewController.Factory _previewControllerFactory;
        [Inject] private MenuScreenView _menuScreenView;
        
        public LevelPreviewController CreatePreviewController(RequestLevelData requestLevelData)
        {
            var previewView = _previewViewFactory.Create();
            previewView.transform.SetParent(_menuScreenView.GridLayoutGroup.transform, false);
            
            var previewController = _previewControllerFactory.Create(previewView, requestLevelData);
            
            return previewController;
        }
    }
}