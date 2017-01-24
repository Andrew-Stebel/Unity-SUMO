using UnityEngine;
using System;
using System.Collections;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

public class CarProperties : MonoBehaviour
{

	public string id;
	public float x;
	public float y;
	public float angle;
	public double speed;
	public bool visited;

    //update this car's information based on the node data
	public void updateCarPosition (XmlNode curData)
	{
        //NOTE: when moving on the y axis, we actually move the car's on the z for 3D perspective
        //rotate car's direction
        changeOrientation(curData);

        //update data
		x = float.Parse (curData.Attributes ["x"].Value);
		y = float.Parse (curData.Attributes ["y"].Value);
		angle = float.Parse (curData.Attributes ["angle"].Value);
		speed = Convert.ToDouble (curData.Attributes ["speed"].Value);
		visited = true;

		transform.position = new Vector3(x,0.8f,y);
	}

    //rotate the car to examine it's next destination
    void changeOrientation(XmlNode curData)
    {
        if (x != float.Parse(curData.Attributes["x"].Value) &&
            y != float.Parse(curData.Attributes["y"].Value))
        {
            transform.LookAt(new Vector3(float.Parse(curData.Attributes["x"].Value),
                                           0.8f,
                                           float.Parse(curData.Attributes["y"].Value)));
        }
        else if (x != float.Parse(curData.Attributes["x"].Value))
        {
            transform.LookAt(new Vector3(float.Parse(curData.Attributes["x"].Value), 0.8f, y));
        }

        else if (y != float.Parse(curData.Attributes["y"].Value))
        {
            transform.LookAt(new Vector3(x, 0.8f, float.Parse(curData.Attributes["y"].Value)));
        }
    }

    //move the cars on their own from currentPosition to next
    //void Update()
    //{
    //    float step = 10f*Time.deltaTime;

    //    transform.position = Vector3.MoveTowards(transform.position,
    //                                                 new Vector3(x, 0.8f, y),
    //                                                 step);
    //}

}
