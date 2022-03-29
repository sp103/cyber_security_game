using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using Mirror;

public class XmlReader
{
    XmlDocument doc;

    public void LoadData()
    {
        TextAsset xmlText = Resources.Load<TextAsset>("GameData");
        doc = new XmlDocument();
        doc.LoadXml(xmlText.text);
    }

    public SyncList<AttackVector> LoadVectors()
    {
        XmlNodeList vectors = doc.SelectNodes("/game_data/attack_vectors/attack_vector");
        SyncList<AttackVector> AttackVectors = new SyncList<AttackVector>();
        foreach (XmlNode vector in vectors)
        {
            AttackVectors.Add(new AttackVector(vector["to"].InnerText, vector["from"].InnerText, vector["enabled"].InnerText));
        }
        return AttackVectors;
    }

    public SyncList<ResourceRoute> LoadRoutes()
    {
        XmlNodeList routes = doc.SelectNodes("/game_data/resource_routes/resource_route[@two_way='False']");
        XmlNodeList twoWay_routes = doc.SelectNodes("/game_data/resource_routes/resource_route[@two_way='True']");
        SyncList<ResourceRoute> ResourceRoutes = new SyncList<ResourceRoute>();
        foreach (XmlNode route in routes)
        {
            ResourceRoutes.Add(new ResourceRoute(route["to"].InnerText, route["from"].InnerText));
        }
        foreach (XmlNode route in twoWay_routes)
        {
            ResourceRoutes.Add(new ResourceRoute(route["to"].InnerText, route["from"].InnerText));
            ResourceRoutes.Add(new ResourceRoute(route["from"].InnerText, route["to"].InnerText));
        }
        return ResourceRoutes;
    }
}
