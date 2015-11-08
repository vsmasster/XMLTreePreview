using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tree;

namespace TreePreview {
	public partial class MainWindow : Form {
        private Node<Record> root;
        private List<Func<IEnumerable<Node<Record>>, IEnumerable<Node<Record>>>> filters = 
            new List<Func<IEnumerable<Node<Record>>, IEnumerable<Node<Record>>>>();
		public MainWindow() {
			InitializeComponent();
		}

        private TreeNode treeSearch(Node<Record> cur) {
            TreeNode result = new TreeNode(cur.Data.ToString());

            foreach (Node<Record> child in cur.Children) 
                result.Nodes.Add(treeSearch(child));

            return result;
        }

        private void updateTree() {
            treeView.BeginUpdate();
            treeView.Nodes.Clear();
            TreeNode rootNode = new TreeNode(root.Data.ToString());
            foreach (Node<Record> child in root.Children)
                rootNode.Nodes.Add(treeSearch(child));

            treeView.Nodes.Add(rootNode);
            treeView.EndUpdate();

            preorderListBox.BeginUpdate();
            preorderListBox.Items.Clear();
            IEnumerable<Node<Record>> list = Tree.Tree.Traverse(root);

            foreach (Node<Record> node in list)
                preorderListBox.Items.Add(node.Data);

            preorderListBox.EndUpdate();
        }

        private void openFile() {
            openFileDialog.Filter = "xml files (.xml)|*.xml|All files(*.*)|(*.*)";
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                root = XmlTree.Load(openFileDialog.FileName);
                updateTree();
            }
        }

        private void openButton_Click(object sender, EventArgs e) {
            openFile();
        }

        private void addButton_Click(object sender, EventArgs e) {
            AddFilterDialog addFilterDialog = new AddFilterDialog();
            if (addFilterDialog.ShowDialog() == DialogResult.OK) {
                filters.Add(addFilterDialog.Result.Result);
                filterListBox.Items.Add(addFilterDialog.Result.ToString());
                filterListBox.SetSelected(filters.Count-1, true);
                root = Tree.Tree.Transform(root,addFilterDialog.Result.Result);
                updateTree();
            }
        }

        private void upButton_Click(object sender, EventArgs e) {
            int index = filterListBox.SelectedIndex;
            if (index != 0) {
                Func<IEnumerable<Node<Record>>, IEnumerable<Node<Record>>> temp = filters[index];
                filters[index] = filters[index - 1];
                filters[index - 1] = temp;

                filterListBox.BeginUpdate();
                Object temp2 = filterListBox.Items[index];
                filterListBox.Items[index] = filterListBox.Items[index - 1];
                filterListBox.Items[index - 1] = temp2;
                filterListBox.SetSelected(index, false);
                filterListBox.SetSelected(index - 1, true);
                filterListBox.EndUpdate();
            }
        }

        private void downButton_Click(object sender, EventArgs e) {
            int index = filterListBox.SelectedIndex;
            if (index != filters.Count - 1) {
                Func<IEnumerable<Node<Record>>, IEnumerable<Node<Record>>> temp = filters[index];
                filters[index] = filters[index + 1];
                filters[index + 1] = temp;

                filterListBox.BeginUpdate();
                Object temp2 = filterListBox.Items[index];
                filterListBox.Items[index] = filterListBox.Items[index + 1];
                filterListBox.Items[index + 1] = temp2;
                filterListBox.SetSelected(index, false);
                filterListBox.SetSelected(index + 1, true);
                filterListBox.EndUpdate();
            }
        }

        private void MainWindow_Load(object sender, EventArgs e) {
            openFile();
        }

        private void saveButton_Click(object sender, EventArgs e) {
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                XmlTree.Save(saveFileDialog.FileName, root);
            }
        }

	}
}
