﻿using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using AccessIO;
using System.ComponentModel;

namespace AccessScrCtrlUI {
    public partial class ObjectTree : UserControl {

        private AccessApp app;
        private delegate void FillTreeDelegate();

        /// <summary>
        /// Get or set the <see cref="AccessApp"/> object wich contains the objects to list
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public AccessApp App { 
            get { return this.app; }
            set {
                if (this.app != null) {
                    this.app.QuitApplication();
                }
                this.app = value;
            }
        }

        /// <summary>
        /// Gets Access file name wich contains the objects to list.
        /// </summary>
        /// <remarks>
        /// Setting this property, also sets the <see cref="App"/> property, open the database and fills the object tree
        /// </remarks>
        [System.ComponentModel.Description("Get MS Access file name wich contains the objects to list.")]
        public string FileName { get; private set; }

        /// <summary>
        /// Default contructor
        /// </summary>
        public ObjectTree() {
            InitializeComponent();
            Disposed += ObjectTree_Disposed;
            tree.PathSeparator = System.IO.Path.DirectorySeparatorChar.ToString();
        }

        private void ObjectTree_Disposed(object sender, EventArgs e) {
            if (App != null)
                App.QuitApplication();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileName">Sets the <see cref="App"/> property and fills the object tree</param>
        public ObjectTree(string fileName) : base() {
            FileName = fileName;
            App = AccessApp.AccessAppFactory(FileName);
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="app">Setting this property also fills the object tree</param>
        public ObjectTree(AccessIO.AccessApp app): base() {
            App = app;
        }

        public void Attach(string fileName, string workingDirectory) {
            FileName = fileName;
            if (!string.IsNullOrEmpty(FileName)) {

                //It could fail for any of following circunstances:
                //User press cancel in security warning dialog when the database has VBA code
                //Macros security level is High and the database has VBA code
                //User cancel or fails the password for a passsword protected database
                //Database is upper version that the version of installed MS Access
                //Database is corrupted
                try {
                    App = AccessApp.AccessAppFactory(FileName);
                    App.OpenDatabase();
                    FillObjectTree(App);
                    App.WorkingCopyPath = workingDirectory;
                } catch {
                    throw;
                }
            }

        }

        /// <summary>
        /// Reload the object tree
        /// </summary>
        public void RefreshList() {
            if (app != null)
                FillObjectTree(app);
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
        /// Raised when SaveSelectedObjects operation finalizes
        /// </summary>
        [System.ComponentModel.Description("Raised when SaveSelectedObjects operation finalizes")]
        public event EventHandler<SelectedObjectsCompletedEventArgs> SaveSelectedObjectsCompleted;

        /// <summary>
        /// Raised before saving a object.
        /// </summary>
        [System.ComponentModel.Description("Raised before saving a object")]        
        public event EventHandler<SelectedObjectsProgressEventArgs> SaveSelectecObjectsProgress;


        /// <summary>
        /// Save, in background, the selected objects to the <see cref="AccessApp.WorkingCopyPath"/> path
        /// </summary>
        public void SaveSelectedObjectsAsync() {

            backgroundWorker.RunWorkerAsync(SelectedNodes);
        }

        /// <summary>
        /// Save the selected objects to the <see cref="AccessApp.WorkingCopyPath"/> path
        /// </summary>
        /// <returns>Number of saved objects</returns>
        /// <remarks>It's recomended to use the async version</remarks>
        public int SaveSelectedObjects() {
            List<IObjecOptions> selectedObjects = this.SelectedNodes;
            foreach (IObjecOptions name in selectedObjects) {
                AccessObject accessObject = AccessObject.CreateInstance(this.App, name.ObjectType, name.Name);
                accessObject.Save();
            }
            return selectedObjects.Count;
        }

    #region private methods

        private TreeImages Images { get; set; }

        private List<IObjecOptions> InternalSelectedNodes(List<IObjecOptions> list, TreeNode root) {
            foreach (TreeNode node in root.Nodes) {
                if (node.Checked) {
                    //If has associated IObjectName, add leaf nodes to the list; else if has children, iterate
                    if (node.Tag != null)
                        list.Add((IObjecOptions)node.Tag);
                    else if (node.Nodes.Count > 0)
                        InternalSelectedNodes(list, node);
                }
            }
            return list;
        }
        
        /// <summary>
        /// Enumerate the allowed object types and, for each, the list of objects for that type
        /// </summary>
        /// <param name="app">Access application</param>
        /// <exception cref="ArgumentNullException"> if <paramref name="app"/> is null</exception>
        private void FillObjectTree(AccessIO.AccessApp app) {
            if (app == null)
                throw new ArgumentNullException("app");

            if (tree.InvokeRequired) {
                tree.Invoke(new FillTreeDelegate(FillTree));
            } else {
                FillTree();
            }
        }

        private void FillTree() {
            const string key = "db";
            Images = new TreeImages(AccessApp.ContainersFactory(FileName));
            tree.ImageList = Images.ImageList;
            tree.ImageKey = key;
            tree.SelectedImageKey = key;

            tree.Nodes.Clear();
            TreeNode root = tree.Nodes.Add(Path.GetFileName(app.FileName));
            root.ToolTipText = app.FileName;
            foreach (ContainerNames container in app.AllowedContainers) {
                TreeNode subItem = root.Nodes.Add(container.DisplayPluralName);
                subItem.ImageKey = container.DefaultExtension.ToString();
                subItem.SelectedImageKey = container.DefaultExtension.ToString();
                foreach (IObjecOptions item in app.LoadObjectNames(container.InvariantName)) {
                    TreeNode node = new TreeNode(item.Name);
                    node.Tag = item;
                    node.ImageKey = app.AllowedContainers.Find(item.ObjectType).FileExtension.ToString();
                    node.SelectedImageKey = node.ImageKey;
                    subItem.Nodes.Add(node);
                }
            }
            root.Expand();
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

        private void ToogleNodes(TreeNode node) {
            node.Checked = !node.Checked;
            foreach (TreeNode child in node.Nodes) {
                child.Checked = !child.Checked;
                ToogleNodes(child);
            }
        }

        private void selectAllMenu_Click(object sender, EventArgs e) {
            if (tree.Nodes.Count > 0) {
                tree.Nodes[0].Checked = true;
                CheckChildren(tree.Nodes[0]);
            }
        }

        private void unselectAllMenu_Click(object sender, EventArgs e) {
            if (tree.Nodes.Count > 0) {
                tree.Nodes[0].Checked = false;
                CheckChildren(tree.Nodes[0]);
            }
        }

        private void toogleNodesMenu_Click(object sender, EventArgs e) {
            if (tree.SelectedNode != null)
                ToogleNodes(tree.SelectedNode);
        }

        private void tree_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                TreeNode selectedNode = tree.GetNodeAt(e.Location);
                if (selectedNode != null)
                    contextMenu.Show(tree, e.Location);
            }
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            //see http://msdn.microsoft.com/en-us/library/9hk12d4y.aspx for Event-based async pattern
            //see http://stackoverflow.com/questions/6184/how-do-i-make-event-callbacks-into-my-win-forms-thread-safe

            List<IObjecOptions> selectedObjects = (List<IObjecOptions>)e.Argument;
            int i = 0;
            foreach (IObjecOptions name in selectedObjects) {
                i++;
                ((BackgroundWorker)sender).ReportProgress(i * 100 / selectedObjects.Count, name);
                AccessObject accessObject = AccessObject.CreateInstance(this.App, name.ObjectType, name.Name);
                accessObject.Save();
            }
            e.Result = selectedObjects.Count;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            SelectedObjectsProgressEventArgs args = new SelectedObjectsProgressEventArgs(e.ProgressPercentage, (IObjecOptions)e.UserState);
            EventHandler<SelectedObjectsProgressEventArgs> tmp = SaveSelectecObjectsProgress;
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
            EventHandler<SelectedObjectsCompletedEventArgs> tmp = SaveSelectedObjectsCompleted;
            if (tmp != null)
                tmp(this, args);
        }
    
    #endregion
    
    }
    

 

}
