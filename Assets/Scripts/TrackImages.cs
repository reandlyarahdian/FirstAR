using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Events;

public class TrackImages : MonoBehaviour
{
    private ARTrackedImageManager imageManager;
    [SerializeField]
    private Vector3 scale;
    [SerializeField]
    private Text text;
    [SerializeField]
    private GameObject[] objects;
    [SerializeField]
    private UnityEvent @event;
    
    private Dictionary<string, GameObject> ARObject = new Dictionary<string, GameObject>();

    private void Awake()
    {
        imageManager = GetComponent<ARTrackedImageManager>();

        foreach(GameObject @object in objects)
        {
            GameObject newObject = @object;
            if (@object.gameObject.scene.rootCount == 0)
            {
                newObject = Instantiate(@object, Vector3.one, Quaternion.identity);
                newObject.name = @object.name;
            }
            ARObject.Add(@object.name, newObject);
            newObject.SetActive(false);
        }
    }

    private void OnEnable() => imageManager.trackedImagesChanged += ImageUpdate;

    private void OnDisable() => imageManager.trackedImagesChanged -= ImageUpdate;

    void ImageUpdate(ARTrackedImagesChangedEventArgs aRTracked)
    {
        foreach (ARTrackedImage trackedImage in aRTracked.added) { UpdateImage(trackedImage); @event.Invoke(); }
        foreach (ARTrackedImage trackedImage in aRTracked.updated) { UpdateImage(trackedImage); }
        foreach (ARTrackedImage trackedImage in aRTracked.removed) { Destroy(trackedImage.gameObject); }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        text.text = trackedImage.referenceImage.name;

        AssignObject(trackedImage.referenceImage.name, trackedImage.transform.position);
    }

    void AssignObject(string name, Vector3 pos)
    {
        if(objects != null)
        {
            ARObject[name].SetActive(true);
            ARObject[name].transform.position = pos;
            ARObject[name].transform.localScale = scale;

            foreach (GameObject gameObject in ARObject.Values)
            {
                if(gameObject.name != name)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
