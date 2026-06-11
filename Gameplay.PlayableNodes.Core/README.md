# Gameplay.PlayableNodes.Core

Core-модуль `PlayableNodes` содержит базовую модель для сборки и запуска анимационных сценариев из serializable-нод.

## Назначение

`PlayableNodes` строит анимацию из трех уровней:

1. `TrackPlayer`, `TrackPlayerCollection` или `RetargetTrackPlayerCollection` запускает трек.
2. `Track` хранит набор `TrackNode`.
3. `TrackNode` хранит target object (`Context`) и список `IAnimation`.

Важная модель исполнения:

- `TrackPlayer` проигрывает один встроенный `Track`.
- `TrackPlayerCollection.PlayAsync(trackName)` ищет активный трек по имени.
- `RetargetTrackPlayerCollection` берет треки из `TrackClip` и перед запуском подменяет `Context` нод объектами из bindings.
- Все активные `TrackNode` внутри одного `Track` запускаются параллельно.
- Все включенные `IAnimation` внутри одного `TrackNode` тоже запускаются параллельно.
- `Delay` относится к конкретной animation node, а не ко всему track/node.
- Для последовательных шагов используйте `Delay`, отдельные треки или последовательные вызовы `PlayAsync` из кода.

## Runtime model

### `IAnimation`

Базовый интерфейс всех анимаций, которые лежат в `TrackNode` через `SerializeReference`.

Ключевые свойства:

- `Pin` - id для runtime-изменений.
- `Enable` - можно выключить конкретную анимацию без удаления.
- `Delay` и `Duration` - timing конкретной анимации.
- `SetTarget(Object target)` - получает `Context` из `TrackNode`.
- `PlayAsync(...)` - запускает анимацию.

### `TargetAnimation<T>`

Базовый класс для нод без обязательной привязки к DOTween. Обрабатывает `Delay`, `Duration`, `Enable`, `Pin` и проверку target type.

Если нода ожидает `Transform`, а в `Context` передан `Component`, будет использован `component.transform`.

### `Track`, `TrackNode`, `TrackClip`

- `Track` - именованный набор `TrackNode`.
- `TrackNode` - один target object и список animation nodes.
- `TrackClip` - `ScriptableObject` для сохранения треков и binding references.

### Player-компоненты

- `TrackPlayer` - проигрывает один встроенный трек.
- `TrackPlayerCollection` - хранит список треков и запускает активный трек по имени.
- `RetargetTrackPlayerCollection` - experimental player для `TrackClip`, который перед запуском назначает contexts из bindings.

Все player-компоненты реализуют `ITracksPlayer`.

## Runtime helpers

`PlayableNodes.Extensions.Extensions` добавляет:

- `ChangeEndValueByPin<T>(pin, value)` - меняет конечное значение у нод с `IChangeEndValue<T>`.
- `ChangeTargetByPin(pin, value)` - меняет `Context` у нод, где есть animation с указанным `Pin`.
- `EnableAnimationByPin(pin, value)` - включает или выключает animation по `Pin`.
- `DrawAnimationGizmos()` - вызывает gizmos у нод с `IDrawGizmosSelected`.

## Values

- `ToFromValue<T>` хранит значение как `ToFromType.Direct` или `ToFromType.Dynamic`.
- `ToFromType.Direct` использует значение из инспектора.
- `ToFromType.Dynamic` берет текущее значение target в момент запуска.

## Interact-компоненты

Эти компоненты не являются `IAnimation`-нодами, но используются другими нодами и particle-сценариями.

- `BaseTargetInteract` - базовый `ITargetInteract` с UnityEvent.
- `TrackTargetInteract` - при interact запускает трек из `TrackPlayerCollection`.

## Пример запуска

```csharp
await collection.PlayAsync("Show");
```

## Runtime retarget by pin

```csharp
collection.ChangeEndValueByPin(pin, targetTransform);
collection.ChangeTargetByPin(pin, newContext);
collection.EnableAnimationByPin(pin, false);

await collection.PlayAsync("Collect");
```

## Важные замечания

- Ноды внутри одного `Track` запускаются параллельно.
- Анимации внутри одного `TrackNode` запускаются параллельно.
- `Context` должен соответствовать target type ноды.
- Для `Transform`-нод можно передать любой `Component`, тогда будет использован `component.transform`.
- `Duration` у мгновенных нод может использоваться как wait после действия.
