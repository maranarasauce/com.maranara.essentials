using UnityEditor;
using UnityEngine;

public class EditorAutomation
{

    [MenuItem("GameObject/Automation/Move to Camera &q", false, 0)]
    static void MoveToCamera()
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null)
            return;
        Transform cur = selected.transform;
        Undo.RecordObject(cur, "Move to Camera");
        var view = SceneView.lastActiveSceneView;
        cur.position = view.camera.transform.position + view.camera.transform.forward;
    }


    [MenuItem("GameObject/Automation/Encapsulate &e", false, 0)]
    static void Encapsulate()
    {

        GameObject[] selected = Selection.gameObjects;
        foreach (GameObject go in selected)
        {
            BoxCollider col = go.GetComponent<BoxCollider>();
            if (col == null)
                col = go.AddComponent<BoxCollider>();

            Undo.RecordObject(col, "Collider Encapsulation");

            Transform parent = go.transform.parent;
            Vector3 pos = go.transform.localPosition;
            Quaternion rot = go.transform.localRotation;
            Vector3 scale = go.transform.localScale;

            go.transform.SetParent(null, true);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;

            col.size = Vector3.zero;
            col.center = Vector3.zero;

            Bounds bds = col.bounds;

            Renderer[] rends = go.GetComponentsInChildren<Renderer>(true);
            bds.center = rends[0].bounds.center;
            foreach (Renderer r in rends)
            {
                if (r is ParticleSystemRenderer)
                {
                    continue;
                }
                if (!r.gameObject.activeInHierarchy || !r.enabled)
                    continue;

                bds.Encapsulate(r.bounds);
            }

            Matrix4x4 mat = Matrix4x4.identity;
            mat.SetTRS(go.transform.position, go.transform.rotation, go.transform.localScale);
            col.center += bds.center;
            col.center = mat.inverse.MultiplyPoint3x4(col.center);
            col.size = mat.inverse.MultiplyVector(bds.size);

            go.transform.SetParent(parent, true);
            go.transform.localPosition = pos;
            go.transform.localRotation = rot;
            go.transform.localScale = scale;
        }
    }


    [MenuItem("GameObject/Automation/Name Selection Ordered", false, 0)]
    static void OrderedName()
    {
        GameObject[] selected = Selection.gameObjects;
        Undo.RecordObjects(selected, "Rename Selection");
        string name = string.Copy(selected[0].name);
        for (int i = 0; i < selected.Length; i++)
        {
            if (i == 0)
                continue;
            int x = i + 1;
            selected[i].name = string.Concat(name, " ", x.ToString());
        }
    }

    [MenuItem("GameObject/Automation/Parenting/Make Empty Parent", false, 0)]
    static void Empty()
    {
        foreach (GameObject grabbableObj in Selection.gameObjects)
        {
            if (grabbableObj == null)
            {
                // uhoh
                return;
            }

            var obj2 = new GameObject(grabbableObj.name);
            obj2.transform.parent = grabbableObj.transform.parent;
            obj2.transform.position = grabbableObj.transform.position;
            obj2.transform.rotation = grabbableObj.transform.rotation;
            obj2.transform.SetSiblingIndex(grabbableObj.transform.GetSiblingIndex());
            obj2.transform.localScale = grabbableObj.transform.localScale;
            grabbableObj.transform.SetParent(obj2.transform, true);
        }
    }

    [MenuItem("GameObject/Automation/Parenting/Make Centered Empty Parent", false, 0)]
    static void EmptyCenter()
    {
        foreach (GameObject grabbableObj in Selection.gameObjects)
        {
            if (grabbableObj == null)
            {
                // uhoh
                return;
            }

            var obj2 = new GameObject(grabbableObj.name);
            obj2.transform.parent = grabbableObj.transform.parent;
            obj2.transform.position = grabbableObj.GetComponentInChildren<Renderer>().bounds.center; /* grabbableObj.transform.position */
            obj2.transform.rotation = grabbableObj.transform.rotation;
            obj2.transform.SetSiblingIndex(grabbableObj.transform.GetSiblingIndex());
            obj2.transform.localScale = grabbableObj.transform.localScale;
            grabbableObj.transform.SetParent(obj2.transform, true);
        }
    }

    [MenuItem("GameObject/Automation/Parenting/Make Bottom Centered Empty Parent", false, 0)]
    static void EmptyCenterBottom()
    {
        foreach (GameObject grabbableObj in Selection.gameObjects)
        {
            if (grabbableObj == null)
            {
                // uhoh
            }

            var bounds = grabbableObj.GetComponentInChildren<Renderer>().bounds;
            var calculatedButtomCenter = bounds.center;
            calculatedButtomCenter.y -= bounds.size.y / 2;

            var obj2 = new GameObject(grabbableObj.name);
            obj2.transform.parent = grabbableObj.transform.parent;
            obj2.transform.position = calculatedButtomCenter; /* grabbableObj.transform.position */
            obj2.transform.rotation = grabbableObj.transform.rotation;
            obj2.transform.SetSiblingIndex(grabbableObj.transform.GetSiblingIndex());
            obj2.transform.localScale = grabbableObj.transform.localScale;
            grabbableObj.transform.SetParent(obj2.transform, true);
        }
    }
}
