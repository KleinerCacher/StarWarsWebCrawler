﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace WebCrawler
{
    public class TransportationWeaponMapping
    {
        private static readonly Lazy<TransportationWeaponMapping> lazy
            = new Lazy<TransportationWeaponMapping>(() => new TransportationWeaponMapping());

        private const string fileName = "StarWarsWeaponList.txt";
        private Hashtable weaponsByName = new Hashtable();
        private Dictionary<string, string> transportationWithAdditionalNames = new Dictionary<string, string>();

        private TransportationWeaponMapping()
        {
            string[] lines = File.ReadAllLines(fileName);

            foreach (string line in lines)
            {
                ConvertLineToWeaponListAndSaveToWeaponList(line);
            }
        }

        public static TransportationWeaponMapping Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private void ConvertLineToWeaponListAndSaveToWeaponList(string line)
        {
            if (line.StartsWith("--"))
            {
                // Commentline do not use
                return;
            }

            string[] sections = line.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
            string name = sections[0].Trim();
            if (name.Contains("("))
            {
                string additionalName = name.Substring(name.IndexOf('(') + 1, name.LastIndexOf(')') - name.IndexOf('(') - 1);
                name = name.Substring(0, name.IndexOf('('));
                transportationWithAdditionalNames.Add(name, additionalName);
            }

            List<Weapon> weaponList = new List<Weapon>();
            for (int i = 1; i < sections.Length; i++)
            {
                string[] weaponStats = sections[i].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                weaponList.Add(WeaponFactory.CreateWeapon(weaponStats));
            }

            weaponsByName.Add(name, weaponList);
        }

        public string GetAdditionalTransportationNameIfSet(string currentTransportationName)
        {
            if (transportationWithAdditionalNames.ContainsKey(currentTransportationName))
            {
                return transportationWithAdditionalNames[currentTransportationName];
            }

            return string.Empty;
        }

        public List<Weapon> GetWeaponsByTransportationName(string name)
        {
            if (weaponsByName.ContainsKey(name))
            {
                return weaponsByName[name] as List<Weapon>;
            }

            return new List<Weapon>();
        }
    }
}
