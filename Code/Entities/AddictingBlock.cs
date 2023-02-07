using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Linq;

namespace Celeste.Mod.ForkKILLETHelper.Code.Entities {
    [Tracked(true)]
    [CustomEntity("ForkKILLETHelper/AddictingBlock")]
    public class AddictingBlock : Solid {
        public AddictingBlock(Vector2 position, float width, float height, int triggerMethod, bool safe)
            : base(position, width, height, safe) {
            StandToTrigger = (triggerMethod & 0b01) > 0;
            ClimbToTrigger = (triggerMethod & 0b10) > 0;
        }

        public AddictingBlock(EntityData data, Vector2 offset)
            : this(data.Position + offset, data.Width, data.Height, data.Int("triggerMethod", 1), false) {}

        public bool Triggered = false;
        public readonly bool StandToTrigger;
        public readonly bool ClimbToTrigger;

        public override void Render() {
            var color = Color.HotPink;

            if (Triggered) {
                var level = SceneAs<Level>();
                var player = level.Tracker.GetEntity<Player>();
                var addictingComponent = player?.Get<AddictingComponent>();

                if (addictingComponent != null) {
                    float greenAndBlue = 1f - (float) addictingComponent.TimeToDie / addictingComponent.MaxTimeToDie;
                    color = new Color(255, greenAndBlue, greenAndBlue);
                }
            }

            // Top
            Draw.Line(Position + new Vector2(1, 0), Position + new Vector2(Width, 0), StandToTrigger ? color : Color.AliceBlue);

            // Left & right
            Draw.Line(Position + new Vector2(1, 0), Position + new Vector2(1, Height - 1), ClimbToTrigger ? color : Color.AliceBlue);
            Draw.Line(Position + new Vector2(Width, 0), Position + new Vector2(Width, Height - 1), ClimbToTrigger ? color : Color.AliceBlue);

            // Bottom
            Draw.Line(Position + new Vector2(0, Height - 1), Position + new Vector2(Width, Height - 1), Color.AliceBlue);
        }

        public void TriggerByPlayer(Player player) {
            ForkKILLETHelperModule.Session.addicted = true;

            var component = player.Get<AddictingComponent>();
            if (component == null) {
                var addictingComponent = new AddictingComponent(1.5f);
                player.Add(addictingComponent);
                var transitionListener = new TransitionListener {
                    OnOutBegin = () => {
                        player.Remove(addictingComponent);
                    }
                };
                player.Add(transitionListener);
            }
            else {
                component.TimeToDie = 1;
            }

            if (Triggered) return; // If self is not triggered, set all blocks triggered.

            Level level = SceneAs<Level>();
            var addictingBlocks = level.Tracker.GetEntities<AddictingBlock>().Cast<AddictingBlock>();
            foreach (var block in addictingBlocks) {
                block.Triggered = true;
            }
        }

        public override void Update() {
            if (StandToTrigger && HasPlayerOnTop()) {
                Player player = GetPlayerOnTop();
                TriggerByPlayer(player);
            }
            if (ClimbToTrigger && HasPlayerClimbing()) {
                Player player = GetPlayerClimbing();
                TriggerByPlayer(player);
            }
        }
    }

    public class AddictingComponent : Component {
        public AddictingComponent(float maxTimeToDie) : base(true, false) {
            TimeToDie = maxTimeToDie;
            MaxTimeToDie = maxTimeToDie;
        }

        public float MaxTimeToDie { get; }

        public float TimeToDie;

        public override void Update() {
            TimeToDie -= Engine.DeltaTime;
            if (TimeToDie < 0) {
                Level level = SceneAs<Level>();
                var player = level.Tracker.GetEntity<Player>();
                player.Die(new Vector2(0, 0));
            }
        }
    }
}