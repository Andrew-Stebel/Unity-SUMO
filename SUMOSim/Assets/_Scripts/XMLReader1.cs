using UnityEngine;
using System.Xml;
using System;
using System.Collections.Generic;

public class XMLReader1 : MonoBehaviour
{

    /*pass in the directory URL for the car data generated from the command line:
   sumo –c map.sumo.cfg --fcd-output carData */
    string path = "C:\\Users\\Andrew\\downloads\\map\\crossVehicles";

    public List<GameObject> activeCars; //contains all cars currently in the scene
    XmlNodeList nodeList;   //contain every time frame of the simulation in a node list
    int readTime = -1;      //current timespace

    // Begin XML reading as soon as simulation begins
    void Start()
    {
        //open and load the file
        XmlReader reader = XmlReader.Create(path);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(reader);

        nodeList = xmlDoc.GetElementsByTagName("timestep");

        //start reading nodes, repeat every 0.1 seconds
        InvokeRepeating("readXml", 0, .1f);
    }


    //read a node in the xml file	
    void readXml()
    {
        readTime++;     //starts at 0 seconds

        //validate each car's existance
        foreach (GameObject c in activeCars)
            c.GetComponent<CarProperties>().visited = false;

        readTimeStep();

        //delete any cars not in latest node
        deleteObjects();
    }

    void readTimeStep()
    {
        int count = 0;
        foreach (XmlNode node in nodeList) //foreach timestep
        {
            //check if we are at the right time in simulation
            if (count == readTime)
            {
                foreach (XmlNode nn in node) //update each vehicle in that timestep
                {
                    determineCar(nn);
                }
                break;  //done this timestep
            }
            else
                count++;
        }
    }

    //determine if the current car is already in the scene
    void determineCar(XmlNode curNode)
    {
        GameObject car = alive(curNode);
        //update a car
        if (car)
        {
            car.GetComponent<CarProperties>().updateCarPosition(curNode);
        }
        //if not in the scene, create and add a new car
        else
        {
            //create the car on the scene view
            car = Instantiate(getCarData(curNode),
                               new Vector3(float.Parse(curNode.Attributes["x"].Value), 0.8f, float.Parse(curNode.Attributes["y"].Value)),
                               new Quaternion(0, 0, 0, 0)) as GameObject;

            //add new car to list
            activeCars.Add(car);
        }
    }

    //create a new car gameObject and fill it's information based on the xml element
    GameObject getCarData(XmlNode curNode)
    {
        //load random car color prefab
        System.Random rnd = new System.Random();
        GameObject car;

        //1 = orange car
        if (rnd.Next(1, 3) == 1)
            car = Resources.Load("Simp_Car") as GameObject;
        //2 = purple car
        else
            car = Resources.Load("Simp_Car2") as GameObject;

        //add data from element
        car.GetComponent<CarProperties>().id = curNode.Attributes["id"].Value;
        car.GetComponent<CarProperties>().x = float.Parse(curNode.Attributes["x"].Value);
        car.GetComponent<CarProperties>().y = float.Parse(curNode.Attributes["y"].Value);
        car.GetComponent<CarProperties>().angle = float.Parse(curNode.Attributes["angle"].Value);
        car.GetComponent<CarProperties>().speed = Convert.ToDouble(curNode.Attributes["speed"].Value);
        car.GetComponent<CarProperties>().visited = true;

        return car;
    }

    //return the currentNode's car from the active list
    //returns null if it doesn't exist
    GameObject alive(XmlNode curNode)
    {
        foreach (GameObject c in activeCars)
        {
            if (curNode.Attributes["id"].Value == c.GetComponent<CarProperties>().id)
            {
                return c;
            }
        }
        return null;
    }

    //delete any finished cars
    void deleteObjects()
    {
        List<GameObject> savedItems = new List<GameObject>();

        foreach (GameObject c in activeCars)
        {
            if (c.GetComponent<CarProperties>().visited)
            {
                savedItems.Add(c);
            }
            //if it wasn't visited, delete it from scene
            else
            {
                Destroy(c);
            }
        }

        //update the list
        activeCars = savedItems;
    }
}
