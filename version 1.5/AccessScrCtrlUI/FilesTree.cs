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
using System.Drawing.Design;

namespace AccessScrCtrlUI {

    /// <summary>
    /// Visualize the files hierachy corresponding to a exported database
    /// </summary>
    public partial class FilesTree : UserControl {
        
        private string workingCopyPath;
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
                FillTree(value);
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
        public List<string> SelectedNodes {
            get {
                List<string> lst = new List<string>();
                if (tree.Nodes.Count == 0)
                    return lst;
                return InternalSelectedNodes(lst, tree.Nodes[0]);
            }
        }

        /// <summary>
        /// Reload the files list
        /// </summary>
        public void RefreshList() {
            FillTree(WorkingCopyPath);
        }

        private List<string> InternalSelectedNodes(List<string> list, TreeNode root) {
            foreach (TreeNode node in root.Nodes) {
                if (node.Checked) {
                    //If has children, iterate; else add leaf nodes to the list
                    if (node.Nodes.Count == 0)
                        list.Add((string)node.Tag);
                    else
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
            if (containers == null)
                return;

            const string key = "folder";
            Images = new TreeImages(containers);
            tree.ImageList = Images.ImageList;
            tree.ImageKey = key;
            tree.SelectedImageKey = key;

            DirectoryInfo rootDirectory = new DirectoryInfo(rootPath);
            FillDbPropertiesFiles(rootDirectory, rootNode);

            foreach (DirectoryInfo container in rootDirectory.EnumerateDirectories()) {
                //Check if folder name has an equivalent with allowed container names: 
                //if not, ignore that folder
                ContainerNames names = containers.Find(container.Name);
                if (names != null) {
                    string nodeText = container.Name;
                    TreeNode node = rootNode.Nodes.Add(nodeText);
                    node.ImageKey = names.FileExtension.ToString();
                    node.SelectedImageKey = names.FileExtension.ToString();

                    FillContainerFiles(container, node, names.FileExtension);
                }
            }

            if (rootNode.Nodes.Count == 0) {
                rootNode.Nodes.Add(Properties.Resources.EmptyFileStructure);
                tree.CheckBoxes = false;
            } else
                tree.CheckBoxes = true;
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

        private string GetExtension(string fileName) {
            return Path.GetExtension(Path.GetFileNameWithoutExtension(fileName));
        }

        private void FillDbPropertiesFiles(DirectoryInfo path, TreeNode rootNode) {
            foreach (FileInfo fi in path.EnumerateFiles(string.Format("*.{0}.txt", FileExtensions.dbp))) {
                TreeNode node = rootNode.Nodes.Add(fi.Name);
                node.ImageKey = FileExtensions.dbp.ToString();
                node.SelectedImageKey = FileExtensions.dbp.ToString();
            }
        }

        private void FillContainerFiles(DirectoryInfo container, TreeNode parentNode, FileExtensions fileExtension) {
            foreach (FileInfo fi in container.EnumerateFiles(string.Format("*.{0}.txt", fileExtension))) {
                TreeNode node = new TreeNode(fi.Name);
                node.Tag = fi.FullName;
                node.ImageKey = fileExtension.ToString();
                node.SelectedImageKey = fileExtension.ToString();
                parentNode.Nodes.Add(node);
            }
        }

        private string GetDatabaseFileName(string rootPath) {
            string dbProperties = Path.Combine(rootPath, AccessIO.Properties.Resources.DatabaseProperties + "." + FileExtensions.dbp.ToString() + ".txt");
            if (!File.Exists(dbProperties)) {
                return null;
            } else {
                StreamReader sr = new StreamReader(dbProperties);
                ImportObject import = new ImportObject(sr);
                return import.ReadObjectName();
            }
        }

    }
}
