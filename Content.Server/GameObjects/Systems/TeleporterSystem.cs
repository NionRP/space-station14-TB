using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Maths;
using Robust.Shared.Timing;
using Robust.Shared.Serialization.Manager.Attributes;
using Robust.Shared.ViewVariables;
using System.Collections.Generic;
using Content.Shared.Interaction; 

public sealed class TeleporterSystem : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly IGameTiming _gameTiming = default!;

    private readonly Dictionary<string, (EntityUid, EntityUid)> _teleporterPairs = new();

    public override void Initialize()
    {
        SubscribeLocalEvent<TeleporterComponent, ComponentInit>(OnInit);
        SubscribeLocalEvent<TeleporterComponent, InteractHandEvent>(OnInteract); // Используем взаимодействие
    }

    private void OnInit(EntityUid uid, TeleporterComponent component, ComponentInit args)
    {
        if (component.TeleporterKey == null)
            return;

        // Логика связывания телепортов
        if (_teleporterPairs.TryGetValue(component.TeleporterKey, out var pair))
        {
            component.TargetUid = pair.Item1;
            if (TryComp(pair.Item1, out TeleporterComponent? existingComp))
            {
                existingComp.TargetUid = uid;
            }
            _teleporterPairs.Remove(component.TeleporterKey);
        }
        else
        {
            _teleporterPairs[component.TeleporterKey] = (uid, EntityUid.Invalid);
        }
    }

    private void OnInteract(EntityUid uid, TeleporterComponent component, InteractHandEvent args)
    {
        if (component.TargetUid == null ||
            component.NextTeleport > _gameTiming.CurTime ||
            !args.User.Valid)
            return;

        var target = component.TargetUid.Value;
        if (!Exists(target))
            return;

        // Телепортация
        _transform.SetCoordinates(args.User, Transform(target).Coordinates);
        _transform.SetWorldRotation(args.User, Transform(target).LocalRotation);

        // Установка времени следующего телепорта
        component.NextTeleport = _gameTiming.CurTime + TimeSpan.FromSeconds(component.Cooldown);

        args.Handled = true; // Помечаем событие как обработанное
    }
}
