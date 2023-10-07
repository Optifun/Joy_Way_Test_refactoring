using JoyWay.Core.Resources;
using JoyWay.UI;
namespace JoyWay.Services
{
    public class UIAssetContainer
    {
        public readonly LazyResource<MainMenuUI> MainMenuUI =
            new LazyResource<MainMenuUI>(CoreResources.MainMenuUI);
        public readonly LazyResource<HideableUI> CrosshairUI =
            new LazyResource<HideableUI>(CoreResources.CrosshairUI);
    }
}
