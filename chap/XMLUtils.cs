using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Trivial_Pursuit.Jeu.Entites;

namespace TrivialPursuit

{
    public static class XMLUtils<T>
    {
        public static T Deserialization(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var reader = new StreamReader(filePath))
            {
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "http://www.univ-grenoble-alpes.fr/l3miage/trivialpursuit");
                
                return (T)serializer.Deserialize(reader);
            }
        }

        public static void Serialization(T obj, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, obj);
            }
        }
    }
    
    
}

