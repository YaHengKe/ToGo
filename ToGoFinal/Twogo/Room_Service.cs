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
    public partial class Room_Service : Form
    {
        public Room_Service()
        {
            InitializeComponent();
        }

        global::ToGo.ToGoEntities dbContext = new ToGo.ToGoEntities();

        private void Room_Service_Load(object sender, EventArgs e)
        {
            var q = from r in dbContext.ServiceAndFacilities
                    where r.ServiceFacilityID.ToString().StartsWith("9")
                    select r;
            foreach(var item in q)
            {
                this.checkedListBox1.Items.Add(item.ServiceFacilityCHName);
            }
        }

        List<string> RoomService = new List<string>();
        List<int> RoomServiceID = new List<int>();

        private void button1_Click(object sender, EventArgs e)
        {
            for(int i=0;i<checkedListBox1.CheckedItems.Count;i++)
            {
                RoomService.Add(checkedListBox1.CheckedItems[i].ToString());
            }

            for(int i=0; i<checkedListBox1.CheckedItems.Count;i++)
            {
                string temp = RoomService[i];
                var q = dbContext.ServiceAndFacilities.Where(x => x.ServiceFacilityCHName == temp).Select(x=>x.ServiceFacilityID).ToList();
                foreach(var item in q)
                {
                    RoomServiceID.Add(item);
                }
            }

            for(int i=0; i<RoomServiceID.Count-1;i++)
            {
                this.dbContext.RoomServiceAndFacilities.Local.Add(new RoomServiceAndFacility
                {
                    ServiceFacilityID = RoomServiceID[i],
                    RoomID = 1
                });
                this.dbContext.SaveChanges();
            }
        }
    }
}
