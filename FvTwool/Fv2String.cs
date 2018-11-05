namespace FvTwool
{
    public class Fv2String
    {
        public struct TextureSwapEntry
        {
            public string materialInstanceName;
            public string textureTypeName;
            public int textureIndex;
        } //struct

        public struct BoneModelAttachEntry
        {
            public int fmdlIndex;
            public int frdvIndex;
            //public string unknownIndex0;
            //public string unknownIndex1;
            public int simIndex;
            //public string unknownIndex2;
        } //struct

        public struct CnpModelAttachEntry
        {
            public string cnpStrCode32;
            //public string emptyStrCode32;
            public int fmdlIndex;
            public int frdvIndex;
            //public string unknownIndex0;
            //public string unknownIndex1;
            public int simIndex;
            //public string unknownIndex2;
        } //struct

        public class VariableDataEntry
        {
            public int type = 0;
            public int meshGroupEntryCount = 0;
            public int textureSwapEntryCount = 0;
            public int boneModelAttachEntryCount = 0;
            public int cnpModelAttachEntryCount = 0;
            public VariableDataSubEntry[] variableDataSubEntries = new VariableDataSubEntry[0];
        } //struct

        public class VariableDataSubEntry
        {
            public string[] meshGroupEntries = new string[0];
            public TextureSwapEntry[] textureSwapEntries = new TextureSwapEntry[0];
            public BoneModelAttachEntry[] boneModelAttachEntries = new BoneModelAttachEntry[0];
            public CnpModelAttachEntry[] cnpModelAttachEntries = new CnpModelAttachEntry[0];
        } //VariableDataSubEntry

        public string[] hideMeshGroupEntries = new string[0];
        public string[] showMeshGroupEntries = new string[0];
        public TextureSwapEntry[] textureSwapEntries = new TextureSwapEntry[0];
        public BoneModelAttachEntry[] boneModelAttachEntries = new BoneModelAttachEntry[0];
        public CnpModelAttachEntry[] cnpModelAttachEntries = new CnpModelAttachEntry[0];
        public VariableDataEntry[] variableDataEntries = new VariableDataEntry[0];
        public string[] externalFiles = new string[0];
    } //class
} //namespace
