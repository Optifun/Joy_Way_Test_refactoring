using Core.Resources;
using JoyWay.UI;
namespace Core.Services
{
    public class UIAssetContainer
    {
        public readonly LazyResource<MainMenuUI> MainMenuUI =
            new LazyResource<MainMenuUI>(Constants.MainMenuUI);
        public readonly LazyResource<HideableUI> CrosshairUI =
            new LazyResource<HideableUI>(Constants.CrosshairUI);
    }
}
