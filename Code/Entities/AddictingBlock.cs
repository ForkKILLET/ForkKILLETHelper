using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Linq;

namespace Celeste.Mod.ForkKILLETHelper.Entities {
    [Tracked(true)]
    [CustomEntity("ForkKILLETHelper/AddictingBlock")]
    public class AddictingBlock : Solid {
        public AddictingBlock(Vector2 position, float width, float height, int triggerMethod, bool safe)
            : base(position, width, height, safe) {
            standToTrigger = (triggerMethod & 0b01) > 0;
            climbToTrigger = (triggerMethod & 0b10) > 0;
        }

        public AddictingBlock(EntityData data, Vector2 offset)
            : this(data.Position + offset, data.Width, data.Height, data.Int("triggerMethod", 1), false) {}

        public bool triggered = false;
        public bool standToTrigger;
        public bool climbToTrigger;

        public override void Render() {
            Color color = Color.HotPink;

            if (triggered) {
                Level level = SceneAs<Level>();
                var player = level.Tracker.GetEntity<Player>();
                if (player != null) {
                    var addictingComponent = player.Get<AddictingComponent>();

                    if (addictingComponent != null) {
                        float greenAndBlue = 1f - (float) addictingComponent.timeToDie / addictingComponent.maxTimeToDie;
                        color = new Color(255, greenAndBlue, greenAndBlue);
                    }
                }
            }

            // Top
            Draw.Line(Position + new Vector2(1, 0), Position + new Vector2(Width, 0), standToTrigger ? color : Color.AliceBlue);

            // Left & right
            Draw.Line(Position + new Vector2(1, 0), Position + new Vector2(1, Height - 1), climbToTrigger ? color : Color.AliceBlue);
            Draw.Line(Position + new Vector2(Width, 0), Position + new Vector2(Width, Height - 1), climbToTrigger ? color : Color.AliceBlue);

            // Bottom
            Draw.Line(Position + new Vector2(0, Height - 1), Position + new Vector2(Width, Height - 1), Color.AliceBlue);
        }

        public void TriggerByPlayer(Player player) {
            ForkKILLETHelperModule.Session.addicted = true;

            var component = player.Get<AddictingComponent>();
            if (component == null) {
                var addictingComponent = new AddictingComponent(1.5f);
                player.Add(addictingComponent);
                var transitionListener = new TransitionListener();
                transitionListener.OnOutBegin = () => {
                    player.Remove(addictingComponent);
                };
                player.Add(transitionListener);
            }
            else {
                component.timeToDie = 1;
            }

            if (! triggered) { // If self is not triggered, set all blocks triggered.
                Level level = SceneAs<Level>();
                var addictingBlocks = level.Tracker.GetEntities<AddictingBlock>().Cast<AddictingBlock>();
                foreach (var block in addictingBlocks) {
                    block.triggered = true;
                }
            }
        }

        public override void Update() {
            if (standToTrigger && HasPlayerOnTop()) {
                Player player = GetPlayerOnTop();
                TriggerByPlayer(player);
            }
            if (climbToTrigger && HasPlayerClimbing()) {
                Player player = GetPlayerClimbing();
                TriggerByPlayer(player);
            }
        }
    }

    public class AddictingComponent : Component {
        public AddictingComponent(float maxTimeToDie) : base(true, false) {
            this.timeToDie = maxTimeToDie;
            this.maxTimeToDie = maxTimeToDie;
        }

        public float maxTimeToDie { get; private set; }

        public float timeToDie;

        public override void Update() {
            timeToDie -= Engine.DeltaTime;
            if (timeToDie < 0) {
                Level level = SceneAs<Level>();
                var player = level.Tracker.GetEntity<Player>();
                player.Die(new Vector2(0, 0));
            }
        }
    }
}