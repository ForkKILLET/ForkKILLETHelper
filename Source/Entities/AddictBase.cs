using System.Linq;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.ForkKILLETHelper.Entities;

public class AddictingComponent(float maxTimeToDie) : Component(true, false) {
    public float MaxTimeToDie { get; } = maxTimeToDie;

    public float TimeToDie = maxTimeToDie;

    public override void Update() {
        TimeToDie -= Engine.DeltaTime;
        if (TimeToDie < 0) {
            Level level = SceneAs<Level>();
            var player = level.Tracker.GetEntity<Player>();
            player.Die(new Vector2(0, 0));
        }
    }
}

public class AddictBase {
    public static void MakePlayerAddicted(Player player, Level level) {
        ForkKILLETHelperModule.Session.addicted = true;

        var component = player.Get<AddictingComponent>();
        if (component == null) {
            var addictingComponent = new AddictingComponent(1.5f);
            player.Add(addictingComponent);
            var transitionListener = new TransitionListener();
            transitionListener.OnOutBegin = () => {
                player.Remove(addictingComponent);
                player.Remove(transitionListener);
            };
            player.Add(transitionListener);
        }
        else {
            component.TimeToDie = 1;
        }

        var addictingBlocks = level.Tracker.GetEntities<AddictingBlock>().Cast<AddictingBlock>();
        foreach (var block in addictingBlocks) {
            block.Triggered = true;
        }
    }
}
