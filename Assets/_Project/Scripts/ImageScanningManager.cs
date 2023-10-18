
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageScanningManager : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager m_ARImageManager;
    [SerializeField] private GameObject m_prefab01;
    [SerializeField] private GameObject m_prefab02;

    // Start is called before the first frame update
    void Start()
    {
        m_ARImageManager.trackedImagesChanged += OnImageDetectedEvent;
    }

    private void OnImageDetectedEvent(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage addedImage in args.added)
        {
            Debug.Log("Added");

            string name = addedImage.referenceImage.name;
            Debug.Log(name);
            if (!string.IsNullOrEmpty(name))
            {
                if(name == "image-01")
                {
                    Instantiate(m_prefab01, addedImage.transform);
                }
                if (name == "one")
                {
                    Instantiate(m_prefab02, addedImage.transform);
                }
            }
        }


        foreach (ARTrackedImage item in args.updated)
        {

        }

        foreach (ARTrackedImage item in args.removed)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
