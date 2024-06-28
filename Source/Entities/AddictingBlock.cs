using Celeste.Mod.Entities;
using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Linq;

namespace Celeste.Mod.ForkKILLETHelper.Entities;

public readonly struct TriggerMethod(int triggerMethod)
{
    public readonly bool Top = (triggerMethod & 1) != 0;
    public readonly bool Left = (triggerMethod & 2) != 0;
    public readonly bool Right = (triggerMethod & 4) != 0;
}

public class AddictingSolid {
    public static void Render(
        Vector2 position, float width, float height,
        bool triggered, Level level, TriggerMethod triggerMethod
    ) {
        var color = Color.HotPink;

        if (triggered) {
            var player = level.Tracker.GetEntity<Player>();
            var addictingComponent = player?.Get<AddictingComponent>();

            if (addictingComponent != null) {
                float greenAndBlue = 1f - addictingComponent.TimeToDie / addictingComponent.MaxTimeToDie;
                color = new Color(255, greenAndBlue, greenAndBlue);
            }
        }

        // Top
        Draw.Line(position + new Vector2(1, 0), position + new Vector2(width, 0), triggerMethod.Top ? color : Color.AliceBlue);

        // Left
        Draw.Line(position + new Vector2(1, 0), position + new Vector2(1, height - 1), triggerMethod.Left ? color : Color.AliceBlue);
        
        // Right
        Draw.Line(position + new Vector2(width, 0), position + new Vector2(width, height - 1), triggerMethod.Right ? color : Color.AliceBlue);

        // Bottom
        Draw.Line(position + new Vector2(0, height - 1), position + new Vector2(width, height - 1), Color.AliceBlue);
    }
}

[Tracked(true)]
[CustomEntity("ForkKILLETHelper/AddictingBlock")]
public class AddictingBlock(Vector2 position, float width, float height, int triggerMethod, bool safe) : Solid(position, width, height, safe) {
    public AddictingBlock(EntityData data, Vector2 offset)
        : this(data.Position + offset, data.Width, data.Height, data.Int("triggerMethod", 1), false)
    {}

    public bool Triggered = false;
    public readonly TriggerMethod TriggerMethod = new(triggerMethod);

    public override void Render() {
        AddictingSolid.Render(Position, Width, Height, Triggered, SceneAs<Level>(), TriggerMethod);
    }
    public override void Update() {
        Level level = SceneAs<Level>();
        if (TriggerMethod.Top && HasPlayerOnTop()) {
            Player player = GetPlayerOnTop();
            AddictBase.MakePlayerAddicted(player, level);
        }
        if ((TriggerMethod.Left || TriggerMethod.Right) && HasPlayerClimbing()) {
            Player player = GetPlayerClimbing();
            if (
                TriggerMethod.Left && player.Position.X <= Position.X ||
                TriggerMethod.Right && player.Position.X >= Position.X + Width
            ) AddictBase.MakePlayerAddicted(player, level);
        }
    }
}

