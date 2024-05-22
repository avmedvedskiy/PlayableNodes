![image](https://github.com/avmedvedskiy/PlayableNodes/assets/17832838/a639a671-1c21-438c-8feb-444a1323a185)
![image](https://github.com/avmedvedskiy/PlayableNodes/assets/17832838/4f66e48a-be8e-4527-8bc6-5205bc65c99e)

Save into Scriptable Object

![image](https://github.com/avmedvedskiy/PlayableNodes/assets/17832838/7a992f24-6018-4dd3-b2d7-50a247016042)

Retarget from ScriptableObject

![image](https://github.com/avmedvedskiy/PlayableNodes/assets/17832838/a96554f3-6ca7-45d5-81f7-9fbbfac79748)

Animation Example
```csharp
    [Serializable]
    public class ScaleTransform : TweenAnimation<Transform>
    {
        [SerializeField] private ToFromValue<float> _from;
        [SerializeField] private ToFromValue<float> _to;

        protected override Tweener GenerateTween() => Target
            .DOScale(_to, Duration)
            .ChangeValuesVector(_to, _from);
    }
```

