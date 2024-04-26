using Cysharp.Threading.Tasks;
using PlayableNodes;
using UnityEngine;

namespace PlayableNodes.Samples
{
    public class TestMonoController : MonoBehaviour
    {
        public TrackPlayerCollection _trackPlayerCollection;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _trackPlayerCollection
                    .PlayAsync("Open")
                    .ContinueWith(() => Debug.Log("Completed"))
                    .Forget();
        }
    
    }
    
}
