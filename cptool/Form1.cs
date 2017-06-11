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
        Price_Jubi p = new Price_Jubi();
        Price_19800 p2 = new Price_19800();
        Price_yuanbao p3 = new Price_yuanbao();
        private void Form1_Load(object sender, EventArgs e)
        {
            Dictionary<string, int> cnt = new Dictionary<string, int>();
            foreach(var k in p.GetKeys())
            {
                if (cnt.ContainsKey(k)) cnt[k]++;
                else
                    cnt[k] = 1;
            }
            foreach (var k in p2.GetKeys())
            {
                if (cnt.ContainsKey(k)) cnt[k]++;
                else
                    cnt[k] = 1;
            }
            foreach (var k in p3.GetKeys())
            {
                if (cnt.ContainsKey(k)) cnt[k]++;
                else
                    cnt[k] = 1;
            }
            var vs = new List<string>(cnt.Keys);
            foreach(var v in vs)
            {
                if (cnt[v] < 2)
                    cnt.Remove(v);
            }
            int i = 0;
            vs = new List<string>(cnt.Keys);
            foreach (var k in vs)
            {
                cnt[k] = i;
                i++;
            }
            p.Init(vs);
            p2.Init(vs);
            p3.Init(vs);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var keys = p.GetKeys();
            while (this.listBox1.Items.Count < keys.Length)
            {
                this.listBox1.Items.Add("");
            }
            var keys2 = p2.GetKeys();
            while (this.listBox2.Items.Count < keys2.Length)
            {
                this.listBox2.Items.Add("");
            }
            var keys3 = p3.GetKeys();
            while (this.listBox3.Items.Count < keys3.Length)
            {
                this.listBox3.Items.Add("");
            }

            for (var i = 0; i < keys.Length; i++)
            {
                var pi = p.GetInfo(keys[i]);

                if (pi == null) continue;
                if (pi.change)
                    this.listBox1.Items[i] = pi.ToString();
            }

            for (var i = 0; i < keys2.Length; i++)
            {
                var pi = p2.GetInfo(keys2[i]);

                if (pi == null) continue;
                if (pi.change)
                    this.listBox2.Items[i] = pi.ToString();
            }

            for (var i = 0; i < keys3.Length; i++)
            {
                var pi = p3.GetInfo(keys3[i]);

                if (pi == null) continue;
                if (pi.change)
                    this.listBox3.Items[i] = pi.ToString();
            }
        }
    }
}
