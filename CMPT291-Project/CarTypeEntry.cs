﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace CMPT291_Project
{
    public partial class CarTypeEntry : Form
    {
        public SqlConnection myConnection;
        public SqlCommand myCommand;
        public SqlDataReader myReader;
        public int state = 0;
        public string newCommand;

        public CarTypeEntry()
        {
            InitializeComponent();
            AddRBtn.Checked = true;

            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            SqlConnection myConnection = new SqlConnection(connectionString);

            try
            {
                myConnection.Open(); // Open connection
                myCommand = new SqlCommand();
                myCommand.Connection = myConnection; // Link the command stream to the connection
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error");
                this.Close();
            }

        }

        private void resetEditRemove()
        {
            FindID.Visible = true;
            CarTypeIdBx.Visible = true;
            CarTypeIdBx.Text = String.Empty;

            descentry.Visible = false;
            drateentry.Visible = false;
            wrateentry.Visible = false;
            mrateentry.Visible = false;
        }

        private void ctentrycancel_Click_1(object sender, EventArgs e)
        {
            state = 0;
            this.Close();
        }

        private void ctentryacc_Click_1(object sender, EventArgs e)
        {
            if (AddRBtn.Checked == true) //adds car type to the database
            {
                try
                {
                    myCommand.CommandText = "insert into CarType values ('" + descentry.Text +
                        "'," + drateentry.Text + "," + wrateentry.Text + "," + mrateentry.Text + ")";
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception e2)
                {
                    MessageBox.Show(e2.ToString(), "Error");
                }

                this.Close();
            }

            else if (EditRBtn.Checked == true) //edits car type entry
            {
                try
                {
                    myCommand.CommandText = "update CarType Set Description = '" + descentry.Text +
                        "', DailyRate = " + drateentry.Text + ", WeeklyRate = " + wrateentry.Text +
                        ", MonthlyRate = " + mrateentry.Text + " where CarTypeID = " + CarTypeIdBx.Text;
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception e2)
                {
                    MessageBox.Show(e2.ToString(), "Error");
                }

                this.Close();
            }

            else if (RemoveRBtn.Checked == true) //deletes car type entry
            {
                try
                {
                    myCommand.CommandText = "delete from CarType where CarTypeId = " + CarTypeIdBx.Text;
                    myCommand.ExecuteNonQuery();
                }
                catch (Exception e2)
                {
                    MessageBox.Show(e2.ToString(), "Error");
                }

                this.Close();
            }
        }

        private void AddRBtn_CheckedChanged_1(object sender, EventArgs e)
        {
            FindID.Visible = false;
            CarTypeIdBx.Visible = false;

            //ensures the text boxes are visible and empty
            descentry.Visible = true;
            drateentry.Visible = true;
            wrateentry.Visible = true;
            mrateentry.Visible = true;

            descentry.Text = String.Empty;
            drateentry.Text = String.Empty;
            wrateentry.Text = String.Empty;
            mrateentry.Text = String.Empty;
        }

        private void EditRBtn_CheckedChanged_1(object sender, EventArgs e)
        {
            resetEditRemove();
        }

        private void RemoveRBtn_CheckedChanged_1(object sender, EventArgs e)
        {
            resetEditRemove();
        }

        private void FindID_Click_1(object sender, EventArgs e)
        {
            //converts string to integer 
            int displayID;
            bool success = int.TryParse(CarTypeIdBx.Text, out displayID);

            if (success)
            {
                try
                {
                    myCommand.CommandText = "select * from CarType where CarTypeId = " + displayID;
                    myReader = myCommand.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        //saves variables read and displays them in the appropriate fields
                        while (myReader.Read())
                        {

                            string des = (string)myReader["Description"];
                            decimal dr = (decimal)myReader["DailyRate"];
                            decimal wr = (decimal)myReader["WeeklyRate"];
                            decimal mr = (decimal)myReader["MonthlyRate"];

                            descentry.Visible = true;
                            drateentry.Visible = true;
                            wrateentry.Visible = true;
                            mrateentry.Visible = true;

                            descentry.Text = des;
                            drateentry.Text = dr.ToString();
                            wrateentry.Text = wr.ToString();
                            mrateentry.Text = mr.ToString();
                        }


                    }

                    else
                    {
                        CarTypeIdBx.Text = string.Empty;

                        MessageBox.Show("Invalid Car Type ID", "Error");
                    }
                }
                catch (Exception e2)
                {
                    MessageBox.Show(e2.ToString(), "Error");
                }
                myReader.Close();

            }

            else
            {
                resetEditRemove();
            }
        }
    }
}
