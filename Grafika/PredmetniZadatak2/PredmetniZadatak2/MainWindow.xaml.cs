using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace PredmetniZadatak2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //GLOBAL INDICATORS
        int substationsLoaded = 0;
        int switchesLoaded = 0;
        int nodesLoaded = 0;
        int linesLoaded = 0;

        public MainWindow()
        {
            InitializeComponent();

        }
   
        
        //LOAD MAP
        private void LoadMapButton_Click(object sender, RoutedEventArgs e)
        {
            GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;

            gmap.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;

            gmap.MinZoom = 7;
            gmap.MaxZoom = 40;

            //Set position on Novi Sad
            double blX = 45.2325;
            double blY = 19.793909;
            double trX = 45.277031;
            double trY = 19.894459;
            gmap.Position = new PointLatLng((blX + trX) / 2, (blY + trY) / 2);

            gmap.Zoom = 13;
            gmap.DragButton = System.Windows.Forms.MouseButtons.Left;
            
        }


        #region Load from XML

        private NetworkModel Deserialize(string path)
        {
            NetworkModel networkModel = null;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(NetworkModel));

            try
            {
                StreamReader reader = new StreamReader(path);
                networkModel = (NetworkModel)xmlSerializer.Deserialize(reader);
                reader.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Doslo je do greske: " + e.ToString());
            }

            return networkModel;
        }

        public void LoadSubstations()
        {
            NetworkModel networkModel = Deserialize("Geographic.xml");
            List<SubstationEntity> substationEntities = networkModel.Substations;

            SetMarkersForSubstations(substationEntities);
        }

        public void LoadSwitches()
        {
            NetworkModel networkModel = Deserialize("Geographic.xml");
            List<SwitchEntity> switchEntities = networkModel.Switches;

            SetMarkersForSwitches(switchEntities);
        }

        public void LoadNodes()
        {
            NetworkModel networkModel = Deserialize("Geographic.xml");
            List<NodeEntity> nodeEntities = networkModel.Nodes;

            SetMarkersForNodes(nodeEntities);
        }

        public void LoadLines()
        {
            NetworkModel networkModel = Deserialize("Geographic.xml");
            List<LineEntity> lines = networkModel.Lines;

            SetLines(lines);
        }
        #endregion

        #region Markers
        private void AddMarker(string n, double x, double y)
        {
            GMarkerGoogle marker = null;

            GMapOverlay markersOverlay = new GMapOverlay("markers");

            if (n == "Substation")
            {
                marker = new GMarkerGoogle(new PointLatLng(x, y), GMarkerGoogleType.green_small);
                marker.ToolTipText = "Substation";
            }
            else if (n == "Node")
            {
                marker = new GMarkerGoogle(new PointLatLng(x, y), GMarkerGoogleType.yellow_small);
                marker.ToolTipText = "Node";
            }
            else if (n == "Switch")
            {
                marker = new GMarkerGoogle(new PointLatLng(x, y), GMarkerGoogleType.red_small);
                marker.ToolTipText = "Switch";
            }

            gmap.Overlays.Add(markersOverlay);
            markersOverlay.Markers.Add(marker);

        }

        private void SetMarkersForSubstations(List<SubstationEntity> substationEntities)
        {
            foreach (SubstationEntity e in substationEntities)
            {
                double latitude = 0;
                double longitude = 0;

                MainWindow.ToLatLon(Convert.ToDouble(e.X), Convert.ToDouble(e.Y), 34, out latitude, out longitude);

                e.X = latitude;
                e.Y = longitude;

                AddMarker("Substation", e.X, e.Y);
             
            }
        }

        private void SetMarkersForNodes(List<NodeEntity> nodeEnities)
        {
            foreach (NodeEntity e in nodeEnities)
            {
                double latitude = 0;
                double longitude = 0;

                MainWindow.ToLatLon(Convert.ToDouble(e.X), Convert.ToDouble(e.Y), 34, out latitude, out longitude);

                e.X = latitude;
                e.Y = longitude;

                AddMarker("Node", e.X, e.Y);
            }
        }

        private void SetMarkersForSwitches(List<SwitchEntity> switchEntities)
        {
            foreach (SwitchEntity e in switchEntities)
            {
                double latitude = 0;
                double longitude = 0;

                MainWindow.ToLatLon(Convert.ToDouble(e.X), Convert.ToDouble(e.Y), 34, out latitude, out longitude);

                e.X = latitude;
                e.Y = longitude;

                AddMarker("Switch", e.X, e.Y);
            }
        }

        #endregion

        #region Lines
        private void SetLines(List<LineEntity> lines)
        {
            foreach (LineEntity le in lines)
            {
                
                System.Drawing.Color c = System.Drawing.Color.Black;

                if (le.ConductorMaterial == "Copper")
                {
                    c = System.Drawing.Color.DarkOrange;
                }
                else if (le.ConductorMaterial == "Steel")
                {
                    c = System.Drawing.Color.SteelBlue;
                }
                else if (le.ConductorMaterial == "Acsr")
                {
                    c = System.Drawing.Color.Green;
                }

                bool k = true;

                for (int i = 0; i < le.Vertices.Count - 1; i++)
                {
                    double latitude = 0;
                    double longitude = 0;
                    double latitude1 = 0;
                    double longitude1 = 0;

                    if (k == true)
                    {
                        MainWindow.ToLatLon(Convert.ToDouble(le.Vertices[i].X), Convert.ToDouble(le.Vertices[i].Y), 34, out latitude, out longitude);
                        le.Vertices[i].X = latitude;
                        le.Vertices[i].Y = longitude;
                        k = false;
                    }

                    

                    if (i != le.Vertices.Count - 1)
                    {
                        MainWindow.ToLatLon(Convert.ToDouble(le.Vertices[i + 1].X), Convert.ToDouble(le.Vertices[i + 1].Y), 34, out latitude1, out longitude1);

                        le.Vertices[i + 1].X = latitude1;
                        le.Vertices[i + 1].Y = longitude1;
                    }
                   


                    AddLine(le.Vertices[i].X, le.Vertices[i].Y, le.Vertices[i + 1].X, le.Vertices[i + 1].Y, c);
                }
            }
        }

        private void AddLine(double x1, double y1, double x2, double y2, System.Drawing.Color c)
        {
            GMapOverlay polyOverlay = new GMapOverlay("polygons");
            List<PointLatLng> points = new List<PointLatLng>();
            points.Add(new PointLatLng(x1, y1));
            points.Add(new PointLatLng(x2, y2));
            GMapPolygon polygon = new GMapPolygon(points, "mypolygon");
            polygon.Fill = new SolidBrush(System.Drawing.Color.FromArgb(50, c));
            polygon.Stroke = new System.Drawing.Pen(c, (float)1.5);
            gmap.Overlays.Add(polyOverlay);
            polyOverlay.Polygons.Add(polygon);
            
        }

        #endregion

        //LOAD BUTTON 
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {

            //If Lines combobox is checked load lines 
            if (Lines.IsChecked == true && linesLoaded != 1)
            {
                linesLoaded = 1;
                LoadLines();
            }
            else if (Lines.IsChecked == false) //If Lines combobox is not checked remove all lines from the map 
            {

                linesLoaded = 0;

                for (int i = gmap.Overlays.Count - 1; i > -1; i--)
                {
                    if (gmap.Overlays[i].Markers.Count == 0)
                    {
                        gmap.Overlays.RemoveAt(i);
                        
                    }
                }

                gmap.ReloadMap();
            }

            //If Switches combobox is checked load switches 
            if (Switches.IsChecked == true && switchesLoaded != 1)
            {
                switchesLoaded = 1;
                LoadSwitches();
            }
            else if (Switches.IsChecked == false) //If Switches combobox is not checked remove all switch markers from the map 
            {

                switchesLoaded = 0;

                for (int i = gmap.Overlays.Count - 1; i > -1; i--)
                {

                    if (gmap.Overlays[i].Markers.Count != 0)
                    {
                        if (gmap.Overlays[i].Markers[0].ToolTipText == "Switch")
                        {
                            gmap.Overlays.RemoveAt(i);
                        }
                    }
                }

                gmap.ReloadMap();
            }

            //If Substations combobox is checked load substations 
            if (Substations.IsChecked == true && substationsLoaded != 1)
            {
                substationsLoaded = 1;
                LoadSubstations();
            }
            else if (Substations.IsChecked == false) //If Substations combobox is not checked remove all substation markers from the map 
            {

                substationsLoaded = 0;

                for (int i = gmap.Overlays.Count - 1; i > -1; i--)
                {

                    if (gmap.Overlays[i].Markers.Count != 0)
                    {
                        if (gmap.Overlays[i].Markers[0].ToolTipText == "Substation")
                        {
                            gmap.Overlays.RemoveAt(i);
                        }
                    }
                }

                gmap.ReloadMap();
            }

            //If Nodes combobox is checked load nodes 
            if (Nodes.IsChecked == true && nodesLoaded != 1)
            {
                nodesLoaded = 1;
                LoadNodes();
            }
            else if (Nodes.IsChecked == false) //If Nodes combobox is not checked remove all node markers from the map 
            {

                nodesLoaded = 0;

                for (int i = gmap.Overlays.Count - 1; i > -1; i--)
                {

                    if (gmap.Overlays[i].Markers.Count != 0)
                    {
                        if (gmap.Overlays[i].Markers[0].ToolTipText == "Node")
                        {
                            gmap.Overlays.RemoveAt(i);
                        }
                    }
                }

                gmap.ReloadMap();
            }

            
        }

        //MATH (Conversion of coordinates from xml to map)
        public static void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }

    }
}
