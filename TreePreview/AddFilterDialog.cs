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
	public partial class AddFilterDialog : Form {
        private Filter result;
        public AddFilterDialog() {
            InitializeComponent();
        }

        public Filter Result {
            get { return result; }
        }

        private void addButton_Click(object sender, EventArgs e) {
            result = new Filter();

            if (radioButton1.Checked) {
                result.Type = 0;
                result.RecordKey = textBox1.Text;
                result.ActType = comboBox1.SelectedIndex;
                result.ExtraValue = textBox2.Text;
            } else if (radioButton2.Checked) {
                result.Type = 1;
                result.RecordKey = textBox3.Text;
                result.ActType = comboBox2.SelectedIndex;
                result.ExtraValue = textBox4.Text;
            } else if (radioButton3.Checked) {
                result.Type = 2;
                result.RecordKey = textBox5.Text;
            } else if (radioButton4.Checked) {
                result.Type = 3;
                result.RecordKey = textBox6.Text;
            }

            result.Process();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
	}

    public class Filter {
        private int type, actType;
        private String recordKey, extraValue;
        private Func<IEnumerable<Node<Record>>, IEnumerable<Node<Record>>> result;
        public Func<IEnumerable<Node<Record>>, IEnumerable<Node<Record>>> Result {
            get { return result; }
        }

        public void Process() {
            result = (nodeSet) => {
                List<Node<Record>> res = new List<Node<Record>>();
                if (type == 0) {
                    foreach (Node<Record> node in nodeSet) {
                        if (actType == 0 && node.Data[recordKey].Contains(extraValue))
                            res.Add(node);
                        else if (actType == 1 && node.Data[recordKey].Equals(extraValue))
                            res.Add(node);
                    }
                } else if (type == 1) {
                    foreach (Node<Record> node in nodeSet) {
                        if (actType == 0 && int.Parse(node.Data[recordKey]) == int.Parse(extraValue))
                            res.Add(node);
                        else if (actType == 1 && int.Parse(node.Data[recordKey]) <= int.Parse(extraValue))
                            res.Add(node);
                        else if (actType == 2 && int.Parse(node.Data[recordKey]) >= int.Parse(extraValue))
                            res.Add(node);
                    }
                } else if (type == 2) {
                    return nodeSet.OrderBy(c => c.Data[recordKey]).ToList();
                } else if (type == 3) {
                    res = nodeSet.OrderBy(c => int.Parse(c.Data[recordKey])).ToList();
                }
                return res;
            };
        }
        

        public int Type {
            get { return type; }
            set { type = value; }
        }

        public int ActType {
            get { return actType; }
            set { actType = value; }
        }

        public String RecordKey {
            get { return recordKey; }
            set { recordKey = value; }
        }

        public String ExtraValue {
            get { return extraValue; }
            set { extraValue = value; }
        }

        public override String ToString() {
            String result = "";
            String[] type0 = {"contains","equal"}, type1 = {"==","<=",">="};
            switch (type) {
                case 0:
                    result = "list => list.Where(n => n.Data[\"" + recordKey + "\"] " + type0[actType] + " (\"" + extraValue + "\"))";
                    break;
                case 1:
                    result = "list => list.Where(n => int.Parse(n.Data[\"" + recordKey + "\"]) " + type1[actType] + " (" + extraValue + "))";
                    break;
                case 2:
                    result = "list => list.OrderBy(n => n.Data[\"" + recordKey + "\"])";
                    break;
                case 3:
                    result = "list => list.OrderBy(n => int.Parse(n.Data[\"" + recordKey + "\"])";
                    break;
            }

            return result;
        }
    }
}
