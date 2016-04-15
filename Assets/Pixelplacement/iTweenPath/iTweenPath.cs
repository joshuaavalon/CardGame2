using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("Pixelplacement/iTweenPath")]
public class iTweenPath : MonoBehaviour
{
	public Color PathColor = Color.cyan;
	public List<Vector3> Nodes = new List<Vector3>(){Vector3.zero, Vector3.zero};
	public int NodeCount;
	public bool PathVisible = true;
    public string PathName;
    public bool Initialized;
    public string InitialName="";

    private void OnDrawGizmosSelected()
	{
	    if (!PathVisible) return;
	    if(Nodes.Count > 0){
	        iTween.DrawPath(Nodes.ToArray(), PathColor);
	    }
	}
}