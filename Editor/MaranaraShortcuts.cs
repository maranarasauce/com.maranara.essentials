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
            Undo.RecordObject(go, "Pre-Collider Encapsulation");

            BoxCollider col = go.GetComponent<BoxCollider>();
            if (col == null)
                col = go.AddComponent<BoxCollider>();

            Undo.RecordObject(col, "Collider Encapsulation");

            col.size = Vector3.zero;
            col.center = Vector3.zero;

            Quaternion rot = go.transform.rotation;
            go.transform.rotation = Quaternion.identity;

            Bounds bds = col.bounds;
            Renderer[] rends = go.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer r in rends)
            {
                bds.Encapsulate(r.bounds);
            }

            Matrix4x4 mat = Matrix4x4.identity;
            mat.SetTRS(go.transform.position, go.transform.rotation, go.transform.localScale);
            col.center += bds.center;
            col.center = mat.inverse.MultiplyPoint3x4(col.center);
            col.size = mat.inverse.MultiplyVector(bds.size);
            go.transform.rotation = rot;

            Undo.RecordObject(col, "Collider Encapsulation Calculation");
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
}
