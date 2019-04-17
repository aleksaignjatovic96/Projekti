using PZ4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace pz4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool showSubstationVisual3D = false;
        private bool showNodeVisual3D = false;
        private bool switchNodeVisual3D = false;
        private bool lineNodeVisual3D = false;

        private List<SubstationEntity> substationEntities;
        private List<SwitchEntity> switchEntities;
        private List<NodeEntity> nodeEntities;
        private List<LineEntity> lines;


        private bool mouseDown;
        private int mouseDownLM;
        private Point centerOfViewport;
        private double yaw;
        private double pitch;

        private Point diffOffset = new Point();
        private Point start = new Point();

        public bool ShowSubstationVisual3D
        {
            get
            {
                return this.showSubstationVisual3D;
            }
            set
            {
                this.showSubstationVisual3D = value;
                LoadSubstationModel3D();
            }
        }
        public bool ShowNodeVisual3D
        {
            get
            {
                return this.showNodeVisual3D;
            }
            set
            {
                this.showNodeVisual3D = value;
                LoadNodeModel3D();
            }
        }
        public bool SwitchNodeVisual3D
        {
            get
            {
                return this.switchNodeVisual3D;
            }
            set
            {
                this.switchNodeVisual3D = value;
                LoadSwitchModel3D();
            }
        }
        public bool LineNodeVisual3D
        {
            get
            {
                return this.lineNodeVisual3D;
            }
            set
            {
                this.lineNodeVisual3D = value;
                LoadLineModel3D();
            }
        }

        public bool CanMoveCamera { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            this.Reset();
            this.CanMoveCamera = true;

            LoadElement();

            this.DataContext = this;
        }

        public int collision(Rect3D src)
        {
            int ret = 0;

            if (ShowSubstationVisual3D)
            {
                for (int i = 0; i < ((Model3DGroup)group1.Children[0]).Children.Count; i++)
                {
                    Rect3D bb = ((Model3DGroup)group1.Children[0]).Children[i].Bounds;

                    if (src.IntersectsWith(bb))
                    {
                        ret++;
                    }
                }
            }

            if (ShowNodeVisual3D)
            {
                for (int i = 0; i < ((Model3DGroup)group2.Children[0]).Children.Count; i++)
                {
                    Rect3D bb = ((Model3DGroup)group2.Children[0]).Children[i].Bounds;

                    if (src.IntersectsWith(bb))
                    {
                        ret++;
                    }
                }
            }

            if (SwitchNodeVisual3D)
            {
                for (int i = 0; i < ((Model3DGroup)group3.Children[0]).Children.Count; i++)
                {
                    Rect3D bb = ((Model3DGroup)group3.Children[0]).Children[i].Bounds;

                    if (src.IntersectsWith(bb))
                    {
                        ret++;
                    }
                }
            }


            return ret;
        }

        public void LoadSubstationModel3D()
        {
            if (ShowSubstationVisual3D)
            {

                Model3DGroup modelGroup = new Model3DGroup();
                group1.Children.Add(modelGroup);

                foreach (SubstationEntity e in substationEntities)
                {
                    double latitude = 0;
                    double longitude = 0;

                    MainWindow.ToLatLon(Convert.ToDouble(e.X), Convert.ToDouble(e.Y), 34, out latitude, out longitude);

                    //Ako je van mape neprikazuj
                    if (longitude > 19.894459 || longitude < 19.793909) continue;
                    if (latitude < 45.2325 || latitude > 45.277031) continue;

                    double delta_long = 19.894459 - 19.793909; //HOR
                    double delta_lat = 45.277031 - 45.2325; //VER

                    double delta_h = 2;
                    double delta_v = 2;

                    double vertical_scale = delta_v / delta_lat;
                    double horizontal_scale = delta_h / delta_long;

                    double H = (longitude - 19.894459) * horizontal_scale + 1;
                    double V = (latitude - 45.277031) * vertical_scale + 1;


                    GeometryModel3D Cube1 = new GeometryModel3D();
                    MeshGeometry3D cubeMesh = MCube(H, V);
                    Cube1.Geometry = cubeMesh;
                    Cube1.Material = new DiffuseMaterial(
                              new SolidColorBrush(Colors.Green));

                    e.Obj = Cube1;



                    modelGroup.Children.Add(Cube1);
                }
                //group1.Children.Add(modelGroup);
            }
            else
            {
                group1.Children.Clear();
            }
        }

        public void LoadNodeModel3D()
        {
            if (ShowNodeVisual3D)
            {
                Model3DGroup modelGroup = new Model3DGroup();

                group2.Children.Add(modelGroup);

                foreach (NodeEntity e in nodeEntities)
                {
                    double latitude = 0;
                    double longitude = 0;

                    MainWindow.ToLatLon(Convert.ToDouble(e.X), Convert.ToDouble(e.Y), 34, out latitude, out longitude);


                    //Ako je van mape neprikazuj
                    if (longitude > 19.894459 || longitude < 19.793909) continue;
                    if (latitude < 45.2325 || latitude > 45.277031) continue;

                    double delta_long = 19.894459 - 19.793909; //HOR
                    double delta_lat = 45.277031 - 45.2325; //VER

                    double delta_h = 2;
                    double delta_v = 2;

                    double vertical_scale = delta_v / delta_lat;
                    double horizontal_scale = delta_h / delta_long;

                    double H = (longitude - 19.894459) * horizontal_scale + 1;
                    double V = (latitude - 45.277031) * vertical_scale + 1;
                    double D = 0;


                    // Kolizija
                    Rect3D test = new Rect3D(new Point3D(0 + H, 0 + V, 0), new Size3D(0.01, 0.01, 0.01));
                    int vrati = collision(test);
                    if (vrati > 0)
                    {
                        D = 0.01 * vrati;
                    }
                    //////


                    GeometryModel3D Cube1 = new GeometryModel3D();
                    MeshGeometry3D cubeMesh = MCube(H, V, D);
                    Cube1.Geometry = cubeMesh;
                    Cube1.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Yellow));

                    e.Obj = Cube1;

                    modelGroup.Children.Add(Cube1);

                }
            }
            else
            {
                group2.Children.Clear();
            }
        }

        public void LoadSwitchModel3D()
        {
            if (SwitchNodeVisual3D)
            {
                Model3DGroup modelGroup = new Model3DGroup();
                group3.Children.Add(modelGroup);

                foreach (SwitchEntity e in switchEntities)
                {
                    double latitude = 0;
                    double longitude = 0;

                    MainWindow.ToLatLon(Convert.ToDouble(e.X), Convert.ToDouble(e.Y), 34, out latitude, out longitude);


                    //Ako je van mape neprikazuj
                    if (longitude > 19.894459 || longitude < 19.793909) continue;
                    if (latitude < 45.2325 || latitude > 45.277031) continue;

                    double delta_long = 19.894459 - 19.793909; //HOR
                    double delta_lat = 45.277031 - 45.2325; //VER

                    double delta_h = 2;
                    double delta_v = 2;

                    double vertical_scale = delta_v / delta_lat;
                    double horizontal_scale = delta_h / delta_long;

                    double H = (longitude - 19.894459) * horizontal_scale + 1;
                    double V = (latitude - 45.277031) * vertical_scale + 1;
                    double D = 0;

                    // Kolizija
                    Rect3D test = new Rect3D(new Point3D(0 + H, 0 + V, 0), new Size3D(0.01, 0.01, 0.01));
                    int vrati = collision(test);
                    if (vrati > 0)
                    {
                        D = 0.01 * vrati;
                    }
                    //////

                    GeometryModel3D Cube1 = new GeometryModel3D();
                    MeshGeometry3D cubeMesh = MCube(H, V, D);
                    Cube1.Geometry = cubeMesh;
                    Cube1.Material = new DiffuseMaterial(
                              new SolidColorBrush(Colors.Red));

                    e.Obj = Cube1;

                    modelGroup.Children.Add(Cube1);

                }
            }
            else
            {
                group3.Children.Clear();

            }
        }

        public void LoadLineModel3D()
        {
            if (LineNodeVisual3D)
            {
                Model3DGroup modelGroup = new Model3DGroup();
                group4.Children.Add(modelGroup);

                foreach (LineEntity e in lines)
                {
                    Point3DCollection niz = new Point3DCollection();

                    for (int i = 0; i < e.Vertices.Count ; i++)
                    {
                        double latitude = 0;
                        double longitude = 0;

                        MainWindow.ToLatLon(Convert.ToDouble(e.Vertices[i].X), Convert.ToDouble(e.Vertices[i].Y), 34, out latitude, out longitude);

                        //Ako je van mape neprikazuj
                        if (longitude > 19.894459 || longitude < 19.793909) continue;
                        if (latitude < 45.2325 || latitude > 45.277031) continue;

                        double delta_long = 19.894459 - 19.793909; //HOR
                        double delta_lat = 45.277031 - 45.2325; //VER

                        double delta_h = 10;
                        double delta_v = 10;

                        double vertical_scale = delta_v / delta_lat;
                        double horizontal_scale = delta_h / delta_long;

                        double H = longitude;// (longitude - 19.894459) * horizontal_scale + 5;
                        double V = latitude;//(latitude - 45.277031) * vertical_scale + 5;



                        niz.Add(new Point3D() { X = H, Y = V, Z = 0.2 });

                    }

                    for (int i = 0; i < niz.Count; i++)
                    {
                        if (i != niz.Count - 1)
                        {
                            Point3D start = niz[i];
                            Point3D stop = niz[i + 1];

                            GeometryModel3D lineObj = this.MLine(start, stop);
                            modelGroup.Children.Add(lineObj);

                        }

                    }

                }
            }
            else
            {
                group4.Children.Clear();

            }
        }


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

        public void Reset()
        {
            camera.Position = new Point3D(camera.Position.X, camera.Position.Y, 3);
            camera.Transform = new Transform3DGroup();
            yaw = 0;
            pitch = 0;
        }




        #region Load from XML
        public void LoadElement()
        {
            NetworkModel networkModel = Deserialize("Geographic.xml");

            substationEntities = networkModel.Substations;
            switchEntities = networkModel.Switches;
            nodeEntities = networkModel.Nodes;
            lines = networkModel.Lines;

        }
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

        #endregion

        #region MOUSE

        public void Zoom(double amount)
        {
            // For zooming we simply change the Z-position of the camera
            camera.Position = new Point3D(camera.Position.X, camera.Position.Y, camera.Position.Z - amount/2);
        }

        public void Pitch(double amount)
        {
            pitch += amount;
            this.Rotate();
        }

        public void Yaw(double amount)
        {
            yaw += amount;
            this.Rotate();
        }

        private void Rotate()
        {
            double theta = yaw / 3;
            double phi = pitch / 3;

            // Clamp phi (pitch) between -90 and 90 to avoid 'going upside down'
            // Just remove this if you want to make loopings :)
            if (phi < -90) phi = -90;
            if (phi > 90) phi = 90;

            // Here the rotation magic happens. Ask jemidiah for details, I've no clue :P
            Vector3D thetaAxis = new Vector3D(0, 1, 0);
            Vector3D phiAxis = new Vector3D(-1, 0, 0);

            Transform3DGroup transformGroup = camera.Transform as Transform3DGroup;
            transformGroup.Children.Clear();
            QuaternionRotation3D r = new QuaternionRotation3D(new Quaternion(-phiAxis, phi));
            transformGroup.Children.Add(new RotateTransform3D(r));
            r = new QuaternionRotation3D(new Quaternion(-thetaAxis, theta));
            transformGroup.Children.Add(new RotateTransform3D(r));
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.CanMoveCamera)
            {
                if (e.MiddleButton == MouseButtonState.Pressed)
                {
                    mouseDownLM = 2;
                    mouseDown = true;
                    centerOfViewport = viewport.PointToScreen(new Point(viewport.ActualWidth / 2, viewport.ActualHeight / 2));
                    MouseUtilities.SetPosition(centerOfViewport);
                    this.Cursor = Cursors.None;

                }
                else if (e.LeftButton == MouseButtonState.Pressed)
                {
                    mouseDownLM = 1;
                    mouseDown = true;

                    diffOffset.X = translacija0.OffsetX;
                    diffOffset.Y = translacija0.OffsetY;
                    start = e.GetPosition(this);


                    // oblacic
                    Point pozicijaMisa = e.GetPosition(viewport);
                    Point3D point3D = new Point3D(pozicijaMisa.X, pozicijaMisa.Y, 0);
                    Vector3D pravac = new Vector3D(pozicijaMisa.X, pozicijaMisa.Y, 10);

                    PointHitTestParameters pointParameters = new PointHitTestParameters(pozicijaMisa);
                    RayHitTestParameters rayParameters = new RayHitTestParameters(point3D, pravac);

                    VisualTreeHelper.HitTest(viewport, null, HTResult, pointParameters);

                }

            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Indicate that the mouse is no longer pressed and make the cursor visible again
            mouseDown = false;
            this.Cursor = Cursors.Arrow;
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.CanMoveCamera)
            {

                if (!mouseDown) return;



                if (mouseDownLM == 1)
                {
                    Point end = e.GetPosition(this);
                    double rasponX = end.X - start.X;
                    double rasponY = end.Y - start.Y;

                    double sirina = this.ActualWidth;
                    double visina = this.ActualHeight;

                    int pomocna = 300;
                    double translacijaX = (rasponX * pomocna) / sirina;
                    double translacijaY = (-rasponY * pomocna) / visina;

                    translacija0.OffsetX = diffOffset.X + (translacijaX / (pomocna * 1));
                    translacija0.OffsetY = diffOffset.Y + (translacijaY / (pomocna * 1));

                    translacija1.OffsetX = translacija0.OffsetX;
                    translacija1.OffsetY = translacija0.OffsetY;
                    translacija2.OffsetX = translacija0.OffsetX;
                    translacija2.OffsetY = translacija0.OffsetY;
                    translacija3.OffsetX = translacija0.OffsetX;
                    translacija3.OffsetY = translacija0.OffsetY;
                    translacija4.OffsetX = translacija0.OffsetX;
                    translacija4.OffsetY = translacija0.OffsetY;
                }
                else if (mouseDownLM == 2)
                {
                    Point relativePos = Mouse.GetPosition(viewport);
                    Point actualRelativePos = new Point(relativePos.X - viewport.ActualWidth / 2,
                                                        viewport.ActualHeight / 2 - relativePos.Y);

                    double dx = actualRelativePos.X;
                    double dy = actualRelativePos.Y;

                    yaw += dx;
                    pitch += dy;

                    // Rotate
                    this.Rotate();

                    // Set mouse position back to the center of the viewport in screen coordinates
                    MouseUtilities.SetPosition(centerOfViewport);
                }

            }
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (this.CanMoveCamera)
            {
                // Zoom. Change 100 to a higher value for slower zooming, and vice versa
                this.Zoom(e.Delta / 100);
            }
        }

        private HitTestResultBehavior HTResult(System.Windows.Media.HitTestResult rawresult)
        {
            RayHitTestResult rayResult = rawresult as RayHitTestResult;

            if (rayResult != null)
            {

                foreach (SubstationEntity s in substationEntities)
                {
                    if (s.Obj == rayResult.ModelHit)
                    {
                        MessageBox.Show("SubstationEntity\n\nId: " + s.Id + "\nName: " + s.Name, "System Element Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        mouseDown = false;
                    }
                }

                foreach (NodeEntity s in nodeEntities)
                {
                    if (s.Obj == rayResult.ModelHit)
                    {
                        MessageBox.Show("NodeEntity\n\nId: " + s.Id + "\nName: " + s.Name, "System Element Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        mouseDown = false;
                    }
                }

                foreach (SwitchEntity s in switchEntities)
                {
                    if (s.Obj == rayResult.ModelHit)
                    {
                        MessageBox.Show("SwitchEntity\n\nId: " + s.Id + "\nName: " + s.Name, "System Element Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        mouseDown = false;
                    }
                }

            }


            return HitTestResultBehavior.Stop;
        }

        #endregion


        MeshGeometry3D MCube(double H, double V, double D = 0.0)
        {
            MeshGeometry3D cube = new MeshGeometry3D();
            Point3DCollection corners = new
                                   Point3DCollection();

            double vr = 0.005;
            corners.Add(new Point3D(vr + H, vr + V, D + vr * 2));
            corners.Add(new Point3D(-vr + H, vr + V, D + vr * 2));
            corners.Add(new Point3D(-vr + H, -vr + V, D + vr * 2));
            corners.Add(new Point3D(vr + H, -vr + V, D + vr * 2));
            corners.Add(new Point3D(vr + H, vr + V, D));
            corners.Add(new Point3D(-vr + H, vr + V, D));
            corners.Add(new Point3D(-vr + H, -vr + V, D));
            corners.Add(new Point3D(vr + H, -vr + V, D));
            cube.Positions = corners;

            Int32[] indices ={
   //front
     0,1,2,
     0,2,3,
  //back
     4,7,6,
     4,6,5,
  //Right
     4,0,3,
     4,3,7,
  //Left
     1,5,6,
     1,6,2,
  //Top
     1,0,4,
     1,4,5,
  //Bottom
     2,6,7,
     2,7,3
            };

            Int32Collection Triangles =
                                  new Int32Collection();
            foreach (Int32 index in indices)
            {
                Triangles.Add(index);
            }
            cube.TriangleIndices = Triangles;
            return cube;
        }
        public GeometryModel3D MLine(Point3D start, Point3D stop)
        {
            double x1 = start.Y;
            double y1 = start.X;
            double x2 = stop.Y;
            double y2 = stop.X;

            double dlx = 45.2325;
            double dly = 19.793909;
            double gdx = 45.277031;
            double gdy = 19.894459;

            double centarX = gdx - dlx;
            double centarY = gdy - dly;

            double pozicijaX1 = Math.Abs(dlx - x1);
            double pozicijaY1 = Math.Abs(dly - y1);

            double pozicijaX2 = Math.Abs(dlx - x2);
            double pozicijaY2 = Math.Abs(dly - y2);

            SolidColorBrush brush = new SolidColorBrush(Colors.Black);

            var materijal = new DiffuseMaterial(brush);
            var mesh = new MeshGeometry3D();

            Point3D teme1 = new Point3D(-1 + 2 * pozicijaY1 / centarY    , -1 + 2 * pozicijaX1 / centarX    , 0.005);
            Point3D teme2 = new Point3D(-0.995 + 2 * pozicijaY1 / centarY, -1 + 2 * pozicijaX1 / centarX    , 0.005);
            Point3D teme3 = new Point3D(-1 + 2 * pozicijaY1 / centarY    , -0.995 + 2 * pozicijaX1 / centarX, 0.005);
            Point3D teme4 = new Point3D(-0.995 + 2 * pozicijaY1 / centarY, -0.995 + 2 * pozicijaX1 / centarX, 0.005);

            Point3D teme5 = new Point3D(-1 + 2 * pozicijaY2 / centarY    , -1 + 2 * pozicijaX2 / centarX    , 0.005);
            Point3D teme6 = new Point3D(-0.995 + 2 * pozicijaY2 / centarY, -1 + 2 * pozicijaX2 / centarX    , 0.005);
            Point3D teme7 = new Point3D(-1 + 2 * pozicijaY2 / centarY    , -0.995 + 2 * pozicijaX2 / centarX, 0.005);
            Point3D teme8 = new Point3D(-0.995 + 2 * pozicijaY2 / centarY, -0.995 + 2 * pozicijaX2 / centarX, 0.005);

            mesh.Positions.Add(teme1);
            mesh.Positions.Add(teme2);
            mesh.Positions.Add(teme3);
            mesh.Positions.Add(teme4);
            mesh.Positions.Add(teme5);
            mesh.Positions.Add(teme6);
            mesh.Positions.Add(teme7);
            mesh.Positions.Add(teme8);

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(5);

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(4);

            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(5);

            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(5);

            mesh.TriangleIndices.Add(3);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(7);

            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(7);

            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(2);

            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(4);
            mesh.TriangleIndices.Add(6);

            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(6);
            mesh.TriangleIndices.Add(4);

            mesh.TriangleIndices.Add(5);
            mesh.TriangleIndices.Add(7);
            mesh.TriangleIndices.Add(6);

            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(2);

            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);
            mesh.TriangleIndices.Add(3);

            return new GeometryModel3D(mesh, materijal);
        }

    }

    public static class MouseUtilities
    {
        public static Point GetPosition(Visual relativeTo)
        {
            return relativeTo.PointFromScreen(GetPosition());
        }

        public static Point GetPosition()
        {
            Win32Point w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);
            return new Point(w32Mouse.X, w32Mouse.Y);
        }

        public static void SetPosition(Point pt)
        {
            SetPosition(pt.X, pt.Y);
        }

        public static void SetPosition(double x, double y)
        {
            SetCursorPos((int)x, (int)y);
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "SetCursorPos")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        internal static extern bool SetCursorPos(int x, int y);
    }


}

 


 
   


 