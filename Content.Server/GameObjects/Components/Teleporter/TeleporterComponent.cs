using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.ViewVariables;
using System;

[RegisterComponent]
public sealed partial class TeleporterComponent : Component
{
    [ViewVariables]
    public EntityUid? TargetUid; // ID объекта, к которому будет происходить телепортация

    [DataField("teleporterKey")]
    public string? TeleporterKey; // Ключ для связи двух телепортов

    [DataField("cooldown")]
    public float Cooldown = 2f; // Время перезарядки телепорта (в секундах)

    public TimeSpan NextTeleport; // Время следующего возможного телепорта
}
