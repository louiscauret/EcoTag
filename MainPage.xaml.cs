using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Threading;
//using System.Windows.Forms;

// Pour en savoir plus sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=234238

namespace EcoTagDemo
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public class Alerts
        {
            public string imageName { get; set; }
            public string errorText { get; set; }
        }
        public List<Alerts> mylist;

        //public sealed class _mylist
        //{
        //    public static _mylist list = new _mylist();
        //    _mylist() 
        //    {
        //        mylist = new List<Alerts>();
        //    }
        //    public List<Alerts> mylist;
        //}


        //private List<Alerts> _myAlerts()
        //{
        //    List<Alerts> mylist = new List<Alerts>();
        //    List<string> logo = new List<string>() { "Assets/alerte1_0003s_0006s_0000_fenetre-ouverte.png", "Assets/logo_vmc.png", "Assets/alerte3_0003s_0004s_0000_fenetre-fermée.png", 
        //                            "Assets/logo_chauffe-eau.png", "Assets/logo_robinet.png", "Assets/logo_lumiere.png", };
        //    List<string> text = new List<string>() { "Fermez la fenêtre de la pièce pour conserver votre niveau de confort.",
        //                        "Mettez la VMC de la salle de bain sur 3 pour améliorer la qualité de l'air.",
        //                        "Ouvrez la fenêtre de la chambre parentale pour conserver votre niveau de confort.",
        //                        "Veuillez baisser votre thermostat. La température de votre accumulateur solaire est trop élevée.",
        //                        "Veuillez vérifier la fermeture de vos robinets ou essayez de localiser une fuite. Votre consommation d'eau est anormalement élevée.",
        //                        "Veuillez visser ou changer l'ampoule de vos toilettes, elle ne semble plus fonctionner correctement." };
        //    for (int i = 0; i < 6; i++)
        //    {
        //        Alerts myAlert = new Alerts();

        //        myAlert.imageName = logo[i];
        //        myAlert.errorText = text[i];
        //        mylist.Add(myAlert);
        //    }
        //    return (mylist);
        //}

        //public List<Alerts> AlerteList { get { return _myAlerts(); } }

        public MainPage()
        {
            this.InitializeComponent();
            AppBar.IsOpen = true;
            this.DataContext = this;

            Task t = new Task(DownloadJSONASync);
            t.Start();
        }

        //private List<Alerts> _myAlerts()
        //{
        //    Debug.WriteLine("Je passe dans la fonction _myAlert");
        //    return (mylist.mylist);
        //}

        private async void DownloadJSONASync()
        {
            mylist = new List<Alerts>();
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://192.168.33.140:8081/building/alerts");

            HttpResponseMessage response = await client.GetAsync(client.BaseAddress);

            // verification of statuscode connection to the simulator
            Debug.WriteLine("response.statuscode = {0}", response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                BuildingAlerts myData = Newtonsoft.Json.JsonConvert.DeserializeObject<BuildingAlerts>(jsonString);
                //verification of values in JSon code received
                Debug.WriteLine("water : {0}", myData.water);
                Debug.WriteLine("VMCPower : {0}", myData.VMCPower);
                Debug.WriteLine("ECS : {0}", myData.ECS);

                int _water = Convert.ToInt32(myData.water);
                int _vmc = Convert.ToInt32(myData.VMCPower);
                int _ecs = Convert.ToInt32(myData.ECS);

                if (_water == 1)
                {
                    Alerts myAlert = new Alerts();

                    myAlert.imageName = "Assets/logo_robinet.png";
                    myAlert.errorText = "Il y a une consommation anormale d'eau, vous devriez vérifier votre installation.";
                    Debug.WriteLine("passe dans water");
                    mylist.Add(myAlert);
                }
                if (_vmc == 1)
                {
                    Alerts myAlert = new Alerts();

                    myAlert.imageName = "Assets/logo_vmc.png";
                    myAlert.errorText = "Vérifiez l'entrée d'air de la VMC, elle est peut-être obstruée.";
                    mylist.Add(myAlert);
                    Debug.WriteLine("passe dans vmc");
                }
                if (_ecs == 1)
                {
                    Alerts myAlert = new Alerts();

                    myAlert.imageName = "Assets/logo_chauffe-eau.png";
                    myAlert.errorText = "Vérifiez votre chauffe-eau, l'alimentation solaire semble défaillante.";
                    mylist.Add(myAlert);
                    Debug.WriteLine("passe dans ecs");
                }
                Debug.WriteLine("mylist[0] = {0}", mylist.Count);
                //_myAlerts();
                //await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        public List<Alerts> AlerteList { get { return mylist; } }

        //public static class UriExtensions
        //{
        //    public static Uri SetPort(this Uri uri, int newPort)
        //    {
        //        var builder = new UriBuilder(uri);
        //        builder.Port = newPort;
        //        return builder.Uri;
        //    }
        //}

        //private async void DownloadJSONASync()
        //{
        //    int i = 0;
        //    mylist = new List<Alerts>();
        //    List<string> text = new List<string>() { "room/sdb_1/details",
        //                                            "room/chambre_1/details",
        //                                            "room/chambre_2/details",
        //                                            "room/chambre_3/details",
        //                                            "room/couloir/details",
        //                                            "room/salon/details",
        //                                            "room/cellier/details",
        //                                            "room/cuisine/details",
        //                                            "room/wc_1/details" };

        //    while (i != 42)
        //    {
        //        if (i == 9)
        //        {
        //            i = 0;
        //            mylist = new List<Alerts>();
        //        }
        //        HttpClient client = new HttpClient();
        //        client.BaseAddress = new Uri("http://192.168.33.140:8081/" + text[i]);

        //        HttpResponseMessage response = await client.GetAsync(client.BaseAddress);

        //        // verification of statuscode connection to the simulator
        //        Debug.WriteLine("response.statuscode = {0}", response.StatusCode);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var jsonString = await response.Content.ReadAsStringAsync();
        //            RoomSpec myData = Newtonsoft.Json.JsonConvert.DeserializeObject<RoomSpec>(jsonString);
        //            Window_AirQuality_Message(myData);

        //            // verification of values in JSon code received
        //            Debug.WriteLine("id : {0}", myData.id);
        //            Debug.WriteLine("name : {0}", myData.name);
        //            Debug.WriteLine("temperature : {0}", myData.temperature);
        //            Debug.WriteLine("humidity : {0}", myData.humidity);
        //            Debug.WriteLine("comfort : {0}", myData.comfort);
        //            Debug.WriteLine("airquality : {0}", myData.qai);
        //            Debug.WriteLine("meanLighting : {0}", myData.meanLighting);
        //            Debug.WriteLine("extOpen : {0}", myData.extOpen);
        //            Debug.WriteLine("lightOn : {0}", myData.lightOn);
        //            Debug.WriteLine("presence : {0}", myData.presence);
        //            Debug.WriteLine("heatingOn : {0}", myData.heatingOn);
        //        }
        //        else
        //        {
        //            Debug.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
        //        }

        //        mylist = _myAlerts();

        //        Debug.WriteLine("before sleep");
        //        await Task.Delay(TimeSpan.FromSeconds(5));
        //        Debug.WriteLine("after sleep");
        //        Debug.WriteLine("i = {0}", i);
        //        //Debug.WriteLine("mylist = {1}", mylist);
        //        i++;
        //    }
        //}

        public class RoomList
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class RoomSpec
        {
            public string id { get; set; }
            public string name { get; set; }
            public string temperature { get; set; }
            public string humidity { get; set; }
            public string comfort { get; set; }
            public string qai { get; set; }
            public string meanLighting { get; set; }
            public string extOpen { get; set; }
            public string lightOn { get; set; }
            public string presence { get; set; }
            public string heatingOn { get; set; }
        }

        public class BuildingAlerts
        {
            public string water { get; set; }
            public string VMCPower { get; set; }
            public string ECS { get; set; }
        }

        private void Window_AirQuality_Message(RoomSpec myData)
        {
            mylist = new List<Alerts>();
            int result = Convert.ToInt32(myData.qai);
            int seuilAir = 50;
            Debug.WriteLine("myData.qai = {0}", myData.qai);
            Debug.WriteLine("result = {0}", result);
             if (result == seuilAir)
             {
                 Alerts myAlert = new Alerts();

                 myAlert.imageName = "alerte1_0003s_0006s_0000_fenetre-ouverte.png";
                 myAlert.errorText = "Fermez la fenêtre de " + myData.name + " pour concerver votre niveau de confort";
                 mylist.Add(myAlert);
             }
        }

        private void Window_Heating_Message(RoomSpec myData)
        {

        }

        private void VMC_AirQuality_Message(RoomSpec myData)
        {

        }

        private void VMC_Consumption_Message(RoomSpec myData)
        {

        }

        private void Water_Consumption_Message(RoomSpec myData)
        {

        }

        private void Solar_Thermal_Consumption_Message(RoomSpec myData)
        {

        }

        private void State_Housing(RoomSpec myData)
        {

        }

        private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PostItImg_DragEnter(object sender, DragEventArgs e)
        {
           
        }

        private void PostItImg_Drop(object sender, DragEventArgs e)
        {
            
        }

        //private List<Alerts> _myAlerts()
        //{
        //    Debug.WriteLine("Je passe dans la fonction _myAlert");
        //    return (mylist);
        //}

        private void AlertPointerEnter()
        {
            Closed.Begin();
        }

        private void AlertPointerExit()
        {
            Opened.Begin();
        }

        static bool bolAlert = false;
        static bool bolPlan = false;
        static bool bolPostit = false;

        private void AlertClick(object sender, RoutedEventArgs e)
        {
            bool isclick = bolAlert;

            if (isclick == true)
            {
                AlertPointerExit();
                bolAlert = false;
            }
            else if (isclick == false)
            {
                AlertPointerEnter();
                bolAlert = true;
            }
        }

        private void PlanClick(object sender, RoutedEventArgs e)
        {
            bool isclick = bolPlan;

            if (isclick == true)
            {
                this.PlanLoft.Visibility = Visibility.Collapsed;
                bolPlan = false;
            }
            else if (isclick == false)
            {
                this.PlanLoft.Visibility = Visibility.Visible;
                bolPlan = true;
            }
        }

        private void PostitClick(object sender, RoutedEventArgs e)
        {
            bool isclick = bolPostit;

            if (isclick == true)
            {
                this.postIt.Visibility = Visibility.Collapsed;
                bolPostit = false;
            }
            else if (isclick == false)
            {
                this.postIt.Visibility = Visibility.Visible;
                bolPostit = true;
            }
        }
    }
}
