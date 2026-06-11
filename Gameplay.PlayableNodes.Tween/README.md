# Gameplay.PlayableNodes.Tween

Tween-модуль `PlayableNodes` содержит готовые animation nodes для `Gameplay.PlayableNodes.Core`.

## Назначение

Модуль добавляет:

- базовый `TweenAnimation<T>`;
- `Easing` поверх DOTween `Ease` / `AnimationCurve`;
- готовые DOTween-ноды для Transform, UI, Renderer, TextMeshPro, ParticleSystem, Animation/Animator и Physics;
- runtime/editor-preview helpers для DOTween, legacy Animation и ParticleSystem.

Большинство нод наследуются от `TweenAnimation<T>` или `TargetAnimation<T>` и выбираются в `TrackNode` по типу `Context`.

## Базовые tween-понятия

### `TweenAnimation<T>`

Базовый класс для DOTween-нод. Создает tween через `GenerateTween()`, применяет delay/easing/recyclable settings, запускает runtime playback или editor preview и ждет завершения через UniTask.

### `Easing`

Обертка над DOTween easing. Поддерживает стандартный `Ease`, custom `AnimationCurve` и scale для overshoot/amplitude.

### Общие enum/helper types

- `MoveSpace` - local/global space в transform-нодах.
- `TextDurationType` - считать duration полной длительностью текста или длительностью одного символа.

## Ноды по target type

### `Transform`

- `MoveTransform` - двигает transform к заданной позиции в local/global space.
- `MoveRelativeTransform` - двигает transform на относительное смещение.
- `MoveConstraintTransform` - двигает transform с отдельным easing по X/Y/Z.
- `MoveToTargetTransform` - двигает transform к позиции другого transform.
- `MoveToTargetConstraintTransform` - двигает к target transform с отдельным easing по X/Y/Z.
- `FollowToTargetTransform` - двигает transform к target и обновляет конечную позицию, если target движется.
- `MoveToTarget2DBezierTransform` - двигает к target по 2D bezier path.
- `MoveToTarget3DBezierTransform` - двигает к target по 3D bezier path, может поворачивать объект вдоль пути и рисовать gizmos.
- `LookAtTargetTransform` - поворачивает transform лицом к target position.
- `RotateTransform` - анимирует rotation в local/global space.
- `RotateToTargetTransform` - поворачивает transform к rotation другого target.
- `ScaleTransform` - анимирует uniform scale.
- `ScaleToTargetTransform` - анимирует scale до `localScale` другого transform.
- `ScalePunchTransform` - запускает punch scale эффект.
- `ScaleByCurveTransform` - масштабирует через animation curve, общей или отдельной по осям.
- `ShakeTransform` - запускает shake scale эффект.
- `PositionPunchTransform` - запускает punch position эффект и возвращает стартовую local position.
- `SetActiveTransform` - мгновенно меняет active state GameObject.
- `InteractTarget` - вызывает `ITargetInteract.Interact()` на указанном target.

### `RectTransform`

- `SetAnchoredPositionTransform` - мгновенно задает `anchoredPosition`.
- `AnchoredPositionTransform` - анимирует `anchoredPosition`.
- `SizeTransform` - анимирует `sizeDelta`.
- `SizeByTargetTransform` - анимирует `sizeDelta` до размера другого `RectTransform` с optional offset.
- `RebuildLayoutEveryFrame` - в течение `Duration` каждый frame вызывает `LayoutRebuilder.MarkLayoutForRebuild`.
- `SizeByTextTransform` - анимирует размер `RectTransform` по preferred size указанного `TMP_Text`.

### UI

- `ColorGraphic` - анимирует цвет `UnityEngine.UI.Graphic`.
- `FadeGraphic` - анимирует alpha у `Graphic`.
- `FadeCanvasGroup` - анимирует alpha у `CanvasGroup`.
- `AnimateImageMaterialFloatVariable` - анимирует float shader property на material instance у `Image`.
- `AnimateImageMaterialVectorVariable` - анимирует Vector4 shader property на material instance у `Image`.

### TextMeshPro

- `MaxVisibleCharactersText` - typewriter-анимация через `TMP_Text.maxVisibleCharacters`.
- `SizeByTextTransform` - меняет размер rect под preferred size текста.
- `TextMeshProExtension.SetSmoothValue(...)` - utility extension для плавной смены числового текста (`long`, `int`, `float`).

### Renderer

- `SpriteColorGraphic` - анимирует цвет `SpriteRenderer`.
- `ChangeSortingGroupOrder` - мгновенно меняет `SortingGroup.sortingOrder`.
- `AnimateRendererMaterialFloatVariable` - анимирует float property на renderer material.
- `AnimateRendererMaterialFloatCurve` - анимирует float property по `AnimationCurve`.
- `AnimateRendererMaterialVectorVariable` - анимирует Vector4 property на renderer material.

### `Animation` / `Animator`

- `PlayAnimation` - проигрывает legacy `Animation` clip по имени; поддерживает editor preview.
- `SetBoolAnimator` - задает bool parameter в `Animator` и ждет `Duration`.
- `SetTriggerAnimator` - вызывает animator trigger и ждет `Duration`.
- `TrackAnimation` - запускает трек из `TrackPlayerCollection`, что позволяет вкладывать один playable sequence в другой.

### Physics

- `AddForceRigidbody` - выключает kinematic mode, добавляет force и ждет `Duration`.
- `AddRandomForceRigidbody` - добавляет случайный force в заданном диапазоне.
- `EnableCollider` - мгновенно включает или выключает `Collider`.

### ParticleSystem

- `PlayParticleSystem` - проигрывает particle system в течение `Duration`, поддерживает editor preview.
- `StopParticleSystem` - вызывает stop emitting и ждет `Duration`.
- `PlayParticleToTarget` - запускает `MoveParticlesToTarget` и направляет particles к target transform.

## Interact-компоненты

Эти компоненты не являются `IAnimation`-нодами, но используются нодами и particle-сценариями.

- `AnimationTargetInteract` - при interact проигрывает legacy animation.
- `PunchTargetInteract` - при interact запускает reusable punch scale.
- `PunchTargetPositionInteract` - при interact запускает reusable punch position.
- `MoveParticlesToTarget` - helper для движения particles к target; при достижении вызывает `ITargetInteract` на target.

## Utility classes

- `DOTweenExtensions` - runtime/editor-preview запуск tween, local/global helpers, path helpers, follow-target tween, interact-on-complete.
- `TweenValuesExtensions` - конвертация `ToFromValue<T>` и подмена start/end значений DOTween.
- `ObjectExtensions` - `DestroyOrPreview` и animated `DOSetActive`.
- `AnimationExtensions` - runtime/editor-preview проигрывание legacy `Animation`.
- `ParticleSystemExtensions` - runtime/editor-preview проигрывание и остановка particles.
- `DOTweenTextExtensions` - tween для `TMP_Text.maxVisibleCharacters`.
- `EasingTweenExtension` - применение `Easing` к DOTween tween.
- `SimpleBezierPath` - построение и debug draw 2D bezier path.

## Важные замечания

- Tween-ноды запускаются параллельно с другими включенными animation nodes внутри того же `TrackNode`.
- `Duration` у мгновенных нод часто используется как wait после действия.
- Material-ноды могут создавать временный material instance и восстанавливать исходный material после completion.
- Particle и legacy Animation ноды имеют отдельные пути для editor preview вне Play Mode.
