using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FvTwool
{
    public static class Hashing
    {
        public const ulong MetaFlag = 0x4000000000000;

        private static Dictionary<ulong, string> qarDict = new Dictionary<ulong, string>();
        private static Dictionary<uint, string> fmdlDict = new Dictionary<uint, string>();

        private static readonly List<string> FileExtensions = new List<string>
        {
            "1.ftexs",
            "1.nav2",
            "2.ftexs",
            "3.ftexs",
            "4.ftexs",
            "5.ftexs",
            "6.ftexs",
            "ag.evf",
            "aia",
            "aib",
            "aibc",
            "aig",
            "aigc",
            "aim",
            "aip",
            "ait",
            "atsh",
            "bnd",
            "bnk",
            "cc.evf",
            "clo",
            "csnav",
            "dat",
            "des",
            "dnav",
            "dnav2",
            "eng.lng",
            "ese",
            "evb",
            "evf",
            "fag",
            "fage",
            "fago",
            "fagp",
            "fagx",
            "fclo",
            "fcnp",
            "fcnpx",
            "fdes",
            "fdmg",
            "ffnt",
            "fmdl",
            "fmdlb",
            "fmtt",
            "fnt",
            "fova",
            "fox",
            "fox2",
            "fpk",
            "fpkd",
            "fpkl",
            "frdv",
            "fre.lng",
            "frig",
            "frt",
            "fsd",
            "fsm",
            "fsml",
            "fsop",
            "fstb",
            "ftex",
            "fv2",
            "fx.evf",
            "fxp",
            "gani",
            "geom",
            "ger.lng",
            "gpfp",
            "grxla",
            "grxoc",
            "gskl",
            "htre",
            "info",
            "ita.lng",
            "jpn.lng",
            "json",
            "lad",
            "ladb",
            "lani",
            "las",
            "lba",
            "lng",
            "lpsh",
            "lua",
            "mas",
            "mbl",
            "mog",
            "mtar",
            "mtl",
            "nav2",
            "nta",
            "obr",
            "obrb",
            "parts",
            "path",
            "pftxs",
            "ph",
            "phep",
            "phsd",
            "por.lng",
            "qar",
            "rbs",
            "rdb",
            "rdf",
            "rnav",
            "rus.lng",
            "sad",
            "sand",
            "sani",
            "sbp",
            "sd.evf",
            "sdf",
            "sim",
            "simep",
            "snav",
            "spa.lng",
            "spch",
            "sub",
            "subp",
            "tgt",
            "tre2",
            "txt",
            "uia",
            "uif",
            "uig",
            "uigb",
            "uil",
            "uilb",
            "utxl",
            "veh",
            "vfx",
            "vfxbin",
            "vfxdb",
            "vnav",
            "vo.evf",
            "vpc",
            "wem",
            "xml"
        };

        private static readonly Dictionary<ulong, string> ExtensionsMap = FileExtensions.ToDictionary(HashFileExtension);

        static Hashing()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (File.Exists($"{path}\\fv2_path_dictionary.txt"))
                ReadQARDictionary($"{path}\\fv2_path_dictionary.txt");
            if (File.Exists($"{path}\\fv2_str_dictionary.txt"))
                ReadFmdlDictionary($"{path}\\fv2_str_dictionary.txt");
            if (File.Exists($"{path}\\cust_path_dictionary.txt"))
                ReadQARDictionary($"{path}\\cust_path_dictionary.txt");
            if (File.Exists($"{path}\\cust_str_dictionary.txt"))
                ReadFmdlDictionary($"{path}\\cust_str_dictionary.txt");
        } //constructor

        public static ulong HashFileExtension(string fileExtension) //from private to public
        {
            return HashFileName(fileExtension, false) & 0x1FFF;
        } //HashFileExtension

        /*
         * HashFileNameLegacy
         * Hashes a given string and outputs the ulong result.
         */
        public static ulong HashFileNameLegacy(string text, bool removeExtension = true)
        {
            if (removeExtension)
            {
                int index = text.IndexOf('.');
                text = index == -1 ? text : text.Substring(0, index);
            }

            const ulong seed0 = 0x9ae16a3b2f90404f;
            ulong seed1 = text.Length > 0 ? (uint)((text[0]) << 16) + (uint)text.Length : 0;
            return CityHash.CityHash.CityHash64WithSeeds(text + "\0", seed0, seed1) & 0xFFFFFFFFFFFF;
        } //HashFileNameLegacy

        public static ulong HashFileName(string text, bool removeExtension = true)
        {
            if (removeExtension)
            {
                int index = text.IndexOf('.');
                text = index == -1 ? text : text.Substring(0, index);
            }

            bool metaFlag = false;
            const string assetsConstant = "/Assets/";
            if (text.StartsWith(assetsConstant))
            {
                text = text.Substring(assetsConstant.Length);

                if (text.StartsWith("tpptest"))
                {
                    metaFlag = true;
                }
            }
            else
            {
                metaFlag = true;
            }

            text = text.TrimStart('/');

            const ulong seed0 = 0x9ae16a3b2f90404f;
            byte[] seed1Bytes = new byte[sizeof(ulong)];
            for (int i = text.Length - 1, j = 0; i >= 0 && j < sizeof(ulong); i--, j++)
            {
                seed1Bytes[j] = Convert.ToByte(text[i]);
            }
            ulong seed1 = BitConverter.ToUInt64(seed1Bytes, 0);
            ulong maskedHash = CityHash.CityHash.CityHash64WithSeeds(text, seed0, seed1) & 0x3FFFFFFFFFFFF;

            return metaFlag
                ? maskedHash | MetaFlag
                : maskedHash;
        } //HashFileName

        public static ulong HashFileNameWithExtension(string filePath)
        {
            filePath = DenormalizeFilePath(filePath);
            string hashablePart;
            string extensionPart;
            int extensionIndex = filePath.IndexOf(".", StringComparison.Ordinal);
            if (extensionIndex == -1)
            {
                hashablePart = filePath;
                extensionPart = "";
            }
            else
            {
                hashablePart = filePath.Substring(0, extensionIndex);
                extensionPart = filePath.Substring(extensionIndex + 1, filePath.Length - extensionIndex - 1);
            }

            ulong typeId = 0;
            var extensions = ExtensionsMap.Where(e => e.Value == extensionPart).ToList();
            if (extensions.Count == 1)
            {
                var extension = extensions.Single();
                typeId = extension.Key;
            }
            ulong hash = HashFileName(hashablePart);
            hash = (typeId << 51) | hash;
            return hash;
        } //HashFileNameWithExtension

        public static string DenormalizeFilePath(string filePath)
        {
            return filePath.Replace("\\", "/");
        } //DenormalizeFilePath

        public static void ReadFmdlDictionary(string path)
        {
            foreach (string line in File.ReadAllLines(path))
            {
                ulong hash = HashFileNameLegacy(line);

                if (fmdlDict.ContainsKey((uint)hash) == false)
                    fmdlDict.Add((uint)hash, line);
            } //foreach
        } //ReadFmdlDictionary

        public static void ReadQARDictionary(string path)
        {
            foreach (string line in File.ReadAllLines(path))
            {
                ulong hash = HashFileName(line) & 0x3FFFFFFFFFFFF;

                if (!qarDict.ContainsKey(hash))
                    qarDict.Add(hash, line);
            } //foreach
        } //ReadQARDictionary

        public static string TryGetQarName(ulong hash)
        {
            ulong extensionHash = hash >> 51;
            string extension;
            string name;
            ulong hashWithoutExtenstion = hash & 0x3FFFFFFFFFFFF;

            ExtensionsMap.TryGetValue(extensionHash, out extension);

            if (qarDict.TryGetValue(hashWithoutExtenstion, out name))
                return $"{name}.{extension}";

            return hash.ToString("x");
        } //TryGetQarName

        public static string TryGetFmdlName(uint hash)
        {
            string name;

            if (fmdlDict.TryGetValue(hash, out name))
                return name;

            return hash.ToString("x");
        } //TryGetFmdlName
    } //Hashing
} //namespace