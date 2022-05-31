using System;
using System.IO;
using System.Windows.Forms;

namespace FvTwool
{
    public class Fv2
    {
        public struct TextureSwapEntry
        {
            public uint materialInstanceStrCode32 { get; set; }
            public uint textureTypeStrCode32 { get; set; }
            public short textureIndex { get; set; }
            public short materialParameterIndex { get; set; }
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

        public struct VariableDataEntry
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

            public VariableDataSubEntry[] variableDataSubEntries { get; set; }
        } //struct

        public struct VariableDataSubEntry
        {
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
        public uint materialVectorSectionOffset { get; set; }
        public uint materialParameterSectionCount { get; set; }
        public ushort textureCount { get; set; }
        public byte hideMeshGroupCount { get; set; }
        public byte showMeshGroupCount { get; set; }
        public byte textureSwapCount { get; set; }
        public byte unknown0 { get; set; }
        public byte boneModelAttachmentCount { get; set; }
        public byte cnpModelAttachmentCount { get; set; }

        public uint[] hideMeshGroupEntries { get; set; }
        public uint[] showMeshGroupEntries { get; set; }
        public TextureSwapEntry[] textureSwapEntries { get; set; }
        public BoneModelAttachEntry[] boneModelAttachEntries { get; set; }
        public CnpModelAttachEntry[] cnpModelAttachEntries { get; set; }
        public VariableDataEntry[] variableDataEntries { get; set; }
        public Vector4[] materialParameterEntries { get; set; }
        public ulong[] externalFileEntries { get; set; }

        public void Read(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            {
                try
                {
                    BinaryReader reader = new BinaryReader(stream);

                    signature = reader.ReadUInt64();
                    variableDataSectionOffset = reader.ReadUInt16();
                    externalFileSectionOffset = reader.ReadUInt16();
                    variableDataSectionCount = reader.ReadUInt16();
                    externalFileSectionCount = reader.ReadUInt16();
                    materialVectorSectionOffset = reader.ReadUInt32();
                    materialParameterSectionCount = reader.ReadUInt32();
                    textureCount = reader.ReadUInt16();
                    reader.BaseStream.Position += 6;
                    hideMeshGroupCount = reader.ReadByte();
                    showMeshGroupCount = reader.ReadByte();
                    textureSwapCount = reader.ReadByte();
                    unknown0 = reader.ReadByte();
                    boneModelAttachmentCount = reader.ReadByte();
                    cnpModelAttachmentCount = reader.ReadByte();
                    reader.BaseStream.Position += 2;

                    variableDataEntries = new VariableDataEntry[variableDataSectionCount];
                    materialParameterEntries = new Vector4[materialParameterSectionCount];
                    externalFileEntries = new ulong[externalFileSectionCount];
                    hideMeshGroupEntries = new uint[hideMeshGroupCount];
                    showMeshGroupEntries = new uint[showMeshGroupCount];
                    textureSwapEntries = new TextureSwapEntry[textureSwapCount];
                    boneModelAttachEntries = new BoneModelAttachEntry[boneModelAttachmentCount];
                    cnpModelAttachEntries = new CnpModelAttachEntry[cnpModelAttachmentCount];

                    for (int i = 0; i < hideMeshGroupCount; i++)
                        hideMeshGroupEntries[i] = reader.ReadUInt32();

                    for (int i = 0; i < showMeshGroupCount; i++)
                        showMeshGroupEntries[i] = reader.ReadUInt32();

                    for (int i = 0; i < textureSwapCount; i++)
                        textureSwapEntries[i].materialInstanceStrCode32 = reader.ReadUInt32();
                    for (int i = 0; i < textureSwapCount; i++)
                        textureSwapEntries[i].textureTypeStrCode32 = reader.ReadUInt32();
                    for (int i = 0; i < textureSwapCount; i++)
                        textureSwapEntries[i].textureIndex = reader.ReadInt16();
                    for (int i = 0; i < textureSwapCount; i++)
                        textureSwapEntries[i].materialParameterIndex = reader.ReadInt16();

                    for (int i = 0; i < boneModelAttachmentCount; i++)
                    {
                        boneModelAttachEntries[i].fmdlIndex = reader.ReadInt16();
                        boneModelAttachEntries[i].frdvIndex = reader.ReadInt16();
                        boneModelAttachEntries[i].unknownIndex0 = reader.ReadInt16();
                        boneModelAttachEntries[i].unknownIndex1 = reader.ReadInt16();
                        boneModelAttachEntries[i].simIndex = reader.ReadInt16();
                        boneModelAttachEntries[i].unknownIndex2 = reader.ReadInt16();
                    } //for

                    for (int i = 0; i < cnpModelAttachmentCount; i++)
                    {
                        cnpModelAttachEntries[i].cnpStrCode32 = reader.ReadUInt32();
                        cnpModelAttachEntries[i].emptyStrCode32 = reader.ReadUInt32();
                        cnpModelAttachEntries[i].fmdlIndex = reader.ReadInt16();
                        cnpModelAttachEntries[i].frdvIndex = reader.ReadInt16();
                        cnpModelAttachEntries[i].unknownIndex0 = reader.ReadInt16();
                        cnpModelAttachEntries[i].unknownIndex1 = reader.ReadInt16();
                        cnpModelAttachEntries[i].simIndex = reader.ReadInt16();
                        cnpModelAttachEntries[i].unknownIndex2 = reader.ReadInt16();
                    } //for

                    for(int i = 0; i < variableDataSectionCount; i++)
                    {
                        reader.BaseStream.Position = variableDataSectionOffset + 0x10 * i;

                        variableDataEntries[i].typeEnum = reader.ReadByte();
                        variableDataEntries[i].unknown0 = reader.ReadByte();
                        variableDataEntries[i].subEntryCount = reader.ReadByte();
                        variableDataEntries[i].meshGroupCount = reader.ReadByte();
                        variableDataEntries[i].textureSwapCount = reader.ReadByte();
                        variableDataEntries[i].unknown1 = reader.ReadByte();
                        variableDataEntries[i].boneModelAttachmentCount = reader.ReadByte();
                        variableDataEntries[i].cnpModelAttachmentCount = reader.ReadByte();
                        reader.BaseStream.Position += 4;
                        variableDataEntries[i].offset = reader.ReadUInt32();

                        variableDataEntries[i].variableDataSubEntries = new VariableDataSubEntry[variableDataEntries[i].subEntryCount];
                        reader.BaseStream.Position = variableDataEntries[i].offset;

                        for(int j = 0; j < variableDataEntries[i].subEntryCount; j++)
                        {
                            variableDataEntries[i].variableDataSubEntries[j].meshGroupEntries = new uint[variableDataEntries[i].meshGroupCount];
                            variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries = new TextureSwapEntry[variableDataEntries[i].textureSwapCount];
                            variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries = new BoneModelAttachEntry[variableDataEntries[i].boneModelAttachmentCount];
                            variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries = new CnpModelAttachEntry[variableDataEntries[i].cnpModelAttachmentCount];

                            for (int k = 0; k < variableDataEntries[i].meshGroupCount; k++)
                                variableDataEntries[i].variableDataSubEntries[j].meshGroupEntries[k] = reader.ReadUInt32();

                            for (int k = 0; k < variableDataEntries[i].textureSwapCount; k++)
                                variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].materialInstanceStrCode32 = reader.ReadUInt32();
                            for (int k = 0; k < variableDataEntries[i].textureSwapCount; k++)
                                variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].textureTypeStrCode32 = reader.ReadUInt32();
                            for (int k = 0; k < variableDataEntries[i].textureSwapCount; k++)
                                variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].textureIndex = reader.ReadInt16();
                            for (int k = 0; k < variableDataEntries[i].textureSwapCount; k++)
                                variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].materialParameterIndex = reader.ReadInt16();

                            for (int k = 0; k < variableDataEntries[i].boneModelAttachmentCount; k++)
                            {
                                variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].fmdlIndex = reader.ReadInt16();
                                variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].frdvIndex = reader.ReadInt16();
                                variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].unknownIndex0 = reader.ReadInt16();
                                variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].unknownIndex1 = reader.ReadInt16();
                                variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].simIndex = reader.ReadInt16();
                                variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].unknownIndex2 = reader.ReadInt16();
                            } //for

                            for (int k = 0; k < variableDataEntries[i].cnpModelAttachmentCount; k++)
                            {
                                variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].cnpStrCode32 = reader.ReadUInt32();
                                variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].emptyStrCode32 = reader.ReadUInt32();
                                variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].fmdlIndex = reader.ReadInt16();
                                variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].frdvIndex = reader.ReadInt16();
                                variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].unknownIndex0 = reader.ReadInt16();
                                variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].unknownIndex1 = reader.ReadInt16();
                                variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].simIndex = reader.ReadInt16();
                                variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].unknownIndex2 = reader.ReadInt16();
                            } //for
                        } //for
                    } //for

                    reader.BaseStream.Position = materialVectorSectionOffset;

                    for (int i = 0; i < materialParameterSectionCount; i++)
                    {
                        materialParameterEntries[i] = new Vector4();

                        for (int j = 0; j < 4; j++)
                            materialParameterEntries[i][j] = reader.ReadSingle();
                    } //for

                    reader.BaseStream.Position = externalFileSectionOffset;

                    for (int i = 0; i < externalFileSectionCount; i++)
                        externalFileEntries[i] = reader.ReadUInt64();
                } //try
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                } //catch
                finally
                {
                    stream.Close();
                } //finally
            } //using
        } //Read

        public void Write(string filePath)
        {
            signature = 0x016E697732564F46;
            variableDataSectionCount = (ushort)variableDataEntries.Length;
            externalFileSectionCount = (ushort)externalFileEntries.Length;
            materialParameterSectionCount = (uint)materialParameterEntries.Length;
            hideMeshGroupCount = (byte)hideMeshGroupEntries.Length;
            showMeshGroupCount = (byte)showMeshGroupEntries.Length;
            textureSwapCount = (byte)textureSwapEntries.Length;
            unknown0 = 0;
            boneModelAttachmentCount = (byte)boneModelAttachEntries.Length;
            cnpModelAttachmentCount = (byte)cnpModelAttachEntries.Length;

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                try
                {
                    BinaryWriter writer = new BinaryWriter(stream);

                    writer.Write(signature);
                    writer.WriteZeroes(4);
                    writer.Write(variableDataSectionCount);
                    writer.Write(externalFileSectionCount);
                    writer.WriteZeroes(4);
                    writer.Write(materialParameterSectionCount);
                    writer.Write(textureCount);
                    writer.WriteZeroes(6);
                    writer.Write(hideMeshGroupCount);
                    writer.Write(showMeshGroupCount);
                    writer.Write(textureSwapCount);
                    writer.Write(unknown0);
                    writer.Write(boneModelAttachmentCount);
                    writer.Write(cnpModelAttachmentCount);
                    writer.WriteZeroes(2);

                    for (int i = 0; i < hideMeshGroupCount; i++)
                        writer.Write(hideMeshGroupEntries[i]);

                    for (int i = 0; i < showMeshGroupCount; i++)
                        writer.Write(showMeshGroupEntries[i]);

                    for(int i = 0; i < textureSwapCount; i++)
                        writer.Write(textureSwapEntries[i].materialInstanceStrCode32);

                    for (int i = 0; i < textureSwapCount; i++)
                        writer.Write(textureSwapEntries[i].textureTypeStrCode32);

                    for (int i = 0; i < textureSwapCount; i++)
                        writer.Write(textureSwapEntries[i].textureIndex);

                    for (int i = 0; i < textureSwapCount; i++)
                        writer.Write(textureSwapEntries[i].materialParameterIndex);

                    for (int i = 0; i < boneModelAttachmentCount; i++)
                    {
                        writer.Write(boneModelAttachEntries[i].fmdlIndex);
                        writer.Write(boneModelAttachEntries[i].frdvIndex);
                        writer.Write(boneModelAttachEntries[i].unknownIndex0);
                        writer.Write(boneModelAttachEntries[i].unknownIndex1);
                        writer.Write(boneModelAttachEntries[i].simIndex);
                        writer.Write(boneModelAttachEntries[i].unknownIndex2);
                    } //for

                    for (int i = 0; i < cnpModelAttachmentCount; i++)
                    {
                        writer.Write(cnpModelAttachEntries[i].cnpStrCode32);
                        writer.Write(cnpModelAttachEntries[i].emptyStrCode32);
                        writer.Write(cnpModelAttachEntries[i].fmdlIndex);
                        writer.Write(cnpModelAttachEntries[i].frdvIndex);
                        writer.Write(cnpModelAttachEntries[i].unknownIndex0);
                        writer.Write(cnpModelAttachEntries[i].unknownIndex1);
                        writer.Write(cnpModelAttachEntries[i].simIndex);
                        writer.Write(cnpModelAttachEntries[i].unknownIndex2);
                    } //for

                    if (writer.BaseStream.Position % 0x10 != 0)
                        writer.WriteZeroes(0x10 - (int)writer.BaseStream.Position % 0x10);

                    variableDataSectionOffset = (ushort)writer.BaseStream.Position;

                    for(int i = 0; i < variableDataSectionCount; i++)
                    {
                        writer.Write(variableDataEntries[i].typeEnum);
                        writer.Write(variableDataEntries[i].unknown0);
                        writer.Write(variableDataEntries[i].subEntryCount);
                        writer.Write(variableDataEntries[i].meshGroupCount);
                        writer.Write(variableDataEntries[i].textureSwapCount);
                        writer.Write(variableDataEntries[i].unknown1);
                        writer.Write(variableDataEntries[i].boneModelAttachmentCount);
                        writer.Write(variableDataEntries[i].cnpModelAttachmentCount);
                        writer.WriteZeroes(8);
                    } //for

                    for (int i = 0; i < variableDataSectionCount; i++)
                    {
                        variableDataEntries[i].offset = (uint)writer.BaseStream.Position;

                        for(int j = 0; j < variableDataEntries[i].subEntryCount; j++)
                        {
                            for(int k = 0; k < variableDataEntries[i].meshGroupCount; k++)
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].meshGroupEntries[k]);

                            for(int k = 0; k < variableDataEntries[i].textureSwapCount; k++)
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].materialInstanceStrCode32);

                            for (int k = 0; k < variableDataEntries[i].textureSwapCount; k++)
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].textureTypeStrCode32);

                            for (int k = 0; k < variableDataEntries[i].textureSwapCount; k++)
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].textureIndex);

                            for (int k = 0; k < variableDataEntries[i].textureSwapCount; k++)
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].materialParameterIndex);

                            for (int k = 0; k < variableDataEntries[i].boneModelAttachmentCount; k++)
                            {
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].fmdlIndex);
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].frdvIndex);
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].unknownIndex0);
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].unknownIndex1);
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].simIndex);
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].unknownIndex2);
                            } //for

                            for (int k = 0; k < variableDataEntries[i].cnpModelAttachmentCount; k++)
                            {
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].cnpStrCode32);
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].emptyStrCode32);
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].fmdlIndex);
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].frdvIndex);
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].unknownIndex0);
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].unknownIndex1);
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].simIndex);
                                writer.Write(variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].unknownIndex2);
                            } //for
                        } //for
                    } //for

                    if (writer.BaseStream.Position % 0x10 != 0)
                        writer.WriteZeroes(0x10 - (int)writer.BaseStream.Position % 0x10);

                    materialVectorSectionOffset = (ushort)writer.BaseStream.Position;

                    for (int i = 0; i < materialParameterSectionCount; i++)
                        for (int j = 0; j < 4; j++)
                            writer.Write(materialParameterEntries[i][j]);

                    externalFileSectionOffset = (ushort)writer.BaseStream.Position;

                    for (int i = 0; i < externalFileSectionCount; i++)
                        writer.Write(externalFileEntries[i]);

                    //Offset writing time!
                    writer.BaseStream.Position = 8;
                    writer.Write(variableDataSectionOffset);
                    writer.Write(externalFileSectionOffset);

                    writer.BaseStream.Position = 0x10;
                    writer.Write(materialVectorSectionOffset);

                    for (int i = 0; i < variableDataSectionCount; i++)
                    {
                        writer.BaseStream.Position = variableDataSectionOffset + 0x10 * i + 0xC;
                        writer.Write(variableDataEntries[i].offset);
                    } //for
                } //try
                catch (Exception e)
                {
                    stream.Close();
                    MessageBox.Show($"{e.Message}\n{e.StackTrace}");
                } //catch
            } //using
        } //Write

        public void GetDataFromFv2String(Fv2String fv2String)
        {
            hideMeshGroupEntries = new uint[fv2String.hideMeshGroupEntries.Length];
            showMeshGroupEntries = new uint[fv2String.showMeshGroupEntries.Length];
            textureSwapEntries = new TextureSwapEntry[fv2String.textureSwapEntries.Length];
            boneModelAttachEntries = new BoneModelAttachEntry[fv2String.boneModelAttachEntries.Length];
            cnpModelAttachEntries = new CnpModelAttachEntry[fv2String.cnpModelAttachEntries.Length];
            variableDataEntries = new VariableDataEntry[fv2String.variableDataEntries.Length];
            materialParameterEntries = new Vector4[fv2String.materialParameterEntries.Length];
            externalFileEntries = new ulong[fv2String.externalFiles.Length];
            textureCount = 0;

            for (int i = 0; i < hideMeshGroupEntries.Length; i++)
            {
                try
                {
                    hideMeshGroupEntries[i] = (uint)UInt64.Parse(fv2String.hideMeshGroupEntries[i], System.Globalization.NumberStyles.HexNumber);
                } //try
                catch
                {
                    hideMeshGroupEntries[i] = (uint)Hashing.HashFileNameLegacy(fv2String.hideMeshGroupEntries[i]);
                } //catch
            } //for

            for (int i = 0; i < showMeshGroupEntries.Length; i++)
            {
                try
                {
                    showMeshGroupEntries[i] = (uint)UInt64.Parse(fv2String.showMeshGroupEntries[i], System.Globalization.NumberStyles.HexNumber);
                } //try
                catch
                {
                    showMeshGroupEntries[i] = (uint)Hashing.HashFileNameLegacy(fv2String.showMeshGroupEntries[i]);
                } //catch
            } //for

            for (int i = 0; i < textureSwapEntries.Length; i++)
            {
                try
                {
                    textureSwapEntries[i].materialInstanceStrCode32 = (uint)UInt64.Parse(fv2String.textureSwapEntries[i].materialInstanceName, System.Globalization.NumberStyles.HexNumber);
                } //try
                catch
                {
                    textureSwapEntries[i].materialInstanceStrCode32 = (uint)Hashing.HashFileNameLegacy(fv2String.textureSwapEntries[i].materialInstanceName);
                } //catch

                try
                {
                    textureSwapEntries[i].textureTypeStrCode32 = (uint)UInt64.Parse(fv2String.textureSwapEntries[i].textureTypeName, System.Globalization.NumberStyles.HexNumber);
                } //try
                catch
                {
                    textureSwapEntries[i].textureTypeStrCode32 = (uint)Hashing.HashFileNameLegacy(fv2String.textureSwapEntries[i].textureTypeName);
                } //catch

                textureSwapEntries[i].textureIndex = (short)fv2String.textureSwapEntries[i].textureIndex;
                textureSwapEntries[i].materialParameterIndex = (short)fv2String.textureSwapEntries[i].materialParameterIndex;
            } //for

            for (int i = 0; i < boneModelAttachEntries.Length; i++)
            {
                boneModelAttachEntries[i].fmdlIndex = (short)fv2String.boneModelAttachEntries[i].fmdlIndex;
                boneModelAttachEntries[i].frdvIndex = (short)fv2String.boneModelAttachEntries[i].frdvIndex;
                boneModelAttachEntries[i].unknownIndex0 = -1;
                boneModelAttachEntries[i].unknownIndex1 = -1;
                boneModelAttachEntries[i].simIndex = (short)fv2String.boneModelAttachEntries[i].simIndex;
                boneModelAttachEntries[i].unknownIndex2 = -1;
            } //for

            for (int i = 0; i < cnpModelAttachEntries.Length; i++)
            {
                try
                {
                    cnpModelAttachEntries[i].cnpStrCode32 = (uint)UInt64.Parse(fv2String.cnpModelAttachEntries[i].cnpStrCode32, System.Globalization.NumberStyles.HexNumber);
                } //try
                catch
                {
                    cnpModelAttachEntries[i].cnpStrCode32 = (uint)Hashing.HashFileNameLegacy(fv2String.cnpModelAttachEntries[i].cnpStrCode32);
                } //catch

                cnpModelAttachEntries[i].emptyStrCode32 = 0xBF169F98;
                cnpModelAttachEntries[i].fmdlIndex = (short)fv2String.cnpModelAttachEntries[i].fmdlIndex;
                cnpModelAttachEntries[i].frdvIndex = (short)fv2String.cnpModelAttachEntries[i].frdvIndex;
                cnpModelAttachEntries[i].unknownIndex0 = -1;
                cnpModelAttachEntries[i].unknownIndex1 = -1;
                cnpModelAttachEntries[i].simIndex = (short)fv2String.cnpModelAttachEntries[i].simIndex;
                cnpModelAttachEntries[i].unknownIndex2 = -1;
            } //for

            for (int i = 0; i < variableDataEntries.Length; i++)
            {
                variableDataEntries[i].typeEnum = (byte)fv2String.variableDataEntries[i].type;
                variableDataEntries[i].unknown0 = 0x2;
                variableDataEntries[i].subEntryCount = (byte)fv2String.variableDataEntries[i].variableDataSubEntries.Length;
                variableDataEntries[i].meshGroupCount = (byte)fv2String.variableDataEntries[i].meshGroupEntryCount;
                variableDataEntries[i].textureSwapCount = (byte)fv2String.variableDataEntries[i].textureSwapEntryCount;
                variableDataEntries[i].unknown1 = 0x0;
                variableDataEntries[i].boneModelAttachmentCount = (byte)fv2String.variableDataEntries[i].boneModelAttachEntryCount;
                variableDataEntries[i].cnpModelAttachmentCount = (byte)fv2String.variableDataEntries[i].cnpModelAttachEntryCount;

                variableDataEntries[i].variableDataSubEntries = new VariableDataSubEntry[fv2String.variableDataEntries[i].variableDataSubEntries.Length];

                for (int j = 0; j < variableDataEntries[i].subEntryCount; j++)
                {
                    variableDataEntries[i].variableDataSubEntries[j].meshGroupEntries = new uint[variableDataEntries[i].meshGroupCount];
                    variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries = new TextureSwapEntry[variableDataEntries[i].textureSwapCount];
                    variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries = new BoneModelAttachEntry[variableDataEntries[i].boneModelAttachmentCount];
                    variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries = new CnpModelAttachEntry[variableDataEntries[i].cnpModelAttachmentCount];

                    for (int k = 0; k < variableDataEntries[i].meshGroupCount; k++)
                        try
                        {
                            variableDataEntries[i].variableDataSubEntries[j].meshGroupEntries[k] = (uint)UInt64.Parse(fv2String.variableDataEntries[i].variableDataSubEntries[j].meshGroupEntries[k], System.Globalization.NumberStyles.HexNumber);
                        } //try
                        catch
                        {
                            variableDataEntries[i].variableDataSubEntries[j].meshGroupEntries[k] = (uint)Hashing.HashFileNameLegacy(fv2String.variableDataEntries[i].variableDataSubEntries[j].meshGroupEntries[k]);
                        } //catch

                    for(int k = 0; k < variableDataEntries[i].textureSwapCount; k++)
                    {
                        try
                        {
                            variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].materialInstanceStrCode32 = (uint)UInt64.Parse(fv2String.variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].materialInstanceName, System.Globalization.NumberStyles.HexNumber);
                        } //try
                        catch
                        {
                            variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].materialInstanceStrCode32 = (uint)Hashing.HashFileNameLegacy(fv2String.variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].materialInstanceName);
                        } //catch

                        try
                        {
                            variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].textureTypeStrCode32 = (uint)UInt64.Parse(fv2String.variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].textureTypeName, System.Globalization.NumberStyles.HexNumber);
                        } //try
                        catch
                        {
                            variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].textureTypeStrCode32 = (uint)Hashing.HashFileNameLegacy(fv2String.variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].textureTypeName);
                        } //catch
                        
                        variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].textureIndex = (short)fv2String.variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].textureIndex;
                        variableDataEntries[i].variableDataSubEntries[j].textureSwapEntries[k].materialParameterIndex = -1;
                    } //for

                    for (int k = 0; k < variableDataEntries[i].boneModelAttachmentCount; k++)
                    {
                        variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].fmdlIndex = (short)fv2String.variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].fmdlIndex;
                        variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].frdvIndex = (short)fv2String.variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].frdvIndex;
                        variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].unknownIndex0 = -1;
                        variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].unknownIndex1 = -1;
                        variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].simIndex = (short)fv2String.variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].simIndex;
                        variableDataEntries[i].variableDataSubEntries[j].boneModelAttachEntries[k].unknownIndex2 = -1;
                    } //for

                    for (int k = 0; k < variableDataEntries[i].cnpModelAttachmentCount; k++)
                    {
                        try
                        {
                            variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].cnpStrCode32 = (uint)UInt64.Parse(fv2String.variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].cnpStrCode32, System.Globalization.NumberStyles.HexNumber);
                        } //try
                        catch
                        {
                            variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].cnpStrCode32 = (uint)Hashing.HashFileNameLegacy(fv2String.variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].cnpStrCode32);
                        } //catch

                        variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].emptyStrCode32 = 0xBF169F98;
                        variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].fmdlIndex = (short)fv2String.variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].fmdlIndex;
                        variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].frdvIndex = (short)fv2String.variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].frdvIndex;
                        variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].unknownIndex0 = -1;
                        variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].unknownIndex1 = -1;
                        variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].simIndex = (short)fv2String.variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].simIndex;
                        variableDataEntries[i].variableDataSubEntries[j].cnpModelAttachEntries[k].unknownIndex2 = -1;
                    } //for
                } //for
            } //for

            for (int i = 0; i < materialParameterEntries.Length; i++)
            {
                materialParameterEntries[i] = new Vector4();

                for (int j = 0; j < 4; j++)
                    materialParameterEntries[i][j] = Single.Parse(fv2String.materialParameterEntries[i][j]);
            } //for

            for (int i = 0; i < externalFileEntries.Length; i++)
            {
                try
                {
                    externalFileEntries[i] = UInt64.Parse(fv2String.externalFiles[i], System.Globalization.NumberStyles.HexNumber);
                    
                } //try
                catch
                {
                    externalFileEntries[i] = Hashing.HashFileNameWithExtension(fv2String.externalFiles[i]);
                } //catch

                if((externalFileEntries[i] & 0x1568000000000000) == 0x1568000000000000)
                    textureCount++;
            } //for
        } //GetDataFromFv2String
    } //class
} //namespace
