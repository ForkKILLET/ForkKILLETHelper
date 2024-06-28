using System;
using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.Utils;

namespace Celeste.Mod.ForkKILLETHelper.Entities;

[Tracked(true)]
[CustomEntity("ForkKILLETHelper/AddictingRefill")]
public class AddictingRefill : Refill {
    private readonly DynamicData baseData;

    private readonly float respawnTime;

    private readonly int refillDashNum;

    public AddictingRefill(EntityData data, Vector2 offset): base(data, offset) {
        baseData = new(typeof(Refill), this);
        
        oneUse = data.Bool("oneUse", false);
        respawnTime = data.Float("respawnTime", 2.5f);
        refillDashNum = twoDashes ? 2 : 1;

        var sprite = new Sprite(GFX.Game, "objects/ForkKILLETHelper/addictingRefill/idle");
        sprite.AddLoop("idle", "", 0.1f);
        sprite.Play("idle");
        sprite.CenterOrigin();
        Remove(baseData.Get<Sprite>("sprite"));
        Add(sprite);
        baseData.Set("sprite", sprite);

        Get<PlayerCollider>().OnCollide = OnPlayer;
    }

    public new void OnPlayer(Player player) {
        if (
            player.Dashes < Math.Min(player.MaxDashes, refillDashNum) ||
            player.Stamina < 20f ||
            player.Get<AddictingComponent>() != null
        ) {
            player.RefillDash();
            player.RefillStamina();
            AddictBase.MakePlayerAddicted(player, SceneAs<Level>());

            Audio.Play(twoDashes ? "event:/new_content/game/10_farewell/pinkdiamond_touch" : "event:/game/general/diamond_touch", Position);
            Input.Rumble(RumbleStrength.Medium, RumbleLength.Medium);
            Collidable = false;
            Add(new Coroutine(RefillRoutine(player)));
            respawnTimer = respawnTime;
        }
    }
}
