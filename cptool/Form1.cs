using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cptool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        PriceTool ptool = new PriceTool();
        private void Form1_Load(object sender, EventArgs e)
        {
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = false;
            this.dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            ptool.Init();

            var keys = ptool.GetKeys();
            this.dataGridView1.Columns.Add("name", "name");
            var ps = ptool.GetPrices();
            for (var i = 0; i < ptool.GetPrices().Length; i++)
            {
                var pkey = "price" + (i).ToString();
                this.dataGridView1.Columns.Add(pkey, "price" + ps[i]);
                this.dataGridView1.Columns[pkey].Width = 150;
                this.dataGridView1.Columns.Add("vol" + (i).ToString(), "vol" + ps[i]);
            }
            for (var i = 0; i < keys.Length; i++)
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].Cells[0].Value = ptool.GetInfos(keys[i]).GetDesc() + keys[i];
            }
        }
        class changeInfo
        {

        }
        Dictionary<string, string> chance = new Dictionary<string, string>();

        private void timer1_Tick(object sender, EventArgs e)
        {
            var keys = ptool.GetKeys();

            foreach (var k in new List<string>(chance.Keys))
            {
                chance[k] = "-";
            }

            for (var i = 0; i < keys.Length; i++)
            {
                var ps = ptool.GetPrices();
                var pinfo = ptool.GetInfos(keys[i]);
                for (var j = 0; j < ps.Length; j++)
                {
                    var pi = pinfo[j];
                    if (pi == null) continue;
                    var key = "price" + j;
                    this.dataGridView1.Rows[i].Cells[key].Value = pi.price + "(" + pi.buy + "/" + pi.sell + ")";

                    var keyv = "vol" + j;
                    this.dataGridView1.Rows[i].Cells[keyv].Value = pi.vol;
                }
                //差价发现

                for (var x = 0; x < ps.Length; x++)
                {
                    for (var y = x + 1; y < ps.Length; y++)
                    {
                        if (x == y) continue;

                        if (pinfo[x] == null || pinfo[y] == null) continue;


                        if (pinfo[x].sell * 1.01 < pinfo[y].buy)
                        {
                            var key = pinfo.GetDesc() + ":"+ps[x] +"->"+ ps[y];
                            chance[key] = "机会";
                        }
                        else if (pinfo[x].buy > pinfo[y].sell * 1.01)
                        {
                            var key = pinfo.GetDesc() +":"+ps[y]+"->" + ps[x];
                            chance[key] = "机会";
                        }
                    }
                }

            }

            while (listBox1.Items.Count < chance.Keys.Count)
            {
                listBox1.Items.Add("");
            }
            var list = new List<string>(chance.Keys);
            for (var i = 0; i < chance.Keys.Count; i++)
            {
                var ckey = list[i];
                listBox1.Items[i] = ckey + ":" + chance[ckey];
            }
        }
    }
}
