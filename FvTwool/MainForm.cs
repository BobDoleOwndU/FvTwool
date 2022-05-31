﻿using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace FvTwool
{
    /****************************************************************
     * 
     * WARNING TO ALL WHO VENTURE FORTH:
     * CODE FROM YOUR WORST NIGHTMARE AWAITS!
     * 
     ****************************************************************/
    public partial class MainForm : Form
    {
        const int CONTROL_SPACING = 43;
        const int LABEL_SPACING = 3;
        const int LABEL_WIDTH = 126;
        Fv2String fv2String = new Fv2String();

        public MainForm(string[] args)
        {
            InitializeComponent();

            if(args.Length > 0)
            {
                Read(args[0]);
            }
        } //MainForm

        public void Read(string path)
        {
            if (Path.GetExtension(path) == ".fv2")
            {
                Fv2 fv2 = new Fv2();
                fv2.Read(path);
                fv2String.GetDataFromFv2(fv2);

                targetComboBox.SelectedIndex = 0;
                ShowStaticDataControls();
            }
        }

        private ComboBox GetExternalFileComboBox()
        {
            ComboBox comboBox = new ComboBox();
            comboBox.Items.Add("None");
            comboBox.Items.AddRange(fv2String.externalFiles);
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.DropDownWidth = 300;
            return comboBox;
        } //GetExternalFileComboBox

        private ComboBox GetMaterialVectorComboBox()
        {
            ComboBox comboBox = new ComboBox();
            comboBox.Items.Add("None");
            comboBox.Items.AddRange(fv2String.materialParameterEntries);
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.DropDownWidth = 300;
            return comboBox;
        } //GetMaterialVectorComboBox

        private void ResizeArray<T>(object sender, EventArgs e, ref T[] array) where T : new()
        {
            byte textValue;
            TextBox textBox = (TextBox)sender;

            if (Byte.TryParse(textBox.Text, out textValue))
                Utils.ResizeArray(ref array, textValue);
        } //ResizeArray

        private void ResizeStringArray(object sender, EventArgs e, ref string[] array)
        {
            byte textValue;
            TextBox textBox = (TextBox)sender;

            if (Byte.TryParse(textBox.Text, out textValue))
                Utils.ResizeStringArray(ref array, textValue);
        } //ResizeArray

        private void ResizeVariableDataEntryArray(object sender, EventArgs e, Fv2String.VariableDataEntry variableDataEntry, string type)
        {
            for (int i = 0; i < variableDataEntry.variableDataSubEntries.Length; i++)
            {
                switch (type)
                {
                    case "meshgroups":
                        ResizeStringArray(sender, e, ref variableDataEntry.variableDataSubEntries[i].meshGroupEntries);
                        break;
                    case "textureswaps":
                        ResizeArray(sender, e, ref variableDataEntry.variableDataSubEntries[i].textureSwapEntries);
                        break;
                    case "boneattachments":
                        ResizeArray(sender, e, ref variableDataEntry.variableDataSubEntries[i].boneModelAttachEntries);
                        break;
                    case "cnpattachments":
                        ResizeArray(sender, e, ref variableDataEntry.variableDataSubEntries[i].cnpModelAttachEntries);
                        break;
                } //switch
            } //for
        } //UpdateVariableDataEntryArrays

        private void ShowExternalFileControls()
        {
            groupBox1.Controls.Clear();

            Label filesLabel = new Label();
            filesLabel.Location = new System.Drawing.Point(6, 16 + LABEL_SPACING);
            filesLabel.Width = LABEL_WIDTH;
            filesLabel.Text = "File Count";
            groupBox1.Controls.Add(filesLabel);

            TextBox externalFilesTextBox = new TextBox();
            externalFilesTextBox.Location = new System.Drawing.Point(filesLabel.Right + 10, 16);
            externalFilesTextBox.Text = fv2String.externalFiles.Length.ToString();
            externalFilesTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => ResizeStringArray(sender, e, ref fv2String.externalFiles));
            externalFilesTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateExternalFileControls());
            groupBox1.Controls.Add(externalFilesTextBox);

            UpdateExternalFileControls();
        } //ShowTextureControls

        private void ShowMaterialParameterControls()
        {
            groupBox1.Controls.Clear();

            Label materialParameterLabel = new Label();
            materialParameterLabel.Location = new System.Drawing.Point(6, 16 + LABEL_SPACING);
            materialParameterLabel.Width = LABEL_WIDTH;
            materialParameterLabel.Text = "Material Parameter Count";
            groupBox1.Controls.Add(materialParameterLabel);

            TextBox materialParameterTextbox = new TextBox();
            materialParameterTextbox.Location = new System.Drawing.Point(materialParameterLabel.Right + 10, 16);
            materialParameterTextbox.Text = fv2String.materialParameterEntries.Length.ToString();
            materialParameterTextbox.TextChanged += new EventHandler((object sender, EventArgs e) => ResizeArray(sender, e, ref fv2String.materialParameterEntries));
            materialParameterTextbox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateMaterialParameterControls());
            groupBox1.Controls.Add(materialParameterTextbox);

            UpdateMaterialParameterControls();
        } //ShowMaterialParameterControls

        private void ShowStaticDataControls()
        {
            groupBox1.Controls.Clear();

            Label hideMeshGroupLabel = new Label();
            hideMeshGroupLabel.Location = new System.Drawing.Point(6, 16 + LABEL_SPACING);
            hideMeshGroupLabel.Width = LABEL_WIDTH;
            hideMeshGroupLabel.Text = "Hide Mesh Group Count";
            groupBox1.Controls.Add(hideMeshGroupLabel);

            TextBox hideMeshGroupTextBox = new TextBox();
            hideMeshGroupTextBox.Location = new System.Drawing.Point(hideMeshGroupLabel.Right + 10, 16);
            hideMeshGroupTextBox.Text = fv2String.hideMeshGroupEntries.Length.ToString();
            hideMeshGroupTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => ResizeStringArray(sender, e, ref fv2String.hideMeshGroupEntries));
            hideMeshGroupTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateStaticDataControls());
            groupBox1.Controls.Add(hideMeshGroupTextBox);

            Label showMeshGroupLabel = new Label();
            showMeshGroupLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING + LABEL_SPACING);
            showMeshGroupLabel.Width = LABEL_WIDTH;
            showMeshGroupLabel.Text = "Show Mesh Group Count";
            groupBox1.Controls.Add(showMeshGroupLabel);

            TextBox showMeshGroupTextBox = new TextBox();
            showMeshGroupTextBox.Location = new System.Drawing.Point(showMeshGroupLabel.Right + 10, 16 + CONTROL_SPACING);
            showMeshGroupTextBox.Text = fv2String.showMeshGroupEntries.Length.ToString();
            showMeshGroupTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => ResizeStringArray(sender, e, ref fv2String.showMeshGroupEntries));
            showMeshGroupTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateStaticDataControls());
            groupBox1.Controls.Add(showMeshGroupTextBox);

            Label textureSwapLabel = new Label();
            textureSwapLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 2 + LABEL_SPACING);
            textureSwapLabel.Width = LABEL_WIDTH;
            textureSwapLabel.Text = "Texture Swap Count";
            groupBox1.Controls.Add(textureSwapLabel);

            TextBox textureSwapTextBox = new TextBox();
            textureSwapTextBox.Location = new System.Drawing.Point(textureSwapLabel.Right + 10, 16 + CONTROL_SPACING * 2);
            textureSwapTextBox.Text = fv2String.textureSwapEntries.Length.ToString();
            textureSwapTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => ResizeArray(sender, e, ref fv2String.textureSwapEntries));
            textureSwapTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateStaticDataControls());
            groupBox1.Controls.Add(textureSwapTextBox);

            Label boneAttachLabel = new Label();
            boneAttachLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 3 + LABEL_SPACING);
            boneAttachLabel.Width = LABEL_WIDTH;
            boneAttachLabel.Text = "Bone Attach Count";
            groupBox1.Controls.Add(boneAttachLabel);

            TextBox boneAttachTextBox = new TextBox();
            boneAttachTextBox.Location = new System.Drawing.Point(boneAttachLabel.Right + 10, 16 + CONTROL_SPACING * 3);
            boneAttachTextBox.Text = fv2String.boneModelAttachEntries.Length.ToString();
            boneAttachTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => ResizeArray(sender, e, ref fv2String.boneModelAttachEntries));
            boneAttachTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateStaticDataControls());
            groupBox1.Controls.Add(boneAttachTextBox);

            Label cnpAttachLabel = new Label();
            cnpAttachLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 4 + LABEL_SPACING);
            cnpAttachLabel.Width = LABEL_WIDTH;
            cnpAttachLabel.Text = "Cnp Attach Count";
            groupBox1.Controls.Add(cnpAttachLabel);

            TextBox cnpAttachTextBox = new TextBox();
            cnpAttachTextBox.Location = new System.Drawing.Point(boneAttachLabel.Right + 10, 16 + CONTROL_SPACING * 4);
            cnpAttachTextBox.Text = fv2String.cnpModelAttachEntries.Length.ToString();
            cnpAttachTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => ResizeArray(sender, e, ref fv2String.cnpModelAttachEntries));
            cnpAttachTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateStaticDataControls());
            groupBox1.Controls.Add(cnpAttachTextBox);

            UpdateStaticDataControls();
        } //ShowStaticDataControls

        private void ShowVariableDataControls()
        {
            groupBox1.Controls.Clear();

            ComboBox variableDataSelectionComboBox = new ComboBox();
            GroupBox groupBox2 = new GroupBox();

            Label variableDataLabel = new Label();
            variableDataLabel.Location = new System.Drawing.Point(6, 16 + LABEL_SPACING);
            variableDataLabel.Width = LABEL_WIDTH;
            variableDataLabel.Text = "Variable Data Count";
            groupBox1.Controls.Add(variableDataLabel);

            TextBox variableDataTextBox = new TextBox();
            variableDataTextBox.Location = new System.Drawing.Point(variableDataLabel.Right + 10, 16);
            variableDataTextBox.Text = fv2String.variableDataEntries.Length.ToString();
            variableDataTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => ResizeArray(sender, e, ref fv2String.variableDataEntries));
            variableDataTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateComboBox(ref variableDataSelectionComboBox));
            groupBox1.Controls.Add(variableDataTextBox);

            Label variableDataSelectionLabel = new Label();
            variableDataSelectionLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING + LABEL_SPACING);
            variableDataSelectionLabel.Width = LABEL_WIDTH;
            variableDataSelectionLabel.Text = "Selected Variable Data";
            groupBox1.Controls.Add(variableDataSelectionLabel);

            variableDataSelectionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            variableDataSelectionComboBox.Location = new System.Drawing.Point(variableDataSelectionLabel.Right + 10, 16 + CONTROL_SPACING + LABEL_SPACING);
            variableDataSelectionComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateVariableDataEntryGroupBox(groupBox2, fv2String.variableDataEntries[variableDataSelectionComboBox.SelectedIndex]));
            variableDataSelectionComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateVariableDataEntryControls(fv2String.variableDataEntries[variableDataSelectionComboBox.SelectedIndex]));
            groupBox1.Controls.Add(variableDataSelectionComboBox);

            groupBox2.AutoSize = true;
            groupBox2.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 2 + LABEL_SPACING);
            groupBox2.Text = "Selected Entry Data";
            groupBox1.Controls.Add(groupBox2);

            UpdateComboBox(ref variableDataSelectionComboBox);

            mainPanel.Controls.Clear();
        } //ShowVariableDataControls

        private void TargetComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = targetComboBox.SelectedIndex;

            switch (index)
            {
                case 0:
                    ShowStaticDataControls();
                    break;
                case 1:
                    ShowVariableDataControls();
                    break;
                case 2:
                    ShowMaterialParameterControls();
                    break;
                case 3:
                    ShowExternalFileControls();
                    break;
                default:
                    break;
            } //switch
        } //targetComboBox_SelectedIndexChanged

        private void UpdateComboBox(ref ComboBox comboBox)
        {
            comboBox.Items.Clear();

            for (int i = 0; i < fv2String.variableDataEntries.Length; i++)
                comboBox.Items.Add(i);
        } //UpdateComboBox

        private void UpdateExternalFileControls()
        {
            mainPanel.Controls.Clear();

            int heightOffset = 16;

            if (fv2String.externalFiles.Length > 0)
            {
                Label label = new Label();
                label.Location = new System.Drawing.Point(6, heightOffset + LABEL_SPACING);
                label.Width = LABEL_WIDTH;
                label.Text = "External Files";
                mainPanel.Controls.Add(label);

                for (int i = 0; i < fv2String.externalFiles.Length; i++)
                {
                    int index = i;
                    TextBox textBox = new TextBox();
                    textBox.Location = new System.Drawing.Point(label.Right + 10, heightOffset);
                    textBox.Text = fv2String.externalFiles[i];
                    textBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateString(sender, e, ref fv2String.externalFiles[index]));
                    textBox.Width = 300;
                    mainPanel.Controls.Add(textBox);

                    heightOffset += CONTROL_SPACING;
                } //for
            } //if
        } //UpdateExternalFileControls

        private void UpdateMaterialParameterControls()
        {
            mainPanel.Controls.Clear();

            int heightOffset = 16;

            if (fv2String.externalFiles.Length > 0)
            {
                Label label = new Label();
                label.Location = new System.Drawing.Point(6, heightOffset + LABEL_SPACING);
                label.Width = LABEL_WIDTH;
                label.Text = "Material Parameters";
                mainPanel.Controls.Add(label);

                for (int i = 0; i < fv2String.materialParameterEntries.Length; i++)
                {
                    int index = i;
                    GroupBox materialParameterGroupBox = new GroupBox();
                    materialParameterGroupBox.AutoSize = true;
                    materialParameterGroupBox.Location = new System.Drawing.Point(label.Right + 10, heightOffset + LABEL_SPACING);
                    materialParameterGroupBox.Text = $"Material Parameter {i}";
                    mainPanel.Controls.Add(materialParameterGroupBox);

                    Label xLabel = new Label();
                    xLabel.Location = new System.Drawing.Point(6, 16 + LABEL_SPACING);
                    xLabel.Text = "X";
                    xLabel.Width = LABEL_WIDTH;
                    materialParameterGroupBox.Controls.Add(xLabel);

                    TextBox xTextBox = new TextBox();
                    xTextBox.Location = new System.Drawing.Point(xLabel.Right + 10, 16);
                    xTextBox.Text = fv2String.materialParameterEntries[i].x;
                    xTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateString(sender, e, ref fv2String.materialParameterEntries[index].x));
                    materialParameterGroupBox.Controls.Add(xTextBox);

                    Label yLabel = new Label();
                    yLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING + LABEL_SPACING);
                    yLabel.Text = "Y";
                    yLabel.Width = LABEL_WIDTH;
                    materialParameterGroupBox.Controls.Add(yLabel);

                    TextBox yTextBox = new TextBox();
                    yTextBox.Location = new System.Drawing.Point(yLabel.Right + 10, 16 + CONTROL_SPACING);
                    yTextBox.Text = fv2String.materialParameterEntries[i].y;
                    yTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateString(sender, e, ref fv2String.materialParameterEntries[index].y));
                    materialParameterGroupBox.Controls.Add(yTextBox);

                    Label zLabel = new Label();
                    zLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 2 + LABEL_SPACING);
                    zLabel.Text = "Z";
                    zLabel.Width = LABEL_WIDTH;
                    materialParameterGroupBox.Controls.Add(zLabel);

                    TextBox zTextBox = new TextBox();
                    zTextBox.Location = new System.Drawing.Point(xLabel.Right + 10, 16 + CONTROL_SPACING * 2);
                    zTextBox.Text = fv2String.materialParameterEntries[i].z;
                    zTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateString(sender, e, ref fv2String.materialParameterEntries[index].z));
                    materialParameterGroupBox.Controls.Add(zTextBox);

                    Label wLabel = new Label();
                    wLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 3 + LABEL_SPACING);
                    wLabel.Text = "W";
                    wLabel.Width = LABEL_WIDTH;
                    materialParameterGroupBox.Controls.Add(wLabel);

                    TextBox wTextBox = new TextBox();
                    wTextBox.Location = new System.Drawing.Point(xLabel.Right + 10, 16 + CONTROL_SPACING * 3);
                    wTextBox.Text = fv2String.materialParameterEntries[i].w;
                    wTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateString(sender, e, ref fv2String.materialParameterEntries[index].w));
                    materialParameterGroupBox.Controls.Add(wTextBox);

                    heightOffset += materialParameterGroupBox.Height + 3;
                } //for
            } //if
        } //UpdateMaterialParameterControls

        private void UpdateIndex(object sender, EventArgs e, ref int i)
        {
            ComboBox comboBox = (ComboBox)sender;

            i = comboBox.SelectedIndex - 1;
        } //UpdateIndex

        private void UpdateHexInt(object sender, EventArgs e, ref int i)
        {
            int textValue;
            TextBox textBox = (TextBox)sender;

            if (Int32.TryParse(textBox.Text, NumberStyles.HexNumber, new NumberFormatInfo(), out textValue))
                i = textValue;
        } //UpdateInt

        private void UpdateInt(object sender, EventArgs e, ref int i)
        {
            int textValue;
            TextBox textBox = (TextBox)sender;

            if (Int32.TryParse(textBox.Text, out textValue))
                i = textValue;
        } //UpdateInt

        private void UpdateStaticDataControls()
        {
            mainPanel.Controls.Clear();

            int heightOffset = 16;

            if (fv2String.hideMeshGroupEntries.Length > 0)
            {
                Label hideMeshGroupLabel = new Label();
                hideMeshGroupLabel.Location = new System.Drawing.Point(6, heightOffset + LABEL_SPACING);
                hideMeshGroupLabel.Text = "Hide Mesh Groups";
                hideMeshGroupLabel.Width = LABEL_WIDTH;
                mainPanel.Controls.Add(hideMeshGroupLabel);

                for (int i = 0; i < fv2String.hideMeshGroupEntries.Length; i++)
                {
                    int index = i;
                    TextBox textBox = new TextBox();
                    textBox.Location = new System.Drawing.Point(hideMeshGroupLabel.Right + 10, heightOffset);
                    textBox.Text = fv2String.hideMeshGroupEntries[i];
                    textBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateString(sender, e, ref fv2String.hideMeshGroupEntries[index]));
                    mainPanel.Controls.Add(textBox);
                    heightOffset += CONTROL_SPACING;
                } //for
            } //if

            if (fv2String.showMeshGroupEntries.Length > 0)
            {
                Label showMeshGroupLabel = new Label();
                showMeshGroupLabel.Location = new System.Drawing.Point(6, heightOffset + LABEL_SPACING);
                showMeshGroupLabel.Text = "Show Mesh Groups";
                showMeshGroupLabel.Width = LABEL_WIDTH;
                mainPanel.Controls.Add(showMeshGroupLabel);

                for (int i = 0; i < fv2String.showMeshGroupEntries.Length; i++)
                {
                    int index = i;
                    TextBox textBox = new TextBox();
                    textBox.Location = new System.Drawing.Point(showMeshGroupLabel.Right + 10, heightOffset);
                    textBox.Text = fv2String.showMeshGroupEntries[i];
                    textBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateString(sender, e, ref fv2String.showMeshGroupEntries[index]));
                    mainPanel.Controls.Add(textBox);
                    heightOffset += CONTROL_SPACING;
                } //for
            } //if

            if (fv2String.textureSwapEntries.Length > 0)
            {
                Label label = new Label();
                label.Location = new System.Drawing.Point(6, heightOffset + LABEL_SPACING);
                label.Text = "Texture Swaps";
                label.Width = LABEL_WIDTH;
                mainPanel.Controls.Add(label);

                for (int i = 0; i < fv2String.textureSwapEntries.Length; i++)
                {
                    int index = i;
                    GroupBox textureSwapGroupBox = new GroupBox();
                    textureSwapGroupBox.AutoSize = true;
                    textureSwapGroupBox.Location = new System.Drawing.Point(label.Right + 10, heightOffset + LABEL_SPACING);
                    textureSwapGroupBox.Text = $"Texture Swap {i}";
                    mainPanel.Controls.Add(textureSwapGroupBox);

                    Label materialInstanceLabel = new Label();
                    materialInstanceLabel.Location = new System.Drawing.Point(6, 16 + LABEL_SPACING);
                    materialInstanceLabel.Text = "Material Instance";
                    materialInstanceLabel.Width = LABEL_WIDTH;
                    textureSwapGroupBox.Controls.Add(materialInstanceLabel);

                    TextBox materialInstanceTextBox = new TextBox();
                    materialInstanceTextBox.Location = new System.Drawing.Point(materialInstanceLabel.Right + 10, 16);
                    materialInstanceTextBox.Text = fv2String.textureSwapEntries[i].materialInstanceName;
                    materialInstanceTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateString(sender, e, ref fv2String.textureSwapEntries[index].materialInstanceName));
                    textureSwapGroupBox.Controls.Add(materialInstanceTextBox);

                    Label textureTypeLabel = new Label();
                    textureTypeLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING + LABEL_SPACING);
                    textureTypeLabel.Text = "Texture Type";
                    textureTypeLabel.Width = LABEL_WIDTH;
                    textureSwapGroupBox.Controls.Add(textureTypeLabel);

                    TextBox textureTypeTextBox = new TextBox();
                    textureTypeTextBox.Location = new System.Drawing.Point(textureTypeLabel.Right + 10, 16 + CONTROL_SPACING);
                    textureTypeTextBox.Text = fv2String.textureSwapEntries[i].textureTypeName;
                    textureTypeTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateString(sender, e, ref fv2String.textureSwapEntries[index].textureTypeName));
                    textureSwapGroupBox.Controls.Add(textureTypeTextBox);

                    Label materialVectorLabel = new Label();
                    materialVectorLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 2 + LABEL_SPACING);
                    materialVectorLabel.Text = "Material Vector";
                    materialVectorLabel.Width = LABEL_WIDTH;
                    textureSwapGroupBox.Controls.Add(materialVectorLabel);

                    ComboBox materialVectorComboBox = GetMaterialVectorComboBox();
                    materialVectorComboBox.Location = new System.Drawing.Point(materialVectorLabel.Right + 10, 16 + CONTROL_SPACING * 2);
                    try
                    {
                        materialVectorComboBox.SelectedIndex = fv2String.textureSwapEntries[i].materialParameterIndex + 1;
                    } //try
                    catch
                    {
                        materialVectorComboBox.SelectedIndex = 0;
                    } //catch
                    materialVectorComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateIndex(sender, e, ref fv2String.textureSwapEntries[index].materialParameterIndex));
                    textureSwapGroupBox.Controls.Add(materialVectorComboBox);

                    Label textureLabel = new Label();
                    textureLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 3 + LABEL_SPACING);
                    textureLabel.Text = "Texture";
                    textureLabel.Width = LABEL_WIDTH;
                    textureSwapGroupBox.Controls.Add(textureLabel);

                    ComboBox textureComboBox = GetExternalFileComboBox();
                    textureComboBox.Location = new System.Drawing.Point(textureLabel.Right + 10, 16 + CONTROL_SPACING * 3);
                    try
                    {
                        textureComboBox.SelectedIndex = fv2String.textureSwapEntries[i].textureIndex + 1;
                    } //try
                    catch
                    {
                        textureComboBox.SelectedIndex = 0;
                    } //catch
                    textureComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateIndex(sender, e, ref fv2String.textureSwapEntries[index].textureIndex));
                    textureSwapGroupBox.Controls.Add(textureComboBox);

                    heightOffset += textureSwapGroupBox.Height + 3;
                } //for
            } //if

            if (fv2String.boneModelAttachEntries.Length > 0)
            {
                Label label = new Label();
                label.Location = new System.Drawing.Point(6, heightOffset + LABEL_SPACING);
                label.Text = "Bone Attachments";
                label.Width = LABEL_WIDTH;
                mainPanel.Controls.Add(label);

                for (int i = 0; i < fv2String.boneModelAttachEntries.Length; i++)
                {
                    int index = i;
                    GroupBox groupBox = new GroupBox();
                    groupBox.AutoSize = true;
                    groupBox.Location = new System.Drawing.Point(label.Right + 10, heightOffset + LABEL_SPACING);
                    groupBox.Text = $"Bone Attachment {i}";
                    mainPanel.Controls.Add(groupBox);

                    Label fmdlLabel = new Label();
                    fmdlLabel.Location = new System.Drawing.Point(6, 16 + LABEL_SPACING);
                    fmdlLabel.Text = "Fmdl";
                    fmdlLabel.Width = LABEL_WIDTH;
                    groupBox.Controls.Add(fmdlLabel);

                    ComboBox fmdlComboBox = GetExternalFileComboBox();
                    fmdlComboBox.Location = new System.Drawing.Point(fmdlLabel.Right + 10, 16);
                    try
                    {
                        fmdlComboBox.SelectedIndex = fv2String.boneModelAttachEntries[i].fmdlIndex + 1;
                    } //try
                    catch
                    {
                        fmdlComboBox.SelectedIndex = 0;
                    } //catch
                    fmdlComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateIndex(sender, e, ref fv2String.boneModelAttachEntries[index].fmdlIndex));
                    groupBox.Controls.Add(fmdlComboBox);

                    Label frdvLabel = new Label();
                    frdvLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING + LABEL_SPACING);
                    frdvLabel.Text = "Frdv";
                    frdvLabel.Width = LABEL_WIDTH;
                    groupBox.Controls.Add(frdvLabel);

                    ComboBox frdvComboBox = GetExternalFileComboBox();
                    frdvComboBox.Location = new System.Drawing.Point(frdvLabel.Right + 10, 16 + CONTROL_SPACING);
                    try
                    {
                        frdvComboBox.SelectedIndex = fv2String.boneModelAttachEntries[i].frdvIndex + 1;
                    } //try
                    catch
                    {
                        frdvComboBox.SelectedIndex = 0;
                    } //catch
                    frdvComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateIndex(sender, e, ref fv2String.boneModelAttachEntries[index].frdvIndex));
                    groupBox.Controls.Add(frdvComboBox);

                    Label simLabel = new Label();
                    simLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 2 + LABEL_SPACING);
                    simLabel.Text = "Sim";
                    simLabel.Width = LABEL_WIDTH;
                    groupBox.Controls.Add(simLabel);

                    ComboBox simComboBox = GetExternalFileComboBox();
                    simComboBox.Location = new System.Drawing.Point(simLabel.Right + 10, 16 + CONTROL_SPACING * 2);
                    try
                    {
                        simComboBox.SelectedIndex = fv2String.boneModelAttachEntries[i].simIndex + 1;
                    } //try
                    catch
                    {
                        simComboBox.SelectedIndex = 0;
                    } //catch
                    simComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateIndex(sender, e, ref fv2String.boneModelAttachEntries[index].simIndex));
                    groupBox.Controls.Add(simComboBox);

                    heightOffset += groupBox.Height + 3;
                } //for
            } //if

            if (fv2String.cnpModelAttachEntries.Length > 0)
            {
                Label label = new Label();
                label.Location = new System.Drawing.Point(6, heightOffset + LABEL_SPACING);
                label.Text = "Cnp Attachments";
                label.Width = LABEL_WIDTH;
                mainPanel.Controls.Add(label);

                for (int i = 0; i < fv2String.cnpModelAttachEntries.Length; i++)
                {
                    int index = i;
                    GroupBox groupBox = new GroupBox();
                    groupBox.AutoSize = true;
                    groupBox.Location = new System.Drawing.Point(label.Right + 10, heightOffset + LABEL_SPACING);
                    groupBox.Text = $"Cnp Attachment {i}";
                    mainPanel.Controls.Add(groupBox);

                    Label cnpLabel = new Label();
                    cnpLabel.Location = new System.Drawing.Point(6, 16 + LABEL_SPACING);
                    cnpLabel.Text = "Cnp";
                    cnpLabel.Width = LABEL_WIDTH;
                    groupBox.Controls.Add(cnpLabel);

                    TextBox cnpTextBox = new TextBox();
                    cnpTextBox.Location = new System.Drawing.Point(cnpLabel.Right + 10, 16);
                    cnpTextBox.Text = fv2String.cnpModelAttachEntries[i].cnpStrCode32;
                    cnpTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateString(sender, e, ref fv2String.cnpModelAttachEntries[index].cnpStrCode32));
                    groupBox.Controls.Add(cnpTextBox);

                    Label fmdlLabel = new Label();
                    fmdlLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING + LABEL_SPACING);
                    fmdlLabel.Text = "Fmdl";
                    fmdlLabel.Width = LABEL_WIDTH;
                    groupBox.Controls.Add(fmdlLabel);

                    ComboBox fmdlComboBox = GetExternalFileComboBox();
                    fmdlComboBox.Location = new System.Drawing.Point(fmdlLabel.Right + 10, 16 + CONTROL_SPACING);
                    try
                    {
                        fmdlComboBox.SelectedIndex = fv2String.cnpModelAttachEntries[i].fmdlIndex + 1;
                    } //try
                    catch
                    {
                        fmdlComboBox.SelectedIndex = 0;
                    } //catch
                    fmdlComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateIndex(sender, e, ref fv2String.cnpModelAttachEntries[index].fmdlIndex));
                    groupBox.Controls.Add(fmdlComboBox);

                    Label frdvLabel = new Label();
                    frdvLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 2 + LABEL_SPACING);
                    frdvLabel.Text = "Frdv";
                    frdvLabel.Width = LABEL_WIDTH;
                    groupBox.Controls.Add(frdvLabel);

                    ComboBox frdvComboBox = GetExternalFileComboBox();
                    frdvComboBox.Location = new System.Drawing.Point(frdvLabel.Right + 10, 16 + CONTROL_SPACING * 2);
                    try
                    {
                        frdvComboBox.SelectedIndex = fv2String.cnpModelAttachEntries[i].frdvIndex + 1;
                    } //try
                    catch
                    {
                        frdvComboBox.SelectedIndex = 0;
                    } //catch
                    frdvComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateIndex(sender, e, ref fv2String.cnpModelAttachEntries[index].frdvIndex));
                    groupBox.Controls.Add(frdvComboBox);

                    Label simLabel = new Label();
                    simLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 3 + LABEL_SPACING);
                    simLabel.Text = "Sim";
                    simLabel.Width = LABEL_WIDTH;
                    groupBox.Controls.Add(simLabel);

                    ComboBox simComboBox = GetExternalFileComboBox();
                    simComboBox.Location = new System.Drawing.Point(simLabel.Right + 10, 16 + CONTROL_SPACING * 3);
                    try
                    {
                        simComboBox.SelectedIndex = fv2String.cnpModelAttachEntries[i].simIndex + 1;
                    } //try
                    catch
                    {
                        simComboBox.SelectedIndex = 0;
                    } //catch
                    simComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateIndex(sender, e, ref fv2String.cnpModelAttachEntries[index].simIndex));
                    groupBox.Controls.Add(simComboBox);

                    heightOffset += groupBox.Height + 3;
                } //for
            } //if
        } //UpdateStaticDataControls

        private void UpdateString(object sender, EventArgs e, ref string str)
        {
            TextBox textBox = (TextBox)sender;

            str = textBox.Text;
        } //UpdateString

        private void UpdateVariableDataEntryControls(Fv2String.VariableDataEntry variableDataEntry)
        {
            mainPanel.Controls.Clear();

            int groupBoxHeightOffset = 16;

            for (int j = 0; j < variableDataEntry.variableDataSubEntries.Length; j++)
            {
                int index0 = j;
                int heightOffset = 16;
                GroupBox groupBox = new GroupBox();
                groupBox.AutoSize = true;
                groupBox.Location = new System.Drawing.Point(6, groupBoxHeightOffset);
                groupBox.Text = $"Entry {j}";
                mainPanel.Controls.Add(groupBox);

                if (variableDataEntry.meshGroupEntryCount > 0)
                {
                    Label hideMeshGroupLabel = new Label();
                    hideMeshGroupLabel.Location = new System.Drawing.Point(6, heightOffset + LABEL_SPACING);
                    hideMeshGroupLabel.Text = "Mesh Groups";
                    hideMeshGroupLabel.Width = LABEL_WIDTH;
                    groupBox.Controls.Add(hideMeshGroupLabel);

                    for (int i = 0; i < variableDataEntry.meshGroupEntryCount; i++)
                    {
                        int index1 = i;
                        TextBox textBox = new TextBox();
                        textBox.Location = new System.Drawing.Point(hideMeshGroupLabel.Right + 10, heightOffset);
                        textBox.Text = variableDataEntry.variableDataSubEntries[j].meshGroupEntries[i];
                        textBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateString(sender, e, ref variableDataEntry.variableDataSubEntries[index0].meshGroupEntries[index1]));
                        groupBox.Controls.Add(textBox);
                        heightOffset += CONTROL_SPACING;
                    } //for
                } //if

                if (variableDataEntry.textureSwapEntryCount > 0)
                {
                    Label label = new Label();
                    label.Location = new System.Drawing.Point(6, heightOffset + LABEL_SPACING);
                    label.Text = "Texture Swaps";
                    label.Width = LABEL_WIDTH;
                    groupBox.Controls.Add(label);

                    for (int i = 0; i < variableDataEntry.textureSwapEntryCount; i++)
                    {
                        int index1 = i;
                        GroupBox textureSwapGroupBox = new GroupBox();
                        textureSwapGroupBox.AutoSize = true;
                        textureSwapGroupBox.Location = new System.Drawing.Point(label.Right + 10, heightOffset + LABEL_SPACING);
                        textureSwapGroupBox.Text = $"Texture Swap {i}";
                        groupBox.Controls.Add(textureSwapGroupBox);

                        Label materialInstanceLabel = new Label();
                        materialInstanceLabel.Location = new System.Drawing.Point(6, 16 + LABEL_SPACING);
                        materialInstanceLabel.Text = "Material Instance";
                        materialInstanceLabel.Width = LABEL_WIDTH;
                        textureSwapGroupBox.Controls.Add(materialInstanceLabel);

                        TextBox materialInstanceTextBox = new TextBox();
                        materialInstanceTextBox.Location = new System.Drawing.Point(materialInstanceLabel.Right + 10, 16);
                        materialInstanceTextBox.Text = variableDataEntry.variableDataSubEntries[j].textureSwapEntries[i].materialInstanceName;
                        materialInstanceTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateString(sender, e, ref variableDataEntry.variableDataSubEntries[index0].textureSwapEntries[index1].materialInstanceName));
                        textureSwapGroupBox.Controls.Add(materialInstanceTextBox);

                        Label textureTypeLabel = new Label();
                        textureTypeLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING + LABEL_SPACING);
                        textureTypeLabel.Text = "Texture Type";
                        textureTypeLabel.Width = LABEL_WIDTH;
                        textureSwapGroupBox.Controls.Add(textureTypeLabel);

                        TextBox textureTypeTextBox = new TextBox();
                        textureTypeTextBox.Location = new System.Drawing.Point(textureTypeLabel.Right + 10, 16 + CONTROL_SPACING);
                        textureTypeTextBox.Text = variableDataEntry.variableDataSubEntries[j].textureSwapEntries[i].textureTypeName;
                        textureTypeTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateString(sender, e, ref variableDataEntry.variableDataSubEntries[index0].textureSwapEntries[index1].textureTypeName));
                        textureSwapGroupBox.Controls.Add(textureTypeTextBox);

                        Label textureLabel = new Label();
                        textureLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 2 + LABEL_SPACING);
                        textureLabel.Text = "Texture";
                        textureLabel.Width = LABEL_WIDTH;
                        textureSwapGroupBox.Controls.Add(textureLabel);

                        ComboBox textureComboBox = GetExternalFileComboBox();
                        textureComboBox.Location = new System.Drawing.Point(textureLabel.Right + 10, 16 + CONTROL_SPACING * 2);
                        try
                        {
                            textureComboBox.SelectedIndex = variableDataEntry.variableDataSubEntries[j].textureSwapEntries[i].textureIndex + 1;
                        } //try
                        catch
                        {
                            textureComboBox.SelectedIndex = 0;
                        } //catch
                        textureComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateIndex(sender, e, ref variableDataEntry.variableDataSubEntries[index0].textureSwapEntries[index1].textureIndex));
                        textureSwapGroupBox.Controls.Add(textureComboBox);

                        heightOffset += textureSwapGroupBox.Height + 3;
                    } //for
                } //if

                if (variableDataEntry.boneModelAttachEntryCount > 0)
                {
                    Label label = new Label();
                    label.Location = new System.Drawing.Point(6, heightOffset + LABEL_SPACING);
                    label.Text = "Bone Attachments";
                    label.Width = LABEL_WIDTH;
                    groupBox.Controls.Add(label);

                    for (int i = 0; i < variableDataEntry.boneModelAttachEntryCount; i++)
                    {
                        int index1 = i;
                        GroupBox boneAttachGroupBox = new GroupBox();
                        boneAttachGroupBox.AutoSize = true;
                        boneAttachGroupBox.Location = new System.Drawing.Point(label.Right + 10, heightOffset + LABEL_SPACING);
                        boneAttachGroupBox.Text = $"Bone Attachment {i}";
                        groupBox.Controls.Add(boneAttachGroupBox);

                        Label fmdlLabel = new Label();
                        fmdlLabel.Location = new System.Drawing.Point(6, 16 + LABEL_SPACING);
                        fmdlLabel.Text = "Fmdl";
                        fmdlLabel.Width = LABEL_WIDTH;
                        boneAttachGroupBox.Controls.Add(fmdlLabel);

                        ComboBox fmdlComboBox = GetExternalFileComboBox();
                        fmdlComboBox.Location = new System.Drawing.Point(fmdlLabel.Right + 10, 16);
                        try
                        {
                            fmdlComboBox.SelectedIndex = variableDataEntry.variableDataSubEntries[j].boneModelAttachEntries[i].fmdlIndex + 1;
                        } //try
                        catch
                        {
                            fmdlComboBox.SelectedIndex = 0;
                        } //catch
                        fmdlComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateIndex(sender, e, ref variableDataEntry.variableDataSubEntries[index0].boneModelAttachEntries[index1].fmdlIndex));
                        boneAttachGroupBox.Controls.Add(fmdlComboBox);

                        Label frdvLabel = new Label();
                        frdvLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING + LABEL_SPACING);
                        frdvLabel.Text = "Frdv";
                        frdvLabel.Width = LABEL_WIDTH;
                        boneAttachGroupBox.Controls.Add(frdvLabel);

                        ComboBox frdvComboBox = GetExternalFileComboBox();
                        frdvComboBox.Location = new System.Drawing.Point(frdvLabel.Right + 10, 16 + CONTROL_SPACING);
                        try
                        {
                            frdvComboBox.SelectedIndex = variableDataEntry.variableDataSubEntries[j].boneModelAttachEntries[i].frdvIndex + 1;
                        } //try
                        catch
                        {
                            frdvComboBox.SelectedIndex = 0;
                        } //catch
                        frdvComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateIndex(sender, e, ref variableDataEntry.variableDataSubEntries[index0].boneModelAttachEntries[index1].frdvIndex));
                        boneAttachGroupBox.Controls.Add(frdvComboBox);

                        Label simLabel = new Label();
                        simLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 2 + LABEL_SPACING);
                        simLabel.Text = "Sim";
                        simLabel.Width = LABEL_WIDTH;
                        boneAttachGroupBox.Controls.Add(simLabel);

                        ComboBox simComboBox = GetExternalFileComboBox();
                        simComboBox.Location = new System.Drawing.Point(simLabel.Right + 10, 16 + CONTROL_SPACING * 2);
                        try
                        {
                            simComboBox.SelectedIndex = variableDataEntry.variableDataSubEntries[j].boneModelAttachEntries[i].simIndex + 1;
                        } //try
                        catch
                        {
                            simComboBox.SelectedIndex = 0;
                        } //catch
                        simComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateIndex(sender, e, ref variableDataEntry.variableDataSubEntries[index0].boneModelAttachEntries[index1].simIndex));
                        boneAttachGroupBox.Controls.Add(simComboBox);

                        heightOffset += boneAttachGroupBox.Height + 3;
                    } //for
                } //if

                if (variableDataEntry.cnpModelAttachEntryCount > 0)
                {
                    Label label = new Label();
                    label.Location = new System.Drawing.Point(6, heightOffset + LABEL_SPACING);
                    label.Text = "Cnp Attachments";
                    label.Width = LABEL_WIDTH;
                    groupBox.Controls.Add(label);

                    for (int i = 0; i < variableDataEntry.cnpModelAttachEntryCount; i++)
                    {
                        int index1 = i;
                        GroupBox cnpAttachGroupBox = new GroupBox();
                        cnpAttachGroupBox.AutoSize = true;
                        cnpAttachGroupBox.Location = new System.Drawing.Point(label.Right + 10, heightOffset + LABEL_SPACING);
                        cnpAttachGroupBox.Text = $"Cnp Attachment {i}";
                        groupBox.Controls.Add(cnpAttachGroupBox);

                        Label cnpLabel = new Label();
                        cnpLabel.Location = new System.Drawing.Point(6, 16 + LABEL_SPACING);
                        cnpLabel.Text = "Cnp";
                        cnpLabel.Width = LABEL_WIDTH;
                        cnpAttachGroupBox.Controls.Add(cnpLabel);

                        TextBox cnpTextBox = new TextBox();
                        cnpTextBox.Location = new System.Drawing.Point(cnpLabel.Right + 10, 16);
                        cnpTextBox.Text = variableDataEntry.variableDataSubEntries[j].cnpModelAttachEntries[i].cnpStrCode32;
                        cnpTextBox.TextChanged += new EventHandler((object sender, EventArgs e) => UpdateString(sender, e, ref variableDataEntry.variableDataSubEntries[index0].cnpModelAttachEntries[index1].cnpStrCode32));
                        cnpAttachGroupBox.Controls.Add(cnpTextBox);

                        Label fmdlLabel = new Label();
                        fmdlLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING + LABEL_SPACING);
                        fmdlLabel.Text = "Fmdl";
                        fmdlLabel.Width = LABEL_WIDTH;
                        cnpAttachGroupBox.Controls.Add(fmdlLabel);

                        ComboBox fmdlComboBox = GetExternalFileComboBox();
                        fmdlComboBox.Location = new System.Drawing.Point(fmdlLabel.Right + 10, 16 + CONTROL_SPACING);
                        try
                        {
                            fmdlComboBox.SelectedIndex = variableDataEntry.variableDataSubEntries[j].cnpModelAttachEntries[i].fmdlIndex + 1;
                        } //try
                        catch
                        {
                            fmdlComboBox.SelectedIndex = 0;
                        } //catch
                        fmdlComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateIndex(sender, e, ref variableDataEntry.variableDataSubEntries[index0].cnpModelAttachEntries[index1].fmdlIndex));
                        cnpAttachGroupBox.Controls.Add(fmdlComboBox);

                        Label frdvLabel = new Label();
                        frdvLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 2 + LABEL_SPACING);
                        frdvLabel.Text = "Frdv";
                        frdvLabel.Width = LABEL_WIDTH;
                        cnpAttachGroupBox.Controls.Add(frdvLabel);

                        ComboBox frdvComboBox = GetExternalFileComboBox();
                        frdvComboBox.Location = new System.Drawing.Point(frdvLabel.Right + 10, 16 + CONTROL_SPACING * 2);
                        try
                        {
                            frdvComboBox.SelectedIndex = variableDataEntry.variableDataSubEntries[j].cnpModelAttachEntries[i].frdvIndex + 1;
                        } //try
                        catch
                        {
                            frdvComboBox.SelectedIndex = 0;
                        } //catch
                        frdvComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateIndex(sender, e, ref variableDataEntry.variableDataSubEntries[index0].cnpModelAttachEntries[index1].frdvIndex));
                        cnpAttachGroupBox.Controls.Add(frdvComboBox);

                        Label simLabel = new Label();
                        simLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 3 + LABEL_SPACING);
                        simLabel.Text = "Sim";
                        simLabel.Width = LABEL_WIDTH;
                        cnpAttachGroupBox.Controls.Add(simLabel);

                        ComboBox simComboBox = GetExternalFileComboBox();
                        simComboBox.Location = new System.Drawing.Point(simLabel.Right + 10, 16 + CONTROL_SPACING * 3);
                        try
                        {
                            simComboBox.SelectedIndex = variableDataEntry.variableDataSubEntries[j].cnpModelAttachEntries[i].simIndex + 1;
                        } //try
                        catch
                        {
                            simComboBox.SelectedIndex = 0;
                        } //catch
                        simComboBox.SelectedIndexChanged += new EventHandler((object sender, EventArgs e) => UpdateIndex(sender, e, ref variableDataEntry.variableDataSubEntries[index0].cnpModelAttachEntries[index1].simIndex));
                        cnpAttachGroupBox.Controls.Add(simComboBox);

                        heightOffset += cnpAttachGroupBox.Height + 3;
                    } //for
                } //if

                groupBoxHeightOffset += groupBox.Height + CONTROL_SPACING;
            } //for
        } //UpdateVariableDataEntryControls

        private void UpdateVariableDataEntryGroupBox(GroupBox groupBox, Fv2String.VariableDataEntry variableDataEntry)
        {
            groupBox.Controls.Clear();

            GroupBox subGroupBox = new GroupBox();

            Label typeLabel = new Label();
            typeLabel.Location = new System.Drawing.Point(6, 16 + LABEL_SPACING);
            typeLabel.Width = LABEL_WIDTH;
            typeLabel.Text = "Type (Enum)";
            groupBox.Controls.Add(typeLabel);

            TextBox typeTextBox = new TextBox();
            typeTextBox.Location = new System.Drawing.Point(typeLabel.Right + 10, 16);
            typeTextBox.Text = variableDataEntry.type.ToString("x");
            typeTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => UpdateHexInt(sender0, e0, ref variableDataEntry.type));
            groupBox.Controls.Add(typeTextBox);

            Label entryCountLabel = new Label();
            entryCountLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING + LABEL_SPACING);
            entryCountLabel.Width = LABEL_WIDTH;
            entryCountLabel.Text = "Entry Count";
            groupBox.Controls.Add(entryCountLabel);

            TextBox entryCountTextBox = new TextBox();
            entryCountTextBox.Location = new System.Drawing.Point(entryCountLabel.Right + 10, 16 + CONTROL_SPACING);
            entryCountTextBox.Text = variableDataEntry.variableDataSubEntries.Length.ToString();
            entryCountTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => ResizeArray(sender0, e0, ref variableDataEntry.variableDataSubEntries));
            entryCountTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => ResizeVariableDataEntryArray(sender0, e0, variableDataEntry, "meshgroups"));
            entryCountTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => ResizeVariableDataEntryArray(sender0, e0, variableDataEntry, "textureswaps"));
            entryCountTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => ResizeVariableDataEntryArray(sender0, e0, variableDataEntry, "boneattachments"));
            entryCountTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => ResizeVariableDataEntryArray(sender0, e0, variableDataEntry, "cnpattachments"));
            entryCountTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => UpdateVariableDataEntryControls(variableDataEntry));
            groupBox.Controls.Add(entryCountTextBox);

            Label meshGroupLabel = new Label();
            meshGroupLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 2 + LABEL_SPACING);
            meshGroupLabel.Width = LABEL_WIDTH;
            meshGroupLabel.Text = "Mesh Group Count";
            groupBox.Controls.Add(meshGroupLabel);

            TextBox meshGroupTextBox = new TextBox();
            meshGroupTextBox.Location = new System.Drawing.Point(meshGroupLabel.Right + 10, 16 + CONTROL_SPACING * 2);
            meshGroupTextBox.Text = variableDataEntry.meshGroupEntryCount.ToString();
            meshGroupTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => UpdateInt(sender0, e0, ref variableDataEntry.meshGroupEntryCount));
            meshGroupTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => ResizeVariableDataEntryArray(sender0, e0, variableDataEntry, "meshgroups"));
            meshGroupTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => UpdateVariableDataEntryControls(variableDataEntry));
            groupBox.Controls.Add(meshGroupTextBox);

            Label textureSwapLabel = new Label();
            textureSwapLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 3 + LABEL_SPACING);
            textureSwapLabel.Width = LABEL_WIDTH;
            textureSwapLabel.Text = "Texture Swap Count";
            groupBox.Controls.Add(textureSwapLabel);

            TextBox textureSwapTextBox = new TextBox();
            textureSwapTextBox.Location = new System.Drawing.Point(textureSwapLabel.Right + 10, 16 + CONTROL_SPACING * 3);
            textureSwapTextBox.Text = variableDataEntry.textureSwapEntryCount.ToString();
            textureSwapTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => UpdateInt(sender0, e0, ref variableDataEntry.textureSwapEntryCount));
            textureSwapTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => ResizeVariableDataEntryArray(sender0, e0, variableDataEntry, "textureswaps"));
            textureSwapTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => UpdateVariableDataEntryControls(variableDataEntry));
            groupBox.Controls.Add(textureSwapTextBox);

            Label boneAttachLabel = new Label();
            boneAttachLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 4 + LABEL_SPACING);
            boneAttachLabel.Width = LABEL_WIDTH;
            boneAttachLabel.Text = "Bone Attach Count";
            groupBox.Controls.Add(boneAttachLabel);

            TextBox boneAttachTextBox = new TextBox();
            boneAttachTextBox.Location = new System.Drawing.Point(boneAttachLabel.Right + 10, 16 + CONTROL_SPACING * 4);
            boneAttachTextBox.Text = variableDataEntry.boneModelAttachEntryCount.ToString();
            boneAttachTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => UpdateInt(sender0, e0, ref variableDataEntry.boneModelAttachEntryCount));
            boneAttachTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => ResizeVariableDataEntryArray(sender0, e0, variableDataEntry, "boneattachments"));
            boneAttachTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => UpdateVariableDataEntryControls(variableDataEntry));
            groupBox.Controls.Add(boneAttachTextBox);

            Label cnpAttachLabel = new Label();
            cnpAttachLabel.Location = new System.Drawing.Point(6, 16 + CONTROL_SPACING * 5 + LABEL_SPACING);
            cnpAttachLabel.Width = LABEL_WIDTH;
            cnpAttachLabel.Text = "Cnp Attach Count";
            groupBox.Controls.Add(cnpAttachLabel);

            TextBox cnpAttachTextBox = new TextBox();
            cnpAttachTextBox.Location = new System.Drawing.Point(boneAttachLabel.Right + 10, 16 + CONTROL_SPACING * 5);
            cnpAttachTextBox.Text = variableDataEntry.cnpModelAttachEntryCount.ToString();
            cnpAttachTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => UpdateInt(sender0, e0, ref variableDataEntry.cnpModelAttachEntryCount));
            cnpAttachTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => ResizeVariableDataEntryArray(sender0, e0, variableDataEntry, "cnpattachments"));
            cnpAttachTextBox.TextChanged += new EventHandler((object sender0, EventArgs e0) => UpdateVariableDataEntryControls(variableDataEntry));
            groupBox.Controls.Add(cnpAttachTextBox);
        } //UpdateVariableDataEntryGroupBox

        private void ExportButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter =  "Form Variation|*.fv2";
            saveFileDialog.Title = "Choose an output location";

            if(saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Fv2 fv2 = new Fv2();
                fv2.GetDataFromFv2String(fv2String);
                fv2.Write(saveFileDialog.FileName);
            } //if
        } //ExportButton_Click

        private void ImportButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Form Variation|*.fv2";
            openFileDialog.Title = "Select a file";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Read(openFileDialog.FileName);
            } //if
        } //ImportButton_Click

        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            Read(files[0]);
        }

        private void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
    } //class
} //namespace
