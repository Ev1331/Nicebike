namespace Nicebike;
using MySql.Data.MySqlClient;
using Nicebike.Models;
using Nicebike.Views;
using System.Collections.ObjectModel;

public partial class OrderFilling : ContentPage
{
    //Remplissage des �l�ments ListView
    string[] colorList = { "Red", "Blue", "Grey"};
    List<String> modelList = new List<String>();
    string[] sizeList = { "26\"", "28\""};

    List<BikeModel> bikeModels = new List<BikeModel>();
    int i=0;
    public int IdOrder;

    BikesManagement bikesManagement = new BikesManagement();
    BikeModelsManagement bikeModelsManagement = new BikeModelsManagement();
    OrderDetailsManagement orderDetailsManagement = new OrderDetailsManagement();

    public OrderFilling(int Id)
	{
		InitializeComponent();
        IdOrder = Id; // IdOrder devient accessible pour la fonction SaveBike, do not remove (!)

        bikeModels = bikeModelsManagement.GetAllBikeModels();
        foreach (BikeModel bikeModel in bikeModels)
        {
            modelList.Add(bikeModels[i].description);
            i++;
        }

        orderDetailsListView.ItemsSource = orderDetailsManagement.GetOrderBikes(IdOrder); //Liste des v�los de la commande
        colorPicker.ItemsSource = colorList;
        modelPicker.ItemsSource = modelList;
        sizePicker.ItemsSource = sizeList;
    }

    private void RemoveBike(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var IdBike = (int)button.CommandParameter;
        bikesManagement.DeleteBike(IdBike);
    }
    private void AddBike(object sender, EventArgs e)
    {

    }
    public void SaveBike(object sender, EventArgs e)
    {
        Picker color = this.FindByName<Picker>("colorPicker");
        Picker size = this.FindByName<Picker>("sizePicker");
        Picker model = this.FindByName<Picker>("modelPicker");

        bikesManagement.SendBike(colorList, sizeList, bikeModels, color, "...TYPE...", size,  "...REF...", model, "Waiting", IdOrder);
    }
}

public class BikesManagement
{
    int IdBike;
    string connectionString = "server=pat.infolab.ecam.be;port=63309;database=dbNicebike;user=projet_gl;password=root;";
    public List<Bike> GetAllBikes()
    {
        List<Bike> bikes = new List<Bike>();

        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "SELECT * FROM dbNicebike.bike";
        using MySqlCommand command = new MySqlCommand(sql, connection);
        using MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Bike bike = new Bike(
                reader.GetInt32("IdBike"),
                reader.GetString("Colour"),
                reader.GetString("Type"),
                reader.GetString("Size"),
                reader.GetString("Ref"),
                reader.GetInt32("Technician"),
                reader.GetInt32("BikeModel"),
                reader.GetString("Status")
            );
            bikes.Add(bike);
        }
        return bikes;
    }
    public void DeleteBike(int IdBike)
    {
        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "DELETE FROM dbNicebike.orderdetails WHERE Bike = @id";
        using MySqlCommand command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@id", IdBike);

        command.ExecuteNonQuery();

        sql = "DELETE FROM dbNicebike.bike WHERE IdBike = @id";
        using MySqlCommand command2 = new MySqlCommand(sql, connection);
        command2.Parameters.AddWithValue("@id", IdBike);

        command2.ExecuteNonQuery();
    }
    public void SendBike(string[] colorList, string[] sizeList, List<BikeModel> bikeModels, Picker color, string type, Picker size, string reference, Picker bikeModel, string status, int IdOrder)
    {
        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "INSERT INTO dbNicebike.bike (Colour, Type, Size, Ref, Technician, BikeModel, Status) VALUES (@Colour, @Type, @Size, @Ref, @Technician, @BikeModel, @Status)";
        MySqlCommand command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Colour", colorList[color.SelectedIndex]);
        command.Parameters.AddWithValue("@Type", type);
        command.Parameters.AddWithValue("@Size", sizeList[size.SelectedIndex]);
        command.Parameters.AddWithValue("@Ref", reference);
        command.Parameters.AddWithValue("@Technician", 0); //Bike ordered: no technician assigned yet
        command.Parameters.AddWithValue("@BikeModel", bikeModels[bikeModel.SelectedIndex].id);
        command.Parameters.AddWithValue("@Status", status);

        command.ExecuteNonQuery();

        sql = "SELECT IdBike FROM dbNicebike.bike ORDER BY IdBike DESC LIMIT 1";
        MySqlCommand command2 = new MySqlCommand(sql, connection);
        MySqlDataReader reader = command2.ExecuteReader();

        while (reader.Read())
        {
            IdBike = reader.GetInt32("IdBike");  
        }

        using MySqlConnection connection2 = new MySqlConnection(connectionString);
        connection2.Open();
        sql = "INSERT INTO dbNicebike.orderdetails (IdOrder, Bike) VALUES (@IdOrder, @IdBike)";
        MySqlCommand command3 = new MySqlCommand(sql, connection2);
        command3.Parameters.AddWithValue("@IdOrder", IdOrder);
        command3.Parameters.AddWithValue("@IdBike", IdBike);
        command3.ExecuteNonQuery();
    }
}

public class BikeModelsManagement
{
    public List<BikeModel> GetAllBikeModels()
    {
        List<BikeModel> bikeModels = new List<BikeModel>();

        string connectionString = "server=pat.infolab.ecam.be;port=63309;database=dbNicebike;user=projet_gl;password=root;";
        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "SELECT * FROM dbNicebike.bikemodel";
        using MySqlCommand command = new MySqlCommand(sql, connection);
        using MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            BikeModel bikeModel = new BikeModel(
                reader.GetInt32("IdBikeModel"),
                reader.GetInt32("Price"),
                reader.GetString("Description")
            );
            bikeModels.Add(bikeModel);
        }
        return bikeModels;
    }
}
public class OrderDetailsManagement
{
    int id;
    List<int> bikesIdList = new List<int>();
    public List<Bike> orderBikes = new List<Bike>();
    public List<Bike> GetOrderBikes(int IdOrder)

    {
        List<Bike> bikes = new List<Bike>();

        BikesManagement bikesManagement = new BikesManagement();
        List<Bike> observableBikes = bikesManagement.GetAllBikes();

        string connectionString = "server=pat.infolab.ecam.be;port=63309;database=dbNicebike;user=projet_gl;password=root;";
        using MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();

        string sql = "SELECT * FROM dbNicebike.orderdetails WHERE IdOrder = @IdOrder";
        using MySqlCommand command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@IdOrder", IdOrder);
        using MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            id = reader.GetInt32("Bike");
            bikesIdList.Add(id); 
            orderBikes.Add(observableBikes.Find(obj => obj.id == id)); //Monte une liste avec les v�los correspondants
        }

    return orderBikes;
    }
}