using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class XmlReader
{
    XmlDocument doc;

    public void LoadData()
    {
        TextAsset xmlText = Resources.Load<TextAsset>("GameData");
        doc = new XmlDocument();
        doc.LoadXml(xmlText.text);
    }

    public AttackVector[] LoadVectors()
    {
        XmlNodeList vectors = doc.SelectNodes("/game_data/attack_vectors/attack_vector");
        AttackVector[] AttackVectors = new AttackVector[vectors.Count];
        int i = 0;
        foreach (XmlNode vector in vectors)
        {
            AttackVectors[i] = new AttackVector(vector["to"].InnerText, vector["from"].InnerText, vector["enabled"].InnerText);
            i++;
        }
        return AttackVectors;
    }

    public ResourceRoute[] LoadRoutes()
    {
        XmlNodeList routes = doc.SelectNodes("/game_data/resource_routes/resource_route[@two_way='False']");
        XmlNodeList twoWay_routes = doc.SelectNodes("/game_data/resource_routes/resource_route[@two_way='True']");
        ResourceRoute[] ResourceRoutes = new ResourceRoute[routes.Count + (twoWay_routes.Count * 2)];
        int i = 0;
        foreach (XmlNode route in routes)
        {
            ResourceRoutes[i] = new ResourceRoute(route["to"].InnerText, route["from"].InnerText);
            i++;
        }
        foreach (XmlNode route in twoWay_routes)
        {
            ResourceRoutes[i] = new ResourceRoute(route["to"].InnerText, route["from"].InnerText);
            i++;
            ResourceRoutes[i] = new ResourceRoute(route["from"].InnerText, route["to"].InnerText);
            i++;
        }
        return ResourceRoutes;
    }
}
