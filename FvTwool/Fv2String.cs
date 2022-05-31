using System;

namespace FvTwool
{
    public class Fv2String
    {
        public struct TextureSwapEntry
        {
            public string materialInstanceName;
            public string textureTypeName;
            public int textureIndex;
            public int materialParameterIndex;
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
        public Vector4String[] materialParameterEntries = new Vector4String[0];
        public string[] externalFiles = new string[0];

        public void GetDataFromFv2(Fv2 fv2)
        {
            hideMeshGroupEntries = new string[fv2.hideMeshGroupCount];
            showMeshGroupEntries = new string[fv2.showMeshGroupCount];
            textureSwapEntries = new TextureSwapEntry[fv2.textureSwapCount];
            boneModelAttachEntries = new BoneModelAttachEntry[fv2.boneModelAttachmentCount];
            cnpModelAttachEntries = new CnpModelAttachEntry[fv2.cnpModelAttachmentCount];
            variableDataEntries = new VariableDataEntry[fv2.variableDataSectionCount];
            materialParameterEntries = new Vector4String[fv2.materialParameterSectionCount];
            externalFiles = new string[fv2.externalFileSectionCount];

            for (int i = 0; i < fv2.hideMeshGroupCount; i++)
                hideMeshGroupEntries[i] = Hashing.TryGetFmdlName(fv2.hideMeshGroupEntries[i]);

            for (int i = 0; i < fv2.showMeshGroupCount; i++)
                showMeshGroupEntries[i] = Hashing.TryGetFmdlName(fv2.showMeshGroupEntries[i]);

            for (int i = 0; i < fv2.textureSwapCount; i++)
            {
                textureSwapEntries[i].materialInstanceName = Hashing.TryGetFmdlName(fv2.textureSwapEntries[i].materialInstanceStrCode32);
                textureSwapEntries[i].textureTypeName = Hashing.TryGetFmdlName(fv2.textureSwapEntries[i].textureTypeStrCode32);
                textureSwapEntries[i].textureIndex = fv2.textureSwapEntries[i].textureIndex;
                textureSwapEntries[i].materialParameterIndex = fv2.textureSwapEntries[i].materialParameterIndex;
            } //for

            for (int i = 0; i < fv2.boneModelAttachmentCount; i++)
            {
                boneModelAttachEntries[i].fmdlIndex = fv2.boneModelAttachEntries[i].fmdlIndex;
                boneModelAttachEntries[i].frdvIndex = fv2.boneModelAttachEntries[i].frdvIndex;
                boneModelAttachEntries[i].simIndex = fv2.boneModelAttachEntries[i].simIndex;
            } //for

            for (int i = 0; i < fv2.cnpModelAttachmentCount; i++)
            {
                cnpModelAttachEntries[i].cnpStrCode32 = Hashing.TryGetFmdlName(fv2.cnpModelAttachEntries[i].cnpStrCode32);
                cnpModelAttachEntries[i].fmdlIndex = fv2.cnpModelAttachEntries[i].fmdlIndex;
                cnpModelAttachEntries[i].frdvIndex = fv2.cnpModelAttachEntries[i].frdvIndex;
                cnpModelAttachEntries[i].simIndex = fv2.cnpModelAttachEntries[i].simIndex;
            } //for

            for (int i = 0; i < fv2.variableDataSectionCount; i++)
            {
                variableDataEntries[i] = new VariableDataEntry();
                variableDataEntries[i].type = fv2.variableDataEntries[i].typeEnum;
                variableDataEntries[i].meshGroupEntryCount = fv2.variableDataEntries[i].meshGroupCount;
                variableDataEntries[i].textureSwapEntryCount = fv2.variableDataEntries[i].textureSwapCount;
                variableDataEntries[i].boneModelAttachEntryCount = fv2.variableDataEntries[i].boneModelAttachmentCount;
                variableDataEntries[i].cnpModelAttachEntryCount = fv2.variableDataEntries[i].cnpModelAttachmentCount;

                variableDataEntries[i].variableDataSubEntries = new VariableDataSubEntry[fv2.variableDataEntries[i].subEntryCount];

                for (int j = 0; j < fv2.variableDataEntries[i].subEntryCount; j++)
                {
                    variableDataEntries[i].variableDataSubEntries[j] = new VariableDataSubEntry();

                    variableDataEntries[i].variableDataSubEntries[j].meshGroupEntries = new string[fv2.variableDataEntries[i].meshGroupCount];
                    variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries = new TextureSwapEntry[fv2.variableDataEntries[i].textureSwapCount];
                    variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries = new BoneModelAttachEntry[fv2.variableDataEntries[i].boneModelAttachmentCount];
                    variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries = new CnpModelAttachEntry[fv2.variableDataEntries[i].cnpModelAttachmentCount];

                    for (int k = 0; k < fv2.variableDataEntries[i].meshGroupCount; k++)
                        variableDataEntries[i].variableDataSubEntries[j].meshGroupEntries[k] = Hashing.TryGetFmdlName(fv2.variableDataEntries[i].variableDataSubEntries[j].meshGroupEntries[k]);

                    for (int k = 0; k < fv2.variableDataEntries[i].textureSwapCount; k++)
                    {
                        variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].materialInstanceName = Hashing.TryGetFmdlName(fv2.variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].materialInstanceStrCode32);
                        variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].textureTypeName = Hashing.TryGetFmdlName(fv2.variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].textureTypeStrCode32);
                        variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].textureIndex = fv2.variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].textureIndex;
                    } //for

                    for (int k = 0; k < fv2.variableDataEntries[i].boneModelAttachmentCount; k++)
                    {
                        variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].fmdlIndex = fv2.variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].fmdlIndex;
                        variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].frdvIndex = fv2.variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].frdvIndex;
                        variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].simIndex = fv2.variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].simIndex;
                    } //for

                    for (int k = 0; k < fv2.variableDataEntries[i].cnpModelAttachmentCount; k++)
                    {
                        variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].cnpStrCode32 = Hashing.TryGetFmdlName(fv2.variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].cnpStrCode32);
                        variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].fmdlIndex = fv2.variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].fmdlIndex;
                        variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].frdvIndex = fv2.variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].frdvIndex;
                        variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].simIndex = fv2.variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].simIndex;
                    } //for
                } //for
            } //for

            for (int i = 0; i < fv2.materialParameterSectionCount; i++)
            {
                materialParameterEntries[i] = new Vector4String();

                for (int j = 0; j < 4; j++)
                    materialParameterEntries[i][j] = fv2.materialParameterEntries[i][j].ToString();
            } //for

            for (int i = 0; i < fv2.externalFileSectionCount; i++)
            {
                externalFiles[i] = Hashing.TryGetQarName(fv2.externalFileEntries[i]);
            } //for
        } //GetDataFromFv2
    } //class
} //namespace
