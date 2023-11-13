using Cysharp.Threading.Tasks;
using PlayableNodes;
using UnityEngine;

public class TestVM : MonoBehaviour
{
    public BaseTrackPlayer _trackPlayerCollection;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _trackPlayerCollection
                .PlayAsync("Open")
                .ContinueWith(() => Debug.Log("Completed"))
                .Forget();
    }
}