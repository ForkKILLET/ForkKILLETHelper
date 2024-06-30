using System;
using System.Reflection;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.Utils;

namespace Celeste.Mod.ForkKILLETHelper.Entities;

[Tracked(true)]
[CustomEntity("ForkKILLETHelper/AddictingRefill")]
public class AddictingRefill : Refill {
    private readonly DynamicData baseData;
    private readonly Sprite oldIdleSprite;

    private readonly float respawnTime;
    private readonly bool oneTimeSatisfication;
    private bool invalid = false;
    private readonly int refillDashNum;

    private static readonly MethodInfo refillUpdateY = typeof(Refill).GetMethod("UpdateY", BindingFlags.NonPublic | BindingFlags.Instance);

    public AddictingRefill(EntityData data, Vector2 offset): base(data, offset) {
        baseData = new(typeof(Refill), this);
        
        oneUse = data.Bool("oneUse", false);
        oneTimeSatisfication = data.Bool("oneTimeSatisfication", false);
        respawnTime = data.Float("respawnTime", 2.5f);
        refillDashNum = twoDashes ? 2 : 1;

        var sprite = new Sprite(GFX.Game, "objects/ForkKILLETHelper/addictingRefill/idle");
        sprite.AddLoop("idle", "", 0.1f);
        sprite.Play("idle");
        sprite.CenterOrigin();
        oldIdleSprite = baseData.Get<Sprite>("sprite");
        Remove(oldIdleSprite);
        Add(sprite);
        baseData.Set("sprite", sprite);

        if (!data.Bool("wave", true)) {
            SineWave sine = Get<SineWave>();
            Remove(sine);
            sine.Counter = 0f;
            refillUpdateY.Invoke(this, []);
        }

        Get<PlayerCollider>().OnCollide = OnPlayer;
    }

    public new void OnPlayer(Player player) {
        if (
            player.Dashes < Math.Min(player.MaxDashes, refillDashNum) ||
            player.Stamina < 20f ||
            ! invalid && player.Get<AddictingComponent>() != null
        ) {
            
            player.RefillDash();
            player.RefillStamina();
            if (! invalid) AddictBase.MakePlayerAddicted(player, SceneAs<Level>());

            Audio.Play(twoDashes ? "event:/new_content/game/10_farewell/pinkdiamond_touch" : "event:/game/general/diamond_touch", Position);
            Input.Rumble(RumbleStrength.Medium, RumbleLength.Medium);
            Collidable = false;
            Add(new Coroutine(RefillRoutine(player)));
            respawnTimer = respawnTime;

            if (oneTimeSatisfication) {
                Remove(baseData.Get<Sprite>("sprite"));
                Add(oldIdleSprite);
                baseData.Set("sprite", oldIdleSprite);
                invalid = true;
            }
        }
    }
}
