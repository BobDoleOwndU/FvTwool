namespace FvTwool
{
    public class Fv2
    {
        public struct TextureSwapEntry
        {
            public uint materialInstanceStrCode32 { get; set; }
            public uint textureTypeStrCode32 { get; set; }
            public short textureIndex { get; set; }
            public short unknown0 { get; set; }
        } //struct

        public struct BoneModelAttachEntry
        {
            public short fmdlIndex { get; set; }
            public short frdvIndex { get; set; }
            public short unknownIndex0 { get; set; }
            public short unknownIndex1 { get; set; }
            public short simIndex { get; set; }
            public short unknownIndex2 { get; set; }
        } //struct

        public struct CnpModelAttachEntry
        {
            public uint cnpStrCode32 { get; set; }
            public uint emptyStrCode32 { get; set; }
            public short fmdlIndex { get; set; }
            public short frdvIndex { get; set; }
            public short unknownIndex0 { get; set; }
            public short unknownIndex1 { get; set; }
            public short simIndex { get; set; }
            public short unknownIndex2 { get; set; }
        } //struct

        public struct VariableDataSectionEntry
        {
            public byte typeEnum { get; set; }
            public byte unknown0 { get; set; }
            public byte subEntryCount { get; set; }
            public byte meshGroupCount { get; set; }
            public byte textureSwapCount { get; set; }
            public byte unknown1 { get; set; }
            public byte boneModelAttachmentCount { get; set; }
            public byte cnpModelAttachmentCount { get; set; }
            public uint offset { get; set; }

            public uint[] meshGroupEntries { get; set; }
            public TextureSwapEntry[] textureSwapEntries { get; set; }
            public BoneModelAttachEntry[] boneModelAttachEntries { get; set; }
            public CnpModelAttachEntry[] cnpModelAttachEntries { get; set; }
        } //struct

        //Class Vars
        public ulong signature { get; set; }
        public ushort variableDataSectionOffset { get; set; }
        public ushort externalFileSectionOffset { get; set; }
        public ushort variableDataSectionCount { get; set; }
        public ushort externalFileSectionCount { get; set; }
        public uint dataSectionsLength { get; set; }
        public ushort textureCount { get; set; }
        public byte hideMeshGroupCount { get; set; }
        public byte showMeshGroupCount { get; set; }
        public byte materialInstanceCount { get; set; }
        public byte unknown0 { get; set; }
        public byte boneModelAttachmentCount { get; set; }
        public byte cnpModelAttachmentCount { get; set; }

        public uint[] hideMeshGroupEntries { get; set; }
        public uint[] showMeshGroupEntries { get; set; }
        public TextureSwapEntry[] textureSwapEntries { get; set; }
        public BoneModelAttachEntry[] boneModelAttachEntries { get; set; }
        public CnpModelAttachEntry[] cnpModelAttachEntries { get; set; }
        public VariableDataSectionEntry[] variableDataSectionEntries { get; set; }

        public void GetDataFromFv2String(Fv2String fv2String)
        {
            hideMeshGroupEntries = new uint[fv2String.hideMeshGroupEntries.Length];
            showMeshGroupEntries = new uint[fv2String.showMeshGroupEntries.Length];
            textureSwapEntries = new TextureSwapEntry[fv2String.textureSwapEntries.Length];
            boneModelAttachEntries = new BoneModelAttachEntry[fv2String.boneModelAttachEntries.Length];
            cnpModelAttachEntries = new CnpModelAttachEntry[fv2String.cnpModelAttachEntries.Length];
            variableDataSectionEntries = new VariableDataSectionEntry[fv2String.variableDataEntries.Length];

            for(int i = 0; i < hideMeshGroupEntries.Length; i++)
                hideMeshGroupEntries[i] = (uint)Hashing.HashFileNameLegacy(fv2String.hideMeshGroupEntries[i]);

            for (int i = 0; i < showMeshGroupEntries.Length; i++)
                showMeshGroupEntries[i] = (uint)Hashing.HashFileNameLegacy(fv2String.showMeshGroupEntries[i]);

            for(int i = 0; i < textureSwapEntries.Length; i++)
            {
                textureSwapEntries[i].materialInstanceStrCode32 = (uint)Hashing.HashFileNameLegacy(fv2String.textureSwapEntries[i].materialInstanceName);
                textureSwapEntries[i].textureTypeStrCode32 = (uint)Hashing.HashFileNameLegacy(fv2String.textureSwapEntries[i].textureTypeName);
                textureSwapEntries[i].textureIndex = (short)fv2String.textureSwapEntries[i].textureName;
                textureSwapEntries[i].unknown0 = -1;
            } //for
        } //GetDataFromFv2String
    } //class
} //namespace
