namespace GenealogyLogic.Components
{
    using System.Collections.Generic;
    using System.IO;
    using GenealogyLogic.Interfaces;
    using GenealogyLogic.Models;
    using Newtonsoft.Json;

    public class PersistorCore
    {
        public void Persist(string filename, string backupFile, List<IUIPerson> persons)
        {
            if (File.Exists(backupFile))
            {
                File.Delete(backupFile);
            }

            var allSerializablePersons = new List<SerializablePerson>();
            persons.ForEach(x => allSerializablePersons.Add(x.GetSerializeable()));

            var serialized = JsonConvert.SerializeObject(allSerializablePersons, Formatting.Indented);

            using (var sw = new StreamWriter(backupFile))
            {
                sw.Write(serialized);
            }

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            File.Move(backupFile, filename);
        }

        public List<IUIPerson> Restore(string filename)
        {
            var tmpGenealogyMaster = new GenealogyMaster();

            if (!File.Exists(filename))
            {
                return null;
            }

            string json = null;
            using (var sr = new StreamReader(filename))
            {
                json = sr.ReadToEnd();
            }

            var deserializedPersons = JsonConvert.DeserializeObject<List<SerializablePerson>>(json);

            foreach (var serializablePerson in deserializedPersons)
            {
                tmpGenealogyMaster.AddPerson(serializablePerson.ToUIPerson(), autoDefineGender: false);
            }

            foreach (var serializablePerson in deserializedPersons)
            {
                var guids = serializablePerson.ParentGuids;
                foreach (var parentId in guids)
                {
                    tmpGenealogyMaster.ParentRelationManager.AddParent(tmpGenealogyMaster.GetPersonById(serializablePerson.Id), tmpGenealogyMaster.GetPersonById(parentId));
                }
            }

            return tmpGenealogyMaster.GetAll();
        }
    }
}
