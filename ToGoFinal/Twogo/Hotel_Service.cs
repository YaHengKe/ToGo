using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToGo
{
    public partial class Hotel_Service : Form
    {
        public Hotel_Service()
        {
            InitializeComponent();
        }

        global::ToGo.ToGoEntities dbContext2 = new ToGo.ToGoEntities();

        private void Form2_Load(object sender, EventArgs e)
        {
            var q = from f in dbContext2.ServiceAndFacilities
                    where f.ServiceFacilityID.ToString().StartsWith("1")
                    select f;

            //CheckedListBox CLB = new CheckedListBox();

            foreach (var group in q)
            {
                this.checkedListBox1.Items.Add(group.ServiceFacilityCHName);

                // CLB.Items.Add(group.ServiceFacilityID);
            }

        }

        List<string> StrServiceAndFacilities = new List<string>();
        List<int> IntServiceID = new List<int>();

        private void button1_Click(object sender, EventArgs e)
        {
            // 把選中的服務放到集合


            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                StrServiceAndFacilities.Add(checkedListBox1.CheckedItems[i].ToString());
            }

            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                string temp = StrServiceAndFacilities[i];
                var q = dbContext2.ServiceAndFacilities.Where(x => x.ServiceFacilityCHName == temp).Select(x => x.ServiceFacilityID).ToList();
                foreach (var item in q)
                {
                    IntServiceID.Add(item);
                }

                //選取的服務顯示在表格中
                //this.dataGridView1.DataSource = q;
            }

            for (int i = 0; i < IntServiceID.Count - 1; i++)
            {
                this.dbContext2.HotelServiceAndFacilities.Local.Add(new HotelServiceAndFacility
                {
                    ServiceFacilityID = IntServiceID[i],
                    HotelID = 2 //判斷Owner的HotelID

                });
                this.dbContext2.SaveChanges();
            }
            



            //string hotelfac= "";

            //for (int i = 0; i < checkedListBox1.Items.Count; i++)
            //{
            //    if (checkedListBox1.GetItemCheckState(i) == CheckState.Checked)
            //    {
            //        checkedListBox1.SetSelected(i, true);
            //        if (hotelfac == "")
            //        {
            //            hotelfac = checkedListBox1.SelectedItem.ToString();
            //        }
            //        else
            //        {
            //            hotelfac = hotelfac + "|" + checkedListBox1.SelectedItem.ToString();
            //        }
            //    }
            //}

        }

       
    }
}
