﻿using System;
using Nicebike.Models;
using MySql.Data.MySqlClient;
namespace Nicebike.ViewModels
{
	public class EmployeeManagement
    {
        private MySqlConnection connection = new MySqlConnection("server=pat.infolab.ecam.be;port=63309;database=dbNicebike;user=projet_gl;password=root;");
        private string sql;
        public List<Employee> GetAllEmployee()
        {
            List<Employee> employees = new List<Employee>();
            connection.Open();

            sql = "SELECT * FROM dbNicebike.employee";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (reader.GetString("IdEmployee") != "0")
                {
                    Employee employee = new Employee(
                    reader.GetInt32("IdEmployee"),
                    reader.GetString("Name"),
                    reader.GetString("Surname"),
                    reader.GetString("Mail"),
                    reader.GetString("JobTitle"),
                    reader.GetString("Phone"));

                    employees.Add(employee);
                }
            }
            connection.Close();
            return employees;
        }

        public void SendEmployee(string[] jobTitleList, Entry name, Entry surname, Entry mail, Picker jobtitle, Entry phone)
        {
            connection.Open();

            sql = "INSERT INTO dbNicebike.employee (Name, Surname, Mail, JobTitle, Phone) VALUES (@name, @surname, @mail, @jobtitle, @phone)";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@name", name.Text);
            command.Parameters.AddWithValue("@surname", surname.Text);
            command.Parameters.AddWithValue("@mail", mail.Text);
            command.Parameters.AddWithValue("@jobtitle", jobTitleList[jobtitle.SelectedIndex]);
            command.Parameters.AddWithValue("@phone", phone.Text);

            command.ExecuteNonQuery();
            connection.Close();
        }

        public void DeleteEmployee(int idEmployee)

        {
            connection.Open();

            sql = "DELETE FROM dbNicebike.employee WHERE idEmployee = @id";

            using MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", idEmployee);

            command.ExecuteNonQuery();
            connection.Close();

        }

        public void ModifyEmployee(string[] jobTitleList, int id, string name, string surname, string mail, Picker jobtitle, string phone)
        {
            connection.Open();

            sql = "UPDATE dbNicebike.employee SET Name = @name, Surname = @surname, Mail = @mail, JobTitle = @jobtitle, Phone = @phone WHERE IdEmployee = @id";
            MySqlCommand command = new MySqlCommand(sql, connection);

            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@surname", surname);
            command.Parameters.AddWithValue("@mail", mail);
            command.Parameters.AddWithValue("@jobtitle", jobTitleList[jobtitle.SelectedIndex]);
            command.Parameters.AddWithValue("@phone", phone);
            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
            connection.Close();
        }

        public List<Employee> GetTechnician()
        {
            List<Employee> employees = new List<Employee>();

            connection.Open();

            sql = "SELECT * FROM dbNicebike.employee WHERE JobTitle = 'Technician'";
            using MySqlCommand command = new MySqlCommand(sql, connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string jobTitle = reader.GetString("JobTitle");
                if (jobTitle == "Technician")
                {
                    Employee employee = new Employee(
                        reader.GetInt32("IdEmployee"),
                        reader.GetString("Name"),
                        reader.GetString("Surname"),
                        reader.GetString("Mail"),
                        jobTitle,
                        reader.GetString("Phone")
                    );
                    employees.Add(employee);
                }
            }
            connection.Close();
            return employees;
        }
    }
}