using Core.Resources;
using JoyWay.Game.Character;
using JoyWay.Game.Projectiles;
namespace Core.Services
{

    public class AssetContainer
    {
        public readonly LazyResource<CharacterContainer> Character =
            new LazyResource<CharacterContainer>(Constants.Character);

        public readonly LazyResource<Projectile> Fireball =
            new LazyResource<Projectile>(Constants.Fireball);

        public readonly LazyResource<CharacterConfig> CharacterConfig =
            new LazyResource<CharacterConfig>(Constants.CharacterConfig);
    }
}