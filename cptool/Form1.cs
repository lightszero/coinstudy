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

                            var key = pinfo.GetDesc() + ":" + ps[x] + "->" + ps[y];
                            var pb = pinfo[y].buy;
                            var pa = pinfo[x].sell * 1.01;
                            if (pa == 0) continue;
                            var seed = (pb - pa) / pa * 100.0;
                            chance[key] = "机会 " + seed.ToString("0.##") + "% " + pa.ToString("0.####") + "->" + pb.ToString("0.####");
                        }
                        else if (pinfo[x].buy > pinfo[y].sell * 1.01)
                        {
                            var key = pinfo.GetDesc() + ":" + ps[y] + "->" + ps[x];
                            var pb = pinfo[x].buy;
                            var pa = pinfo[y].sell * 1.01;
                            if (pa == 0) continue;
                            var seed = (pb - pa) / pa * 100.0;
                            chance[key] = "机会 " + seed.ToString("0.##") + "% " + pa.ToString("0.####") + "->" + pb.ToString("0.####");
                        }
                    }
                }

            }


            //整理
            var list = new List<string>(chance.Keys);
            var listout = new List<string>(chance.Keys);
            for (var i = 0; i < chance.Keys.Count; i++)
            {
                var ckey = list[i];
                if (chance[ckey] == "-")
                {
                    listout[i] = "-" + ckey + ":" + chance[ckey];
                }
                else
                {
                    listout[i] = " " + ckey + ":" + chance[ckey];
                }
            }
            listout.Sort();
            //输出
            while (listBox1.Items.Count < listout.Count)
            {
                listBox1.Items.Add("");
            }
            while (listBox1.Items.Count > listout.Count)
            {
                listBox1.Items.RemoveAt(0);
            }

            for (var i = 0; i < listout.Count; i++)
            {

                listBox1.Items[i] = listout[i];
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();
        }
    }
}
