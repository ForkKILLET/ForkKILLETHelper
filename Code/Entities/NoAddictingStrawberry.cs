using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.Utils;
using System.Collections;

namespace Celeste.Mod.ForkKILLETHelper.Entities {
    // Lots of code is from ShroomHelper:
    // https://github.com/CommunalHelper/ShroomHelper/blob/dev/Code/Entities/OneDashWingedStrawberry.cs

    [Tracked(true)]
    [CustomEntity("ForkKILLETHelper/NoAddictingStrawberry")]
    public class NoAddictingStrawberry : Strawberry {
        protected DynamicData baseData;
        public bool getFlag;
        public float collectTimer = 0f;

        public NoAddictingStrawberry(EntityData data, Vector2 offset, EntityID gid)
            : base(data, offset, gid) {
            baseData = new DynamicData(typeof(Strawberry), this);
            baseData.Set("Winged", true);
            baseData.Set("Golden", true);
            Remove(Get<DashListener>());
        }

        public override void Update() {
            base.Update();

            Logger.Log("ForkKILLET", $"addicted {ForkKILLETHelperModule.Session.addicted}");
            if (! baseData.Get<bool>("flyingAway") && ForkKILLETHelperModule.Session.addicted) {
                Depth = -1000000;
                Add(new Coroutine(FlyAwayRoutine()));
                baseData.Set("flyingAway", true);
            }

            if (! baseData.Get<bool>("collected")) {
                if (Follower.Leader?.Entity is Player leaderPlayer && !leaderPlayer.Dead) {
                    getFlag = true;
                }

                if (getFlag) {
                    collectTimer += Engine.DeltaTime;
                    if (collectTimer > 0.5f) {
                        OnCollect();
                    }
                }
            }
        }

        private IEnumerator FlyAwayRoutine() {
            baseData.Get<Wiggler>("rotateWiggler").Start();
            baseData.Set("flapSpeed", -200f);
            Tween tween = Tween.Create(Tween.TweenMode.Oneshot, Ease.CubeOut, 0.25f, start: true);
            tween.OnUpdate = delegate (Tween t) {
                baseData.Set("flapSpeed", MathHelper.Lerp(-200f, 0f, t.Eased));
            };
            Add(tween);
            yield return 0.1f;
            Audio.Play("event:/game/general/strawberry_laugh", Position);
            yield return 0.2f;
            if (!Follower.HasLeader) {
                Audio.Play("event:/game/general/strawberry_flyaway", Position);
            }

            tween = Tween.Create(Tween.TweenMode.Oneshot, null, 0.5f, start: true);
            tween.OnUpdate = delegate (Tween t) {
                baseData.Set("flapSpeed", MathHelper.Lerp(0f, -200f, t.Eased));
            };
            Add(tween);
        }
    }
}
