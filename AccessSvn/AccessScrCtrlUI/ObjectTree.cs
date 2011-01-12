using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AccessIO;

namespace AccessScrCtrlUI {
    public partial class ObjectTree : UserControl {

        private string fileName;
        private AccessApp app;

        /// <summary>
        /// Get or set the <see cref="AccessApp"/> object wich contains the objects to list
        /// </summary>
        /// <remarks>
        /// Setting this property also fills the object tree
        /// </remarks>
        [System.ComponentModel.Browsable(false)]
        public AccessApp App { 
            get { return this.app; }
            set {
                this.app = value;
                if (value != null)
                    FillObjectTree(value);
            }
        }

        /// <summary>
        /// Get or set Access file name wich contains the objects to list.
        /// </summary>
        /// <remarks>
        /// Setting this property, also sets the <see cref="App"/> property and fills the object tree
        /// </remarks>
        [System.ComponentModel.Description("Get or set MS Access file name wich contains the objects to list.")]
        public string FileName {
            get { return fileName; }
            set {
                fileName = value;
                if (!String.IsNullOrEmpty(fileName))
                    App = AccessApp.AccessAppFactory(value);
            }
        }

        /// <summary>
        /// Default contructor
        /// </summary>
        public ObjectTree() {
            InitializeComponent();
            tree.PathSeparator = System.IO.Path.DirectorySeparatorChar.ToString();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileName">Sets the <see cref="App"/> property and fills the object tree</param>
        public ObjectTree(string fileName) : base() {
            App = AccessApp.AccessAppFactory(fileName);
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="app">Setting this property also fills the object tree</param>
        public ObjectTree(AccessIO.AccessApp app): base() {
            App = app;
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
        public List<IObjectName> SelectedNodes {
            get {
                List<IObjectName> lst = new List<IObjectName>();
                if (tree.Nodes.Count == 0)
                    return lst;
                return InternalSelectedNodes(lst, tree.Nodes[0]);
            }
        }

        private List<IObjectName> InternalSelectedNodes(List<IObjectName> list, TreeNode root) {
            foreach (TreeNode node in root.Nodes) {
                if (node.Checked) {
                    //If has children, iterate; else add leaf nodes to the list
                    if (node.Nodes.Count == 0)
                        list.Add((IObjectName)node.Tag);
                    else
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
            //TODO: Do the work in background
            if (app == null)
                throw new ArgumentNullException("app");
            tree.Nodes.Clear();
            TreeNode root = tree.Nodes.Add(Path.GetFileName(app.FileName));
            root.ToolTipText = app.FileName;
            foreach (ObjectType objectType in app.AllowedObjetTypes) {
                string nodeText = Properties.Resources.ResourceManager.GetString(String.Concat(objectType, "Plural"));
                TreeNode subItem = root.Nodes.Add(nodeText);
                foreach (IObjectName item in app.LoadObjectNames(objectType)) {
                    TreeNode node = new TreeNode(item.Name);
                    node.Tag = item;
                    subItem.Nodes.Add(node);
                }
            }
        }

        /// <summary>
        /// Checks all the children nodes and makes sure that parents are checked
        /// </summary>
        private void tree_AfterCheck(object sender, TreeViewEventArgs e) {

            //Setting Checked from within AfterCheck event causes the event to be raised multiple times.
            //To avoid this, only check child an parent nodes if the user causes the state change.
            //see AfterCheck documentation
            if (e.Action != TreeViewAction.Unknown) {
                CheckChildren(e.Node);
                CheckParentes(e.Node);
            }
        }

        private void CheckParentes(TreeNode node) {
            while (node.Parent != null) {
                if (node.Parent.Checked == false)
                    node.Parent.Checked = true;
                node = node.Parent;
            }
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
    }
}
