using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_System
{
    public partial class mj_searchProductMasterItem : Form
    {
        mj_ProductMaster frmProductMaster;

        public mj_searchProductMasterItem(mj_ProductMaster frm)
        {
            InitializeComponent();
            frmProductMaster = frm;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchValue = txtSearch.Text.Trim(); // Get the user input from txtSearch
            bool found = false;

            frmProductMaster.dataGridView1.ClearSelection();

            // Loop through the rows in the actual DataGridView in frmProductMaster
            foreach (DataGridViewRow row in frmProductMaster.dataGridView1.Rows)
            {
                // Compare the cell value with the search input
                if (row.Cells[2].Value != null &&
                    row.Cells[2].Value.ToString().Equals(searchValue, StringComparison.OrdinalIgnoreCase))
                {
                    // Select the row and scroll it into view
                    row.Selected = true;
                    frmProductMaster.dataGridView1.FirstDisplayedScrollingRowIndex = row.Index;
                    found = true;
                    break; // Exit the loop when the first match is found
                }
            }

            // If an item is found, close the search form, else show a message
            if (found)
            {
                this.Close(); // Close the search form after selecting the item
            }
            else
            {
                MessageBox.Show("Item not found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
