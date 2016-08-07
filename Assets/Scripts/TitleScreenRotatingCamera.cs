using UnityEngine;

public class TitleScreenRotatingCamera : MonoBehaviour
{
    public GameObject m_tree;
    public int m_rotationSpeed = 15;

    Vector3 m_treePosition;

	// Use this for initialization
	void Start ()
    {
        m_tree = GameObject.FindGameObjectWithTag("MagicTree");
        if (m_tree != null)
            m_treePosition = m_tree.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_treePosition != null)
        {
            var rotatingAxis = new Vector3(0, m_treePosition.y, 0);
            transform.RotateAround(m_treePosition, rotatingAxis, m_rotationSpeed * Time.deltaTime);
        }
    }
}
