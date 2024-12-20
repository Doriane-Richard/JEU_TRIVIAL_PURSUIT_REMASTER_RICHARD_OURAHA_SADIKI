using System;
using System.Xml.Xsl;
using System.IO;
using System.Globalization;
using System.Numerics;
using System.Xml;

namespace TrivialPursuit.chap;

public class Dom_Xpath
{
    private XmlDocument xmlDoc;
    private XmlNamespaceManager nsmgr;
    public Dom_Xpath(string fileXml)
    {
        xmlDoc = new XmlDocument();
        xmlDoc.Load(fileXml);
        nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
        nsmgr.AddNamespace("tp", "http://www.univ-grenoble-alpes.fr/l3miage/trivialpursuit");
    }

    public void Sauvegarde(string filePath)
    {
        xmlDoc.Save(filePath);
    }

    public string GetJoueurNom(XmlNode node)
    {
        return node.SelectSingleNode("tp:nom", nsmgr)?.InnerText ?? "Inconnu";
    }

    public XmlNodeList ApplyXPath(string xpath)
    {
        return xmlDoc.SelectNodes(xpath, nsmgr);
    }

    public int CountElemnts(string xpath)
    {
        var nod = ApplyXPath(xpath);
        return nod != null ? nod.Count : 0;
    }
    public void CaseVide(string filePath)
    {
        using (XmlReader reader = XmlReader.Create(filePath))
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Case")
                {
                    string type = reader.GetAttribute("type");
                    string couleur = reader.GetAttribute("couleur");

                    if (type == "VIDE")
                    {
                        if (reader.ReadToDescendant("Sprite"))
                        {
                            string texture = reader.GetAttribute("texture");
                            string positionX = reader.GetAttribute("positionX");
                            string positionY = reader.GetAttribute("positionY");
                            string size = reader.GetAttribute("size");
                        }
                    }
                }
            }
        }
              }
    public  void AfficherQuestionsHistoire(string filePath)
    {
        using (XmlReader reader = XmlReader.Create(filePath))
        {
            bool isHistoire = false;
            string question = null;
            string reponseCorrecte = null;
            
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "Categorie" && reader.GetAttribute("nom") == "Histoire")
                {
                    isHistoire = true;
                }
                else if (isHistoire && reader.NodeType == XmlNodeType.Element && reader.Name == "Question")
                {
                    reader.Read();
                    question = reader.Value;
                }
                else if (isHistoire && reader.NodeType == XmlNodeType.Element && reader.Name == "Reponse" && reader.GetAttribute("correct") == "true")
                {
                    reponseCorrecte = reader.GetAttribute("texte");
                }
                else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "Categorie")
                {
                    isHistoire = false; 
                }
            }
        }
    }



}











    


