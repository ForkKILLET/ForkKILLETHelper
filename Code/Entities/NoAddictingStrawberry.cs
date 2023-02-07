using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using MonoMod.Utils;
using System.Collections;

namespace Celeste.Mod.ForkKILLETHelper.Code.Entities {
    // Lots of code is from ShroomHelper:
    // https://github.com/CommunalHelper/ShroomHelper/blob/dev/Code/Entities/OneDashWingedStrawberry.cs

    [Tracked(true)]
    [CustomEntity("ForkKILLETHelper/NoAddictingStrawberry")]
    public class NoAddictingStrawberry : Strawberry {
        protected readonly DynamicData BaseData;
        public bool GetFlag;
        public float CollectTimer = 0f;

        public NoAddictingStrawberry(EntityData data, Vector2 offset, EntityID gid)
            : base(data, offset, gid) {
            BaseData = new DynamicData(typeof(Strawberry), this);
            BaseData.Set("Winged", true);
            BaseData.Set("Golden", true);
            Remove(Get<DashListener>());
        }

        public override void Update() {
            base.Update();

            Logger.Log("ForkKILLET", $"addicted {ForkKILLETHelperModule.Session.addicted}");
            if (! BaseData.Get<bool>("flyingAway") && ForkKILLETHelperModule.Session.addicted) {
                Depth = -1000000;
                Add(new Coroutine(FlyAwayRoutine()));
                BaseData.Set("flyingAway", true);
            }

            if (BaseData.Get<bool>("collected")) return;

            if (Follower.Leader?.Entity is Player { Dead: false }) {
                GetFlag = true;
            }
            if (!GetFlag) return;

            CollectTimer += Engine.DeltaTime;
            if (CollectTimer > 0.5f) {
                OnCollect();
            }
        }

        private IEnumerator FlyAwayRoutine() {
            BaseData.Get<Wiggler>("rotateWiggler").Start();
            BaseData.Set("flapSpeed", -200f);
            var tween = Tween.Create(Tween.TweenMode.Oneshot, Ease.CubeOut, 0.25f, start: true);
            tween.OnUpdate = delegate (Tween t) {
                BaseData.Set("flapSpeed", MathHelper.Lerp(-200f, 0f, t.Eased));
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
                BaseData.Set("flapSpeed", MathHelper.Lerp(0f, -200f, t.Eased));
            };
            Add(tween);
        }
    }
}
