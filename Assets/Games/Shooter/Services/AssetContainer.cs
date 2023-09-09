using JoyWay.Core.Resources;
using JoyWay.Games.Shooter.Character;
using JoyWay.Games.Shooter.Projectiles;
using JoyWay.Games.Shooter.StaticData;
namespace JoyWay.Games.Shooter.Services
{

    public class AssetContainer
    {
        public readonly LazyResource<CharacterContainer> Character =
            new LazyResource<CharacterContainer>(Constants.Character);

        public readonly LazyResource<CharacterConfig> CharacterConfig =
            new LazyResource<CharacterConfig>(Constants.CharacterConfig);

        public readonly LazyResource<Projectile> Fireball =
            new LazyResource<Projectile>(Constants.Fireball);
    }
}
