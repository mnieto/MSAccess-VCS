﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AccessIO;
using System.Drawing.Design;

namespace AccessScrCtrlUI {

    /// <summary>
    /// Visualize the files hierachy corresponding to a exported database
    /// </summary>
    public partial class FilesTree : UserControl {
        
        private string workingCopyPath;
        private delegate void FillTreeDelegate(string workingCopyPath);
        private TreeImages Images { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public FilesTree() {
            InitializeComponent();
        }

        /// <summary>
        /// Root local path for the working copy. Setting this property will fill the tree
        /// </summary>
        [System.ComponentModel.Description("Root local path for the working copy")]
        [System.ComponentModel.Browsable(true)]
        [System.ComponentModel.Editor(typeof(System.Windows.Forms.Design.FolderNameEditor), typeof(UITypeEditor))]
        public string WorkingCopyPath {
            get { return workingCopyPath; }
            set {
                if (tree.InvokeRequired) {
                    tree.Invoke(new FillTreeDelegate(FillTree), value);
                } else {
                    FillTree(value);
                }
                workingCopyPath = value;
            }
        }

        /// <summary>
        /// Gets the selected nodes list. Only returns leaf nodes.
        /// </summary>
        /// <remarks>
        /// If no selected nodes returns an empty list
        /// </remarks>
        [System.ComponentModel.Browsable(false)]
        public List<IObjecOptions> SelectedNodes {
            get {
                List<IObjecOptions> lst = new List<IObjecOptions>();
                if (tree.Nodes.Count == 0)
                    return lst;
                return InternalSelectedNodes(lst, tree.Nodes[0]);
            }
        }

        /// <summary>
        /// Raised when LoadSelectedObjects operation finalizes
        /// </summary>
        [System.ComponentModel.Description("Raised when LoadSelectedObjects operation finalizes")]
        public event EventHandler<SelectedObjectsCompletedEventArgs> LoadSelectedObjectsCompleted;

        /// <summary>
        /// Raised before loading a object.
        /// </summary>
        [System.ComponentModel.Description("Raised before loading a object")]
        public event EventHandler<SelectedObjectsProgressEventArgs> LoadSelectecObjectsProgress;

        /// <summary>
        /// Gets the <see cref="AccessProjectType"/> associated to the files structure
        /// </summary>
        public AccessProjectType ProjectType {
            get {
                if (tree.Nodes.Count == 0)
                    throw new InvalidOperationException(Properties.Resources.NotProjectType);
                else {
                    int pos = tree.Nodes[0].Text.LastIndexOf('.');
                    return (AccessProjectType)Enum.Parse(typeof(AccessProjectType), tree.Nodes[0].Text.Substring(pos + 1), true);
                }
            }
        }

        /// <summary>
        /// Gets an array of objects names for a specific object container
        /// </summary>
        /// <param name="objectType">object type to get its objects</param>
        public string[] ObjectNames(ObjectType objectType) {
            string[] result = new string[0];
            if (tree.Nodes.Count == 0)
                return result;

            Containers containers = AccessApp.ContainersFactory(tree.Nodes[0].Text);
            ObjectTypeExtension ote = containers.Find(objectType);
            if (ote != null) {
                ContainerNames names = ote.Container;
                TreeNode[] matchNodes = tree.Nodes[0].Nodes.Find(names.DisplayPluralName, true);
                if (matchNodes.Length == 1) {
                    result = new string[matchNodes[0].Nodes.Count];
                    for (int i = 0; i < matchNodes[0].Nodes.Count; i++) {
                        result[i] = matchNodes[0].Nodes[i].Text;
                    }
                }
            }
            return result;

        }

        /// <summary>
        /// Load, in background, the selected objects to the database referenced by <see cref="AccessApp.FileName"/>
        /// </summary>
        public void LoadSelectedObjectsAsync(AccessApp app) {
            LoadObjectsAsyncParams loadParams = new LoadObjectsAsyncParams();
            loadParams.App = app;
            loadParams.SelectedObjects = SelectedNodes;
            backgroundWorker.RunWorkerAsync(loadParams);
        }
        
        /// <summary>
        /// Reload the files list
        /// </summary>
        public void RefreshList() {
            FillTree(WorkingCopyPath);
        }

        private List<IObjecOptions> InternalSelectedNodes(List<IObjecOptions> list, TreeNode root) {

            //Database node must be processed the first
            TreeNode databaseNode = null;
            if (root.Level == 0) {
                databaseNode = root.LastNode.FirstNode;
                if (databaseNode.Checked)
                    list.Add((IObjecOptions)databaseNode.Tag);
            }

            foreach (TreeNode node in root.Nodes) {
                if (node.Checked) {
                    
                    //If has children, iterate; else add leaf nodes to the list
                    if (node.Nodes.Count == 0) {

                        //Add node only if is 3th level node and itsn't databaseNode
                        if (node.Level > 1) {
                            if (list.Count == 0 || (list.Count > 0 && list[0] != (IObjecOptions)node.Tag))
                                list.Add((IObjecOptions)node.Tag);
                        }
                    } else
                        InternalSelectedNodes(list, node);
                }
            }
            return list;
        }

        private void FillTree(string rootPath) {

            if (String.IsNullOrEmpty(rootPath))
                return;

            if (!Directory.Exists(rootPath))
                throw new ArgumentException(Properties.Resources.RootPathNotValid);

            tree.Nodes.Clear();

            TreeNode rootNode = new TreeNode(BuildRootNodeText(rootPath));
            tree.Nodes.Add(rootNode);

            //Get the list of containers depending on the 'database properties' file
            //If do not exists that file, containers will be null and we don't know what folders structure should to expect
            Containers containers = AccessApp.ContainersFactory(rootNode.Text);
            if (containers == null) {
                rootNode.Nodes.Add(Properties.Resources.EmptyFileStructure);
                tree.CheckBoxes = false;
                return;
            } else {
                tree.CheckBoxes = true;
            }

            const string key = "folder";
            Images = new TreeImages(containers);
            tree.ImageList = Images.ImageList;
            tree.ImageKey = key;
            tree.SelectedImageKey = key;

            DirectoryInfo rootDirectory = new DirectoryInfo(rootPath);
            IEnumerable<DirectoryInfo> directories = rootDirectory.EnumerateDirectories();
            foreach (ContainerNames names in containers) {
                TreeNode node = rootNode.Nodes.Add(names.InvariantName, names.InvariantName);
                node.ImageKey = names.DefaultExtension.ToString();
                node.SelectedImageKey = node.ImageKey;

                DirectoryInfo di = directories.FirstOrDefault<DirectoryInfo>(d => d.Name == names.InvariantName);
                if (di != null) {
                    FillContainerFiles(di, node, names.ObjectTypes);
                }
            }
            rootNode.Expand();
        }

        private string BuildRootNodeText(string rootPath) {
            StringBuilder sb = new StringBuilder();
            sb.Append(Path.GetPathRoot(rootPath));
            sb.Append("...");
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append(Path.GetFileName(rootPath));
            sb.Append(Path.DirectorySeparatorChar);
            sb.Append(GetDatabaseFileName(rootPath));
            return sb.ToString();
        }

        //private string GetExtension(string fileName) {
        //    return Path.GetExtension(Path.GetFileNameWithoutExtension(fileName));
        //}

        private void FillContainerFiles(DirectoryInfo di, TreeNode parentNode, List<ObjectTypeExtension> objectTypes) {
            string fileExtension = "*";
            if (objectTypes.Count == 1)
                fileExtension = objectTypes[0].FileExtension.ToString();

            foreach (FileInfo fi in di.EnumerateFiles(string.Format("*.{0}.txt", fileExtension))) {
                ObjectTypeExtension ote = GetObjectTypeExtension(fi.Name, objectTypes);
                if (ote != null) {
                    TreeNode node = new TreeNode(fi.Name);
                    node.Tag = new ObjectOptions(fi.FullName, ote.ObjectType);
                    node.ImageKey = ote.FileExtension.ToString();
                    node.SelectedImageKey = ote.FileExtension.ToString();
                    parentNode.Nodes.Add(node);
                }
            }
        }

        private ObjectTypeExtension GetObjectTypeExtension(string fileName, List<ObjectTypeExtension> objectTypes) {
            FileExtensions extension = Containers.GetFileExtension(fileName);
            return objectTypes.Find(x => x.FileExtension == extension);
        }

        private string GetDatabaseFileName(string rootPath) {
            rootPath += @"\" + ObjectType.General.ToString();
            string dbProperties = Path.Combine(rootPath, AccessIO.Properties.Resources.DatabaseProperties + "." + FileExtensions.dbp.ToString() + ".txt");
            if (!File.Exists(dbProperties)) {
                return Path.GetFileName(dbProperties);
            } else {
                try {
                    StreamReader sr = new StreamReader(dbProperties);
                    ImportObject import = new ImportObject(sr);
                    return import.ReadObjectName();
                } catch (WrongFileFormatException ex) {
                    if (ex.LineNumber <= 1)
                        return Path.GetFileName(dbProperties);
                    else
                        throw;
                }
            }
        }

        /// <summary>
        /// Checks all the children nodes and makes sure that parents are checked
        /// </summary>
        private void tree_AfterCheck(object sender, TreeViewEventArgs e) {

            //Setting Checked from within AfterCheck event causes the event to be raised multiple times.
            //To avoid this, only check child in parent nodes if the user causes the state change.
            //see AfterCheck documentation
            if (e.Action != TreeViewAction.Unknown) {
                CheckChildren(e.Node);
                CheckParents(e.Node);
            }
        }

        private void CheckParents(TreeNode node) {
            if (node.Checked == false) {
                while (node.Parent != null) {
                    if (HasCheckedChilder(node.Parent))
                        break;
                    else {
                        node.Parent.Checked = false;
                        node = node.Parent;
                    }

                }
            } else {
                while (node.Parent != null) {
                    if (node.Parent.Checked == false)
                        node.Parent.Checked = true;
                    node = node.Parent;
                }
            }
        }

        private bool HasCheckedChilder(TreeNode treeNode) {
            foreach (TreeNode item in treeNode.Nodes) {
                if (item.Checked)
                    return true;
            }
            return false;
        }

        private void CheckChildren(TreeNode node) {
            foreach (TreeNode child in node.Nodes) {
                child.Checked = node.Checked;
                CheckChildren(child);
            }
        }

        private void contextMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e) {
            for (int i = contextMenu.Items.Count - 1; i >= 0; i--)
                contextMenu.Items.RemoveAt(i);
        }

        private void tree_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                TreeNode node = tree.GetNodeAt(e.X, e.Y);
                if (node != null && node.Tag != null) {
                    tree.SelectedNode = node;
                    ObjectOptions objectOptions = (ObjectOptions)node.Tag;
                    if (objectOptions.Options != null)
                        objectOptions.Options.DisplayOptions(this, contextMenu, e.Location);
                }
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {

            LoadObjectsAsyncParams loadParams = (LoadObjectsAsyncParams)e.Argument;
            AccessIO.BackupHelper backup = new AccessIO.BackupHelper(loadParams.App.FileName);
            backup.DoBackUp();
            string currentObjectName = null;
            try {
                List<IObjecOptions> selectedObjects = loadParams.SelectedObjects;
                int i = 0;
                foreach (IObjecOptions currentObject in selectedObjects) {
                    currentObjectName = currentObject.Name;
                    ((BackgroundWorker)sender).ReportProgress(i++ * 100 / selectedObjects.Count, currentObject);
                    AccessObject accessObject = AccessObject.CreateInstance(loadParams.App, currentObject.ObjectType, currentObject.ToString());
                    accessObject.Options = currentObject.Options;
                    accessObject.Load(currentObjectName);
                }
                backup.Commit();
                e.Result = selectedObjects.Count;
            } catch (Exception ex) {
                backup.RollBack();
                string msg = String.Format(Properties.Resources.ErrorLoadingObject, currentObjectName, ex.Message);
                throw new Exception(msg, ex);
            }
            
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            SelectedObjectsProgressEventArgs args = new SelectedObjectsProgressEventArgs(e.ProgressPercentage, (IObjecOptions)e.UserState);
            EventHandler<SelectedObjectsProgressEventArgs> tmp = LoadSelectecObjectsProgress;
            if (tmp != null)
                tmp(this, args);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            SelectedObjectsCompletedEventArgs args;
            //Accessing to e.Result when there is an error throws a exception.
            if (e.Error == null)
                args = new SelectedObjectsCompletedEventArgs(e.Error, (int)e.Result);
            else
                args = new SelectedObjectsCompletedEventArgs(e.Error, 0);
            EventHandler<SelectedObjectsCompletedEventArgs> tmp = LoadSelectedObjectsCompleted;
            if (tmp != null)
                tmp(this, args);
        }

        #region helper classes

        struct LoadObjectsAsyncParams {
            public AccessApp App;
            public List<IObjecOptions> SelectedObjects;
        }

        #endregion
    }
    
}
