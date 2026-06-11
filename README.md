# PlayableNodes 🎬

Удобная система для создания и воспроизведения последовательностей анимаций в Unity.

## 📑 Содержание
- [📝 Введение](#-введение)
- [📦 Состав пакета](#-состав-пакета)
- [🔧 Установка](#-установка)
- [✨ Основные особенности](#-основные-особенности)
- [📚 Документация](#-документация)
- [💻 Пример использования](#-пример-использования)
- [🔍 Инспектор](#-инспектор)
- [🤝 Совместимость](#-совместимость)

## 📝 Введение
`PlayableNodes` позволяет объединять анимации в узлы и треки, запускать их последовательно или параллельно и легко ретаргетировать на другие объекты.

## 📦 Состав пакета
- **Gameplay.PlayableNodes.Core** – базовые классы узлов и треков.
- **Gameplay.PlayableNodes.Tween** – готовые анимации на базе DOTween.

## 🔧 Установка
1. Откройте *Package Manager* в Unity.
2. Выберите **Add package from git URL...**.
3. Укажите адрес для Core-модуля:
   ```
   https://github.com/avmedvedskiy/PlayableNodes.git?path=Gameplay.PlayableNodes.Core
   ```
4. Повторите шаг и установите модуль Tween:
   ```
   https://github.com/avmedvedskiy/PlayableNodes.git?path=Gameplay.PlayableNodes.Tween
   ```

## ✨ Основные особенности
- Асинхронный запуск анимаций через `UniTask`.
- Создание треков из набора узлов с любыми анимациями.
- Поддержка DOTween и собственных анимаций.
- Сохранение и ретаргетирование треков через ScriptableObject.

## 📚 Документация

- [Gameplay.PlayableNodes.Core](Gameplay.PlayableNodes.Core/README.md) - архитектура, треки, ноды, player-компоненты и runtime helpers.
- [Gameplay.PlayableNodes.Tween](Gameplay.PlayableNodes.Tween/README.md) - готовые tween-ноды, сгруппированные по target type.

## 💻 Пример использования
```csharp
[Serializable]
public class ScaleTransform : TweenAnimation<Transform>
{
    [SerializeField] private ToFromValue<float> _from = ToFromValue<float>.Dynamic;
    [SerializeField] private ToFromValue<float> _to;

    protected override Tween GenerateTween() =>
        Target
            .DOScale(_to, Duration)
            .ChangeValuesVectorOnStart(_to, _from);
}
```

### TrackPlayerCollection
Чтобы запустить один из треков коллекции по имени, добавьте на объект компонент
`TrackPlayerCollection` и вызовите метод `PlayAsync`:

```csharp
public class Example : MonoBehaviour
{
    [SerializeField] private TrackPlayerCollection _collection;

    private async UniTask Start()
    {
        await _collection.PlayAsync("Intro");
    }
}
```

## 🔍 Инспектор
![image](https://github.com/avmedvedskiy/PlayableNodes/assets/17832838/a639a671-1c21-438c-8feb-444a1323a185)
![image](https://github.com/avmedvedskiy/PlayableNodes/assets/17832838/4f66e48a-be8e-4527-8bc6-5205bc65c99e)

**Сохранение в ScriptableObject**
![image](https://github.com/avmedvedskiy/PlayableNodes/assets/17832838/7a992f24-6018-4dd3-b2d7-50a247016042)

**Ретаргет из ScriptableObject**
![image](https://github.com/avmedvedskiy/PlayableNodes/assets/17832838/a96554f3-6ca7-45d5-81f7-9fbbfac79748)

## 🤝 Совместимость
Пакет рассчитан на Unity **2022.3** и требует установленный **DOTween**.
